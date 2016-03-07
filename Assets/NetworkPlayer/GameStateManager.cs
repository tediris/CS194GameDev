using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameStateManager : NetworkBehaviour {

	public MapGen mapGenerator;
	[SyncVar] public string curSeed = "null";
	PlayerIDs idStore;
	List<GameObject> players;
	ServerComm localPlayerComm;
	CoinPlacement coinPlacer = null;
	public GameObject eggPrefab;

	public GameObject GetLocalPlayer() {
		return localPlayerComm.gameObject;
	}

	public override void OnStartServer()
	{
		Debug.Log ("Server started");
		curSeed = System.DateTime.Now.ToString();
		players = new List<GameObject> ();
		mapGenerator.GenerateNewMap (curSeed);
		coinPlacer = GameObject.Find ("Coins").GetComponent<CoinPlacement>();
		coinPlacer.PlaceCoins ();
		//coinPlacer.PlaceEnemies ();
		// disable client stuff
	}

	public override void OnStartClient()
	{
		Debug.Log("Client started");
		StartCoroutine (GenMapOnClient ());
		// register client events, enable effects

	}

	IEnumerator GenMapOnClient() {
		while (curSeed == "null") {
			yield return new WaitForSeconds (0.1f);
		}
		if (!isServer)
			mapGenerator.GenerateNewMap (curSeed);
	}

	public void AddPlayer(GameObject player) {
		if (!isServer) {
			Debug.Log ("WARNING: trying to add a player on a client");
			return;
		}
		players.Add (player);
	}

	public void StoreLocalPlayer(ServerComm comm) {
		localPlayerComm = comm;
	}

	public void SpawnEgg(Vector3 loc) {
		if (!isServer)
			return;
		GameObject egg = (GameObject)Instantiate (eggPrefab, loc, Quaternion.identity);
		egg.name = "Egg_Capture";
		egg.transform.parent = mapGenerator.gameObject.transform;
		NetworkServer.Spawn (egg);
	}

	public void CreateOverNetwork (GameObject go, Vector3 pos) {
		if (!isServer)
			return;
		GameObject instance = (GameObject)Instantiate (go, pos, Quaternion.identity);
		instance.transform.parent = mapGenerator.gameObject.transform;
		NetworkServer.Spawn (instance);
	}

	public void SpawnEggMonster(GameObject go, Vector3 pos, List<Rigidbody2D> waypoints) {
		if (!isServer)
			return;
		GameObject monster = (GameObject)Instantiate (go, pos, Quaternion.identity);
		WaypointFollow wayFollow = monster.GetComponent<WaypointFollow> ();
		if (wayFollow != null) {
			wayFollow.wayPoints = waypoints;
			wayFollow.enable = true;
		}
		monster.transform.parent = mapGenerator.gameObject.transform;
		NetworkServer.Spawn (monster);
	}

	public void GenerateNewMap() {
		//DESTROY EVERYTHING
		if (isServer) {
			curSeed = System.DateTime.Now.ToString ();
			CmdGenerateMaps (curSeed);
			coinPlacer.PlaceCoins ();
			//coinPlacer.PlaceEnemies ();
		} else {
			Debug.LogError ("STOP TRYING TO MAKE A NEW LEVEL AS A CLIENT T_T");
		}
	}

	public void ResetPlayerLocation(float x, float y) {
		if (isServer) {
			CmdReturnPlayers (x, y);
		} else {
			Debug.LogError ("Non command trying to move players around");
		}
	}

	// Use this for initialization
	void Awake () {
		mapGenerator = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	[Command]
	void CmdGenerateMaps(string seed) {
		RpcGenerateMaps (seed);
	}

	void SendToGraveyard(GameObject obj) {
		StartCoroutine (DestroyOnNextFrame (obj));
	}

	IEnumerator DestroyOnNextFrame(GameObject o) { 
		yield return new WaitForFixedUpdate ();
		Destroy (o);
	}

	[ClientRpc]
	void RpcGenerateMaps(string seed) {
		var children = new List<GameObject>();
		foreach (Transform child in mapGenerator.gameObject.transform) children.Add(child.gameObject);
		children.ForEach(child => SendToGraveyard(child));
		mapGenerator.GenerateNewMap (seed);
	}

	[Command]
	void CmdReturnPlayers(float x, float y) {
		RpcReturnPlayer (x, y);
	}

	[ClientRpc]
	void RpcReturnPlayer(float x, float y) {
		Debug.Log ("trying to move players back to start");
		localPlayerComm.MoveTo (x, y);
	}
}

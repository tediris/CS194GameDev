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

	[ClientRpc]
	void RpcGenerateMaps(string seed) {
		var children = new List<GameObject>();
		foreach (Transform child in mapGenerator.gameObject.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
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

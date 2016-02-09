using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameStateManager : NetworkBehaviour {

	public MapGen mapGenerator;
	[SyncVar] public string curSeed = "null";

	public override void OnStartServer()
	{
		Debug.Log ("Server started");
		curSeed = System.DateTime.Now.ToString();
		mapGenerator.GenerateNewMap (curSeed);
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

	public void GenerateNewMap() {
		//DESTROY EVERYTHING
		if (isServer) {
			curSeed = System.DateTime.Now.ToString ();
			CmdGenerateMaps (curSeed);
		} else {
			Debug.LogError ("STOP TRYING TO MAKE A NEW LEVEL AS A CLIENT T_T");
		}
	}

	// Use this for initialization
	void Awake () {
		mapGenerator = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
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
}

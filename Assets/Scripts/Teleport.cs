using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Teleport : NetworkBehaviour {

	public float toX;
	public float toY;
	public bool isEnd = false;

	private PlayerIDs idStore;
	private GameStateManager GSManager;

	// Use this for initialization
	void Start () {
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();
		GSManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}

	// Update is called once per frame
//	void Update () {
//
//	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<PlayerMove>().MoveTo(toX, toY);
//			Camera.main.transform.position = new Vector3 (toX, toY, Camera.main.transform.position.z);
			if (isEnd) {
				ReturnPlayers ();
			}
		}
	}

	[ClientCallback]
	void ReturnPlayers() {
		CmdReturnPlayers ();
	}

	[Command]
	void CmdReturnPlayers() {
		RpcReturnPlayer ();
		GSManager.GenerateNewMap ();
	}

	[ClientRpc]
	void RpcReturnPlayer() {
		GameObject.Find (idStore.localID).GetComponent<PlayerMove> ().MoveTo (toX, toY);
	}

}

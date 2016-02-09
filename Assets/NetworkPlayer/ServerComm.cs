﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerComm : NetworkBehaviour {

	GameStateManager GSManager;
	PlayerIDs idStore;

	// Use this for initialization
	void Start () {
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();
		GSManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		if (isServer) {
			GSManager.AddPlayer (this.gameObject);
		}
		if (isLocalPlayer) {
			GSManager.StoreLocalPlayer (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RequestMap() {
		if (isLocalPlayer) {
			CmdRequestNewMap ();
		} else {
			Debug.Log ("Not the local player, should not be requesting a new map");
		}
	}

	public void MovePlayers(float x, float y) {
		if (isLocalPlayer) {
			CmdRequestPositionReset (x, y);
		} else {
			Debug.Log ("Not the local player, should not be requesting a move ");
		}
	}

	public void MoveTo(float x, float y) {
		if (!isLocalPlayer) {
			Debug.LogWarning ("Not the local player, do not try to command");
			return;
		}
		CmdSetPosition (x, y);
	}

	[Command]
	void CmdRequestPositionReset(float x, float y) {
		GSManager.ResetPlayerLocation (x, y);
	}

	[Command]
	void CmdRequestNewMap() {
		GSManager.GenerateNewMap ();
	}

	[Command]
	void CmdSetPosition (float x, float y) {
		RpcSetPosition (x, y);
	}

	[ClientRpc]
	void RpcSetPosition(float x, float y) {
		gameObject.transform.position = new Vector3 (x, y, gameObject.transform.position.z);
	}
}
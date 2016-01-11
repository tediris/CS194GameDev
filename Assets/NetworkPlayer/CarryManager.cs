using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CarryManager : NetworkBehaviour {

	List<GameObject> players;

	// Use this for initialization
	void Start () {
		players = new List<GameObject> ();
		int playerLayer = LayerMask.NameToLayer ("Player");
		Physics2D.IgnoreLayerCollision (playerLayer, playerLayer);
	}

	public void AddPlayer(GameObject player) {
		Debug.Log ("Player " + players.Count + " connected");
		player.GetComponent<NetSetup> ().playerNum = players.Count;
		players.Add (player);
	}

	public GameObject GetPlayer(int playerNum) {
		if (playerNum >= 0 && playerNum < players.Count) {
			return players [playerNum];
		}
		return null;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
	}
}

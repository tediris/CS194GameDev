using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TreasureChest : NetworkBehaviour {

	GameStateManager gsManager;

	// Use this for initialization
	void Start () {
		gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		Debug.Log ("Am I server? " + isServer);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isServer) {
			Debug.Log ("Server detected collision");
		} else {
			Debug.Log ("Client detected collision");
		}
		Debug.Log (other.gameObject);
		if (!isServer)
			return;
		if (other.gameObject.tag == "Player") {
			// player collected treasure
			Debug.Log(other.name + " found a treasure");
			gsManager.numTreasures--;
			if (gsManager.numTreasures < 1) {
				Debug.Log ("Found the last treasure, ending the level");
				gsManager.ResetGame ();
			}
		}
	}
}

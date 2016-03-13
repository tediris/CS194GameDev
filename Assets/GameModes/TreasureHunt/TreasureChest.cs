using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TreasureChest : NetworkBehaviour {

	GameStateManager gsManager;

	// Use this for initialization
	void Start () {
		gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!isServer)
			return;
		if (other.gameObject.tag == "Player") {
			// player collected treasure
			if (other.gameObject.GetComponent<PlayerHealth> ().alive) {
				Debug.Log (other.name + " found a treasure");
				gsManager.numTreasures--;
				Destroy (this.gameObject);
				if (gsManager.numTreasures < 1) {
					Debug.Log ("Found the last treasure, ending the level");
					gsManager.ResetGame ();
				}
			}
		}
	}
}

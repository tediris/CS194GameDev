using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ApplyDamage : NetworkBehaviour {

	GameStateManager stateManager;

	void Start() {
		stateManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (stateManager.GetLocalPlayer () == null) {
			return;
		}
		if (other.gameObject.name == stateManager.GetLocalPlayer().name) {
			other.GetComponent<PlayerControl> ().DidGetHit (this.gameObject.GetComponent<Rigidbody2D>().position);
			other.GetComponent<PlayerHealth> ().Hit ();
		}
	}
}

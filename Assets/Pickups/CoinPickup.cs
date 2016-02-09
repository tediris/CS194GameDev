using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinPickup : NetworkBehaviour {

	public int value = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			if (!isServer) {
				Destroy (this.gameObject);
			} else {
				int playerNum = other.name [other.name.Length - 1];
				other.gameObject.GetComponent<CoinCollector> ().CmdIncrementCoins (value);
				Destroy (this.gameObject);
			}
		}
	}
}

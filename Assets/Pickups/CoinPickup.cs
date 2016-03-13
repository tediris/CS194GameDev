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
			if (!other.gameObject.GetComponent<PlayerHealth> ().alive)
				return;
			if (isServer) {
				int playerNum = other.name [other.name.Length - 1];
				other.gameObject.GetComponent<CoinCollector> ().CmdIncrementCoins (value);
			}
			this.transform.position = new Vector3(100000000.0f,10000000.0f,10000000.0f);
			StartCoroutine (DestroyOnNextFrame (this.gameObject));
		}
	}

	public void CollectCoin(GameObject player) {
		if (isServer) {
			int playerNum = player.name [player.name.Length - 1];
			player.GetComponent<CoinCollector> ().CmdIncrementCoins (value);
		}
		this.transform.position = new Vector3(100000000.0f,10000000.0f,10000000.0f);
		StartCoroutine (DestroyOnNextFrame (this.gameObject));
	}

	IEnumerator DestroyOnNextFrame(GameObject o) { 
		yield return new WaitForFixedUpdate ();
		Destroy (o);
	}
}

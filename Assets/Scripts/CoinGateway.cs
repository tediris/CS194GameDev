using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinGateway : NetworkBehaviour {

	public float deltaX;
	public float deltaY;
	public int coinRequirement;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	//	void Update () {
	//
	//	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			CoinCollector collector = other.GetComponent<CoinCollector> ();
			if (collector.numCoins >= coinRequirement) {
				other.GetComponent<PlayerMove> ().MoveByDelta (deltaX, deltaY);
			}
		}
	}

}

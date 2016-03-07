using UnityEngine;
using System.Collections;

public class CollectingRadius : MonoBehaviour {

	public bool contact = false;

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Coin") {
			contact = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Coin") {
			contact = false;
		}
	}
}

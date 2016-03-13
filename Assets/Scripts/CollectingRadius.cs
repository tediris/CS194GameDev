using UnityEngine;
using System.Collections;

public class CollectingRadius : MonoBehaviour {

	int numContacting = 0;

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public bool Contact() {
		return numContacting > 0;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Coin") {
			numContacting++;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Coin") {
			numContacting--;
		}
	}
}

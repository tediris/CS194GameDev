using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetCoinCollect : MonoBehaviour {

	HashSet<GameObject> coins;

	PetFollow followScript;

	public float seekDuration = 10f;
	bool seeking = false;

	// Use this for initialization
	void Start () {
		coins = new HashSet<GameObject> ();
		followScript = GetComponent<PetFollow> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SeekCoins() {
		followScript.followingPlayer = false;
		seeking = true;
		StartCoroutine (SeekTimer ());

	}

	IEnumerator SeekTimer() {
		yield return new WaitForSeconds (seekDuration);
		seeking = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Coin") {
			coins.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Coin") {
			coins.Remove (other.gameObject);
		}
	}
}

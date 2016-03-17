using UnityEngine;
using System.Collections;

public class LevelTypeDescription : MonoBehaviour {

	public string description = "You didn't set the text!";
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<PlayerNotification> ().SetPlayerNotification (description, Color.white);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<PlayerNotification> ().ClearPlayerNotification ();
		}
	}
}

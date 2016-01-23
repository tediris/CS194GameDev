using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		GameObject player = coll.gameObject;
		player.GetComponent<PlayerControl> ().enabled = false;
		GameObject completionText = GameObject.Find ("CompletionText");
		completionText.GetComponent<Text> ().color = new Color (0, 0, 0);
	}
}

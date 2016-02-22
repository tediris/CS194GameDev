using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SuperJumpManager : MonoBehaviour {

	public Text superJumpModeText;

	// Use this for initialization
	void Start () {
		superJumpModeText = GameObject.Find ("PowerJumpMode").GetComponent<Text> ();
		superJumpModeText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerControl control = other.gameObject.GetComponent<PlayerControl> ();
			control.hasJetpack = true;
			Destroy (this.gameObject);
		}
	}
}

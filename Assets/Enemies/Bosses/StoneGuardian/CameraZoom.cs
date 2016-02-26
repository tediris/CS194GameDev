using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public Camera mainCam;
	float size;
	public float largeSize = 3.0f;
	bool zoomOut = false;
	string localPlayerName = "null";

	// Use this for initialization
	void Start () {
		size = mainCam.orthographicSize;
	}

	void Update() {
		if (zoomOut) {
			mainCam.orthographicSize = Mathf.Lerp (mainCam.orthographicSize, largeSize, 0.05f);
		} else {
			mainCam.orthographicSize = Mathf.Lerp (mainCam.orthographicSize, size, 0.05f);
		}
	}

	void GetLocalPlayer() {
		localPlayerName = GameObject.Find ("GameState").GetComponent<GameStateManager> ().GetLocalPlayer ().name;
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			if (localPlayerName == "null") {
				GetLocalPlayer ();
			}
			if (other.gameObject.name == localPlayerName) {
				zoomOut = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			if (other.gameObject.name == localPlayerName) {
				zoomOut = false;
			}
		}
	}
}

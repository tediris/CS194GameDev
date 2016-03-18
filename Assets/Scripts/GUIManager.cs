using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameObject netManager;
	public GameObject controlMenu;
	bool controlMenuVisible = false;
	PlayerControl.GeneralInput playerInput;

	// Use this for initialization
	void Start () {
		netManager = GameObject.Find ("Menu");
		controlMenu = GameObject.Find ("Controls");
		netManager.SetActive(false);
		controlMenu.SetActive (false);
		StartCoroutine (FindLocalPlayerControl ());
	}

	void Update() {
		if (playerInput == null)
			return;
		if (playerInput.MenuButton ()) {
			ToggleControlMenu ();
		}
	}

	IEnumerator FindLocalPlayerControl() {
		yield return new WaitForSeconds (3.0f);
		playerInput = GameObject.Find ("GameState").GetComponent<GameStateManager> ().GetLocalPlayer ().GetComponent<PlayerControl> ().input;
		if (playerInput == null)
			StartCoroutine(FindLocalPlayerControl ());
	}

	void ToggleControlMenu() {
		controlMenuVisible = !controlMenuVisible;
		controlMenu.SetActive (controlMenuVisible);
	}
}

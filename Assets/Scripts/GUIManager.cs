using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GUIManager : MonoBehaviour {

	public NetworkManagerHUD netManager;

	// Use this for initialization
	void Start () {
		netManager = GameObject.Find ("Network").GetComponent<NetworkManagerHUD>();
		netManager.showGUI = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			netManager.showGUI = !netManager.showGUI;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager {

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void StartupHost() {
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame() {
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress() {
		string ipAddress = GameObject.Find ("IPAddressBox").transform.FindChild ("Text").GetComponent<Text> ().text;
		Debug.Log (ipAddress);
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort() {
		NetworkManager.singleton.networkPort = 7777;
	}
}

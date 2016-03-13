using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GenericNetSetup : NetworkBehaviour {

	public List<MonoBehaviour> activeScripts;

	// Use this for initialization
	void Start () {
		foreach (MonoBehaviour script in activeScripts) {
			script.enabled = isServer;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}

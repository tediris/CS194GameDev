using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupBaitOwl : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (!isServer) {
			Destroy (this.GetComponentInChildren<BaitFinder> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

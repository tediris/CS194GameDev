using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PickupManager : NetworkBehaviour {

	[SyncVar] int counter = 0;

	// Use this for initialization
//	void Start () {
//	
//	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public int getID() {
		int id = counter;
		counter++;
		return id;
	}
}

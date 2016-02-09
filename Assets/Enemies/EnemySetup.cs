using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySetup : NetworkBehaviour {

	public MonoBehaviour controlScript;

	// Use this for initialization
	void Start () {
		if (isServer) {
			controlScript.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

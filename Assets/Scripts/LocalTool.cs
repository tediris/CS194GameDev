using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LocalTool : NetworkBehaviour {
	[SyncVar]
	public int playerNum = -1;
	public GameObject pickup;

	// Use this for initialization
//	void Start () {
//		Debug.Log ("Hello");
//	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent == null) {
			if (playerNum != -1) {
				GameObject player = GameObject.Find ("Player" + playerNum);
				transform.parent = player.transform;
			}
		} else {
//			this.enabled = false;
		}
	}

}

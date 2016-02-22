using UnityEngine;
using System.Collections;

public class ShrineRespawn : MonoBehaviour {

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerHealth health = other.gameObject.GetComponent<PlayerHealth> ();
			health.BringToLife ();
		}
	}
}

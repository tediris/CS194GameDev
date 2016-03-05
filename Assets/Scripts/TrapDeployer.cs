using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrapDeployer : NetworkBehaviour {

	public Transform droppingTrap;
	public Transform raisingTrap;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	//	void Update () {
	//
	//	}

	void OnCollisionEnter2D(Collision2D collision) {
		Collider2D other = collision.collider;
		Debug.Log ("Entered trigger on trap deployer");
		if (other.tag == "Player") {
			if (droppingTrap) {
				TrapManager manager = droppingTrap.GetComponent<TrapManager> ();
				manager.DropTrap ();
			}
			if (raisingTrap) {
				TrapManager manager = raisingTrap.GetComponent<TrapManager> ();
				manager.RaiseTrap ();
			}	
		}
	}
}

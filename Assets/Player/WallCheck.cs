using UnityEngine;
using System.Collections;

public class WallCheck : MonoBehaviour {

	public bool touchingWall = false;
	public PlayerControl masterController;
	// Use this for initialization
	void Start () {
		masterController = GetComponentInParent<PlayerControl> ();
	}
	
	public bool TouchingWall() {
		return touchingWall;
	}

	Collider2D wall = null;

	void OnTriggerEnter2D(Collider2D other) {
		touchingWall = true;
		wall = other;
		if (!masterController.grounded) {
			masterController.grabbingWall = true;
			masterController.SetGravity (false);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		touchingWall = false;
		if (masterController.grabbingWall) {
			masterController.grabbingWall = false;
			masterController.SetGravity (true);
		}
	}
}

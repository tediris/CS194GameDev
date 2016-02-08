using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallCheck : MonoBehaviour {

	public bool touchingWall = false;
	public PlayerControl masterController;
	HashSet<Collider2D> walls;
	// Use this for initialization
	void Start () {
		masterController = GetComponentInParent<PlayerControl> ();
		walls = new HashSet<Collider2D> ();
	}
	
	public bool TouchingWall() {
		return touchingWall;
	}

	Collider2D wall = null;

	void OnTriggerEnter2D(Collider2D other) {
		touchingWall = true;
		wall = other;
		walls.Add (other);
		if (!masterController.grounded) {
			masterController.grabbingWall = true;
			masterController.SetGravity (false);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		walls.Remove (other);
		if (walls.Count > 0)
			return;
		touchingWall = false;
		if (masterController.grabbingWall) {
			masterController.grabbingWall = false;
			masterController.SetGravity (true);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallCheck : MonoBehaviour {

    public bool touchingWall = false;
    public PlayerControl masterController;
    int wallCount = 0;
    // Use this for initialization
    void Start () {
		masterController = GetComponentInParent<PlayerControl> ();
    }

	public bool TouchingWall() {
		return touchingWall;
    }

	public void ResetOnTeleport() {
		wallCount = 0;
		touchingWall = false;
		DetachFromWall();
	}

	void DetachFromWall() {
		if (masterController.grabbingWall) {
			masterController.grabbingWall = false;
			masterController.anim.SetBool ("climbing", false);
			masterController.anim.SetBool ("onWall", false);
			masterController.SetGravity (true);
		}
    }

	void AttachToWall() {
		masterController.grabbingWall = true;
		masterController.SetGravity (false);
		masterController.anim.SetBool ("onWall", true);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Ground") {
			return;
		}
        touchingWall = true;
        wallCount++;
        AttachToWall();
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Ground") {
			return;
		}
        wallCount--;
        if (wallCount > 0)
			return;
		touchingWall = false;
		DetachFromWall();
	}
}

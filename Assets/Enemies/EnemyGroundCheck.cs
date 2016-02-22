using UnityEngine;
using System.Collections;

public class EnemyGroundCheck : MonoBehaviour {

	public bool touchingGround = true;
	public int numTouching = 1;

	void OnTriggerEnter2D(Collider2D col) {
		numTouching++;
		touchingGround = true;
	}

	void OnTriggerExit2D(Collider2D col) {
		numTouching--;
		if (numTouching < 1) {
			touchingGround = false;
			numTouching = 0;
		}
	}
}

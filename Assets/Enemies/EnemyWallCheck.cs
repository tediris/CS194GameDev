using UnityEngine;
using System.Collections;

public class EnemyWallCheck : MonoBehaviour {

	public bool touchingWall = false;
	public int numTouching = 0;

	void OnTriggerEnter2D(Collider2D col) {
		numTouching++;
		touchingWall = true;
	}

	void OnTriggerExit2D(Collider2D col) {
		numTouching--;
		if (numTouching < 1) {
			touchingWall = false;
			numTouching = 0;
		}
	}
}

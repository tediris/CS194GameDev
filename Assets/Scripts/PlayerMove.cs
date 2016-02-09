using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	// Use this for initialization
	public void MoveTo(float x, float y) {
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}

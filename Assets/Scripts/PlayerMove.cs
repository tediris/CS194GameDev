using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	// Use this for initialization
	public void MoveTo(float x, float y) {
		if (isLocalPlayer) {
			transform.position = new Vector3 (x, y, transform.position.z);
		}
	}

	public void MoveByDelta(float deltaX, float deltaY) {
		if (isLocalPlayer) {
			float x = transform.position.x + deltaX;
			float y = transform.position.y + deltaY;
			transform.position = new Vector3 (x, y, transform.position.z);
		}
	}
}

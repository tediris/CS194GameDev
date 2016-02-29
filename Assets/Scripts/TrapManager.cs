using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrapManager : NetworkBehaviour {
	enum TrapState { Ready, Down, Falling, Rising };
	TrapState trapState = TrapState.Ready;
	Rigidbody2D trapBody;
	SpringJoint2D spring;
	Vector2 homePosition;

	// Use this for initialization
	void Start () {
		trapBody = this.transform.GetComponent<Rigidbody2D> ();
		trapState = TrapState.Ready;
		spring = this.transform.GetComponent<SpringJoint2D> ();
		homePosition = trapBody.position;
	}

	// Update is called once per frame
	void Update () {
		if (trapState == TrapState.Ready) {
			spring.distance = 0.1f;
			spring.enabled = true;
		}

		if (trapState == TrapState.Falling || trapState == TrapState.Down) {
			spring.enabled = false;
		}

		if (trapState == TrapState.Rising) {
			if (Vector2.Distance (homePosition, this.trapBody.position) < 1) {
				trapState = TrapState.Ready;
			}
			spring.distance = 0.1f;
			spring.enabled = true;
		}
	}

	public void DropTrap () {
		Debug.Log ("Dropping trap");
		if (trapState == TrapState.Ready) {
			trapState = TrapState.Falling;
		}
	}

	public void RaiseTrap() {
		Debug.Log ("Raising trap");
		trapState = TrapState.Rising;
	}
}

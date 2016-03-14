using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrapManager : NetworkBehaviour {
	public enum TrapState { Ready, Down, Falling, Rising };
	public TrapState trapState = TrapState.Ready;
//	public Vector2 homePosition;
	Rigidbody2D trapBody;
	SpringJoint2D spring;


	// Use this for initialization
	void Start () {
		trapBody = this.transform.GetComponent<Rigidbody2D> ();
		trapState = TrapState.Ready;
		spring = this.transform.GetComponent<SpringJoint2D> ();
		spring.distance = 0.1f;
		spring.enabled = true;
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
			if (trapBody.velocity.magnitude < 1f && Vector2.Distance (spring.connectedBody.transform.TransformPoint(spring.connectedAnchor), trapBody.position) < 1f) {
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

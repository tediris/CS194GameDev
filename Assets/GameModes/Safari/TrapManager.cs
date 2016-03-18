using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TrapManager : NetworkBehaviour {
	
	public enum TrapState { Ready, Down, Falling, Rising };
	public TrapState trapState = TrapState.Ready;
	Rigidbody2D trapBody;
	SpringJoint2D spring;
	Dictionary<GameObject, List<System.Func<TrapManager, bool>>> targets = new Dictionary<GameObject, List<System.Func<TrapManager, bool>>> ();

	// Use this for initialization
	void Start () {
		trapBody = this.transform.GetComponent<Rigidbody2D> ();
		trapState = TrapState.Ready;
		spring = this.transform.GetComponent<SpringJoint2D> ();
		spring.distance = 0.25f;
		spring.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (trapState == TrapState.Ready) {
			spring.distance = 0.25f;
			spring.enabled = true;
			trapBody.constraints = RigidbodyConstraints2D.FreezeAll;
		}

		if (trapState == TrapState.Falling || trapState == TrapState.Down) {
			spring.enabled = false;
			trapBody.constraints = RigidbodyConstraints2D.FreezeAll ^ RigidbodyConstraints2D.FreezePositionY;
		}

//		if (trapState == TrapState.Falling && trapBody.velocity.magnitude < 1e-2f) {
//			trapState = TrapState.Down;
//		}

		if (trapState == TrapState.Rising) {
			trapBody.constraints = RigidbodyConstraints2D.FreezeAll ^ RigidbodyConstraints2D.FreezePositionY;
			spring.distance = 0.25f;
			spring.enabled = true;
			if (trapBody.velocity.magnitude < 1e-2f && Vector2.Distance (spring.connectedBody.transform.TransformPoint(spring.connectedAnchor), trapBody.position) < 1f) {
				trapState = TrapState.Ready;
			}
		}
	}

	public void AddTarget(GameObject target, System.Func<TrapManager, bool> onCapture) {
		if (!targets.ContainsKey (target)) {
			targets.Add (target, new List<System.Func<TrapManager, bool>> ());
		}
		targets [target].Add (onCapture);
	}

	public void DropTrap () {
		if (trapState == TrapState.Ready) {
			Debug.Log ("Dropping trap");
			trapState = TrapState.Falling;
		}
	}

	public void RaiseTrap() {
		if (trapState == TrapState.Down) {
			Debug.Log ("Raising trap");
			trapState = TrapState.Rising;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (targets.ContainsKey(coll.gameObject)) {
			// Collided with target, need to see if target is enclosed
			EdgeCollider2D myCollider = gameObject.GetComponent<EdgeCollider2D> ();
			Collider2D otherCollider = coll.collider;
			if (myCollider.bounds.Contains (otherCollider.bounds.min) 
				&& myCollider.bounds.Contains (otherCollider.bounds.max)) {
				HashSet<System.Func<TrapManager, bool>> toRemove = new HashSet<System.Func<TrapManager, bool>> ();
				foreach (System.Func<TrapManager, bool> responder in targets[coll.gameObject]) {
					bool shouldRemove = responder.Invoke (this);
					if (shouldRemove) {
						toRemove.Add (responder);
					}
				}

				foreach (System.Func<TrapManager, bool> responder in toRemove) {
					targets[coll.gameObject].Remove (responder);
				}
				if (targets [coll.gameObject].Count == 0) {
					targets.Remove (coll.gameObject);
				}
			}
		}

		if (coll.gameObject.tag == "Ground") {
			if (trapState == TrapState.Falling) {
				trapState = TrapState.Down;
			}
		}
	}
}

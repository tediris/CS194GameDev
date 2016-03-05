using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointFollow : MonoBehaviour {

	public List<Rigidbody2D> wayPoints;

	Rigidbody2D body;
	public float speed = 0.3f;
	int currTarget = 0;
	float closeDist = 0.1f;
	public bool enable = false;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}

	float distToWaypoint(int waypointNum) {
		return (body.position - wayPoints [waypointNum].position).magnitude;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!enable) {
			return;
		}
		if (distToWaypoint (currTarget) > closeDist) {
			// set the velocty
		} else {
			currTarget++;
			currTarget = currTarget % wayPoints.Count;
		}
		SetVelocityToWaypoint (currTarget);
	}

	void SetVelocityToWaypoint(int waypointNum) {
		Vector2 target = wayPoints[waypointNum].position;
		Vector2 delta = target - body.position;
		body.velocity = delta.normalized * speed;
	}
}

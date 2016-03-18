using UnityEngine;
using System.Collections;

public class MonsterWander : MonoBehaviour {

	Rigidbody2D body;
	Vector2 startPosition;
	System.Random rand;
	bool wandering = true;
	public float wanderRadius = 2.0f;
	public float speed = 0.3f;
	Vector2 destination;

	// Use this for initialization
	void Start () {
		rand = new System.Random ();
		body = GetComponent<Rigidbody2D> ();
		startPosition = body.position;
	}

	void Enable() {
		wandering = true;
		startPosition = body.position;
	}

	void Disable() {
		wandering = false;
	}

	private float distToDest() {
		return (body.position - destination).magnitude;
	}

	int randomSign() {
		int val = rand.Next (0, 2);
		if (val == 0) {
			return -1;
		} else {
			return 1;
		}
	}

	private void setNewDest() {
		while (true) {
			float xOffset = (float) (rand.NextDouble () * randomSign ());
			float yOffset = (float) (rand.NextDouble () * randomSign ());
			Vector2 offsetVector = new Vector2 (xOffset, yOffset).normalized * wanderRadius;
			Vector2 potentialDestination = offsetVector + startPosition;
			if ((potentialDestination - destination).magnitude > (wanderRadius / 2)) {
				destination = potentialDestination;
				body.velocity = (destination - body.position).normalized * speed;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (wandering) {
			if (distToDest () < 0.2f) {
				setNewDest ();
			}
		}
	}
}

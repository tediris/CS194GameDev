using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FlyingEnemy : NetworkBehaviour {

	public float travelRadius = 3f;
	public float speed = 0.1f;

	Vector2 startPosition;
	public Vector2 currPosition;
	public Vector2 destination;

	Rigidbody2D body;

	System.Random rand;

	int randomSign() {
		int val = rand.Next (0, 2);
		if (val == 0) {
			return -1;
		} else {
			return 1;
		}
	}

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		startPosition = new Vector2 (body.position.x, body.position.y);
		destination = new Vector2(startPosition.x, startPosition.y);
		rand = new System.Random (System.DateTime.Now.GetHashCode());
	}

	private float distToDest() {
		return (body.position - destination).magnitude;
	}

	private void setNewDest() {
		while (true) {
			float xOffset = (float) (rand.NextDouble () * randomSign ());
			float yOffset = (float) (rand.NextDouble () * randomSign ());
			Vector2 offsetVector = new Vector2 (xOffset, yOffset).normalized * travelRadius;
			Vector2 potentialDestination = offsetVector + startPosition;
			if ((potentialDestination - destination).magnitude > (travelRadius / 2)) {
				destination = potentialDestination;
				body.velocity = (destination - body.position).normalized * speed;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currPosition = body.position;
		if (distToDest () < 0.2f) {
			setNewDest ();
		}
	}
}

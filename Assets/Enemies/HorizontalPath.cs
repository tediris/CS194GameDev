using UnityEngine;
using System.Collections;

public class HorizontalPath : MonoBehaviour {

	Rigidbody2D body;
	Vector2 startPos;
	bool movingUp = true;
	public float travelRadius = 2.0f;
	public float speed = 0.3f;
	System.Random rand;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		startPos = body.position;
		rand = new System.Random (System.DateTime.Now.GetHashCode ());
		movingUp = (rand.Next(2) == 0);
		speed += (float) rand.NextDouble () * speed;
	}

	float distToPoint(Vector2 dest) {
		return (body.position - dest).magnitude;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (movingUp) {
			Vector2 dest = startPos + Vector2.right * travelRadius;
			if (distToPoint (dest) < 0.05) {
				movingUp = false;
			} else {
				body.velocity = (dest - body.position).normalized * speed;
			}
		} else {
			Vector2 dest = startPos + Vector2.left * travelRadius;
			if (distToPoint (dest) < 0.05) {
				movingUp = true;
			} else {
				body.velocity = (dest - body.position).normalized * speed;
			}
		}
	}
}

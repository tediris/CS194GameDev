using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	Rigidbody2D rigidBody;
	Vector2 velocity;
	Collider2D aaronSucks;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		velocity = new Vector2 (1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		rigidBody.velocity = velocity;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Bounds otherBounds = other.collider.bounds;
			float otherBottom = (otherBounds.center + otherBounds.extents) [1];
			Bounds thisBounds = (GetComponent<CircleCollider2D> ()).bounds;
			float thisTop = (thisBounds.center + thisBounds.extents) [1];

			if (otherBottom - thisTop < 0.0f) {
				// Player on top
				Destroy(this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag != "Player") {
			float otherTop = (other.bounds.center + other.bounds.extents) [1];
			Bounds thisBounds = (GetComponent<CircleCollider2D> ()).bounds;
			float thisBottom = (thisBounds.center - thisBounds.extents) [1];
			if (thisBottom - otherTop < 0.0) {
				velocity.x = -velocity.x;
			}
		}
	}
}

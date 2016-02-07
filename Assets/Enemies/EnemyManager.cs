using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public List<string> playerTags;
	public List<string> groundTags;
	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = new Vector2 (1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Bounds thisBounds = (GetComponent<CircleCollider2D> ()).bounds;
		if (playerTags.Contains(collision.gameObject.tag)) {
			Vector2 vector = new Vector2 (0, -1);
			foreach (ContactPoint2D contact in collision.contacts) {
				if (Vector2.Dot (vector, contact.normal) > 0 && contact.point.y > thisBounds.center.y) {
					Destroy(this.gameObject);
					break;
				}
			}
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (!(playerTags.Contains(other.gameObject.tag))) {
			float otherTop = (other.bounds.center + other.bounds.extents).y;
			Bounds thisBounds = (GetComponent<CircleCollider2D> ()).bounds;
			float thisBottom = (thisBounds.center - thisBounds.extents).y;
			if (thisBottom - otherTop < 0.0) {
				rigidBody.velocity.Set (-rigidBody.velocity.x, rigidBody.velocity.y);
			}
		}
	}
}

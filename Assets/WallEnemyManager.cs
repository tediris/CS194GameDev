using UnityEngine;
using System.Collections;

public class WallEnemyManager : MonoBehaviour {

	Vector2 velocity;
	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		velocity = new Vector2 (0.0f, -0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		rigidBody.velocity = velocity;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		velocity.y = -velocity.y;
	}
}

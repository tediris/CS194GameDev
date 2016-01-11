using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformControl : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public float jumpSpeed = 5.0f;
	public List<string> groundTags;
	[HideInInspector] public bool facingRight = true;

	public float distToBottom;

	public bool grounded;

	Rigidbody2D playerBody;

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		float h = Input.GetAxis ("Horizontal");
		float yVelocity = playerBody.velocity.y;

		if (grounded && Input.GetKeyDown (KeyCode.W)) {
			yVelocity = jumpSpeed;
			grounded = false;
		}

		playerBody.velocity = new Vector2 (h*maxSpeed, yVelocity);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Collider2D collider = collision.collider;
		foreach (string validTag in groundTags) {
			if (collider.tag == validTag) {
				if (transform.position.y > collider.transform.position.y)
					grounded = true;
			}
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

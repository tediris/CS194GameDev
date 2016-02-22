using UnityEngine;
using System.Collections;

public class GroundEnemy : MonoBehaviour {

	public EnemyGroundCheck groundCheck;
	public EnemyWallCheck wallCheck;
	public float speed;
	private Rigidbody2D body;
	float lowpass = 0.3f;
	float timer = 0f;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		body.velocity = new Vector2 (speed, body.velocity.y);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		if (timer > lowpass) {
			if (!groundCheck.touchingGround) {
				// change direction
				body.velocity = new Vector2 (-1 * body.velocity.x, body.velocity.y);
				timer = 0f;

			} else if (wallCheck.touchingWall) {
				// change direction
				body.velocity = new Vector2 (-1 * body.velocity.x, body.velocity.y);
				timer = 0f;
			}
		}
	}
}

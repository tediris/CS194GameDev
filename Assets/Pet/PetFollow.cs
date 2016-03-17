using UnityEngine;
using System.Collections;

public class PetFollow : MonoBehaviour {

	public Rigidbody2D playerTarget;
	public float followDistance;
	public float maxDistance;
	public float moveSpeed = 2.0f;
	public float jumpThreshold = 50f;
	public float lerpSpeed = 0.05f;

	public bool followingPlayer = true;

	private Vector2 targetPosition;

	private Rigidbody2D petBody;

	// Use this for initialization
	void Start () {
		petBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!followingPlayer)
			return;

		if (playerTarget == null)
			return;

		if ((playerTarget.position - petBody.position).magnitude > jumpThreshold) {
			transform.position = playerTarget.position;
			return;
		}

		targetPosition = playerTarget.position + new Vector2 (followDistance, followDistance);
		if (Vector2.Distance (petBody.position, targetPosition) > maxDistance) {
			petBody.velocity = Vector2.Lerp(petBody.velocity, (targetPosition - petBody.position).normalized * moveSpeed, lerpSpeed);
		} else {
			petBody.velocity = Vector2.Lerp(petBody.velocity, Vector2.zero, lerpSpeed);
		}

		if (petBody.velocity.x < -0.01f) {
			transform.localScale = new Vector2 (-1f, 1f);
		} else if (petBody.velocity.x > 0.01f) {
			transform.localScale = new Vector2 (1f, 1f);
		}

	}
}

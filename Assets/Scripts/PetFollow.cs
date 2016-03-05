using UnityEngine;
using System.Collections;

public class PetFollow : MonoBehaviour {

	public Rigidbody2D playerTarget;
	public float followDistance;
	public float maxDistance;
	public float lerpSpeed = 0.1f;

	public bool followingPlayer = true;

	private Vector2 targetPosition;
	private Vector2 curVelocity;

	private Rigidbody2D petBody;

	// Use this for initialization
	void Start () {
		petBody = GetComponent<Rigidbody2D> ();
		curVelocity = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (!followingPlayer)
			return;

		if (playerTarget == null)
			return;
		
		targetPosition = playerTarget.position + new Vector2 (followDistance, followDistance);
		if (Vector2.Distance (petBody.position, targetPosition) > maxDistance) {
			curVelocity = Vector2.Lerp (curVelocity, targetPosition - petBody.position, lerpSpeed);
		} else {
			curVelocity = Vector2.Lerp (curVelocity, Vector2.zero, lerpSpeed);
		}
		petBody.velocity = curVelocity;

		if (curVelocity.x < 0f) {
			transform.localScale = new Vector2 (-1f, 1f);
		} else {
			transform.localScale = new Vector2 (1f, 1f);
		}

	}
}

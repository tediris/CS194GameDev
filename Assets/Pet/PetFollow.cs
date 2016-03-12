using UnityEngine;
using System.Collections;

public class PetFollow : MonoBehaviour {

	public Rigidbody2D playerTarget;
	public float followDistance;
	public float maxDistance;
	public float moveSpeed = 2.0f;

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
		
		targetPosition = playerTarget.position + new Vector2 (followDistance, followDistance);
		if (Vector2.Distance (petBody.position, targetPosition) > maxDistance) {
			petBody.velocity = (targetPosition - petBody.position).normalized * moveSpeed;;
		} else {
			petBody.velocity = Vector2.zero;
		}

		if (petBody.velocity.x < 0f) {
			transform.localScale = new Vector2 (-1f, 1f);
		} else {
			transform.localScale = new Vector2 (1f, 1f);
		}

	}
}

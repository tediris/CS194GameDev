using UnityEngine;
using System.Collections;

public class TopDownControl : MonoBehaviour {

	public float maxSpeed = 5.0f;

	Rigidbody2D playerBody;

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		playerBody.velocity = new Vector2 (h, v) * maxSpeed;
		if (playerBody.velocity.magnitude > maxSpeed) {
			playerBody.velocity = playerBody.velocity.normalized * maxSpeed;
		}
	}
}

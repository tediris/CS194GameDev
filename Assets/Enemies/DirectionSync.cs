using UnityEngine;
using System.Collections;

public class DirectionSync : MonoBehaviour {

	Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 localScale = gameObject.transform.localScale;
		if (body.velocity.x < 0) {
			gameObject.transform.localScale = new Vector3 (Mathf.Abs(localScale.x), localScale.y, localScale.z);
		} else {
			gameObject.transform.localScale = new Vector3 (-1 * Mathf.Abs(localScale.x), localScale.y, localScale.z);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaitFinder : MonoBehaviour {

	const string BAIT_TAG = "Bait";
	HashSet<GameObject> activeBaits;
	Rigidbody2D body;
	GameObject pursuedBait;

	// Use this for initialization
	void Start () {
		activeBaits = new HashSet<GameObject> ();
		body = transform.parent.GetComponent<Rigidbody2D> ();
		StartCoroutine ("UpdatePursuedBait");
	}

	IEnumerator UpdatePursuedBait() {
		for (;;) {
			HashSet<GameObject> baitsToRemove = new HashSet<GameObject> ();
			float totalDistance = 0f;
			foreach (GameObject bait in activeBaits) {
				float distance = Vector2.Distance (bait.transform.position, body.position);
				totalDistance += distance;
			}

			float totalProbabilities = 20f; // Initiailizing to 4 is a fudge factor to give the bait finder a high prior of not moving towards a bait
			foreach (GameObject bait in activeBaits) {
				float distance = Vector2.Distance (bait.transform.position, body.position);
				totalProbabilities += Mathf.Exp (Mathf.Pow(distance, -0.5f));
			}
			Debug.Log ("Total probability " + totalProbabilities);

			pursuedBait = null;
			float desiredProbability = Random.value;
			float currentProbability = 0f;
			foreach (GameObject bait in activeBaits) {
				float distance = Vector2.Distance (bait.transform.position, body.position);
				float probability = Mathf.Exp (Mathf.Pow(distance, -0.5f)) / totalProbabilities;
				currentProbability += probability;
				if (currentProbability > desiredProbability) {
					pursuedBait = bait;
					Debug.Log ("Moving to bait");
					break;
				}
			}

			yield return new WaitForSeconds (0.5f);
		}
	}

	// Update is called once per frame
	void Update () {
		if (pursuedBait) {
			Rigidbody2D baitBody = pursuedBait.GetComponent<Rigidbody2D> ();
			Vector2 direction = baitBody.position - body.position;
			body.velocity = Vector2.ClampMagnitude (direction, 0.5f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == BAIT_TAG) {
			activeBaits.Add (other.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == BAIT_TAG) {
			activeBaits.Remove (coll.gameObject);
			Destroy (coll.gameObject);
		}
	}
}

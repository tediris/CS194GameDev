using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EggTracker : NetworkBehaviour {

	public Rigidbody2D egg;
	public float spinRadius = 1.0f;
	public float speed = 0.3f;
	public bool chase = false;
	Rigidbody2D body;
	Vector2 targetPos = Vector2.zero;
	System.Random rand;
	Vector2 eggStartPosition;
	public MonoBehaviour controlScript;
	bool coroutineStarted = false;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		rand = new System.Random (System.DateTime.Now.GetHashCode());
		targetPos = body.position;
		if (!isServer) {
			this.enabled = false;
		}
	}

	void FindEgg() {
		GameObject eggObj = GameObject.Find ("Egg_Capture");
		if (eggObj != null) { 
			egg = eggObj.GetComponent<Rigidbody2D> ();
			eggStartPosition = egg.position;
		}
	}

	IEnumerator waitForEggToMove() {
		while ((eggStartPosition - egg.position).magnitude < 0.1f) {
			yield return new WaitForSeconds (1.0f);
		}
		Debug.Log ("EGG HAS BEEN STOLEN");
		chase = true;
		controlScript.enabled = false;
	}

	int randomSign() {
		int val = rand.Next (0, 2);
		if (val == 0) {
			return -1;
		} else {
			return 1;
		}
	}

	float distToTarget(Rigidbody2D target) {
		return (body.position - target.position).magnitude;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (egg == null) {
			FindEgg ();
			return;
		} else if (!chase) {
			if (!coroutineStarted) {
				StartCoroutine (waitForEggToMove ());
				coroutineStarted = true;
			}
			return;
		}
		if (distToTarget (egg) > 2 * spinRadius) {
			// move towards the egg
			Vector2 movement = (egg.position - body.position).normalized * speed;
			body.velocity = movement;
		} else {
			// randomly pick a point on the edge
			if ((body.position - targetPos).magnitude < 0.1f) {
				setNewDest ();
			} else if (targetPos == Vector2.zero) {
				setNewDest ();
			}
		}
	}

	private void setNewDest() {
		while (true) {
			float xOffset = (float) (rand.NextDouble () * randomSign ());
			float yOffset = (float) (rand.NextDouble () * randomSign ());
			Vector2 offsetVector = new Vector2 (xOffset, yOffset).normalized * spinRadius;
			Vector2 potentialDestination = offsetVector + egg.position;
			if ((potentialDestination - targetPos).magnitude > (spinRadius / 4)) {
				targetPos = potentialDestination;
				body.velocity = (targetPos - body.position).normalized * speed;
				break;
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Carryable : NetworkBehaviour {

	[SyncVar]
	public bool carried = false;
	public float carryHeight = 0.3f;
	Transform toFollow = null;
	public float throwSpeedHorizontal = 2.0f;
	public float throwSpeedVertical = 3.0f;
	Rigidbody2D body= null;

	// Use this for initialization
	void Start () {
		if (!isServer) {
			this.enabled = false;
		}
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (carried) {
			gameObject.transform.position = toFollow.position + Vector3.up * carryHeight;
		}
	}

	public void SetCarrying(int playerNum) {
		CmdSetCarrying (playerNum);	
	}

	public void UnsetCarry() {
		CmdUnsetCarry ();
	}

	[Command]
	void CmdUnsetCarry() {
		body.velocity = new Vector2 (throwSpeedHorizontal * Mathf.Sign(toFollow.localScale.x), throwSpeedVertical);
		carried = false;
		body.isKinematic = false;
		toFollow = null;
	}

	[Command]
	void CmdSetCarrying(int playerNum) {
		GameObject player = GameObject.Find ("Player" + playerNum);
		carried = true;
		body.isKinematic = true;
		toFollow = player.transform;
	}
}

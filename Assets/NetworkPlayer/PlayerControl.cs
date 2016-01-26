using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	public float maxSpeed = 5.0f;
	public float jumpSpeed = 20.0f;
	public List<string> groundTags;
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool grounded = true;
	private bool wallJumped = false; // Indicate whether or not we have already walljumped since last being grounded
	private Collider2D currentPlatformCollider; // Platform that we are currently colliding with

	public float distToBottom;

	private bool carrying = false;
	int carryingPlayer = -1;

	Rigidbody2D playerBody;

	NetSetup networkInfo;
	GameObject playerManager;
	CarryManager carryManager;

	CarryControl carryControl;

	bool canMoveHorizontally() {
		return !carryControl.carried;
	}

	bool canJump() {
		return !carrying && grounded || (!grounded && !wallJumped && currentPlatformCollider);
	}

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		networkInfo = GetComponent<NetSetup> ();
		carryControl = GetComponent<CarryControl> ();
	}

	void FindPlayerManager() {
		while (carryManager == null) {
			carryManager = GameObject.Find ("PlayerManager").GetComponent<CarryManager> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (currentPlatformCollider && !playerBody.IsTouching (currentPlatformCollider)) {
			currentPlatformCollider = null;
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			if (!carrying) {
				CarryPlayer ();
			} else {
				ThrowPlayer ();
			}
		}

		float h = Input.GetAxis ("Horizontal");

		if (canJump() && Input.GetKeyDown (KeyCode.W)) {
			playerBody.velocity =  new Vector2 (playerBody.velocity.x, jumpSpeed);
			if (!grounded) {
				wallJumped = true;
			} else {
				grounded = false;
			}
		}

		if (canMoveHorizontally() && Mathf.Abs(h) > 0) {
			playerBody.velocity = new Vector2 (h * maxSpeed, playerBody.velocity.y);
		}

		if (h > 0 && !facingRight) {
			Flip ();
		} else if (h < 0 && facingRight) {
			Flip ();
		}
	}

	void ThrowPlayer() {
		CmdThrowPlayer (carryingPlayer, transform.localScale.x);
		carrying = false;
		carryingPlayer = -1;
	}

	[Command]
	void CmdThrowPlayer(int playerNum, float direction) {
		Debug.Log ("Throwing player " + playerNum);
		GameObject carried = GameObject.Find("Player" + playerNum);
		carried.GetComponent<ColorControl> ().CmdSetColor (Color.white);
		carried.GetComponent<CarryControl> ().CmdUnsetCarry (direction);
	}

	void CarryPlayer() {
		float spriteOffset = 0.5f;
		float pickupDist = 1.0f;
		Vector2 start = playerBody.position + transform.localScale.x * Vector2.right * spriteOffset;
		Vector2 dir = transform.localScale.x * Vector2.right * pickupDist;
		Debug.DrawRay (start, dir, Color.blue, 1f);
		RaycastHit2D hit = Physics2D.Raycast (start, dir, pickupDist);
		if (hit.collider != null && hit.collider.tag == "Player") {
			carrying = true;
			carryingPlayer = hit.collider.gameObject.GetComponent<NetSetup> ().playerNum;
			CmdInitiatePickup (networkInfo.playerNum, carryingPlayer);
		}
	}

	[Command]
	void CmdInitiatePickup(int carryPlayer, int playerNum) {
		Debug.Log ("Player " + carryPlayer + " is picking up player " + playerNum);
		GameObject carried = GameObject.Find("Player" + playerNum);
		carried.GetComponent<ColorControl> ().CmdSetColor (Color.green);
		carried.GetComponent<CarryControl> ().CmdSetCarry(carryPlayer);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Collider2D collider = collision.collider;
		foreach (string validTag in groundTags) {
			if (collider.tag == validTag) {
				currentPlatformCollider = collider;

				// Check if all contacts are between -45 and 45 degs to normal of top of collider
				// If it is, then we consider ourselves grounded
				Vector2 rightVector = new Vector2 (1, 1);
				Vector2 leftVector = new Vector2 (-1, 1);
				foreach (ContactPoint2D contact in collision.contacts) {
					if (Vector2.Dot (rightVector, contact.normal) > 0 && Vector2.Dot (leftVector, contact.normal) > 0) {
						grounded = true;
						wallJumped = false;
						break;
					}
				}
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
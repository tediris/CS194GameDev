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
	Animator anim;

	private float dirInput = 0.0f;
	private float buttonValue = 0.0f;
	[Range(0f, 1f)]
	public float responsiveness = 0.5f;

	private float airSpeed = 0.0f;
	public float maxAirSpeed = 3.0f;
	public float airControl = 0.05f;
	public float climbSpeed = 1.0f;
	private float gravityInitial = 0.0f;

	private WallCheck wallCheck;

	bool canMoveHorizontally() {
		return !carryControl.carried;
	}

	bool canJump() {
		return !carrying && grounded || (!grounded /*&& !wallJumped*/ && currentPlatformCollider);
	}

	public void SetGravity(bool enable) {
		if (enable) {
			playerBody.gravityScale = gravityInitial;
		} else {
			playerBody.gravityScale = 0f;
		}
	}

	void UpdateLRMovement() {
		buttonValue = 0.0f;
		if (Input.GetKey (KeyCode.A)) {
			buttonValue -= 1.0f;
		}
		if (Input.GetKey(KeyCode.D)) {
			buttonValue += 1.0f;	
		}
		dirInput = Mathf.Lerp (dirInput, buttonValue, responsiveness);
	}

	bool moveButtonDown() {
		return Mathf.Abs (dirInput) > 0f;
	}

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		networkInfo = GetComponent<NetSetup> ();
		carryControl = GetComponent<CarryControl> ();
		anim = GetComponent<Animator> ();
		gravityInitial = playerBody.gravityScale;
		wallCheck = GetComponentInChildren<WallCheck> ();
	}

	void FindPlayerManager() {
		while (carryManager == null) {
			carryManager = GameObject.Find ("PlayerManager").GetComponent<CarryManager> ();
		}
	}

	public bool grabbingWall = false;

	void FixedUpdate() {
		if (!grounded) {
			if (wallCheck.touchingWall) {
				if (grabbingWall) {
					playerBody.velocity = Vector2.zero;
					if (Input.GetKey(KeyCode.W)) {
						playerBody.velocity += Vector2.up * climbSpeed;
					}
					if (Input.GetKey(KeyCode.S)) {
						playerBody.velocity -= Vector2.up * climbSpeed;
					}

				}
			} else {
				if (Mathf.Abs (buttonValue) < float.Epsilon) {
					airSpeed = Mathf.Lerp (airSpeed, 0f, 0.1f);
				} else {
					airSpeed = playerBody.velocity.x;
					airSpeed += dirInput * airControl;
				}
				airSpeed = Mathf.Clamp (airSpeed, -maxAirSpeed, maxAirSpeed);
				playerBody.velocity = new Vector2 (airSpeed, playerBody.velocity.y);
			}
		}
			
	}

	// Update is called once per frame
	void Update () {
		UpdateLRMovement ();

		if (currentPlatformCollider && (!playerBody.IsTouching (currentPlatformCollider) || grounded)) {
			currentPlatformCollider = null;
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			if (!carrying) {
				CarryPlayer ();
			} else {
				ThrowPlayer ();
			}
		}

		if (grabbingWall && Input.GetKeyDown(KeyCode.X)) {
			grabbingWall = false;
			SetGravity (true);
		}

		float h = Input.GetAxis ("Horizontal");

		if (canJump() && (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.U))) {
			playerBody.velocity =  new Vector2 (playerBody.velocity.x, jumpSpeed);
			airSpeed = playerBody.velocity.x;
			if (!grounded) {
				wallJumped = true;
			} else {
				grounded = false;
			}
		}

		if (canMoveHorizontally ()) {
			if (grounded) {
				playerBody.velocity = new Vector2 (dirInput * maxSpeed, playerBody.velocity.y);
				if (Mathf.Abs (dirInput) > 0.5f) {
					anim.SetBool ("standing", false);
				} else {
					anim.SetBool ("standing", true);
				}
			}
		} 

		if (Mathf.Abs(playerBody.velocity.y) > 0.1f) {
			anim.SetBool ("grounded", false);
			if (playerBody.velocity.y > 0) {
				anim.SetBool ("rising", true);
			} else {
				anim.SetBool ("rising", false);
			}
		} else {
			anim.SetBool ("grounded", true);
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
		float spriteOffset = 0.2f;
		float pickupDist = 0.5f;
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

	private Collision2D wallCollision = null;

	void OnCollisionEnter2D(Collision2D collision) {
		foreach (string validTag in groundTags) {
			if (collision.gameObject.tag == validTag) {
				currentPlatformCollider = collision.collider;

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

	[Command] 
	void CmdFlipClients() {
		RpcFlipClients ();
	}

	[ClientRpc]
	void RpcFlipClients() {
		if (isLocalPlayer)
			return;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		CmdFlipClients ();
	}
}
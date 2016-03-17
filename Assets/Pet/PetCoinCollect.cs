using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetCoinCollect : PetAction {

	HashSet<GameObject> coins;
	GameObject curTarget = null;
	Rigidbody2D targetBody = null;

	PetFollow followScript;
	Rigidbody2D petBody;
	public CollectingRadius radius;

	public float coinDist = 0.2f;
	public float seekDuration = 20f;
	public float cooldownDuration = 60f;
	public float moveSpeed = 2.0f;
	public GameObject player;
	bool seeking = false;
	bool cooldown = false;

	Vector2 curVelocity;

	PlayerNotification activationStatus;

	// Use this for initialization
	void Start () {
		curVelocity = Vector2.zero;
		petBody = GetComponent<Rigidbody2D> ();
		coins = new HashSet<GameObject> ();
		followScript = GetComponent<PetFollow> ();

//
//		StartCoroutine (SeekInFifteen());
	}

	// Update is called once per frame
	void Update () {
		if (seeking && player != null) {
			if (!(coins.Count == 0)) {
				followScript.followingPlayer = false;
				if (curTarget == null) {
					curTarget = GetTarget ();
					if (curTarget == null) {
						coins.Clear ();
					} else {
						targetBody = curTarget.GetComponent<Rigidbody2D> ();
					}
				} else {
					if (radius.Contact()) {
						curTarget.GetComponent<CoinPickup> ().CollectCoin (player);
						curTarget = null;
						targetBody = null;
					} else {
						petBody.velocity = (targetBody.position - petBody.position).normalized * moveSpeed;
					}
				}
			} else {
				followScript.followingPlayer = true;
			}
		}
	}

	GameObject GetTarget() {
		IEnumerator setEnum = coins.GetEnumerator ();
		setEnum.MoveNext ();
		while (setEnum.Current == null) {
			if (!setEnum.MoveNext ())
				break;
		}
		return (GameObject)setEnum.Current;
	}

	public override void Activate() {
		if (!cooldown) {
			SeekCoins ();
		} else {
			activationStatus.SetPlayerTimedNotification ("Pet ability is on cooldown.", Color.white, 3.0f);
		}
	}

	public override void Setup(GameObject player) {
		this.player = player;
		activationStatus = player.GetComponent<PlayerNotification> ();
	}

	public void SeekCoins() {
		Debug.Log ("STARTING TO SEEK COINS");
		followScript.followingPlayer = false;
		seeking = true;
		cooldown = true;
		StartCoroutine (SeekTimer ());
		StartCoroutine (CooldownTimer ());
	}

	void StopSeeking() {
		Debug.Log ("STOPPING SEEK");
		curTarget = null;
		targetBody = null;
		seeking = false;
		followScript.followingPlayer = true;
	}

	IEnumerator SeekTimer() {
		yield return new WaitForSeconds (seekDuration);
		StopSeeking ();
	}

	IEnumerator CooldownTimer() {
		yield return new WaitForSeconds (cooldownDuration);
		cooldown = false;
		activationStatus.SetPlayerTimedNotification ("Pet ability ready!", Color.white, 3.0f);
	}

	IEnumerator SeekInFifteen() {
		yield return new WaitForSeconds (15f);
		if (player == null) {
			StartCoroutine (SeekInFifteen ());
		} else {
			SeekCoins ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isServer) {
			if (other.tag == "Coin") {
				coins.Add (other.gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (isServer) {
			if (other.tag == "Coin") {
				if (other.gameObject == curTarget) {
					curTarget = null;
				}
				coins.Remove (other.gameObject);
			}
		}
	}
}

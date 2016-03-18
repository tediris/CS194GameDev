using UnityEngine;
using System.Collections;

public class PetJumpBoost : PetAction {

	public float boostDuration = 20f;
	public float cooldownDuration = 60f;

	bool cooldown = false;

	public GameObject player;
	PlayerControl playerControl;

	float originalJump, originalMoveSpeed;
	public float boostMultiplier = 1.5f;

	float originalFollowDist;
	float originalMaxDist;
	PetFollow follow;

	PlayerNotification activationStatus;

	// Use this for initialization
	void Start () {
		follow = GetComponent<PetFollow> ();
		originalFollowDist = follow.followDistance;
		originalMaxDist = follow.maxDistance;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void BoostJump() {
		playerControl.jumpSpeed = playerControl.jumpSpeed * boostMultiplier;
		playerControl.airControl = playerControl.airControl * boostMultiplier;
		follow.followDistance = 0f;
		follow.maxDistance = 0f;
		cooldown = true;

		StartCoroutine (BoostTimer ());
		StartCoroutine (CooldownTimer ());
	}

	void StopBoosting() {
		playerControl.jumpSpeed = originalJump;
		playerControl.airControl = originalMoveSpeed;
		follow.followDistance = originalFollowDist;
		follow.maxDistance = originalMaxDist;
	}

	public override void Activate() {
		if (!cooldown) {
			activationStatus.SetPlayerTimedNotification ("Pet ability activated!", Color.white, 3.0f);
			BoostJump ();
		} else {
			activationStatus.SetPlayerTimedNotification ("Pet ability is on cooldown.", Color.white, 3.0f);
		}
	}

	public override void Setup(GameObject player) {
		this.player = player;
		playerControl = player.GetComponent<PlayerControl> ();
		originalJump = playerControl.jumpSpeed;
		originalMoveSpeed = playerControl.airControl;
		activationStatus = player.GetComponent<PlayerNotification> ();
	}

	IEnumerator BoostTimer() {
		yield return new WaitForSeconds (boostDuration);
		StopBoosting ();
	}

	IEnumerator CooldownTimer() {
		yield return new WaitForSeconds (cooldownDuration);
		cooldown = false;
		activationStatus.SetPlayerTimedNotification ("Pet ability ready!", Color.white, 3.0f);
	}
}

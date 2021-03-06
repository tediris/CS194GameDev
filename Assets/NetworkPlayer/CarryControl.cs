﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CarryControl : NetworkBehaviour {

	public bool carried = false;
	Rigidbody2D carrier = null;
	public float throwSpeed = 7.5f;
	public float carryHeight = 0.3f;

	Rigidbody2D body;
	float standardGrav;
	PlayerControl playerController;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		playerController = GetComponent<PlayerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		if (carried && carrier != null)
			body.MovePosition (carrier.position + Vector2.up * carryHeight);
	}

	[ClientRpc]
	void RpcSetCarry(int carryPlayer) {
		carrier = GameObject.Find("Player" + carryPlayer).GetComponent<Rigidbody2D>();
		carried = true;
		body.isKinematic = true;
	}

	[Command]
	public void CmdSetCarry(int carryPlayer) {
		body.isKinematic = true;
		RpcSetCarry (carryPlayer);
	}

	[ClientRpc]
	void RpcUnsetCarry(float direction) {
		carried = false;
		carrier = null;
		body.isKinematic = false;
		if (isLocalPlayer) {
			body.velocity = new Vector2 (direction * throwSpeed, throwSpeed);
			playerController.grounded = false;
			playerController.SetAirSpeed(body.velocity.x);

		}
	}

	[Command]
	public void CmdUnsetCarry(float direction) {
		body.isKinematic = false;
		RpcUnsetCarry (direction);
	}
}

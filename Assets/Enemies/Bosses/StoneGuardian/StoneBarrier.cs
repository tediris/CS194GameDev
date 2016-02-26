using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StoneBarrier : NetworkBehaviour {

	public List<SpriteRenderer> sprites;
	BoxCollider2D coll;

	// Use this for initialization
	void Start () {
		coll = GetComponent<BoxCollider2D> ();
	}

	[Command]
	public void CmdEnable() {
		RpcEnableBarrier ();
	}

	[Command]
	public void CmdDisable() {
		RpcDisableBarrier ();
	}

	[ClientRpc]
	void RpcDisableBarrier() {
		DisableSprites ();
		coll.enabled = false;
	}

	[ClientRpc]
	void RpcEnableBarrier() {
		EnableSprites ();
		coll.enabled = true;
	}

	void EnableSprites() {
		foreach (SpriteRenderer sp in sprites) {
			sp.enabled = true;
		}
	}

	void DisableSprites() {
		foreach (SpriteRenderer sp in sprites) {
			sp.enabled = false;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EggCarry : DeathListener {

	[SerializeField]
	public GameObject playerObj;
	Transform playerBody;
	//Trans body;
	//[SyncVar]
	public bool carried = false;
	float offset = 0.3f;
	public Vector2 startRoom = new Vector2 (3.2f, 98.4f);

	public override void PlayerDied(GameObject player) {
		CmdDrop ();
	}

	// Use this for initialization
	void Start () {
		//body = GetComponent<Rigidbody2D> ();
		transform.parent = null; // detach the egg from the room prefab
//		if (isServer) {
//			NetworkServer.Spawn (this.gameObject);
//		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!isServer)
			return;
		// check to see if the egg was retrieved
		if (carried)
			return;
			
		if (other.tag == "Player") {
			if (other.gameObject.GetComponent<PlayerHealth> ().alive) {
				Debug.Log (other.gameObject.name + " is picking up the object");
				CmdSetCarried (other.gameObject.name);
			}
		}
	}

	[Command]
	void CmdSetCarried(string playerName) {
		carried = true;
		PlayerHealth pH = GameObject.Find (playerName).GetComponent<PlayerHealth>();
		pH.addDeathListener (this);
		RpcSetCarried (playerName);
		// set the death listener to drop the egg
	}

	[ClientRpc]
	void RpcSetCarried(string playerName) {
		Debug.Log ("RPC Set carried called,  PLAYER: " + playerName);
		playerObj = GameObject.Find (playerName);
		playerBody = playerObj.transform;
		carried = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (carried) {
			if (playerObj != null) {
				Vector3 targetPos = playerObj.transform.position + Vector3.up * offset;
				gameObject.transform.position = new Vector3 (targetPos.x, targetPos.y, transform.position.z);
			}
		}
	}

	[Command]
	public void CmdDrop() {
		this.gameObject.transform.position = playerObj.transform.position;
		playerObj = null;
		playerBody = null;
		carried = false;
	}

	[ClientRpc]
	void RpcDrop() {
		this.gameObject.transform.position = playerObj.transform.position;
		playerObj = null;
		playerBody = null;
		carried = false;
	}
}

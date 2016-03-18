using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetSetup : NetworkBehaviour {

	[SyncVar]
	public int playerNum = -1;

	public List<RuntimeAnimatorController> animators;
	public List<RuntimeAnimatorController> deathAnimatorz;

	private PlayerIDs idStore;

	// Use this for initialization
	void Start () {
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();


		//Physics2D.Ig
		if (isServer) {
			GameObject.Find ("PlayerManager").GetComponent<CarryManager> ().AddPlayer (gameObject);
		}

		if (isLocalPlayer) {
			GetComponent<PlayerControl> ().enabled = true;
			GetComponent<ItemManager> ().enabled = true;
			GetComponentInChildren<WallCheck> ().enabled = true;
			GameObject.Find ("Main Camera").GetComponent<UnityStandardAssets._2D.Camera2DFollow> ().SetTarget (transform);
			//GameObject.Find ("MinimapCamera").GetComponent<UnityStandardAssets._2D.Camera2DFollow> ().SetTarget(transform);
		} else {
			Destroy (GetComponentInChildren<WallCheck> ().gameObject);
		}
		StartCoroutine (SetName ());
	}

	IEnumerator SetName() {
		while (playerNum < 0) {
			yield return new WaitForSeconds (0.1f);
		}
		gameObject.name = "Player" + playerNum;

		if (isLocalPlayer) {
			idStore.localID = gameObject.name;
		}
			
		GetComponent<Animator> ().runtimeAnimatorController = animators [playerNum];
		GameObject ghostlyObj = transform.FindChild ("Ghost").gameObject;
		ghostlyObj.GetComponent<Animator> ().runtimeAnimatorController = deathAnimatorz [playerNum];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

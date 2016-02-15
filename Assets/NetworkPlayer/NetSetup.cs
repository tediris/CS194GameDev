using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetSetup : NetworkBehaviour {

	[SyncVar]
	public int playerNum = -1;

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
			GameObject.Find ("Main Camera").GetComponent<UnityStandardAssets._2D.Camera2DFollow> ().SetTarget(transform);
			//GameObject.Find ("MinimapCamera").GetComponent<UnityStandardAssets._2D.Camera2DFollow> ().SetTarget(transform);
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

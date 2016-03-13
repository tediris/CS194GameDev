using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Teleport : NetworkBehaviour {

	public float toX;
	public float toY;
	public bool isEnd = false;

	private PlayerIDs idStore;
	private GameStateManager GSManager;
	private NotificationText notificationText;
	private MapGen mapGen;

	// Use this for initialization
	void Start () {
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();
		GSManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		notificationText = GameObject.Find ("Notice").GetComponent<NotificationText> ();
		mapGen = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
	}

	// Update is called once per frame
//	void Update () {
//
//	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<PlayerMove> ().MoveTo (toX, toY);
//			Camera.main.transform.position = new Vector3 (toX, toY, Camera.main.transform.position.z);
			if (isEnd && other.GetComponent<PlayerHealth> ().alive) {
				ServerComm serverComm = other.gameObject.GetComponent<ServerComm> ();
				serverComm.RequestMap ();
				serverComm.MovePlayers (toX, toY);
			} else {
				if (mapGen.gameMode == MapGen.GameMode.Steal) {
					notificationText.SetTimedNotice ("Steal the egg!", Color.white, 3);
				}
			}
		}
	}

}

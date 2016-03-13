using UnityEngine;
using System.Collections;

public class ShrineTele : MonoBehaviour {

	public float toX;
	public float toY;

	private PlayerIDs idStore;
	private GameStateManager GSManager;

	// Use this for initialization
	void Start () {
		idStore = GameObject.Find ("PlayerManager").GetComponent<PlayerIDs> ();
		GSManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}

	// Update is called once per frame
	//	void Update () {
	//
	//	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Egg") {
			GameObject player = other.GetComponent<EggCarry> ().playerObj;
			player.GetComponent<PlayerMove> ().MoveTo (toX, toY);
			//			Camera.main.transform.position = new Vector3 (toX, toY, Camera.main.transform.position.z);
			if (player.GetComponent<PlayerHealth>().alive) {
				ServerComm serverComm = player.GetComponent<ServerComm> ();
				serverComm.RequestMap ();
				serverComm.MovePlayers (toX, toY);
			}
			Destroy (other.gameObject);
		}
	}
}

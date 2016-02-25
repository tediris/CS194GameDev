using UnityEngine;
using System.Collections;

public class ShrineRespawn : MonoBehaviour {

	GameStateManager stateManager;
	ScreenAction screenAction;

	// Use this for initialization
	void Start () {
		stateManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == stateManager.GetLocalPlayer().name) {
			PlayerHealth health = other.gameObject.GetComponent<PlayerHealth> ();
			health.Heal ();
			health.CmdRevivePlayer ();
		}
	}
}

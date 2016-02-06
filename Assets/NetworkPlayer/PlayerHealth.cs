using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
	[SyncVar] public int maxHealth = 5;
	[SyncVar] public int health = 5;

	Rigidbody2D playerBody;
	GameObject playerManager;

	// Use this for initialization
	void Start () {
		playerBody = this.gameObject.GetComponent<Rigidbody2D> ();
	}

	void FindPlayerManager() {
		while (playerManager == null) {
			playerManager = GameObject.Find ("PlayerManager");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		GUI.Box (new Rect (playerBody.transform.localPosition.x, playerBody.transform.localPosition.y, 20, 20), health + "/" + maxHealth);
	}

	public void TakeDamage(int damage, Damager damager) {
		health = Mathf.Max(health - damage, 0);
		if (health == 0) {
			Destroy (this.gameObject);
		}
	}
}
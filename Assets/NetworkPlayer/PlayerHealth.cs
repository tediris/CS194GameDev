using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
	[SyncVar] public int maxHealth = 5;
	[SyncVar] public int health = 5;

	GameObject playerManager;
	Transform panel;
	RectTransform healthBar;
	RectTransform healthBarBackground;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
		} else {
			// Need to actually use the 
//			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
			return;
		}
		healthBarBackground = panel.FindChild ("Health Background").GetComponent<RectTransform> ();
		healthBar = healthBarBackground.FindChild ("Health Foreground").GetComponent<RectTransform> ();
	}

	void FindPlayerManager() {
		while (playerManager == null) {
			playerManager = GameObject.Find ("PlayerManager");
		}
	}

	// Update is called once per frame
	void Update () {
		if (healthBar) {
			Vector3 newScale = new Vector3 ((1.0f * health) / maxHealth, healthBar.localScale.y, healthBar.localScale.z);
			healthBar.localScale = newScale;
		}
	}

	public void TakeDamage(int damage, Damager damager) {
		health = Mathf.Max(health - damage, 0);
		if (health == 0) {
			Destroy (this.gameObject);
		}
	}
}
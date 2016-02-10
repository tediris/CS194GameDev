using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
	public int maxHealth = 3;
	[SyncVar] public int health = 3;
	private bool canBeHit = true;

	GameObject playerManager;
	Transform panel;
//	RectTransform healthBar;
//	RectTransform healthBarBackground;

	Image[] hearts;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			hearts = new Image[maxHealth];
//			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
			for (int i = 0; i < maxHealth; i++) {
				hearts [i] = GameObject.Find ("Heart" + (i + 1)).GetComponent<Image> ();
			}
		} else {
			// Need to actually use the 
//			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
			return;
		}
//		healthBarBackground = panel.FindChild ("Health Background").GetComponent<RectTransform> ();
//		healthBar = healthBarBackground.FindChild ("Health Foreground").GetComponent<RectTransform> ();
	}

	public void Hit() {
		if (!canBeHit)
			return;
		HitAnimation ();
		canBeHit = false;
		StartCoroutine (Invulnerable(0.5f));
		health--;
		hearts [health].color = Color.black;
		if (health == 0) {
			Debug.Log ("CHARACTER DIED");
			Heal ();
		}
	}

	void HitAnimation() {
		//?
	}

	IEnumerator Invulnerable(float time) {
		yield return new WaitForSeconds (time);
		canBeHit = true;
	}

	public void Heal() {
		health = maxHealth;
		for (int i = 0; i < maxHealth; i++) {
			hearts [i].color = Color.white;
		}
	}

//	void FindPlayerManager() {
//		while (playerManager == null) {
//			playerManager = GameObject.Find ("PlayerManager");
//		}
//	}
//
	// Update is called once per frame
//	void Update () {
//		if (healthBar) {
//			Vector3 newScale = new Vector3 ((1.0f * health) / maxHealth, healthBar.localScale.y, healthBar.localScale.z);
//			healthBar.localScale = newScale;
//		}
//	}

//	public void TakeDamage(int damage, Damager damager) {
//		health = Mathf.Max(health - damage, 0);
//		if (health == 0) {
//			Destroy (this.gameObject);
//		}
//	}
}
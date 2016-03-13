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

//	Animator playerAnimator;
	PlayerControl playerControl;
	SpriteRenderer playerSprite;
	GameObject ghosty;
	public float deathAnimationTime = 0.6f;

	public bool alive = true;

	private NotificationText notificationText;

	Image[] hearts;

	ScreenAction screenAction;

	List<DeathListener> deathListeners;

	// Use this for initialization
	void Start () {
		deathListeners = new List<DeathListener> ();
		screenAction = GameObject.Find ("Screen").GetComponent<ScreenAction> ();
		playerSprite = GetComponent<SpriteRenderer> ();
		playerControl = GetComponent<PlayerControl> ();
//		playerAnimator = GetComponent<Animator> ();
		foreach (Transform child in this.gameObject.transform) {
			if (child.name == "Ghost") {
				ghosty = child.gameObject;
			}
		}
		if (isLocalPlayer) {
			hearts = new Image[maxHealth];
//			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
			for (int i = 0; i < maxHealth; i++) {
				hearts [i] = GameObject.Find ("Heart" + (i + 1)).GetComponent<Image> ();
			}
//			playerAnimator = GetComponent<Animator> ();
//			playerSprite = GetComponent<SpriteRenderer> ();
//			foreach (Transform child in this.gameObject.transform) {
//				if (child.name == "Ghost") {
//					ghosty = child.gameObject;
//				}
//			}
		} else {
			// Need to actually use the
//			panel = GameObject.Find ("Canvas").transform.FindChild ("Player 0");
			return;
		}
//		healthBarBackground = panel.FindChild ("Health Background").GetComponent<RectTransform> ();
//		healthBar = healthBarBackground.FindChild ("Health Foreground").GetComponent<RectTransform> ();
		notificationText = GameObject.Find("Notice").GetComponent<NotificationText> ();
	}

	public void addDeathListener(DeathListener dl) {
		deathListeners.Add (dl);
		Debug.Log("Death Listener added");
	}

	public void Hit() {
		if (!canBeHit || !alive)
			return;
		HitAnimation ();
		canBeHit = false;
		StartCoroutine (Invulnerable(0.5f));
		health--;
		hearts [health].color = Color.black;
		if (health == 0) {
			Debug.Log ("CHARACTER DIED");
//			Heal ();
			BringToDeath();
			CmdKillPlayer ();
		}
	}

	public void ToggleAlive() {
		alive = !alive;
//		playerAnimator.enabled = alive;
		playerSprite.enabled = alive;
		ghosty.SetActive(!alive);
	}

	public void BringToLife() {
		if (!alive) {
			ToggleAlive ();
		}
	}

	public void BringToDeath() {
		if (alive) {
			playerControl.enabled = false;
			StartCoroutine (WaitForGhost ());
			ToggleAlive ();

			notificationText.SetTimedNotice ("Oh no, return to the shrine to revive.", Color.white, 3);
		}
	}


    void CallDeathListeners()
    {
        Debug.Log("calling death listeners");
        List<DeathListener> toRemove = new List<DeathListener>();
        for (int i = 0; i < deathListeners.Count; i++)
        {
            deathListeners[i].PlayerDied(this.gameObject);
            if (deathListeners[i].destroyOnUse())
            {
                toRemove.Add(deathListeners[i]);
            }
        }
        for (int i = 0; i < toRemove.Count; i++)
        {
            deathListeners.Remove(toRemove[i]);
        }
    }

	IEnumerator WaitForGhost() {
		yield return new WaitForSeconds (deathAnimationTime);
		if (isLocalPlayer) {
			playerControl.enabled = true;
		}
	}

	[Command]
	void CmdKillPlayer() {
		CallDeathListeners ();
		RpcKillPlayer ();
	}

	[ClientRpc]
	void RpcKillPlayer() {
		BringToDeath ();
	}

	[Command]
	public void CmdRevivePlayer() {
		RpcRevivePlayer ();
	}

	[ClientRpc]
	void RpcRevivePlayer() {
		BringToLife ();
	}

	void HitAnimation() {
		Camera.main.GetComponent<CameraShake> ().Shake (1f);
		playerSprite.color = Color.red;
	}

	IEnumerator Invulnerable(float time) {
		yield return new WaitForSeconds (time);
		canBeHit = true;
		playerSprite.color = Color.white;
	}

	public void Heal() {
		for (int i = 0; i < maxHealth; i++) {
			hearts [i].color = Color.white;
		}
		if (!alive) {
			screenAction.Flash (Color.white);
		} else {
			if (health != maxHealth)
				screenAction.Flash (new Color(1.0f, 0.8f, 1.0f, 0.7f));
		}
		health = maxHealth;
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

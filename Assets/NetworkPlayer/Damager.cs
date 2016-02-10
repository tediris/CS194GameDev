using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Damager : NetworkBehaviour {

	public int damage = 0;
	public List<string> damagableTags;

	Rigidbody2D damagerBody;

	// Use this for initialization
	void Start () {
		damagerBody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
	}

//	void OnCollisionEnter2D(Collision2D collision) {
//		Collider2D collider = collision.collider;
//		foreach (string validTag in damagableTags) {
//			if (collider.tag == validTag) {
//				PlayerHealth damagedHealth = collider.gameObject.GetComponent<PlayerHealth> ();
//				damagedHealth.TakeDamage (damage, this);
//			}
//		}
//	}
}
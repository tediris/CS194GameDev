using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FireInCircle : NetworkBehaviour {

	public GameObject projectilePrefab;
	public int numProjectiles;
	public float fireRate = 2.0f;
	public float fireSpeed = 0.7f;
	System.Random rand;

	// Use this for initialization
	void Start () {
		if (!isServer) {
			this.enabled = false;
			return;
		}
		rand = new System.Random (System.DateTime.Now.GetHashCode ());
		StartCoroutine (FireBulletsLoop (fireRate));
	}

	IEnumerator FireBulletsLoop(float time) {
		while (true) {
			CmdFireBullets ();
			yield return new WaitForSeconds (time);
		}
	}

	[Command]
	void CmdFireBullets() {
		float angleOffset = (float) (Mathf.PI * 2) / numProjectiles;
		float angle = (float) (rand.NextDouble () * (Mathf.PI * 2));
		for (int i = 0; i < numProjectiles; i++) {
			Vector2 speed = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
			speed = speed.normalized * fireSpeed;
			GameObject projectile = (GameObject) Instantiate (projectilePrefab, this.gameObject.transform.position, Quaternion.identity);
			projectile.GetComponent<Rigidbody2D> ().velocity = speed;
			NetworkServer.Spawn (projectile);
			angle += angleOffset;
		}
	}

}

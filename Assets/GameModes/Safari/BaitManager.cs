using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaitManager : MonoBehaviour {

	public int baitCount;
	public List<GameObject> baits;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < baitCount; i++) {
			GameStateManager gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
			Vector3 location = new Vector3 (100f, 100f, 0f);
			GameObject bait = gsManager.SpawnBait (location);
			baits.Add (bait);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject MoveEggToPlayer(PlayerControl player) {
		if (baits.Count == 0) {
			Debug.Log ("No bait left");
			return null;
		}

		GameObject bait = baits [0];
		baits.Remove (bait);
		Rigidbody2D body = bait.GetComponent<Rigidbody2D> ();
		body.MovePosition (player.gameObject.transform.position);
		Debug.Log ("Moved bait to player");
		return bait;
	}
}

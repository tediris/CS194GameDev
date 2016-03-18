using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaitManager : MonoBehaviour {

	public int baitCount;
	public GameObject baitPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
//	void Update () {
//			
//	}

	public GameObject MoveEggToPlayer(PlayerControl player) {
		GameStateManager gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		GameObject bait = gsManager.CreateOverNetworkInstant (baitPrefab, Vector2.zero);
		Rigidbody2D body = bait.GetComponent<Rigidbody2D> ();
		body.MovePosition (player.gameObject.transform.position);
		Debug.Log ("Moved bait to player");
		return bait;
	}
}

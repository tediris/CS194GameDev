using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SafariManager : NetworkBehaviour {

	//	public int baitCount;
	//	public List<GameObject> baits;
	public GameObject enemyPrefab;
	public GameObject trapPrefab;
	GameStateManager gs;
	MapGen mg;

	// Use this for initialization
	void Start () {
		mg = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		gs = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
	}
		
	public void StartManager () {
		mg = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		gs = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		if (mg.gameMode	== MapGen.GameMode.Safari) {
			CreateTrap ();
			CreateEnemy ();
		}
	}

	void CreateEnemy () {
		MapGen.Room room = mg.GetRandomRoom ();
		float deltaX = Random.value * mg.roomWidth;
		float deltaY = Random.value * mg.roomHeight;
		Vector2 pos = new Vector2 (room.x + deltaX, room.y - deltaY);

		GameObject enemy = gs.CreateOverNetworkInstant (enemyPrefab, pos);
	}

	void CreateTrap () {
		while (true) {
			MapGen.Room room = mg.GetRandomRoom ();
			float deltaX = Random.value * mg.roomWidth;
			float deltaY = Random.value * mg.roomHeight;
			Vector2 pos = new Vector2 (room.x + deltaX, room.y - deltaY);

			int layerMask = 1 << 8;
			RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.up, layerMask);
			if (hit.collider != null) {
				Vector2 trapPos = new Vector2(hit.point.x, hit.point.y - 0.1f);
				GameObject trap = gs.CreateOverNetworkInstant (trapPrefab, trapPos);
				SpringJoint2D joint = trap.GetComponent<SpringJoint2D> ();
				Rigidbody2D trapBody = trap.GetComponent<Rigidbody2D> ();

				Rigidbody2D colliderRigidbody = hit.collider.gameObject.AddComponent<Rigidbody2D> ();
				colliderRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
				joint.connectedBody = colliderRigidbody;
				joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint (hit.point);

				trapBody.MovePosition (trapPos);
				break;
			}
		}
	}
}

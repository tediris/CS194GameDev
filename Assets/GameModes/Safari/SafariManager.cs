using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SafariManager : NetworkBehaviour {

	//	public int baitCount;
	//	public List<GameObject> baits;
	public GameObject enemyPrefab;
	public GameObject trapPrefab;
	public GameObject leverPrefab;
	GameStateManager gs;
	MapGen mg;
	TrapManager tm;

	// Use this for initialization
	void Start () {
		
	}
		
	public void StartManager () {
		mg = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		gs = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		if (mg.gameMode	== MapGen.GameMode.Safari) {
			GameObject trap = CreateTrap ();
			if (!trap) {
				Debug.Log ("Error: could not create trap");
				return;
			}
			GameObject enemy = CreateEnemy ();

			tm = trap.GetComponent<TrapManager> ();
			tm.AddTarget(enemy, ((manager) => {
				Debug.Log("Captured enemy");
				return true;
			}));
		}
	}
		
	GameObject CreateEnemy () {
		MapGen.Room room = mg.GetRandomRoom ();
		float deltaX = Random.value * mg.roomWidth;
		float deltaY = Random.value * mg.roomHeight;
		Vector2 pos = new Vector2 (room.x + deltaX, room.y - deltaY);

		GameObject enemy = gs.CreateOverNetworkInstant (enemyPrefab, pos);
		return enemy;
	}

	GameObject CreateTrap () {
		int count = 0;
		while (count < 20) {
			count += 1;
			MapGen.Room room = mg.GetRandomRoom ();
			float deltaX = Random.value * mg.roomWidth;
			float deltaY = Random.value * mg.roomHeight;
			Vector2 trapCastPos = new Vector2 (room.x + deltaX, room.y - deltaY);
			Vector2 trapLeftCastPos = new Vector2 (trapCastPos.x - 0.3f, trapCastPos.y);
			Vector2 trapRightCastPos = new Vector2 (trapCastPos.x + 0.3f, trapCastPos.y);

			deltaX = Random.value * mg.roomWidth;
			deltaY = Random.value * mg.roomHeight;
			Vector2 leverCastPos = new Vector2 (room.x + deltaX, room.y - deltaY);

			int layerMask = 1 << 8;
			RaycastHit2D trapHit = Physics2D.Raycast (trapCastPos, Vector2.up, layerMask);
			RaycastHit2D trapLeftClearHit = Physics2D.Raycast (trapCastPos, Vector2.down, 5f, layerMask);
			RaycastHit2D trapRightClearHit = Physics2D.Raycast (trapCastPos, Vector2.down, 5f, layerMask);
			RaycastHit2D leverHit = Physics2D.Raycast (leverCastPos, Vector2.down, layerMask);
			if (trapLeftClearHit.collider == null && trapRightClearHit.collider == null
				&& trapHit.collider != null 
				&& leverHit.collider != null
				&& Vector2.Dot(Vector2.down, trapHit.normal) > 0f 
				&& Vector2.Dot(Vector2.up, leverHit.normal) > 0f) {
				Vector2 trapPos = new Vector2(trapHit.point.x, trapHit.point.y - 1f);
				GameObject trap = gs.CreateOverNetworkInstant (trapPrefab, trapPos);
				SpringJoint2D joint = trap.GetComponent<SpringJoint2D> ();
				Rigidbody2D trapBody = trap.GetComponent<Rigidbody2D> ();

				Rigidbody2D colliderRigidbody = trapHit.collider.gameObject.AddComponent<Rigidbody2D> ();
				colliderRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
				joint.connectedBody = colliderRigidbody;
				joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint (trapHit.point);

				trapBody.MovePosition (trapPos);

				Vector2 leverPos = new Vector2 (leverHit.point.x, leverHit.point.y + 0.1f);
				GameObject lever = gs.CreateOverNetworkInstant (leverPrefab, leverPos);
				TrapDeployer deployer = lever.GetComponent<TrapDeployer> ();
				deployer.droppingTrap = trap;
				deployer.raisingTrap = trap;

				return trap;
			}
		}
		return null;
	}
}

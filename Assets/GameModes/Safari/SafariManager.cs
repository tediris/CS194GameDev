using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SafariManager : NetworkBehaviour {

	public GameObject enemyPrefab;
	public GameObject trapPrefab;
	public GameObject leverPrefab;
	TrapManager tm;

	// Use this for initialization
	void Start () {
		
	}

	public void Spawn(Vector3 trapPos, Vector3 leverPos) {
		GameStateManager gs = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		MapGen mg = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();

		int layerMask = 1 << LayerMask.NameToLayer ("Default");
		RaycastHit2D hit = Physics2D.Raycast (trapPos, Vector2.up, Mathf.Infinity, layerMask);

		GameObject trap = gs.CreateOverNetworkInstant (trapPrefab, trapPos);
		SpringJoint2D joint = trap.GetComponent<SpringJoint2D> ();
		Rigidbody2D trapBody = trap.GetComponent<Rigidbody2D> ();

		Rigidbody2D colliderRigidbody = hit.collider.gameObject.AddComponent<Rigidbody2D> ();
		joint.connectedBody = colliderRigidbody;
		joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint (hit.point);
		colliderRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

		GameObject lever = gs.CreateOverNetworkInstant (leverPrefab, leverPos);
		TrapDeployer deployer = lever.GetComponent<TrapDeployer> ();
		deployer.droppingTrap = trap;
		deployer.raisingTrap = trap;

		MapGen.Room room = mg.GetRandomRoom ();
		float deltaX = Random.value * mg.roomWidth;
		float deltaY = Random.value * mg.roomHeight;
		Vector2 pos = new Vector2 (room.x + deltaX, room.y - deltaY);

		GameObject enemy = gs.CreateOverNetworkInstant (enemyPrefab, pos);

	 	tm = trap.GetComponent<TrapManager> ();
		tm.AddTarget(enemy, ((manager) => {
			Debug.Log("Captured enemy");
			GameObject.Find("GameState").GetComponent<GameStateManager>().ResetGame();
			return true;
		}));
	}
		
//	public void StartManager () {
//		mg = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
//		gs = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
//		if (mg.gameMode	== MapGen.GameMode.Safari) {
//			GameObject trap = CreateTrap ();
//			if (!trap) {
//				Debug.Log ("Error: could not create trap");
//				GameObject.Find("GameState").GetComponent<GameStateManager>().ResetGame();
//				return;
//			}
//			GameObject enemy = CreateEnemy ();
//
//			tm = trap.GetComponent<TrapManager> ();
//			tm.AddTarget(enemy, ((manager) => {
//				Debug.Log("Captured enemy");
//				GameObject.Find("GameState").GetComponent<GameStateManager>().ResetGame();
//				return true;
//			}));
//		}
//	}
		
//	GameObject CreateEnemy () {
//		MapGen.Room room = mg.GetRandomRoom ();
//		float deltaX = Random.value * mg.roomWidth;
//		float deltaY = Random.value * mg.roomHeight;
//		Vector2 pos = new Vector2 (room.x + deltaX, room.y - deltaY);
//
//		GameObject enemy = gs.CreateOverNetworkInstant (enemyPrefab, pos);
//		return enemy;
//	}
//
//	GameObject CreateTrap () {
//		int count = 0;
//		while (count < 10000) {
//			count += 1;
//			MapGen.Room room = mg.GetRandomRoom ();
//			float deltaX = Random.value * mg.roomWidth/2;
//			float deltaY = Random.value * mg.roomHeight/2;
//			Vector2 trapCastPos = new Vector2 (room.x + mg.roomWidth/4 + deltaX, room.y - mg.roomHeight/4 - deltaY);
//
//			deltaX = Random.value * mg.roomWidth/2;
//			deltaY = Random.value * mg.roomHeight/2;
//			Vector2 leverCastPos = new Vector2 (room.x + mg.roomWidth/4 + deltaX, room.y - mg.roomHeight/4 - deltaY);
//
//			int layerMask = LayerMask.NameToLayer("Default") | LayerMask.NameToLayer("Pickups");
//			RaycastHit2D trapHit = Physics2D.CircleCast (trapCastPos, 0.3f, Vector2.up, layerMask);
//			if (!(trapHit.collider != null && Vector2.Dot(Vector2.down, trapHit.normal) > 0f)) {
//				continue;
//			}
//
//			Vector2 trapClearCastPos = new Vector2 (trapCastPos.x, trapCastPos.y);
//			RaycastHit2D trapClearHit = Physics2D.CircleCast (trapClearCastPos, 0.4f, Vector2.down, layerMask);
//			if (!(trapClearHit.collider != null && trapClearHit.distance > 1f)) {
//				continue;
//			}
//
//			RaycastHit2D leverHit = Physics2D.CircleCast (leverCastPos, 0.3f, Vector2.down, layerMask);
//			if (!(leverHit.collider != null && Vector2.Dot (Vector2.up, leverHit.normal) > 0f)) {
//				continue;
//			}
//
//			RaycastHit2D leverClearHit = Physics2D.CircleCast (leverCastPos, 0.2f, Vector2.down, layerMask);
//			if (!(leverClearHit.collider == null || leverClearHit.distance > 0.1f)) {
//				continue;
//			}
//
//			Debug.Log (trapHit.point);
//
//			Vector2 trapPos = new Vector2(trapHit.point.x, trapHit.point.y);
//			GameObject trap = gs.CreateOverNetworkInstant (trapPrefab, trapCastPos);
//			SpringJoint2D joint = trap.GetComponent<SpringJoint2D> ();
//			Rigidbody2D trapBody = trap.GetComponent<Rigidbody2D> ();
//
//			Rigidbody2D colliderRigidbody = trapHit.collider.gameObject.AddComponent<Rigidbody2D> ();
//			joint.connectedBody = colliderRigidbody;
//			joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint (trapHit.point);
//			colliderRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
//
//
////			Vector2 trapBodyPos = new Vector2 (trapPos.x, trapPos.y + 0.25f);
////			trapBody.MovePosition (trapBodyPos);
//
//			Vector2 leverPos = new Vector2 (leverHit.point.x, leverHit.point.y);
//			GameObject lever = gs.CreateOverNetworkInstant (leverPrefab, leverPos);
//			TrapDeployer deployer = lever.GetComponent<TrapDeployer> ();
//			deployer.droppingTrap = trap;
//			deployer.raisingTrap = trap;
//
//			return trap;
//		}
//		return null;
//	}
}

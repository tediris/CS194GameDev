using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetEnemySpawner : MonoBehaviour {

	public List<GameObject> thingsToSpawn;
	public List<Vector3> spawnPositions;

	// Use this for initialization
	void Start () {
		if (thingsToSpawn == null) {
			// nothing to spawn
		} else {
			GameStateManager gameState = GameObject.Find ("GameState").GetComponent<GameStateManager>();
			for (int i = 0; i < thingsToSpawn.Count; i++) {
				gameState.CreateOverNetwork (thingsToSpawn [i], 
					spawnPositions [i] + this.gameObject.transform.position);
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour {

	public List<GameObject> monsters;

	// Use this for initialization
	void Start () {
		// pick a random monster to spawn
		System.Random rand = new System.Random();
		GameObject toSpawn = monsters [rand.Next (monsters.Count)];
		GameStateManager gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		gsManager.SpawnEggMonster (toSpawn, gameObject.transform.position, GetComponent<WaypointFollow>().wayPoints);
		Destroy (this.gameObject);
	}
}

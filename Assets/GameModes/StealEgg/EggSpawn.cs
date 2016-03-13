using UnityEngine;
using System.Collections;

public class EggSpawn : MonoBehaviour {

	public GameObject eggPrefab;

	// Use this for initialization
	void Start () {
		GameStateManager gameState = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		GameObject eggDummy = GetComponentInChildren<EggDummy> ().gameObject;
		gameState.CreateOverNetworkInstant (eggPrefab, eggDummy.transform.position);
		Destroy (eggDummy);
	}
}

using UnityEngine;
using System.Collections;

public class TreasureSpawn : MonoBehaviour {

	public GameObject treasurePrefab;

	// Use this for initialization
	void Start () {
		GameStateManager gameState = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
		GameObject treasureDummy = GetComponentInChildren<TreasureDummy> ().gameObject;
		gameState.CreateOverNetworkInstant (treasurePrefab, treasureDummy.transform.position);
		Destroy (treasureDummy);
	}
}

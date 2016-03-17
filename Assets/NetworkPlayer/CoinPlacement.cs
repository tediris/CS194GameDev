using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CoinPlacement : NetworkBehaviour {

	MapGen generator = null;
	public int numCoins = 10;
	public int numEnemies = 5;
	public GameObject coinPrefab;
	public GameObject enemyPrefab;
	System.Random rand = null;

	// Use this for initialization
	void Start () {
	}
	
	public void PlaceCoins() {
		if (generator == null) {
			generator = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		}
		if (rand == null) {
			rand = new System.Random ();
		}
		var children = new List<GameObject>();
		foreach (Transform child in gameObject.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		MapGen.MapBound bound = generator.GetMapBounds ();
		float xScale = bound.botRight.x - bound.topLeft.x - 0.5f;
		float yScale = bound.botRight.y - bound.topLeft.y + 0.5f;
		Vector2 basePos = bound.topLeft + Vector2.down * 0.3f + Vector2.right * 0.3f;

		for (int i = 0; i < numCoins; i++) {
			while (true) {
				float xPos = ((float) rand.NextDouble ()) * xScale;
				float yPos = ((float)rand.NextDouble ()) * yScale;
				Vector2 pos = basePos + new Vector2 (xPos, yPos);
				RaycastHit2D rcast = Physics2D.Raycast (pos, Vector2.down, 0.1f);
				if (rcast.collider == null) {
					// place the coin
					GameObject coin = Instantiate (coinPrefab, new Vector3 (pos.x, pos.y, 0), Quaternion.identity) as GameObject;
					coin.transform.parent = this.gameObject.transform;
					NetworkServer.Spawn (coin);
					break;
				}
			}
		}

		for (int i = 0; i < numEnemies; i++) {
			while (true) {
				float xPos = ((float) rand.NextDouble ()) * xScale;
				float yPos = ((float) rand.NextDouble ()) * yScale;
				Vector2 pos = basePos + new Vector2 (xPos, yPos);
				RaycastHit2D rcast = Physics2D.Raycast (pos, Vector2.down, 0.1f);
				if (rcast.collider) {
					Debug.Log ("Raycast hit collider");
				} else {
					// place the coin
					GameObject enemy = Instantiate (enemyPrefab, new Vector3 (pos.x, pos.y, 0), Quaternion.identity) as GameObject;
					enemy.transform.parent = this.gameObject.transform;
					NetworkServer.Spawn (enemy);
					break;
				}
			}
		}
	}
}

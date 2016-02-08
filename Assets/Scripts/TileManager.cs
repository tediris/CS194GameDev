using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	public SpriteRenderer tile;
	public int n_tiles = 20;

	// Use this for initialization
	void Start () {
		// Really get (2 * n_tiles)^2

		Vector2 tileSize = new Vector2 (
			                   tile.bounds.size.x / transform.localScale.x, 
			                   tile.bounds.size.y / transform.localScale.y
		                   );
		for (int i = -n_tiles; i < n_tiles; i++) {
			for (int j = -n_tiles; j < n_tiles; j++) {
				Instantiate (tile, new Vector3 (i * tileSize.x, j * tileSize.y, 0), Quaternion.identity);
			}
		}
		//Instantiate (tile, new Vector3 (0, 0, 0), Quaternion.identity);
		//Instantiate (tile, new Vector3 (tileSize.x, tileSize.y, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

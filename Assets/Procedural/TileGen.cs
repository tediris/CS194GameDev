using UnityEngine;
using System.Collections;

public class TileGen : MonoBehaviour {

	public enum TileType
	{
		Wall,
		Empty
	};

	public int width = 50;
	public int height = 50;

	public GameObject wall;

	public int tileSize = 32;

	TileType[,] map;

	// Use this for initialization
	void Start () {
		InitMap ();
		SpawnMap ();
	}

	void InitMap() {
		map = new TileType[width, height];
		Room r = new Room (10, 10, 5);
		r.fillMap (map);
	}

	void SpawnMap() {
		float offset = ((float) tileSize) / 100.0f;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Vector3 pos = Vector3.up * y * offset + Vector3.right * x * offset;
				if (map [x, y] == TileType.Wall) {
					GameObject tile = Instantiate (wall, pos, Quaternion.identity) as GameObject;
					tile.transform.parent = gameObject.transform;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public class Room {
		int x, y;
		int radius;

		public enum Shape {
			Rect = 0,
			Circle = 1
		};

		public Shape shape;

		public Room(int x, int y, int radius) {
			this.x = x;
			this.y = y;
			this.radius = radius;
			shape = Shape.Rect;
		}

		public void fillMap(TileType[,] tilemap) {
			if (shape == Shape.Rect) {
				fillRect(tilemap);
			}
		}

		void fillRect(TileType[,] tilemap) {
			for (int i = Mathf.Max (x - radius, 1); i < Mathf.Min (tilemap.GetLength (0) - 1, x + radius); i++) {
				for (int j = Mathf.Max(y - radius, 1); j < Mathf.Min(tilemap.GetLength(1) - 1, y + radius); j++) {
					tilemap[i, j] = TileType.Empty;
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public int numRoomsWidth = 4;
	public int numRoomsHeight = 2;
	public int minRoomSize = 4;
	public int maxRoomSize = 8;
	Room[,] rooms;

	TileType[,] map;

	System.Random rand;
	public string seed = "mario";

	// Use this for initialization
	void Start () {
		rand = new System.Random (seed.GetHashCode());
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
	
	void CreateRoomLayout() {
		rooms = new Room[numRoomsWidth, numRoomsHeight];
		//int roomWidthSpacing = wid
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				int size = rand.Next (maxRoomSize - minRoomSize + 1) + minRoomSize;
				rooms [x, y] = new Room (x, y, size);
			}
		}
		int startX = rand.Next (numRoomsWidth);
		int startY = rand.Next (numRoomsHeight);

	}

	public class Room {
		int x, y;
		int radius;
		public List<Room> connectedRooms;

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

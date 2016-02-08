using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGen : MonoBehaviour {

	public enum TileType
	{
		Wall,
		Empty
	};

	public enum RoomType
	{
		N, E, S, W,
		NE, NS, NW,
		SE, SW, EW,
		NES, NEW, NSW, SEW,
		OMNI
	}

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
		CreateRoomLayout ();
//		for (int x = 0; x < numRoomsWidth; x++) {
//			for (int y = 0; y < numRoomsHeight; y++) {
//				rooms [x, y].fillMap (map);
//				rooms [x, y].fillConnections (map);
//			}
//		}
		//Room r = new Room (10, 10, 5);
		//r.fillMap (map);
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
		int roomWidthSpacing = width / numRoomsWidth;
		int roomHeightSpacing = height / numRoomsHeight;
		int xOffset = maxRoomSize + 1;
		int yOffset = maxRoomSize + 1;
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				int size = rand.Next (maxRoomSize - minRoomSize + 1) + minRoomSize;
				rooms [x, y] = new Room (xOffset + x * roomWidthSpacing , yOffset + y * roomHeightSpacing, x, y, 20, 10);
			}
		}
		int startX = rand.Next (numRoomsWidth);
		int startY = rand.Next (numRoomsHeight);

		RecursiveBuildMaze (startX, startY);
	}

	List<Room> GetConnectedUnvisitedRooms(int x, int y) {
		List<Room> connected = new List<Room> ();
		if (x > 0 && rooms [x - 1, y].connectedRooms.Count == 0) {
			connected.Add (rooms [x - 1, y]);
		}
		if (x < numRoomsWidth - 1 && rooms [x + 1, y].connectedRooms.Count == 0) {
			connected.Add(rooms[x + 1, y]);
		}
		if (y > 0 && rooms [x, y - 1].connectedRooms.Count == 0) {
			connected.Add (rooms [x, y - 1]);
		}
		if (y < numRoomsHeight - 1 && rooms [x, y + 1].connectedRooms.Count == 0) {
			connected.Add (rooms [x, y + 1]);
		}
		return connected;
	}

	void RecursiveBuildMaze(int x, int y) {
		while (true) {
			List<Room> connected = GetConnectedUnvisitedRooms (x, y);
			if (connected.Count == 0)
				return;
			Room nextRoom = connected [rand.Next (connected.Count)];
			rooms [x, y].connectedRooms.Add (nextRoom);
			nextRoom.connectedRooms.Add (rooms [x, y]);
			RecursiveBuildMaze (nextRoom.gridX, nextRoom.gridY);
		}
	}

	public class Room {
		public int x, y;
		public int gridX, gridY;
		int width, height;
		public List<Room> connectedRooms;

		public enum Shape {
			Rect = 0,
			Circle = 1
		};

		public Shape shape;

		public Room(int x, int y, int gridX, int gridY, int width, int height) {
			this.x = x;
			this.y = y;
			this.gridX = gridX;
			this.gridY = gridY;
			this.width = width;
			this.height = height;
			shape = Shape.Rect;
			connectedRooms = new List<Room>();
		}

		public void fillMap(TileType[,] tilemap) {
			if (shape == Shape.Rect) {
				fillRect(tilemap);
			}
		}

		public void fillConnections(TileType[,] tilemap) {
			foreach (Room room in connectedRooms) {
				int currX = x;
				int currY = y;
				while (currX != room.x || currY != room.y) {
					int xDist = Mathf.Abs (currX - room.x);
					int yDist = Mathf.Abs (currY - room.y);
					if (xDist > yDist) {
						// move in the x direction
						int xDelta = (room.x - currX) / xDist;
						currX += xDelta;
						tilemap [currX, currY] = TileType.Empty;
					} else {
						int yDelta = (room.y - currY) / yDist;
						currY += yDelta;
						tilemap [currX, currY] = TileType.Empty;
					}
				}
			}
		}

		void fillRect(TileType[,] tilemap) {
			for (int i = Mathf.Max (x - width/2, 1); i < Mathf.Min (tilemap.GetLength (0) - 1, x + width/2); i++) {
				for (int j = Mathf.Max(y - height/2, 1); j < Mathf.Min(tilemap.GetLength(1) - 1, y + height/2); j++) {
					tilemap[i, j] = TileType.Empty;
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {

	public enum Direction
	{
		NORTH = 0,
		SOUTH = 1,
		EAST = 2,
		WEST = 3
	}

	public enum RoomType
	{
		N, E, S, W,
		NE, NS, NW,
		SE, SW, EW,
		NES, NEW, NSW, SEW,
		OMNI
	}

	public List<GameObject> omniPrefabs;
	public List<GameObject> northPrefabs;
	public List<GameObject> eastPrefabs;
	public List<GameObject> southPrefabs;
	public List<GameObject> westPrefabs;
	public List<GameObject> northEastPrefabs;
	public List<GameObject> northSouthPrefabs;
	public List<GameObject> northWestPrefabs;
	public List<GameObject> southEastPrefabs;
	public List<GameObject> southWestPrefabs;
	public List<GameObject> eastWestPrefabs;
	public List<GameObject> northEastSouthPrefabs;
	public List<GameObject> northEastWestPrefabs;
	public List<GameObject> northSouthWestPrefabs;
	public List<GameObject> southEastWestPrefabs;

	public int tileSize = 32;

	public float roomWidth = 6.4f;
	public float roomHeight = 3.2f;

	public int numRoomsWidth = 4;
	public int numRoomsHeight = 2;
	Room[,] rooms;

	public System.Random rand;
	public string seed = "mario";

	// Use this for initialization
	void Start () {
		rand = new System.Random (seed.GetHashCode());
		InitMap ();
		SpawnMap ();
	}

	void InitMap() {
		CreateRoomLayout ();
	}

	void SpawnMap() {
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				rooms [x, y].Create ();
			}
		}
	}

	void CreateRoomLayout() {
		rooms = new Room[numRoomsWidth, numRoomsHeight];
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				rooms [x, y] = new Room (x, y, this);
			}
		}
		int startX = rand.Next (numRoomsWidth);
		int startY = rand.Next (numRoomsHeight);

		RecursiveBuildMaze (startX, startY);
	}

	List<Room> GetConnectedUnvisitedRooms(int x, int y) {
		List<Room> connected = new List<Room>();
		if (x > 0 && !rooms [x - 1, y].IsConnected()) {
			connected.Add (rooms [x - 1, y]);
		}
		if (x < numRoomsWidth - 1 && !rooms [x + 1, y].IsConnected()) {
			connected.Add(rooms[x + 1, y]);
		}
		if (y > 0 && !rooms [x, y - 1].IsConnected()) {
			connected.Add (rooms [x, y - 1]);
		}
		if (y < numRoomsHeight - 1 && !rooms [x, y + 1].IsConnected()) {
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
			rooms [x, y].connectedRooms[(int)rooms[x, y].GetRelation(nextRoom.x, nextRoom.y)] = true;
			nextRoom.connectedRooms [(int)nextRoom.GetRelation (x, y)] = true;;
			RecursiveBuildMaze (nextRoom.x, nextRoom.y);
		}
	}

	public class Room {
		public int x, y;
		public bool[] connectedRooms;
		public MapGen generator;

		public Room(int x, int y, MapGen gen) {
			this.x = x;
			this.y = y;
			connectedRooms = new bool[4];
			generator = gen;
		}

		public bool IsConnected() {
			for (int i = 0; i < 4; i++) {
				if (connectedRooms [i]) {
					return true;
				}
			}
			return false;
		}

		public Direction GetRelation(int x, int y) {
			if (this.x > x) {
				return Direction.EAST;
			}
			if (this.x < x) {
				return Direction.WEST;
			}
			if (this.y > y) {
				return Direction.NORTH;
			}
			return Direction.SOUTH;
		}

		public void Create() {
			Vector3 pos = Vector3.up * y * generator.roomHeight + Vector3.right * x * generator.roomWidth;

			GameObject prefab;

			if (connectedRooms[(int)Direction.NORTH] && connectedRooms[(int)Direction.SOUTH] && connectedRooms[(int)Direction.EAST] && connectedRooms[(int)Direction.WEST]) {
				prefab = generator.omniPrefabs[generator.rand.Next(generator.omniPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.EAST] && connectedRooms [(int)Direction.WEST]) {
				prefab = generator.northEastWestPrefabs[generator.rand.Next(generator.northEastWestPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.EAST] && connectedRooms [(int)Direction.SOUTH]) {
				prefab = generator.northEastSouthPrefabs[generator.rand.Next(generator.northEastSouthPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.SOUTH] && connectedRooms [(int)Direction.WEST]) {
				prefab = generator.northSouthWestPrefabs[generator.rand.Next(generator.northSouthWestPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.SOUTH] && connectedRooms [(int)Direction.EAST] && connectedRooms [(int)Direction.WEST]) {
				prefab = generator.southEastWestPrefabs[generator.rand.Next(generator.southEastWestPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.EAST]) {
				prefab = generator.northEastPrefabs[generator.rand.Next(generator.northEastPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.WEST]) {
				prefab = generator.northWestPrefabs[generator.rand.Next(generator.northWestPrefabs.Count)];
			}  else if (connectedRooms [(int)Direction.NORTH] && connectedRooms [(int)Direction.SOUTH]) {
				prefab = generator.northSouthPrefabs[generator.rand.Next(generator.northSouthPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.SOUTH] && connectedRooms [(int)Direction.EAST]) {
				prefab = generator.southEastPrefabs[generator.rand.Next(generator.southEastPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.SOUTH] && connectedRooms [(int)Direction.WEST]) {
				prefab = generator.southWestPrefabs[generator.rand.Next(generator.southWestPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.WEST] && connectedRooms [(int)Direction.EAST]) {
				prefab = generator.eastWestPrefabs[generator.rand.Next(generator.eastWestPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.NORTH]) {
				prefab = generator.northPrefabs[generator.rand.Next(generator.northPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.EAST]) {
				prefab = generator.eastPrefabs[generator.rand.Next(generator.eastPrefabs.Count)];
			} else if (connectedRooms [(int)Direction.WEST]) {
				prefab = generator.westPrefabs[generator.rand.Next(generator.westPrefabs.Count)];
			} else {
				prefab = generator.southPrefabs[generator.rand.Next(generator.southPrefabs.Count)];
			}

			GameObject tile = Instantiate (prefab, pos, Quaternion.identity) as GameObject;
			tile.transform.parent = generator.gameObject.transform;
		}
	}
}

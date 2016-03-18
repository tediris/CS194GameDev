using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility;

public class MapGen : MonoBehaviour {

	public enum GameType
	{
		Race = 0,
		Treasure = 1,
		Steal = 2
	}

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

	public enum GameMode {
		Race = 0,
		Treasure = 1,
		Steal = 2,
		Safari = 3
	}
			
	public GameMode gameMode;
	public bool testGameMode = false;
	public GameMode debugGameMode;

	public GameObject endPortal;

	public GameObject startRoomPrefab;
	public List<GameObject> eggRoomPrefab;
	public GameObject trapRoomPrefab;
	public GameObject treasureRoomPrefab;
	public List<GameObject> sealingWallNSEWPrefabs;
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

	public int numTreasureRooms = 1;

	public int numRandomConnections = 4;
	Room[,] rooms;

	[HideInInspector]
	public System.Random rand;
	public string seed = "mario";

	private Teleport portalScript;

	public string levelDescription;
	
	// Use this for initialization
	void Start () {
//		portalScript = GameObject.Find ("StartPortal").GetComponent<Teleport> ();
//		rand = new System.Random (seed.GetHashCode());
//		InitMap ();
//		SpawnMap ();
	}

	public class MapBound
	{
		public Vector2 topLeft, botRight;
		public MapBound(Vector2 topLeft, Vector2 botRight) {
			this.topLeft = topLeft;
			this.botRight = botRight;
		}
	}

	public MapBound GetMapBounds() {
		Vector2 minVec = new Vector2((numRoomsWidth - 1) * -roomWidth, 0f);
		Vector2 maxVec = new Vector2(roomWidth, numRoomsHeight * -roomHeight);
		return new MapBound (minVec, maxVec);
	}

	public void GenerateNewMap(string seed) {
		portalScript = GameObject.Find ("StartPortal").GetComponent<Teleport> ();
		rand = new System.Random (seed.GetHashCode());
		ChooseGameMode ();
		InitMap ();
		SpawnMap ();
	}

	void ChooseGameMode() {
		int numGameModes = System.Enum.GetValues (typeof(GameMode)).Length;
		gameMode = (GameMode) rand.Next (numGameModes);
		if (testGameMode) {
			gameMode = debugGameMode;
		}
	}

	void InitMap() {
		CreateRoomLayout ();
		RemoveRandomWalls ();
	}

	void RemoveRandomWalls() {
		for (int i = 0; i < numRandomConnections; i++) {
			int x = rand.Next (numRoomsWidth);
			int y = rand.Next (numRoomsHeight);

			List<Room> neighbors = GetUnconnectedNeighbors (x, y);
			if (neighbors.Count == 0)
				continue;
			Room nextRoom = neighbors [rand.Next (neighbors.Count)];
			rooms [y, x].connectedRooms[(int)rooms[y, x].GetRelation(nextRoom.x, nextRoom.y)] = true;
			nextRoom.connectedRooms [(int)nextRoom.GetRelation (x, y)] = true;;
		}
	}

	void SpawnMap() {
		SetupGameMode();
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				rooms [y, x].Create ();
			}
		}
	}

	void SetupGameMode() {
		if (gameMode == GameMode.Race) {
			SetupRaceMode ();
		} else if (gameMode == GameMode.Treasure) {
			SetupTreasureMode ();
		} else if (gameMode == GameMode.Steal) {
			SetupStealMode ();
		} else if (gameMode == GameMode.Safari) {
			SetupSafariMode ();
		}
	}

	void SetupSafariMode() {
		levelDescription = LevelDescriptions.StealLevel;
		int maxDist = -1;
		int maxDistX = -1;
		int maxDistY = -1;
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				if (rooms [y, x].dist > maxDist) {
					maxDist = rooms [y, x].dist;
					maxDistX = x;
					maxDistY = y;
				}
			}
		}
		rooms [maxDistY, maxDistX].isTrapRoom = true;
	}

	void SetupRaceMode() {
		levelDescription = LevelDescriptions.RaceLevel;
		int maxDist = -1;
		int maxDistX = -1;
		int maxDistY = -1;
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				if (rooms [y, x].dist > maxDist) {
					maxDist = rooms [y, x].dist;
					maxDistX = x;
					maxDistY = y;
				}
			}
		}
		CreateEnd (maxDistX, maxDistY);
	}

	void SetupTreasureMode() {
		levelDescription = LevelDescriptions.TreasureLevel;
		List<Pair<int, int>> treasureRooms = new List<Pair<int, int>>();
		while (treasureRooms.Count < numTreasureRooms) {
			int x = rand.Next (numRoomsWidth);
			int y = rand.Next (numRoomsHeight);
			Pair<int, int> room = new Pair<int,int> (x, y);
			if (rooms [x, y].dist == 0)
				continue;
			if (!treasureRooms.Contains(room)) {
				treasureRooms.Add(room);
				rooms [x, y].hasTreasure = true;
			}
		}
		GameObject.Find ("GameState").GetComponent<GameStateManager> ().numTreasures = numTreasureRooms;
	}

	void SetupStealMode() {
		levelDescription = LevelDescriptions.StealLevel;
		int maxDist = -1;
		int maxDistX = -1;
		int maxDistY = -1;
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				if (rooms [y, x].dist > maxDist) {
					maxDist = rooms [y, x].dist;
					maxDistX = x;
					maxDistY = y;
				}
			}
		}
		rooms [maxDistY, maxDistX].isEggRoom = true;
	}

	Room RandomRoom () {
		if (rooms.Length == 0) {
			return null;
		}
		int x = rand.Next (numRoomsWidth);
		int y = rand.Next (numRoomsHeight);
		Room room = rooms [x, y];
		return room;
	}

	public Room GetRandomRoom() {
		return RandomRoom ();
	}

//	void SetupSafariMode() {
//		bool flag = false;
//		while (!flag) {
//			Room room = RandomRoom ();
//			if (!room.connectedRooms [(int)Direction.NORTH]) {
//				room.isTrapRoom = true;
//				flag = true;
//			}
//		}
//	}

	void CreateEnd(int x, int y) {
		Vector3 pos = Vector3.up * (-y - 0.5f) * roomHeight + Vector3.right * (-x + 0.5f) * roomWidth;

		GameObject portal = Instantiate (endPortal, pos, Quaternion.identity) as GameObject;
		portal.transform.parent = transform;

		Teleport endPortalScript = portal.GetComponent<Teleport> ();
		endPortalScript.toX = 3.2f;
		endPortalScript.toY = 98.4f;
		endPortalScript.isEnd = true;
	}

	void CreateRoomLayout() {
		rooms = new Room[numRoomsHeight, numRoomsWidth];
		for (int x = 0; x < numRoomsWidth; x++) {
			for (int y = 0; y < numRoomsHeight; y++) {
				rooms [y, x] = new Room (x, y, this, -1);
			}
		}
		int startX = rand.Next (numRoomsWidth);
		int startY = rand.Next (numRoomsHeight);

		portalScript.toX = -startX * roomWidth + 0.5f * roomWidth;
		portalScript.toY = -startY * roomHeight - 0.5f * roomHeight;

		RecursiveBuildMaze (startX, startY, 0);
	}

	List<Room> GetUnconnectedNeighbors(int x, int y) {
		List<Room> neighbors = new List<Room>();
		if (x > 0 && !rooms[y,x].connectedRooms[(int) Direction.WEST]) {
			neighbors.Add (rooms [y, x-1]);
		}
		if (x < numRoomsWidth - 1 && !rooms[y,x].connectedRooms[(int) Direction.EAST]) {
			neighbors.Add(rooms[y, x+1]);
		}
		if (y > 0 && !rooms[y,x].connectedRooms[(int) Direction.SOUTH]) {
			neighbors.Add (rooms [y-1, x]);
		}
		if (y < numRoomsHeight - 1 && !rooms[y,x].connectedRooms[(int) Direction.NORTH]) {
			neighbors.Add (rooms [y+1, x]);
		}
		return neighbors;
	}

	List<Room> GetConnectedUnvisitedRooms(int x, int y) {
		List<Room> connected = new List<Room>();
		if (x > 0 && !rooms [y, x-1].IsConnected()) {
			connected.Add (rooms [y, x-1]);
		}
		if (x < numRoomsWidth - 1 && !rooms [y, x+1].IsConnected()) {
			connected.Add(rooms[y, x+1]);
		}
		if (y > 0 && !rooms [y-1, x].IsConnected()) {
			connected.Add (rooms [y-1, x]);
		}
		if (y < numRoomsHeight - 1 && !rooms [y+1, x].IsConnected()) {
			connected.Add (rooms [y+1, x]);
		}
		return connected;
	}

	void RecursiveBuildMaze(int x, int y, int dist) {
		rooms[y,x].dist = dist;
		while (true) {
			List<Room> connected = GetConnectedUnvisitedRooms (x, y);
			if (connected.Count == 0)
				return;
			Room nextRoom = connected [rand.Next (connected.Count)];
			rooms [y, x].connectedRooms[(int)rooms[y, x].GetRelation(nextRoom.x, nextRoom.y)] = true;
			nextRoom.connectedRooms [(int)nextRoom.GetRelation (x, y)] = true;
			RecursiveBuildMaze (nextRoom.x, nextRoom.y, dist + 1);
		}
	}

	public class Room {
		public int x, y, dist;
		public bool[] connectedRooms;
		public MapGen generator;
		public bool hasTreasure = false;
		public bool isEggRoom = false;
		public bool isTrapRoom = false;

		public Room(int x, int y, MapGen gen, int dist) {
			this.x = x;
			this.y = y;
			this.dist = dist;
			connectedRooms = new bool[4];
			generator = gen;
			hasTreasure = false;
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
			Vector3 pos = Vector3.up * -y * generator.roomHeight + Vector3.right * -x * generator.roomWidth;

			GameObject prefab;

			if (isEggRoom) {
				GameStateManager gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
				GameObject eggRoom = generator.eggRoomPrefab[generator.rand.Next(generator.eggRoomPrefab.Count)];
				prefab = Instantiate (eggRoom, pos, Quaternion.identity) as GameObject;
				// get the position of the egg
				EggDummy eggID = prefab.GetComponentInChildren<EggDummy>();
				Vector3 eggPos = eggID.transform.position;
				Destroy (eggID.gameObject);
				gsManager.SpawnEgg (eggPos);
				for (int i = 0; i < 4; i++) {
					if (!connectedRooms [i]) {
						GameObject wall = Instantiate (generator.sealingWallNSEWPrefabs [i], pos, Quaternion.identity) as GameObject;
						wall.transform.parent = prefab.transform;
					}
				}
				prefab.transform.parent = generator.gameObject.transform;
				return;
			}

			if (isTrapRoom) {
				GameStateManager gsManager = GameObject.Find ("GameState").GetComponent<GameStateManager> ();
				GameObject trapRoom = generator.trapRoomPrefab;
				prefab = Instantiate (trapRoom, pos, Quaternion.identity) as GameObject;
				// get the position of the egg
				TrapLocation trapLoc = prefab.GetComponentInChildren<TrapLocation>();
				Vector3 trapPos = trapLoc.transform.position;
				LeverLocation leverLoc = prefab.GetComponentInChildren<LeverLocation>();
				Vector3 leverPos = leverLoc.transform.position;
				Destroy (trapLoc.gameObject);
				Destroy (leverLoc.gameObject);

				for (int i = 0; i < 4; i++) {
					if (!connectedRooms [i]) {
						GameObject wall = Instantiate (generator.sealingWallNSEWPrefabs [i], pos, Quaternion.identity) as GameObject;
						wall.transform.parent = prefab.transform;
					}
				}
				prefab.transform.parent = generator.gameObject.transform;

				SafariManager safariManager = GameObject.Find ("GameState").GetComponent<SafariManager> ();
				safariManager.Spawn (trapPos, leverPos);

				return;
			}
				
			if (this.dist == 0) {
				Debug.Log("Room " + x + ", " + y + " has dist 0");
				prefab = Instantiate (generator.startRoomPrefab, pos, Quaternion.identity) as GameObject;
				for (int i = 0; i < 4; i++) {
					if (!connectedRooms [i]) {
						GameObject wall = Instantiate (generator.sealingWallNSEWPrefabs [i], pos, Quaternion.identity) as GameObject;
						wall.transform.parent = prefab.transform;
					}
				}
				prefab.transform.parent = generator.gameObject.transform;
				prefab.GetComponentInChildren<LevelTypeDescription> ().description = generator.levelDescription;
				return;
			}

			if (hasTreasure) {
				Debug.Log ("Adding treasure to Room " + x + ", " + y);
				prefab = Instantiate (generator.treasureRoomPrefab, pos, Quaternion.identity) as GameObject;
				for (int i = 0; i < 4; i++) {
					if (!connectedRooms [i]) {
						GameObject wall = Instantiate (generator.sealingWallNSEWPrefabs [i], pos, Quaternion.identity) as GameObject;
						wall.transform.parent = prefab.transform;
					}
				}
				prefab.transform.parent = generator.gameObject.transform;
				return;
			}

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

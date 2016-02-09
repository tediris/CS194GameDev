using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPlacements : MonoBehaviour {

	private float tileSize = 0.32f;

	class Point {
		public int x, y;
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	public List<Platform> platforms;

	public void FindItemPlacements(int[,] data) {
		platforms = new List<Platform> ();
		Point start = null;
		Point end = null;
		for (int y = 0; y < data.GetLength (0) - 1; y++) {
			for (int x = 0; x < data.GetLength (1); x++) {
				int val = data [y, x];
				if (val == -1) {
					if (start == null) {
						start = new Point (x, y);
						end = new Point (x, y);
					} else {
						end = new Point (x, y);
					}
				} else {
					if (start != null) {
						Vector2 startPoint = new Vector2 (tileSize * start.x, tileSize * -start.y);
						Vector2 endPoint = new Vector2 (tileSize * end.x, tileSize * -end.y);
						platforms.Add (new Platform (startPoint, endPoint));
						start = null;
						end = null;
					}
				}
			}
			start = null;
			end = null;
		}
	}

	public class Platform
	{
		Vector2 start, end;
		public Platform(Vector2 start, Vector2 end) {
			this.start = start;
			this.end = end;

		}
	}

	// Use this for initialization
	void Start () {
		Debug.Log (platforms.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

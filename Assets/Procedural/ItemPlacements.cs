using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPlacements : MonoBehaviour {

	class Point {
		public int x, y;
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	public List<Platform> platforms;

	public void FindItemPlacements(int[,] data) {
		Point start = null;
		Point end = null;
		for (int y = 0; y < data.GetLength (0) - 1; y++) {
			for (int x = 0; x < data.GetLength (1); x++) {
				int val = data [y, x];
				if (val == -1) {
					if (start == null) {
						start = new Point (x, y);
					} else {

					}
				} else {

				}
				//if (start != 
			}
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

	void Awake() {
		platforms = new List<Platform> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

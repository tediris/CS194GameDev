using UnityEngine;
using System.Collections;

public class PlatformGen : MonoBehaviour {

	System.Random rand;
	public string seed = "test";

	public GameObject platform;
	public int numLevels = 5;
	public float minHeight = 1;
	public float maxHeight = 2;
	public int maxPlatforms = 4;
	public float vertDist = 3;
	public float horizDist = 3;
	public Vector3 start = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		rand = new System.Random (seed.GetHashCode());
		PlacePlatforms ();
	}

	float RandFloat(float min, float max) {
		float dist = max - min;
		return min + dist * (float) rand.NextDouble ();
	}

	int RandSign() {
		if (rand.NextDouble () > 0.5) {
			return 1;
		} else {
			return -1;
		}
	}

	void PlacePlatforms() {
		Instantiate (platform, start, Quaternion.identity);
		for (int i = 1; i < numLevels; i++) {
			int numToGen = rand.Next (1, maxPlatforms);
			for (int j = 0; j < numToGen; j++) {
				float y = RandFloat (minHeight, maxHeight + 1);
				float x = RandFloat (-5, 5);
				Vector3 pos = start + Vector3.up * (y + RandSign() * vertDist * i) + Vector3.right * (x + RandSign() * horizDist * i);
				Instantiate (platform, pos, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

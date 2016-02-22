using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility;

public class RandomCreation : MonoBehaviour {

	public List<GameObject> randomEntities;/* = new List< Pair<GameObject, float> >(); */
	public List<float> randChances;
	MapGen generator;
	System.Random rand;

	// Use this for initialization
	void Start () {
		Debug.Log ("calling noise start func");
		Debug.Log (randomEntities.Count);
		generator = GameObject.Find ("TileGenParent").GetComponent<MapGen> ();
		rand = new System.Random (generator.seed.GetHashCode());
		SetupEntities ();
	}

	void SetupEntities() {
		List<GameObject> toDestroy = new List<GameObject> ();
		for (int i = 0; i < randomEntities.Count; i++) {
			float chance = randChances[i];
			if ((float)rand.NextDouble () > chance) {
				// activate the entity
				toDestroy.Add(randomEntities[i]);
			}
		}
		toDestroy.ForEach(obj => Destroy(obj));
	}

	// Update is called once per frame
	void Update () {
	
	}
}

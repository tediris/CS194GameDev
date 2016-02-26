using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StoneSpawner : NetworkBehaviour {

	public bool active = false;
	float timer = 0f;
	public float timeBetweenSpawns = 0.5f;
	GameObject parentContainer;
	public GameObject blockPrefab;
	System.Random rand;
	public List<Sprite> blockSprites;
	public int numBlocks = 0;
	public int maxBlocks = 30;

	// Use this for initialization
	void Start () {
		parentContainer = new GameObject ();
		rand = new System.Random (System.DateTime.Now.GetHashCode());
	}

	void OnDestroy() {
		Destroy (parentContainer);
	}

	void CustomDestroy(GameObject o)
	{
		o.transform.position = new Vector3(100000000.0f,10000000.0f,10000000.0f);
		StartCoroutine (DestroyOnNextFrame (o));
	}

	IEnumerator DestroyOnNextFrame(GameObject o) { 
		yield return new WaitForFixedUpdate ();
		Destroy (o);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
		timer += Time.deltaTime;
		if (timer > timeBetweenSpawns) {
			timer = 0f;
			// spawn a block randomly
			if (active) {
				SpawnRandomBlock ();
				numBlocks++;
				if (numBlocks > maxBlocks) {
					numBlocks = 0;
					// destroy all the blocks
					var children = new List<GameObject>();
					foreach (Transform child in parentContainer.transform) children.Add(child.gameObject);
					children.ForEach(child => CustomDestroy(child));
				}
			}
		}
	}

	void SpawnRandomBlock() {
		int offset = rand.Next (0, 3);
		float xOffset = offset * 0.32f;
		Vector3 spawnPos = this.gameObject.transform.position + Vector3.right * xOffset;
		GameObject newBlock = (GameObject)Instantiate (blockPrefab, spawnPos, Quaternion.identity);
		newBlock.transform.parent = parentContainer.transform;
		Sprite randSprite = blockSprites [rand.Next (0, blockSprites.Count)];
		SpriteRenderer spRenderer = newBlock.GetComponent<SpriteRenderer> ();
		spRenderer.sprite = randSprite;
		NetworkServer.Spawn (newBlock);
	}
}

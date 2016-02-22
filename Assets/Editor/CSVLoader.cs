using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using TiledSharp;
using Utility;

public class CSVLoader : EditorWindow {

	public TextAsset fileToLoad;
	public Sprite sprites;
	public Transform parentTransform;
	public GameObject spriteBase;
	public TextAsset colliderFile;
	public GameObject colliderBase;

	// TMX Parsing Info
	public TextAsset tmxFile;
	public string baseLayerName = "";
	public string optionalLayerName = "";
	public GameObject optionalBase;
	public string colliderLayerName = "";
	public GameObject fireDino;
	public string enemyLayerName = "";

	public string[] options;
	public Sprite[] allSprites;
	public string[] files;
	public int index;

	int[,] tileMapping;

	[MenuItem ("Tools/CSVLoader")]
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(CSVLoader));
	}

//	public Sprite[] SpriteArray = {};

	void OnGUI () {
		// The actual window code goes here
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("CSV File", GUILayout.Width(128));
		fileToLoad = (TextAsset) EditorGUILayout.ObjectField(fileToLoad, typeof(TextAsset),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Collider JS File", GUILayout.Width(128));
		colliderFile = (TextAsset) EditorGUILayout.ObjectField(colliderFile, typeof(TextAsset),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Parent Transform", GUILayout.Width(128));
		parentTransform = (Transform) EditorGUILayout.ObjectField(parentTransform, typeof(Transform),true,GUILayout.Width(128));
		if (parentTransform != null) 
			parentTransform.position = new Vector3 (0, 0, 0);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("SpritePrefab", GUILayout.Width(128));
		spriteBase = (GameObject) EditorGUILayout.ObjectField(spriteBase, typeof(GameObject),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("ColliderPrefab", GUILayout.Width(128));
		colliderBase = (GameObject) EditorGUILayout.ObjectField(colliderBase, typeof(GameObject),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("OptionalPrefab", GUILayout.Width(128));
		optionalBase = (GameObject) EditorGUILayout.ObjectField(optionalBase, typeof(GameObject),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button("Generate Tilemap", GUILayout.Width(128)))
		{   
			if (fileToLoad != null) {
				tileMapping = CSVLoader.ParseCSV (fileToLoad);
				Debug.Log (allSprites.Length);
				GenerateTiles ();
			}
		}

		if (GUILayout.Button("Generate Colliders", GUILayout.Width(128)))
		{   
			if (colliderFile != null) {
				GenerateColliders (colliderFile);
			}
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button("Find Platforms", GUILayout.Width(128)))
		{   
			if (fileToLoad != null) {
				ItemPlacements items = parentTransform.gameObject.AddComponent<ItemPlacements> ();
				tileMapping = CSVLoader.ParseCSV (fileToLoad);
				items.FindItemPlacements (tileMapping);
			}
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TMX File", GUILayout.Width(128));
		tmxFile = (TextAsset) EditorGUILayout.ObjectField(tmxFile, typeof(TextAsset),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Base Layer", GUILayout.Width(128));
		baseLayerName = (string) EditorGUILayout.TextField(baseLayerName, GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Collider Layer", GUILayout.Width(128));
		colliderLayerName = (string) EditorGUILayout.TextField(colliderLayerName, GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Optional Tile Layer", GUILayout.Width(128));
		optionalLayerName = (string) EditorGUILayout.TextField(optionalLayerName, GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Enemy Layer", GUILayout.Width(128));
		enemyLayerName = (string) EditorGUILayout.TextField(enemyLayerName, GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("FireDinoEnemy Prefab", GUILayout.Width(128));
		fireDino = (GameObject) EditorGUILayout.ObjectField(fireDino, typeof(GameObject),true,GUILayout.Width(128));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button("Parse TMX", GUILayout.Width(128)))
		{   
			if (tmxFile != null) {
				//ItemPlacements items = parentTransform.gameObject.AddComponent<ItemPlacements> ();
				//tileMapping = CSVLoader.ParseCSV (fileToLoad);
				//items.FindItemPlacements (tileMapping);
				var map = new TmxMap("Assets/Levels/" + tmxFile.name + ".xml");
//				var objectLayer = map.Tilesets["Tile Layer 1"];
				//Debug.Log (map.Tilesets);
				GenerateTiles(map);
			}
		}
		GUILayout.EndHorizontal ();

		if (!Directory.Exists(Application.dataPath + "/Tilemaps/"))
		{
			//Directory.CreateDirectory(Application.dataPath + "/Tilemaps/");
			AssetDatabase.CreateFolder("Assets", "Tilemaps");
			AssetDatabase.Refresh();
			Debug.Log("Created Tilemaps Directory");
		}
		files = Directory.GetFiles(Application.dataPath + "/Tilemaps/", "*.png");
		options = new string[files.Length];
		EditorGUILayout.LabelField("Tile Map", GUILayout.Width(256));
		for(int i = 0; i < files.Length; i++)
		{
			options[i] = files[i].Replace(Application.dataPath + "/Tilemaps/", "");
		}
		index = EditorGUILayout.Popup(index, options, GUILayout.Width(256));
		allSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Tilemaps/" + options[index]).Select(x => x as Sprite).Where(x => x != null).ToArray();	
	}

	public void GenerateTiles() {
		for (int x = 0; x < tileMapping.GetLength (0); x++) {
			for (int y = 0; y < tileMapping.GetLength (1); y++) {
				float xpos = y * 0.32f;
				float ypos = x * -0.32f;
				int tile = tileMapping [x, y];
				if (tile >= 0) {
					GameObject go = Instantiate (spriteBase);
					go.transform.position = new Vector3 (xpos, ypos, go.transform.position.z);
					go.GetComponent<SpriteRenderer> ().sprite = allSprites [tile];
					go.transform.parent = parentTransform;
				}
			}
		}
	}

	public void GenerateTiles(TmxMap map) {

		if (baseLayerName != "") {
			TmxLayer baseLayer = map.Layers [baseLayerName];
			foreach (var tile in baseLayer.Tiles) {
				int val = tile.Gid - 1;
				if (val != -1) {
					float xpos = tile.X * 0.32f;
					float ypos = tile.Y * -0.32f;
					GameObject go = Instantiate (spriteBase);
					go.transform.position = new Vector3 (xpos, ypos, go.transform.position.z);
					go.GetComponent<SpriteRenderer> ().sprite = allSprites [val];
					go.transform.parent = parentTransform;
				}
			}
		}

		if (optionalLayerName != "") {

			TmxLayer optionalLayer = map.Layers [optionalLayerName];
			//parentTransform.gameObject.AddComponent<RandomCreation> ();
			RandomCreation rCreate = parentTransform.gameObject.GetComponent<RandomCreation> ();
			rCreate.randomEntities = new List<GameObject> ();
			rCreate.randChances = new List<float> ();
			foreach (var tile in optionalLayer.Tiles) {
				int val = tile.Gid - 1;
				if (val != -1) {
					float xpos = tile.X * 0.32f;
					float ypos = tile.Y * -0.32f;
					GameObject go = Instantiate (optionalBase);
					go.transform.position = new Vector3 (xpos, ypos, go.transform.position.z);
					go.GetComponent<SpriteRenderer> ().sprite = allSprites [val];
					go.transform.parent = parentTransform;
					rCreate.randomEntities.Add (go);
					rCreate.randChances.Add (0.5f);
				}
			}
		}

		if (colliderLayerName != "") {

			TmxObjectGroup colliderGroup = map.ObjectGroups [colliderLayerName];
			foreach (var item in colliderGroup.Objects) {
				// instantiate the collider
				GameObject go = Instantiate (colliderBase);
				BoxCollider2D collider = go.GetComponent<BoxCollider2D> ();

				// extract the information
				float colWidth = (float)item.Width * 0.01f;
				float colHeight = (float)item.Height * 0.01f;
				float xPos = (float)item.X * 0.01f + (colWidth / 2);
				float yPos = (float)item.Y * -0.01f - (colHeight / 2);

				// position the collider
				collider.size = new Vector2 (colWidth, colHeight);
				go.transform.position = new Vector3 (xPos, yPos, go.transform.position.z);
				go.transform.parent = parentTransform;
			}
		}

		if (enemyLayerName != "") {

			TmxObjectGroup enemyGroup = map.ObjectGroups [enemyLayerName];
			foreach (var enemy in enemyGroup.Objects) {
				// instantiate the enemy
				GameObject go = null;
				if (enemy.Name == "FireDino") {
					go = Instantiate (fireDino);
				}

				if (go == null)
					continue;
				// extract the information
				float xPos = (float)enemy.X * 0.01f + (0.16f);
				float yPos = (float)enemy.Y * -0.01f - (0.16f);

				// position the enemy
				go.transform.position = new Vector3 (xPos, yPos, go.transform.position.z);
				go.transform.parent = parentTransform;
			}
		}
	}

	public void GenerateColliders(TextAsset data) {
		//var N = JSON.Parse(the_JSON_string);
		var N = JSON.Parse(data.text);
		for (int i = 0; i < N.Count; i++) {
			JSONNode colliderInfo =  N [i];
			GameObject go = Instantiate (colliderBase);
			BoxCollider2D collider = go.GetComponent<BoxCollider2D> ();
			float colWidth = colliderInfo ["width"].AsInt * 0.01f;
			float colHeight = colliderInfo["height"].AsInt * 0.01f;
			collider.size = new Vector2 (colWidth, colHeight);
			float xPos = colliderInfo ["x"].AsInt * 0.01f + (colWidth/2);
			float yPos = colliderInfo ["y"].AsInt * -0.01f - (colHeight/2);
			go.transform.position = new Vector3 (xPos, yPos, go.transform.position.z);
			go.transform.parent = parentTransform;
		}
	}
		

	public static int[,] ParseCSV(TextAsset file) {
		string[] lines = file.text.Split('\n');
		int limit = lines.Length;
		if (lines [lines.Length - 1] == "") {
			limit--;
		} 
		int[,] result = new int[limit, lines [0].Split (',').Length];
		for (var i = 0; i < limit; i ++ ) {
			string[] parts = lines[i].Split(","[0]);
			for ( var x = 0; x < parts.Length; x ++ ) {
				result [i, x] = int.Parse (parts [x]);
			}
		}
		return result;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Linq;
using SimpleJSON;

public class CSVLoader : EditorWindow {

	public TextAsset fileToLoad;
	public Sprite sprites;
	public Transform parentTransform;
	public GameObject spriteBase;
	public TextAsset colliderFile;
	public GameObject colliderBase;

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

//		// "target" can be any class derrived from ScriptableObject 
//		// (could be EditorWindow, MonoBehaviour, etc)
//		ScriptableObject target = this;
//		SerializedObject so = new SerializedObject(target);
//		SerializedProperty stringsProperty = so.FindProperty("SpriteArray");
//
//		EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
//		so.ApplyModifiedProperties(); // Remember to apply modified properties

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

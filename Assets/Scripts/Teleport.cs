using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	public float toX;
	public float toY;
	public bool isEnd = false;

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//
//	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			other.transform.position = new Vector3 (toX, toY, other.transform.position.z);
//			Camera.main.transform.position = new Vector3 (toX, toY, Camera.main.transform.position.z);
		}
	}
}

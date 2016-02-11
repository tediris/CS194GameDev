using UnityEngine;
using System.Collections;

public class RotateSpriteZ : MonoBehaviour {

	Transform transform;
	public float speed = 100f;

	// Use this for initialization
	void Start () {
		transform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up, Time.deltaTime * speed);
	}
}

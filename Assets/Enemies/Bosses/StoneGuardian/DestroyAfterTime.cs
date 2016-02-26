using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifetime;

	void CustomDestroy(GameObject o)
	{
		o.transform.position = new Vector3(100000000.0f,10000000.0f,10000000.0f);
		StartCoroutine (DestroyOnNextFrame (o));
	}

	IEnumerator DestroyOnNextFrame(GameObject o) { 
		yield return new WaitForFixedUpdate ();
		Destroy (o);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyObject (lifetime));
	}

	IEnumerator DestroyObject(float time) {
		yield return new WaitForSeconds (time);
		CustomDestroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

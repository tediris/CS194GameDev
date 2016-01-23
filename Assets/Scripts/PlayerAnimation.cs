using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public float threshold = 0.05f;

	string boolName = "playerMoving";

	Rigidbody2D playerBody;
	Animator anim;
	Animator[] childAnims;

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		childAnims = GetComponentsInChildren<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (playerBody.velocity.x) > threshold) {
			setBools (true);
		} else {
			setBools (false);
		}
	}

	void setBools(bool state) {
		anim.SetBool (boolName, state);
		foreach (Animator childAnim in childAnims) {
			childAnim.SetBool (boolName, state);
		}
	}

	public void Rescan() {
		childAnims = GetComponentsInChildren<Animator> ();
	}
}

using UnityEngine;
using System.Collections;

public class PetGenericInteract : MonoBehaviour {

	public string actionMessage = ""; //Put the message you want to have display on activate here
	public PetAction petScript; //Put the script your pet will use on activate here
	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void Activate() {
		petScript.Activate ();
	}

	public void Setup(GameObject player) {
		petScript.Setup (player);
	}
}

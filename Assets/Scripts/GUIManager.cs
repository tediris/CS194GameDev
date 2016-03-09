using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameObject netManager;

	// Use this for initialization
	void Start () {
		netManager = GameObject.Find ("Menu");
		netManager.SetActive(false);
	}
}

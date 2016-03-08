using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	public GameObject petForSale;
	public int price = 30;

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<PlayerShop> ().EnableBuy (petForSale, price);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<PlayerShop> ().DisableBuy ();
		}
	}
}

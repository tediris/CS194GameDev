using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerShop : NetworkBehaviour {

	public List<GameObject> pets;

	bool canBuy = false;

	GameObject petAvailable;
	int buyPrice;

	CoinCollector coinCol;
	PlayerControl control;

	NotificationText shopText;

	// Use this for initialization
	void Start () {
		coinCol = GetComponent<CoinCollector> ();
		control = GetComponent<PlayerControl> ();
		shopText = GameObject.Find ("Notice").GetComponent<NotificationText> ();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void Buy() {
		if (canBuy) {
			if (coinCol.numCoins >= buyPrice) {
				coinCol.numCoins = coinCol.numCoins - buyPrice;
				coinCol.SetText (coinCol.numCoins);

				CmdSpawnPet (pets.IndexOf(petAvailable));
			}
		}
	}

	[Command]
	void CmdSpawnPet(int petIndex) {
		Debug.Log ("Received command call");
		Rigidbody2D playerBody = this.gameObject.GetComponent<Rigidbody2D> ();
		GameObject pet = pets [petIndex];
		GameObject newPet = (GameObject) Instantiate (pet, playerBody.position, Quaternion.identity);
		newPet.GetComponent<PetFollow> ().playerTarget = playerBody;
		newPet.GetComponent<PetGenericInteract> ().Setup(this.gameObject);
		NetworkServer.Spawn (newPet);
		RpcSetPet (newPet);
	}

	[ClientRpc]
	void RpcSetPet(GameObject newPet) {
		Debug.Log (control);
		Debug.Log (newPet);
		control.SetPet (newPet);
		shopText.SetTimedNotice ("Press " + control.input.Button("Activate") + " to activate your pet's ability!", Color.white, 5f);
	}

	public void EnableBuy(GameObject petForSale, int price) {
		if (isLocalPlayer) {
			canBuy = true;
			petAvailable = petForSale;
			buyPrice = price;
			shopText.SetNotice (price + " coins! Press the " + control.input.Button ("Buy") + " key to buy", Color.white);
		}
	}

	public void DisableBuy() {
		if (isLocalPlayer) {
			canBuy = false;
			shopText.ClearNotice ();
		}
	}
}

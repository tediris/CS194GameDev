using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerShop : NetworkBehaviour {

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

				CmdSpawnPet (petAvailable, this.gameObject);
			}
		}
	}

	[Command]
	void CmdSpawnPet(GameObject pet, GameObject player) {
		Rigidbody2D playerBody = player.GetComponent<Rigidbody2D> ();
		GameObject newPet = (GameObject) Instantiate (pet, playerBody.position, Quaternion.identity);
		newPet.GetComponent<PetFollow> ().playerTarget = playerBody;
		newPet.GetComponent<PetGenericInteract> ().Setup(player);
		NetworkServer.Spawn (newPet);
		RpcSetPet (newPet);
	}

	[ClientRpc]
	void RpcSetPet(GameObject newPet) {
		if (isLocalPlayer) {
			Debug.Log (control);
			Debug.Log (newPet);
			control.SetPet (newPet);
			shopText.SetTimedNotice ("Press " + control.input.Button("Activate") + " to activate your pet's ability!", Color.white, 5f);
		}
	}

	public void EnableBuy(GameObject petForSale, int price) {
		canBuy = true;
		petAvailable = petForSale;
		buyPrice = price;
		shopText.SetNotice (price + " coins! Press the "+ control.input.Button("Buy") + " key to buy", Color.white);
	}

	public void DisableBuy() {
		canBuy = false;
		shopText.ClearNotice ();
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerShop : NetworkBehaviour {

	bool canBuy = false;

	GameObject petAvailable;
	int buyPrice;

	CoinCollector coinCol;
	PlayerControl control;



	// Use this for initialization
	void Start () {
		coinCol = GetComponent<CoinCollector> ();
		control = GetComponent<PlayerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
		}
	}

	public void EnableBuy(GameObject petForSale, int price) {
		Debug.Log ("Can buy!");
		canBuy = true;
		petAvailable = petForSale;
		buyPrice = price;
	}

	public void DisableBuy() {
		Debug.Log ("Can't buy...");
		canBuy = false;
	}
}

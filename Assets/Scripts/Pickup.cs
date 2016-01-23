using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickup : NetworkBehaviour {

	public GameObject[] tools;
	public int toolToEquipID = 0;
	PickupManager manager;

	void Start() {
		//create the tool pickup visual
//		if (isServer) {
//			gameObject.name = "Pickup" + GameObject.Find("PickupManager").GetComponent<PickupManager> ().getID();
//		}
		SetupPickup();
	}

	public void SetPickup(int ID) {
		toolToEquipID = ID;
	}

	void SetupPickup() {
		GameObject item = tools[toolToEquipID];
		GameObject toEquip = Instantiate (item) as GameObject;
		toEquip.transform.parent = this.transform;
		toEquip.transform.position = this.transform.position;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (!isServer) {
			return;
		}
		if (other.gameObject.CompareTag ("Player")) {
			//NetworkServer.Destroy (gameObject);
			//other.gameObject.GetComponent<ItemManager> ().Equip (toolToEquipID);
			int playerNum = other.gameObject.GetComponent<NetSetup> ().playerNum;
			CmdPickupEnable(playerNum, toolToEquipID, gameObject.name);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (!isServer) {
			return;
		}
		if (other.gameObject.CompareTag ("Player")) {
			//NetworkServer.Destroy (gameObject);
			//other.gameObject.GetComponent<ItemManager> ().Equip (toolToEquipID);
			int playerNum = other.gameObject.GetComponent<NetSetup> ().playerNum;
			CmdPickupEnable(playerNum, -1, "");
		}
	}


	/*
	void OnTriggerStay2D (Collider2D other) {
//		if (isLocalPlayer) {
			if (other.gameObject.CompareTag ("Player")) {
				if (Input.GetKeyDown (KeyCode.E)) {
				Debug.Log ("Pickup started");
					other.gameObject.GetComponent<ItemManager> ().Equip (toolToEquipID);
					PickedUp (gameObject);
				}
			}
//		}
	}

	[ClientCallback]
	void PickedUp(GameObject tool) {
		CmdDestroyItem (tool);
	}

	[Command]
	void CmdDestroyItem(GameObject item) {
		NetworkServer.Destroy (item);
	}
	*/

	[Command]
	void CmdPickupEnable (int playerNum, int toolID, string name) {
		//Debug.Log (carryManager);
		//GameObject carried = carryManager.GetPlayer (playerNum);
		GameObject picker = GameObject.Find("Player" + playerNum);
		picker.GetComponent<ItemManager> ().SetEnabled (toolID, name);
	}

}

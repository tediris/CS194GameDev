using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour {

	public Vector3 hipOffset;
	public GameObject[] tools;
	public GameObject pickupPrefab;
//	PlayerAnimation animationScript;
	[SyncVar] public int itemAvailable = -1;
	NetSetup netInfo;
	string pickupName = "";
	bool carryingItem = false;
	int carriedItemID = -1;

	// Use this for initialization
	void Start () {
//		animationScript = GetComponent<PlayerAnimation> ();
		netInfo = GetComponent<NetSetup>();
	}
//	
//	// Update is called once per frame
	void Update () {
		if (itemAvailable != -1) {
			if (Input.GetKeyDown (KeyCode.E)) {
				if (!carryingItem) {
					carryingItem = true;
					EquipItem (itemAvailable);
				} else {
					carryingItem = false;
					DequipItem ();
				}
			}
		}
	}

	public void SetEnabled(int enabled, string name) {
		itemAvailable = enabled;
		pickupName = name;
	}

//	public void Equip(int toolID) {
//		GameObject tool = tools [toolID];
//		EquipItem (tool);
////		GameObject toEquip = Instantiate (tool) as GameObject;
////		toEquip.transform.parent = this.transform;
////		Vector3 offset = new Vector3 (this.transform.localScale.x * hipOffset.x, hipOffset.y, hipOffset.z);
////		toEquip.transform.position = this.transform.position + offset;//new Vector2(this.transform.position.x + hipOffset.x, this.transform.;
////		animationScript.Rescan();
//	}

	[ClientCallback]
	void DequipItem() {
		CmdSpawnPickup (carriedItemID, netInfo.playerNum);
	}

	[Command]
	void CmdSpawnPickup (int itemID, int playerNum) {
		GameObject toSpawn = Instantiate (pickupPrefab) as GameObject;
		toSpawn.transform.position = transform.position;
		toSpawn.GetComponent<Pickup>().SetPickup (itemID);
		NetworkServer.Spawn (toSpawn);
		RpcUnsetItemParent (itemID, playerNum);
	}

	[ClientRpc]
	void RpcUnsetItemParent(int itemID, int playerNum) {
		GameObject player = GameObject.Find("Player" + playerNum);
		foreach (Transform child in player.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	[ClientCallback]
	void EquipItem(int itemID) {
		carriedItemID = itemID;
		CmdEquipItem (itemID, netInfo.playerNum);
	}

	[Command]
	void CmdEquipItem(int itemID, int playerNum) {
		
//		NetworkServer.SpawnWithClientAuthority (item, connectionToClient);
//		NetworkServer.Spawn(item);

		Destroy (GameObject.Find(pickupName));
		RpcSetItemParent (itemID, playerNum);
	}

	[ClientRpc]
	void RpcSetItemParent(int itemID, int playerNum) {
		GameObject item = tools[itemID];
		GameObject toEquip = Instantiate (item) as GameObject;
		toEquip.transform.parent = this.transform;
		Vector3 offset = new Vector3 (this.transform.localScale.x * hipOffset.x, hipOffset.y, hipOffset.z);
		toEquip.transform.position = this.transform.position + offset;
		toEquip.GetComponent<LocalTool> ().playerNum = playerNum;
//		Debug.Log (item);
		GameObject player = GameObject.Find("Player" + playerNum);
//		Debug.Log (this);
		toEquip.transform.parent = player.transform;

	}

}

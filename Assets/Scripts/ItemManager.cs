using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour {

	public Vector3 hipOffset;
	public GameObject[] tools;
//	PlayerAnimation animationScript;
	[SyncVar]
	public int itemAvailable = -1;
	NetSetup netInfo;
	string pickupName = "";

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
				EquipItem (itemAvailable);
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
	void EquipItem(int itemID) {
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

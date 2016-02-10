using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinCollector : NetworkBehaviour {

	private Text printedNumCoins;
	[SyncVar]
	public int numCoins = 0;

	void Start() {
		printedNumCoins = GameObject.Find ("NumCoins").GetComponent<Text> ();
		Debug.Log (printedNumCoins);
	}

	[Command]
	public void CmdIncrementCoins(int value) {
		Debug.Log ("Adding coins to " + gameObject.name);
		//RpcIncrementCoins (value);
		numCoins += value;
	}

	[ClientRpc]
	void RpcIncrementCoins(int value) {
		if (isLocalPlayer) {
			numCoins += value;
			printedNumCoins.text = ":" + numCoins;
			Debug.Log (printedNumCoins.text);
		}
	}
}

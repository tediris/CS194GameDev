using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinCollector : NetworkBehaviour {

	[SyncVar]
	public int numCoins = 0;
	private Text coinText;

	void Start() {
		coinText = GameObject.Find ("NumCoins").GetComponent<Text> ();
	}

	[Command]
	public void CmdIncrementCoins(int value) {
		Debug.Log ("Adding coins to " + gameObject.name);
		numCoins += value;
		RpcIncrementCoins (numCoins);
	}

	[ClientRpc]
	void RpcIncrementCoins(int value) {
		if (isLocalPlayer) {
			SetText (value);
		}
	}

	public void SetText(int value) {
		coinText.text = value.ToString ();
	}
}

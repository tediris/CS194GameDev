using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinCollector : NetworkBehaviour {

	[SyncVar]
	public int numCoins = 0;

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
		}
	}
}

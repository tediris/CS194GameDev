using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ColorControl : NetworkBehaviour {

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> (); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[ClientRpc]
	void RpcSetColor(Color color) {
		sprite.color = color;
	}

	[ClientCallback]
	void SetColor(Color color) {
		CmdSetColor (color);
	}

	[Command]
	public void CmdSetColor(Color color) {
		RpcSetColor (color);
	}
}

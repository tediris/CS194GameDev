using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class DestroyAllChildren : NetworkBehaviour {

	public void ResetLevel() {
		var children = new List<GameObject>();
		foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}
}

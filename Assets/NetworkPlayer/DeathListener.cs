using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DeathListener : NetworkBehaviour {

	public virtual bool destroyOnUse() {
		return true;
	}

	// Override this if you want to have stuff happen on player death, 
	// then add to player_health script in list
	public virtual void PlayerDied(GameObject player) {
		/* Empty */
	}
}

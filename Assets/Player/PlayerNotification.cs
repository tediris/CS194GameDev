using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNotification : NetworkBehaviour {

	private NotificationText noticeText;

	// Use this for initialization
	void Start () {
		noticeText = GameObject.Find ("Notice").GetComponent<NotificationText> ();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void SetPlayerNotification(string notice, Color color) {
		Debug.Log ("SET NOTICE");
		if (isLocalPlayer) {
			noticeText.SetNotice (notice, color);
		}
	}

	public void SetPlayerTimedNotification(string notice, Color color, float time) {
		if (isLocalPlayer) {
			noticeText.SetTimedNotice (notice, color, time);
		}
	}

	public void ClearPlayerNotification() {
		if (isLocalPlayer) {
			noticeText.ClearNotice ();
		}
	}
}

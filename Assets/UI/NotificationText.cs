using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour {

	Text notice;
	string timedNotice;
	bool onTimedNotice = false;

	// Use this for initialization
	void Start () {
		notice = GetComponent<Text> ();
	}

	public void SetNotice(string text, Color color) {
		notice.text = text;
		notice.color = color;
	}

	public void ClearNotice() {
		if (onTimedNotice) {
			notice.text = timedNotice;
		} else {
			notice.text = "";
		}
	}

	public void SetTimedNotice(string text, Color color, float time) {
		if (!onTimedNotice) {
			timedNotice = text;
			onTimedNotice = true;
			SetNotice (text, color);
			StartCoroutine (ClearTimer (time));
		} else {
			Debug.LogWarning ("Setting multiple timed notices");
		}
	}

	IEnumerator ClearTimer(float time) {
		yield return new WaitForSeconds (time);
		onTimedNotice = false;
		ClearNotice ();
	}
}

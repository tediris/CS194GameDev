using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour {

	Text notice;
	string timedNotice;
	int onTimedNotice = 0;

	// Use this for initialization
	void Start () {
		notice = GetComponent<Text> ();
	}

	public void SetNotice(string text, Color color) {
		notice.text = text;
		notice.color = color;
	}

	public void ClearNotice() {
		if (onTimedNotice != 0) {
			notice.text = timedNotice;
		} else {
			notice.text = "";
		}
	}

	public void SetTimedNotice(string text, Color color, float time) {
		timedNotice = text;
		onTimedNotice++;
		SetNotice (text, color);
		StartCoroutine (ClearTimer (time));
	}

	IEnumerator ClearTimer(float time) {
		yield return new WaitForSeconds (time);
		onTimedNotice--;
		ClearNotice ();
	}
}

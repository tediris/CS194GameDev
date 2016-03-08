using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour {

	Text notice;

	// Use this for initialization
	void Start () {
		notice = GetComponent<Text> ();
	}

	public void SetNotice(string text, Color color) {
		Debug.Log ("Notice set");
		notice.text = text;
		notice.color = color;
	}

	public void ClearNotice() {
		Debug.Log ("Notice cleared");
		notice.text = "";
	}
}

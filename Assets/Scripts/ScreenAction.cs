using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenAction : MonoBehaviour {

	Image screen;
	public float flashSpeed = 0.2f;

	bool flashing = false;

	Color targetColor;

	// Use this for initialization
	void Start () {
		screen = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (flashing) {
			screen.color = Color.Lerp (screen.color, targetColor, flashSpeed);
			if (screen.color.a >= 0.95f * targetColor.a) {
				targetColor = GetTransparentTarget ();
			} else if (screen.color.a <= 0.05f) {
				flashing = false;
			}
		}
	}

	public void Flash(Color color) {
		targetColor = color;
		flashing = true;
	}

	Color GetTransparentTarget() {
		return new Color (targetColor.r, targetColor.g, targetColor.b, 0.0f);
	}


}

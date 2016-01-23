using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class TorchFlicker : MonoBehaviour {

	public float rangeRange = 5.0f;
	public float intensityRange = 0.25f;
	public float rangeRate = 2.0f;
	public float intensityRate = 3.0f;
	Light torch;
	float middleIntensity, middleRange;


	// Use this for initialization
	void Start () {
		torch = GetComponent<Light> ();
		middleRange = torch.range;
		middleIntensity = torch.intensity;
		StartCoroutine (FlickerRange());
		StartCoroutine (FlickerIntensity());
//		Invoke ("Flicker", 0.1f);
	}
	
	// Update is called once per frame
//	void Update () {
//	}

	IEnumerator FlickerRange() {
		while (true) {
			torch.range = middleRange + rangeRange / 2 * Mathf.Sin (Time.fixedTime * 2 * Mathf.PI * rangeRate);
			yield return new WaitForSeconds (0.1f);
		}
//		Invoke ("Flicker", 0.1f);
	}

	IEnumerator FlickerIntensity() {
		while (true) {
			torch.intensity = (middleIntensity + intensityRange / 2 * Mathf.Sin (Time.fixedTime * 2 * Mathf.PI * intensityRate));
			yield return new WaitForSeconds (0.1f);
		}
	}
}

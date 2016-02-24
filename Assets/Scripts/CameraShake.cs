using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	float duration = 0.25f;
	bool shaking = false;
	float lerpDist = 0.2f;

	Vector3 target;

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
	void Update () {
		if (shaking) {
			transform.position = Vector3.Lerp(transform.position, target, lerpDist);
		}
	}

	public void Shake(float intensity) {
		shaking = true;
		SingleShake (intensity);
		StartCoroutine (StartShakeTimer());
		StartCoroutine (ShakeRepeat(intensity));
	}

	void SingleShake(float intensity) {
		Vector3 shakeAddition = new Vector3 (Random.Range (-intensity, intensity), Random.Range (-intensity, intensity), 0.0f);
		target = Vector3.Lerp(transform.position, transform.position + shakeAddition, lerpDist);
	}

	IEnumerator ShakeRepeat(float intensity) {
		yield return new WaitForSeconds (0.1f);
		if (shaking) {
			SingleShake (intensity);
			StartCoroutine (ShakeRepeat(intensity));
		}
	}

	IEnumerator StartShakeTimer() {
		yield return new WaitForSeconds (duration);
		shaking = false;
	}
}

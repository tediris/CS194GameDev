using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GaurdianControl : NetworkBehaviour {

	public StoneSpawner leftSpawn;
	public StoneSpawner rightSpawn;
	public StoneBarrier leftBarrier;
	public StoneBarrier rightBarrier;

	public enum GaurdianState {
		Idle = 0,
		Left = 1,
		Right = 2,
		Both = 3
	}

	public float timeInState = 10.0f;
	public float timeInIdle = 5.0f;
	float timer = 0f;
	GaurdianState state = GaurdianState.Idle;
	System.Random rand;
	Animator anim;

	[SyncVar]
	public int health = 3;

	// Use this for initialization
	void Start () {
		if (!isServer) {
			this.enabled = false;
		}
		rand = new System.Random ();
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!isServer)
			return;
		if (other.tag == "FallingBlock") {
			// apply damage
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
		timer += Time.deltaTime;
		if (state == GaurdianState.Idle) {
			if (timer > timeInIdle) {
				timer = 0f;
				state = (GaurdianState)rand.Next (1, 4);
				if (state == GaurdianState.Left) {
					leftSpawn.active = true;
					leftBarrier.CmdDisable ();
					anim.SetBool ("raisingLeft", true);
				} else if (state == GaurdianState.Right) {
					rightSpawn.active = true;
					rightBarrier.CmdDisable ();
					anim.SetBool ("raisingRight", true);
				} else {
					leftSpawn.active = true;
					leftBarrier.CmdDisable ();
					rightSpawn.active = true;
					rightBarrier.CmdDisable ();
					anim.SetBool ("raisingBoth", true);
				}
			}
		} else {
			if (timer > timeInState) {
				timer = 0f;
				state = GaurdianState.Idle;
				leftSpawn.active = false;
				leftBarrier.CmdEnable ();
				rightSpawn.active = false;
				rightBarrier.CmdEnable ();
				anim.SetBool ("raisingLeft", false);
				anim.SetBool ("raisingRight", false);
				anim.SetBool ("raisingBoth", false);
			}
		}
	}
}

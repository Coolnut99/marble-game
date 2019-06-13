using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFlag : MonoBehaviour {

	private bool checkpointOn;
	private MeshRenderer meshR;

	// Use this for initialization
	void Start () {
		//Color color = new Color(1, 1, 1, 0.5f);
		checkpointOn = true;
		meshR = GetComponent<MeshRenderer>();
	}
	
	public void SetCheckpointOff() {
		checkpointOn = false;
	}

	public void SetCheckpointOn() {
		checkpointOn = true;
	}

	public bool Checkpoint() {
		return checkpointOn;
	}

	public void SetCheckpointColor(bool c) {
		Color color = new Color(1f, 0f, 1f, 0.1f);
		Color color2 = new Color(1f, 0f, 1f, 0.1f);
		if (c == false) {
			meshR.material.color = color;
			Debug.Log ("Color changed");
		} else {
			meshR.material.color = color2;
			Debug.Log ("Color changed");
		}
	}
}

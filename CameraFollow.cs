using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Transform marblePos;

	[SerializeField]
	private float xDistance, zDistance, yDistance; // This is where the camera should be located

	// Use this for initialization
	void Start () {
		marblePos = GameObject.Find("Marble").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = transform.position;
		temp.x = marblePos.position.x + xDistance;
		temp.z = marblePos.position.z + zDistance;
		temp.y = marblePos.position.y + yDistance;
		transform.position = temp;
	}
}

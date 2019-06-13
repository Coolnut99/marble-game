using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is an obsolete part of Fred's Marble Game and may be deleted.
public class GameOverText : MonoBehaviour {

	private Text text;
	private Transform marblePos;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		marblePos = GameObject.Find("Marble").transform;
		text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (marblePos.position.y <= -10) {
			text.text = "GAME OVER FOOL!";
		}
	}
}

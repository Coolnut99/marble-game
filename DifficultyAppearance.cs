using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAppearance : MonoBehaviour {

	public bool EasyActive, MediumActive, HardActive;
	private int difficulty;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
		difficulty = GameObject.Find("Game Controller").GetComponent<Score>().gameDifficulty;
		if (difficulty == 0 && EasyActive == true) {
			this.gameObject.SetActive(true);
			Debug.Log("Active on Easy.");
		} else if (difficulty == 1 && MediumActive == true) {
			this.gameObject.SetActive(true);
			Debug.Log("Active on Medium.");
		} else if (difficulty == 2 && HardActive == true) {
			this.gameObject.SetActive(true);
			Debug.Log("Active on Hard.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

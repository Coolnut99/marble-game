using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

	private Text text;
	// Use this for initialization
	void Start () {
		text = GameObject.Find("High Score Text").GetComponent<Text>();
		GameObject.Find("Game Controller").GetComponent<Score>().ResetScore();
		GameObject.Find("Game Controller").GetComponent<Score>().gameDifficulty = PlayerPrefsManager.GetDifficulty();
		GameObject.Find("Game Controller").GetComponent<Score>().SetHighScore(PlayerPrefsManager.GetDifficulty());
		text.text = "High Score: " + GameObject.Find("Game Controller").GetComponent<Score>().highScore;

		Debug.Log ("Difficulty is: " + GameObject.Find("Game Controller").GetComponent<Score>().gameDifficulty.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {
			Application.LoadLevel(1);
		}

		if (Input.GetKeyDown(KeyCode.I)) {
			Application.LoadLevel("Instructions");
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			Application.LoadLevel("Options");
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log("Quitting game");
			Application.Quit();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	public static Score instance;
	public float gameScore;
	public int gameLevel;
	public float highScore;
	public int gameDifficulty;

	// Use this for initialization
	void Awake () {
		MakeSingleton();
		gameDifficulty = PlayerPrefsManager.GetDifficulty();
		//PlayerPrefsManager.SetHighScoreEasy(1000f);
		gameScore = 0f;
		gameLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeSingleton() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void ResetScore() {
		gameScore = 0f;
		gameLevel = 1;
	}

	public void SetHighScore (int d) {
		if (d == 0) {
			highScore = PlayerPrefsManager.GetHighScoreEasy();
			Debug.Log("Easy Score");
		} else if (d == 1) {
			highScore = PlayerPrefsManager.GetHighScoreMedium();
			Debug.Log("Medium Score");
		} else if (d == 2) {
			highScore = PlayerPrefsManager.GetHighScoreHard();
			Debug.Log("Hard Score");
		} else {
			Debug.LogError ("Cannot load high score.");
		}
	}

	public void SaveNewHighScore(float newHighScore) {
		switch (gameDifficulty) {
			case 0:
			PlayerPrefsManager.SetHighScoreEasy(newHighScore);
			break;

			case 1:
			PlayerPrefsManager.SetHighScoreMedium(newHighScore);
			break;

			case 2:
			PlayerPrefsManager.SetHighScoreHard(newHighScore);
			break;

			default:
			Debug.LogError ("Difficulty unknown, cannot save high score.");
			break;

		}
	}
}

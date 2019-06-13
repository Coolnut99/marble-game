using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

//	public static PlayerPrefsManager instance;

	const string MASTER_VOLUME_KEY = "master_volume";

	const string HIGH_SCORE_KEY_EASY = "high_score_easy";
	const string HIGH_SCORE_KEY_MEDIUM = "high_score_medium";
	const string HIGH_SCORE_KEY_HARD = "high_score_hard";

	const string DIFFICULTY_KEY = "difficulty_key"; // 0 for Easy, 1 for Medium, 2 for Hard; default = Easy

	// Use this for initialization
	void Awake () {
	//	MakeSingleton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*
	void MakeSingleton() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}*/

	public static void SetHighScoreEasy (float score) {
		PlayerPrefs.SetFloat (HIGH_SCORE_KEY_EASY, score);
	}

	public static void SetHighScoreMedium (float score) {
		PlayerPrefs.SetFloat (HIGH_SCORE_KEY_MEDIUM, score);
	}

	public static void SetHighScoreHard (float score) {
		PlayerPrefs.SetFloat (HIGH_SCORE_KEY_HARD, score);
	}

	public static float GetHighScoreEasy () {
		return PlayerPrefs.GetFloat (HIGH_SCORE_KEY_EASY);
	}

	public static float GetHighScoreMedium () {
		return PlayerPrefs.GetFloat (HIGH_SCORE_KEY_MEDIUM);
	}

	public static float GetHighScoreHard () {
		return PlayerPrefs.GetFloat (HIGH_SCORE_KEY_HARD);
	}




	public static void SetMasterVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError ("Volume out of range.");
		}
	}

	public static float GetMasterVolume() {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}


	// Sets difficulty -- if out of range, default to Easy
	public static void SetDifficulty (int difficulty) {
		if (difficulty >= 0 && difficulty <= 2) {
			PlayerPrefs.SetInt(DIFFICULTY_KEY, difficulty);
		} else {
			Debug.LogWarning ("Difficulty out of range -- setting back to Easy");
			PlayerPrefs.SetInt(DIFFICULTY_KEY, 0);
		}
	}

	public static int GetDifficulty() {
		return PlayerPrefs.GetInt(DIFFICULTY_KEY);
	}
}

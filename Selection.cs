using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour {

	[SerializeField]
	private Text volumeText, difficultyText, helpText;

	[SerializeField]
	private GameObject [] arrow;

	private float volume;
	private int difficulty;

	// Selections from 0-3
	private int selection;

	// Use this for initialization
	void Start () {
		//Placeholders for now
		float v = PlayerPrefsManager.GetMasterVolume() * 10;
		SetVolume(v);
		SetDifficulty(PlayerPrefsManager.GetDifficulty());

		for (int i = 0; i < arrow.Length; i++) {
			arrow[i].SetActive(false);
		}
		selection = 0;
		SetSelection(selection);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			selection--;
			SetSelection(selection);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			selection++;
			SetSelection(selection);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow) && selection == 0) {
			SetVolume(1f);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) && selection == 0) {
			SetVolume(-1f);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow) && selection == 1) {
			SetDifficulty(1);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) && selection == 1) {
			SetDifficulty(-1);
		}

		if (Input.GetKeyDown(KeyCode.Return) && selection == 2) {
			SaveChanges();
			ReturnToMainMenu();
		}

		if (Input.GetKeyDown(KeyCode.Return) && selection == 3) {
			ReturnToMainMenu();
		}

	}

	void SetSelection(int sel) {
		if (sel > arrow.Length - 1) {
			Debug.Log("Resetting to 0");
			selection = 0;
		}

		if (sel < 0) {
			Debug.Log("Resetting to 3");
			selection = arrow.Length - 1;
		}

		for (int i = 0; i < arrow.Length; i++) {
			arrow[i].SetActive(false);
		}

		arrow[selection].SetActive(true);

		switch (selection) {
			case 0:
			helpText.text = "Adjust volume of game. Left to decrease, right to increase.";
			break;

			case 1:
			//helpText.text = "Adjust difficulty of game: Easy, Medium, Hard.";
			SetDifficulty(0);
			break;

			case 2:
			helpText.text = "Hit Enter to save changes and return to main menu.";
			break;

			case 3:
			helpText.text = "Hit Enter to return to main menu without saving.";
			break;
		}
	}

	void SetVolume (float vol) {
		volume += vol;
		if (volume >= 10) {
			volume = 10;
			volumeText.text = "Max";
		} else if (volume == 0) {
			volumeText.text = "Off";
		} else if (volume < 0) {
			volume = 0;
			volumeText.text = "Off";
		} else {
			volumeText.text = volume.ToString();
		}
	}

	void SetDifficulty (int dif) {
		difficulty += dif;
		if (difficulty > 2) {
			difficulty = 2;
		} else if (difficulty < 0) {
			difficulty = 0;
		}

		switch (difficulty) {
			case 0:
			difficultyText.text = "Easy";
			helpText.text = "Easy difficulty. More time, fewer hazards.";
			break;

			case 1:
			difficultyText.text = "Medium";
			helpText.text = "Medium difficulty. Average time and hazards.";
			break;

			case 2: 
			difficultyText.text = "Hard";
			helpText.text = "Hard difficulty. Less time to complete, and more hazards.";
			break;

			default:
			difficulty = 0;
			difficultyText.text = "Easy";
			helpText.text = "Easy difficulty. More time, fewer hazards.";
			Debug.LogWarning("Difficulty out of range -- resetting to Easy");
			break;
		}
	}

	void SaveChanges () {
		PlayerPrefsManager.SetMasterVolume(volume / 10f);
		PlayerPrefsManager.SetDifficulty(difficulty);
	}

	void ReturnToMainMenu() {
		Application.LoadLevel(0);
	}
}

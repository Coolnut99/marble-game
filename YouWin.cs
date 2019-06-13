using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YouWin : MonoBehaviour {

	private Text yourScoreText, highScoreText;

	// Use this for initialization
	void Start () {
		yourScoreText = GameObject.Find("Your Score Text").GetComponent<Text>();
		highScoreText = GameObject.Find("High Score Text").GetComponent<Text>();

		yourScoreText.text = "Your Score: " + GameObject.Find("Game Controller").GetComponent<Score>().gameScore;
		highScoreText.text = "High Score: " + GameObject.Find("Game Controller").GetComponent<Score>().highScore;

		GameObject.Find("Game Controller").GetComponent<Score>().SaveNewHighScore(GameObject.Find("Game Controller").GetComponent<Score>().highScore);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			GameObject.Find("Game Controller").GetComponent<Score>().ResetScore();
			Application.LoadLevel(0);
		}
	}
}

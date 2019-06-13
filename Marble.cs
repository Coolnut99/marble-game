using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marble : MonoBehaviour {

	//Game variables
	public float force = 10f;				// Amount of force given to marble if direction is pressed
	public float criticalTime = 15f;		// Time left before critical beeping sound
	public int timeRemaining = 60;			// Time remaining (can be changed in inspector)
	public int timeBonus = 10;				// Time given if a green cube is picked up (can be changed in inspector)
	private float distanceTraveled;			// Distance marble travelled -- use to give points per certain units of distance
	private Rigidbody myRigidBody;			// Marble's RigidBody component
	private Vector3 currentPos, newPos;		// Vector3s of the marble and NewPos is used to calculate distance travelled
	private float score = 0f;				// Player score
	private int timer;						// Time remaining
	private bool goal;						// Have we reached the level goal?
	private bool criticalIsPlaying;			// Is the "critical time left" sound playing?
	private bool canPause;					// Checks if you can pause the game (you can't during game over, for example)
	private float levelBonus = 1000f;		// Level bonus (times level)
	private Vector3 checkpoint; 			// Marble respawns here if it "dies"
	private float gameVolume;				// Volume of game from 0-1

	//Text and UI
	private Text scoreText;					// Player score text
	private Text gameOverText;				// Comment when bonus is picked up, etc.
											// Name is an artifact from early build of game when all it said was "GAME
											// OVER" when you lost

	private Text timerText;					// Time left
	private Text timeBonusText;				// Text shown when you clear a stage, grants a bonus of time left times 100
	private Text marbleText;				// Marble's "comments". For now, it is only a "Here we go!"
	private Text pauseGameText;				// Pause game text
	private Text quitText;					// Part of the pause menu, shows that you can quit by pressing "Q"

	//Sounds in the game
	private AudioSource woohoo;				// Woohoo when the marble clears the game
	private AudioSource marbleDeath;		// Wilhelm Scream when marble "dies"
	private AudioSource youLose;			// The Price is Right losing horns if you lose the game
	private AudioSource youSuck;			// A "YOU SUCK!" if you lose the game
	private AudioSource bonusItem;			// Chime when a bonus is picked up.
	private AudioSource bonusTime;			// Chime when a time bonus is picked up.
	private AudioSource criticalSound;		// Critical sound warning horns
	private AudioSource stageClear;			// Stage clear music
	private AudioSource explosion;			// Explosion when a marble hits a hazard
	private AudioSource checkpointSound;	// Chime when you touch a checkpoint
	private AudioSource backgroundMusic;	// Game background music

	// Use this for initialization
	void Start () {
		// Physics
		myRigidBody = GetComponent<Rigidbody>();

		// Text
		scoreText = GameObject.Find("Score").GetComponent<Text>();
		gameOverText = GameObject.Find("Text").GetComponent<Text>();
		timerText = GameObject.Find("Timer").GetComponent<Text>();
		timeBonusText = GameObject.Find("Time Bonus").GetComponent<Text>();
		marbleText = GameObject.Find("Marble Text").GetComponent<Text>();
		pauseGameText = GameObject.Find("Pause Game").GetComponent<Text>();
		quitText = GameObject.Find("Quit Game").GetComponent<Text>();

		// Sound Effects
		woohoo = GetComponent<AudioSource>();
		marbleDeath = GameObject.Find("OH NO").GetComponent<AudioSource>();
		youLose = GameObject.Find("You Lose").GetComponent<AudioSource>();
		youSuck = GameObject.Find("You Suck").GetComponent<AudioSource>();
		bonusItem = GameObject.Find("Bonus Item").GetComponent<AudioSource>();
		bonusTime = GameObject.Find("Bonus Time").GetComponent<AudioSource>();
		criticalSound = GameObject.Find("Critical Time").GetComponent<AudioSource>();
		stageClear = GameObject.Find("Stage Clear").GetComponent<AudioSource>();
		explosion = GameObject.Find("Explosion").GetComponent<AudioSource>();
		checkpointSound = GameObject.Find("Checkpoint Sound").GetComponent<AudioSource>(); 
		backgroundMusic = GameObject.Find("BGM 1").GetComponent<AudioSource>();
		gameVolume = PlayerPrefsManager.GetMasterVolume();

		// Start
		StartTheGame();

	}
	
	// FixedUpdate every few frames
	void FixedUpdate () {
		moveSphere();
		UpdateScore();
		if (transform.position.y <= -10) {
			Respawn();
		}
	}

	// Update is called once per frame
	void Update() {

		// Pauses the game
		if(Input.GetKeyDown(KeyCode.P) && canPause == true)	{
			if (Time.timeScale == 1f) {
				Time.timeScale = 0f;
				pauseGameText.gameObject.SetActive(true);
				quitText.gameObject.SetActive(true);
				AudioListener.volume = gameVolume / 3;

			} else if (Time.timeScale == 0f)	{
			Time.timeScale = 1f;
				pauseGameText.gameObject.SetActive(false);
				quitText.gameObject.SetActive(false);
				AudioListener.volume = gameVolume;
			}
		}

		//Quits the game
		if(Input.GetKeyDown(KeyCode.Q) && canPause == true && Time.timeScale == 0f) {
			if (score > GameObject.Find("Game Controller").GetComponent<Score>().highScore) {
				GameObject.Find("Game Controller").GetComponent<Score>().highScore = score;
				GameObject.Find("Game Controller").GetComponent<Score>().SaveNewHighScore(score);
			}
			Application.LoadLevel(0);
		}
	}

/// <summary> Functions within this program
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>

	// Initialize and reset everthing here
	void StartTheGame() {
		StopCoroutine("TimeLeft");
		timer = timeRemaining;
		timerText.text = "Time: " + timer;
		timeBonusText.text = "";
		Time.timeScale = 1f;
		distanceTraveled = 0f;
		gameOverText.text = "";
		marbleText.text = "";
		goal = false;
		criticalIsPlaying = false;
		pauseGameText.gameObject.SetActive(false);
		quitText.gameObject.SetActive(false);
		canPause = true;
		AudioListener.volume = gameVolume;

		transform.position = new Vector3(0f, 1f, 0f);
		myRigidBody.velocity = new Vector3(0f, 0f, 0f);
		currentPos = newPos = new Vector3(0f, 0f, 0f);

		checkpoint = transform.position;
		levelBonus = 1000f * GameObject.Find("Game Controller").GetComponent<Score>().gameLevel;
		score = GameObject.Find("Game Controller").GetComponent<Score>().gameScore;
		scoreText.text = "SCORE: " + score;
		StartCoroutine ("TimeLeft");
		StartCoroutine ("HereWeGo");
		backgroundMusic.Play();
	}

	// Used to move the marble
	void moveSphere() {
		currentPos = transform.position;
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		myRigidBody.AddForce(new Vector3(h * force, 0f, v * force));
		distanceTraveled += Mathf.Sqrt(Mathf.Pow((Mathf.Abs(newPos.x - currentPos.x)), 2f) + Mathf.Pow((Mathf.Abs(newPos.z - currentPos.z)), 2f));
		newPos = currentPos;
	}

	// Updates score when marble travels a certain distance (set at 5 units for now)
	void UpdateScore() {
		while (distanceTraveled >= 5) {
			AddScore(10f);
			distanceTraveled -= 5;
		}
	}

	// Checks if marble collides with a trigger
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Checkpoint") {
			GameObject temp = collider.gameObject;
			if (temp.GetComponent<CheckpointFlag>().Checkpoint() == true) {
				checkpoint = collider.gameObject.transform.position;
				temp.GetComponent<CheckpointFlag>().SetCheckpointOff();
				temp.GetComponent<CheckpointFlag>().SetCheckpointColor(false);
				StartCoroutine("CheckpointText");
			}
		}

		if (collider.tag == "Bonus") {
			//StopCoroutine("PlayText");
			bonusItem.Play();
			StartCoroutine(PlayText("BONUS!"));
			AddScore(250f);
			collider.gameObject.SetActive(false);
		}

		if (collider.tag == "Time") {
			//StopCoroutine("PlayText");
			bonusTime.Play();
			StartCoroutine(PlayText("Time Bonus!"));
			timer += timeBonus;
			collider.gameObject.SetActive(false);
		}

		if (collider.tag == "Death") {
			explosion.Play();
			Respawn();
		}

		if (collider.tag == "Goal") {
			if (goal == false) {
				GameObject.Find("Game Controller").GetComponent<Score>().gameLevel++;
				marbleText.text = "BONUS " + levelBonus;
				AddScore(levelBonus);
				AddScore(timer * 100);
				goal = true;
			}
		if (score > GameObject.Find("Game Controller").GetComponent<Score>().highScore) {
			GameObject.Find("Game Controller").GetComponent<Score>().highScore = score;
		}
			StartCoroutine("YouWin");
		}
	}

	// Respawns a new marble if it dies -- make sure it does NOT give points for moving it this way
	void Respawn() {
		myRigidBody.velocity = new Vector3(0f, 0f, 0f);
		currentPos = newPos = checkpoint;
		transform.position = checkpoint;
		StopCoroutine("PlayText");
		StartCoroutine("Ouch");
	}

	// Adds value to score
	void AddScore(float value) {
		score += value;
		scoreText.text = "SCORE: " + score;
	}

	/// <summary>
	/// IEnumerators are placed here. //////////////////////////////////////////////////////////////////
	/// </summary>

	// Marble comment test
	IEnumerator HereWeGo() {
		marbleText.text = "Here we go!";
		yield return new WaitForSecondsRealtime(2f);
		marbleText.text = "";
	}

	// Function for a Game Over
	IEnumerator GameOverFool() {
		backgroundMusic.Stop();
		criticalSound.Stop();
		criticalIsPlaying = false;
		canPause = false;
		youLose.Play();
		StopCoroutine("CheckpointText");
		StopCoroutine("TimeLeft");	
		Time.timeScale = 0f;
		gameOverText.text = "GAME OVER FOOL!";
		yield return new WaitForSecondsRealtime(3f);
		youSuck.Play();
		yield return new WaitForSecondsRealtime(youSuck.clip.length);
		Time.timeScale = 1f;
		GameObject.Find("Game Controller").GetComponent<Score>().gameScore = score;
		if (score > GameObject.Find("Game Controller").GetComponent<Score>().highScore) {
			Debug.Log("New high score");
			GameObject.Find("Game Controller").GetComponent<Score>().highScore = score;
			GameObject.Find("Game Controller").GetComponent<Score>().SaveNewHighScore(score);
		}
		Application.LoadLevel("You Lost");
	}

	// Function when you reach the goal
	IEnumerator YouWin() {
		backgroundMusic.Stop();
		stageClear.Play();
		woohoo.Play();
		StopCoroutine("TimeLeft");
		canPause = false;
		Time.timeScale = 0f;
		gameOverText.text = "Congrats!!!";
		timeBonusText.text = "Time Bonus: " + timer * 100;
		yield return new WaitForSecondsRealtime(woohoo.clip.length);
		Time.timeScale = 1f;
		GameObject.Find("Game Controller").GetComponent<Score>().gameScore = score;
		if (score > GameObject.Find("Game Controller").GetComponent<Score>().highScore) {
			Debug.Log("New high score");
			GameObject.Find("Game Controller").GetComponent<Score>().highScore = score;
			GameObject.Find("Game Controller").GetComponent<Score>().SaveNewHighScore(score);
		}
		Application.LoadLevel(GameObject.Find("Game Controller").GetComponent<Score>().gameLevel);
	}

	// Timer
	IEnumerator TimeLeft() {
		while (timer > 0) {
			yield return new WaitForSeconds(1f);
			timer--;
			timerText.text = "Time: " + timer;
			if (timer <= criticalTime && criticalIsPlaying == false) {
				criticalSound.Play();
				criticalIsPlaying = true;
			} else if (timer > criticalTime) {
				criticalSound.Stop();
				criticalIsPlaying = false;
			}
		}
		if (timer <= 0) {
			StopCoroutine("Ouch");
			StartCoroutine("GameOverFool");
		}
	}

	// Sound and visual effect function when marble dies
	IEnumerator Ouch() {
		StopCoroutine("CheckpointText");
		StopCoroutine("PlayText");
		marbleDeath.Play();
		gameOverText.text = "OUCH!!!!";
		yield return new WaitForSeconds(2f);
		gameOverText.text = "";
	}

	// Sound and visual effect on a checkpoint
	IEnumerator CheckpointText() {
		checkpointSound.Play();
		gameOverText.text = "Checkpoint!";
		yield return new WaitForSeconds(1.5f);
		gameOverText.text = "";
	}

	// Visual effect function when does something (e.g. pick up a bonus item)
	IEnumerator PlayText(string text) {
		gameOverText.text = text;
		yield return new WaitForSeconds(1.5f);
		gameOverText.text = "";

	}


}

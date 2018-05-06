using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreGameLogic : MonoBehaviour {

	public int score = 0;
	public int maxLives = 1;
	public int maxLevel = 10;
	public int bonusDecrement = 3;
	public int currentLevel = 1;
	public bool isBallInteractionEnabled = false;
	public GameObject ball;
	public Transform ballRespawnTransform;

	public AudioSource themeSong;
	public AudioSource coinSound;
	public AudioSource bubbleSound;

	private int lives = 3;
	private int currentBonus = 100;
	private Camera mainCamera;
	private CameraFollow cameraLogic;
	private GameObject currentUI;
	public GameState state = GameState.Welcome;
	private GameObject currentPlatform;
	private CreateHoleWall levelCreator;

	void Start () {
		levelCreator = GetComponent<CreateHoleWall> ();
		mainCamera = GameObject.Find (Constants.CameraHeirarchyName).GetComponent<Camera> ();
		cameraLogic = mainCamera.GetComponent<CameraFollow> ();

		// Just placeholders before game starts
		MakeMovingPlatform ();
		levelCreator.MakeWall ();

		currentUI = (GameObject) Instantiate (Resources.Load ("StartScreenUI"));
	}

	void Update () {
		
		UpdateInGameUI ();
		if (state == GameState.InGame && isBallInteractionEnabled) {
			var isMoving = Input.GetButton ("LeftPaddle") || Input.GetButton ("RightPaddle");
			if (isMoving && bubbleSound.isPlaying == false) {
				bubbleSound.Play ();
			}
		}
	}

	public void NewGame () {
		ResetGame ();
		InvokeRepeating ("ReduceBonus", 1, 0.3F);

		levelCreator.MakeWall ();
		GameObject.Destroy (currentPlatform);
		currentPlatform = null;
		MakeMovingPlatform ();

		coinSound.Play ();
		themeSong.PlayDelayed (1.0f);

		ball.GetComponent<Transform> ().position = ballRespawnTransform.position;
		ball.GetComponent<Rigidbody> ().velocity = Vector3.zero;

		GameObject.Destroy (currentUI);
		currentUI = (GameObject) Instantiate (Resources.Load ("InGameUI"));
		state = GameState.InGame;
		cameraLogic.AdjustBehaviour ();
	}

	public void QuitGame () {
		isBallInteractionEnabled = false;
		CancelInvoke ("ReduceBonus");
		GameObject.Destroy (currentUI);
		currentUI = (GameObject) Instantiate (Resources.Load ("StartScreenUI"));
		state = GameState.Welcome;
		cameraLogic.AdjustBehaviour ();
	}

	void ReduceBonus () {
		currentBonus -= bonusDecrement;
		if (currentBonus < 0) {
			currentBonus = 0;
		}
	}

	void ResetGame () {
		score = 0;
		lives = maxLives;
		currentLevel = 1;
		currentBonus = currentLevel * 100;
		isBallInteractionEnabled = false;
	}

	void GameOver () {
		Debug.Log ("GAME OVER");
		var gameOverScreen = (GameObject) Instantiate (Resources.Load ("GameOverUI"));
		StartCoroutine (DestroyObjectAfter (gameOverScreen, 5.0f));
		Invoke ("QuitGame", 5.0f);
	}

	public void OnBallThroughSuccessHole () {

		isBallInteractionEnabled = false;
		score += 100 + currentBonus;

		if (currentLevel >= maxLevel) {
			GameOver();
			themeSong.Play();
			return;
		}

		currentLevel++;
		MakeMovingPlatform ();
		levelCreator.MakeWall ();
		currentBonus = currentLevel * 100;
	}

	public void OnBallThroughFailHole () {

		isBallInteractionEnabled = false;
		lives--;
		if (lives <= 0) {
			GameOver ();
		} else {
			MakeMovingPlatform ();
		}
	}

	void UpdateInGameUI () {
		if (state != GameState.InGame) {
			return;
		}
		Text healthText = GameObject.Find ("LivesUI").GetComponent<Text> ();
		healthText.text = "Lives: " + lives.ToString ();

		Text scoreText = GameObject.Find ("ScoreUI").GetComponent<Text> ();
		scoreText.text = "Score: " + score.ToString ();

		Text bonusText = GameObject.Find ("BonusUI").GetComponent<Text> ();
		bonusText.text = "Bonus: " + currentBonus.ToString ();
	}

	void MakeMovingPlatform () {

		if (currentPlatform) {
			StartCoroutine (RemovePlatform (currentPlatform));
		}

		currentPlatform = (GameObject) Instantiate (Resources.Load ("MovingPlatform"));
		var transform = currentPlatform.GetComponent<Transform> ();
		transform.position = new Vector3 (1f, -10, 1.2f);
	}

	public IEnumerator RemovePlatform (GameObject platform) {

		var transform = platform.GetComponent<Transform> ();

		float totalTime = 0;

		while (totalTime < 2) {
			transform.Translate (Vector3.up * Time.deltaTime * 10, Space.World);
			totalTime += Time.deltaTime;
			yield return null;
		}

		GameObject.Destroy (platform);
		yield return null;
	}

	public IEnumerator DestroyObjectAfter (GameObject o, float time) {
		yield return new WaitForSeconds (time);
		Debug.Log ("Destroying object after having waited", o);
		Destroy (o);
	}

}
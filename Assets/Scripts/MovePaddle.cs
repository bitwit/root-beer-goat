using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddle : MonoBehaviour {

	public float speed = 1;
	public string inputName = "LeftPaddle";
	private Transform trans;
	private CoreGameLogic gameLogic;
	void Start () {
		trans = GetComponent<Transform> ();
		gameLogic = GameObject.Find (Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic> ();
	}

	void Update () {
		if (gameLogic.state != GameState.InGame) {
			return;
		}

		if (gameLogic.isBallInteractionEnabled == false) {
			return;
		}

		float translation = Input.GetAxis (inputName) * speed;
		translation *= Time.deltaTime;
		trans.Translate (0, translation, 0, Space.World);
	}
}
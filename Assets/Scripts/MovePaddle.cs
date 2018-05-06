using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddle : MonoBehaviour {

	public float speed = 1;
	public string inputName = "LeftPaddle";
	public Transform otherPaddleTransform;

	private float maxDiff = 5.0f;
	private float maxY = 20.0f;

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

		if(!trans || !otherPaddleTransform) {
			return;
		}

		float translation = Input.GetAxis (inputName) * speed;
		translation *= Time.deltaTime;

		float newY = translation + trans.position.y;
		if ( Mathf.Abs(otherPaddleTransform.position.y - newY) > maxDiff ) {
			return; //max distance apart already
		}

		if (newY > maxY) {
			return; //hit top
		}

		trans.Translate (0, translation, 0, Space.World);
	}
}
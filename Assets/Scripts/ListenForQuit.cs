using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForQuit : MonoBehaviour {

	private CoreGameLogic gameLogic;
	void Start () {
		gameLogic = GameObject.Find (Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic> ();
	}

	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			gameLogic.QuitGame ();
		}
	}
}
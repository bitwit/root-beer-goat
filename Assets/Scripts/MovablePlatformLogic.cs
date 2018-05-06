using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatformLogic : MonoBehaviour {

	private CoreGameLogic gameLogic;

	void Start() {
		gameLogic = GameObject.Find(Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic>();
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Player") {
			gameLogic.OnBallContactWithPlatform();
		}
	}


}
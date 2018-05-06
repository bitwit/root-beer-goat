using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTrigger : MonoBehaviour {

	private CoreGameLogic gameLogic;

	void Start () {
		gameLogic = GameObject.Find (Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic> ();
	}

	void OnTriggerEnter (Collider collider) {
		if (gameLogic.isBallInteractionEnabled && collider.CompareTag ("Player")) {
			Debug.Log ("Fail hole!");
			AudioSource audio = GetComponent<AudioSource> ();
			audio.Play ();

			gameLogic.OnBallThroughFailHole ();
		}
	}

}
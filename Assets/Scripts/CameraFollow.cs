using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float rotationSpeed = 20.0f;
	public Transform inGameTransform;
	public Transform standByTransform;

	[SerializeField]
	public Transform target;

	private CoreGameLogic gameLogic;

	private bool isRepositioning = false;

	void Start () {
		gameLogic = GameObject.Find (Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic> ();
		AdjustBehaviour ();
	}

	public void AdjustBehaviour () {
		if (gameLogic.state == GameState.InGame) {
			target = GameObject.Find (Constants.PlayerHeirarchyName).GetComponent<Transform> ();

			var newXForm = inGameTransform;
			newXForm.position = new Vector3(newXForm.position.x, target.position.y, newXForm.position.z);
			StartCoroutine (MoveToPosition (newXForm));
		} else {
			// transform.position = standByPosition.position;
			target = GameObject.Find (Constants.ArcadeBoxHeirarchyName).GetComponent<Transform> ();
			StartCoroutine (MoveToPosition (standByTransform));
		}
	}

	void LateUpdate () {
		// Early out if we don't have a target
		if (!target)
			return;

		if (isRepositioning)
			return;

		if (gameLogic.state == GameState.InGame) {
			var newTransform = transform;
			newTransform.position = new Vector3 (inGameTransform.position.x, target.position.y, inGameTransform.position.z);
			newTransform.rotation = Quaternion.Euler (0, 0, 0);

			transform.position = Vector3.Lerp (transform.position, newTransform.position, 1.0f);
			transform.rotation = Quaternion.Lerp (transform.rotation, newTransform.rotation, 1.0f);
		} else {

			var newTransform = transform;
			newTransform.Translate (Vector3.right * Time.deltaTime * rotationSpeed);
			newTransform.LookAt (target);

			transform.position = Vector3.Lerp (transform.position, newTransform.position, 1.0f);
			transform.rotation = Quaternion.Lerp (transform.rotation, newTransform.rotation, 1.0f);
		}
	}

	public IEnumerator MoveToPosition (Transform t) {

		isRepositioning = true;

		var totalTime = 1.0f;
		var elapsedTime = 0f;
		while (elapsedTime < totalTime) {
			transform.position = Vector3.Lerp (transform.position, t.position, (elapsedTime / totalTime));
			transform.rotation = Quaternion.Lerp (transform.rotation, t.rotation, (elapsedTime / totalTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		isRepositioning = false;
		yield return null;
	}

}
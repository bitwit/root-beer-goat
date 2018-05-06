using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHoleWall : MonoBehaviour {

	private int colCount = 10;
	private int baseChanceOfFailHole = 20;
	private int difficultyIncrement = 10;

	private CoreGameLogic gameLogic;
	private int successHolePositionRow = -1;
	private int successHolePositionCol = -1;

	private GameObject wallContainer;

	void Start () {
		gameLogic = GameObject.Find (Constants.GameLogicHeirarchyName).GetComponent<CoreGameLogic> ();
		DestroyPlaceholders ();
	}

	void DestroyPlaceholders () {
		var placeholders = GameObject.Find ("Placeholders");
		GameObject.Destroy (placeholders);
	}

	public void MakeWall () {

		successHolePositionRow = -1;
		successHolePositionCol = -1;
		if (wallContainer) {
			GameObject.Destroy (wallContainer);
		}

		wallContainer = new GameObject ();

		for (int r = 0; r < gameLogic.maxLevel; r++) {

			if (gameLogic.currentLevel == (r + 1)) {

				var col = Random.Range (0, colCount);
				successHolePositionRow = r;
				successHolePositionCol = col;
				GenerateHoleOfTypeAt ("SuccessHole", r, col);
			}

			for (int c = 0; c < colCount; c++) {

				if (r == successHolePositionRow && c == successHolePositionCol) {
					continue;
				}

				var randNum = Random.Range (0, 100);
				var resourceName = "BlockedWall";
				var chanceOfFailHole = (100 - baseChanceOfFailHole) - (r * difficultyIncrement);
				if (randNum > chanceOfFailHole) {
					resourceName = "FailHole";
				}

				GenerateHoleOfTypeAt (resourceName, r, c);
			}
		}
	}

	void GenerateHoleOfTypeAt (string type, int row, int col) {

		var x = col * 2;
		var y = 2 + (row * 2);
		var z = -3;
		var hole = (GameObject) Instantiate (Resources.Load (type), wallContainer.transform);
		var transform = hole.GetComponent<Transform> ();
		transform.position = new Vector3 (x, y, z);
	}
}
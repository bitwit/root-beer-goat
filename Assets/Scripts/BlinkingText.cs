using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {

	private Text text;
	private float speed = 2f;

	void Start () {
		text = GetComponent<Text> ();
	}

	void Update () {
		text.color = new Color(text.color.r,text.color.g,text.color.b, Mathf.Round(Mathf.PingPong(Time.time * speed, 1.0f)));
	}
}
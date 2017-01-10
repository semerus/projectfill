using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSet : MonoBehaviour {
	//	this script is used to display number of guards on screen
	Text score;

	void Start() {
		score = GetComponentInChildren<Text> ();
	}

	void Update() {
		score.text = "Score: "+ GuardManager.currentScore;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuardRecord : MonoBehaviour {
//	this script is used to display number of guards on screen
	Text guardNum;

	void Start() {
		guardNum = GetComponentInChildren<Text> ();
	}

	void Update() {
		guardNum.text = "Number of Guards "+ GuardManager.guardCount.ToString();
	}
}

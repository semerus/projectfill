using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuardRecord : MonoBehaviour {
	Text guardNum;
	GuardManager gm;

	void Start() {
		guardNum = GetComponentInChildren<Text> ();
		gm = GameObject.Find ("Guard Manager").GetComponent<GuardManager> ();
	}

	void Update() {
		Debug.Log (gm.guardCount);
		guardNum.text = "Number of Guards "+gm.guardCount.ToString();
	}
}

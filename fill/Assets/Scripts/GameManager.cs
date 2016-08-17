using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {

	TextAsset _coords;
	string fileFullPath;

	string[] stringList;

		
	// Use this for initialization
	void Start () {
		TextAsset _coords = (TextAsset)Resources.Load("Assets/coords.csv");
		string fileFullPath = _coords.text;
		string[] stringList = fileFullPath.Split('\n');
		Debug.Log (stringList [0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

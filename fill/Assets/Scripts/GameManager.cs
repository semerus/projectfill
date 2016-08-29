using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	private static GameManager instance;
	private static GameStateEnum currentState;

	/*****************************************************************/
	/* Constructor */
	// private constructor
	private GameManager(){
	}

	/*****************************************************************/
	/* Functions */
	// getter for singleton
	public static GameManager getInstance(){
		if (instance == null) {
			instance = new GameManager ();
		}
		return instance;
	}

	/*****************************************************************/
	/* MonoBehaviour */
	// Use this for initialization
	void Start () {
		/* this is to be done by fileManager
		TextAsset _coords = (TextAsset)Resources.Load("Assets/coords.csv");
		string fileFullPath = _coords.text;
		string[] stringList = fileFullPath.Split('\n');
		Debug.Log (stringList [0]);
		*/

		// Start with main 
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case GameStateEnum.StageSelected:
			levelSelected ();
			break;
		case GameStateEnum.StageGenerated:
			Debug.Log ("Stage Generated");
			break;

		}
	}

	void levelSelected(){
		// 1. read map data from file
		FileManager fm = FileManager.getInstance ();
		MapData md = fm.readMap ("filePath");

		// 2. MapGenerator.createMap(MapData) should generate the map

		// 3. update GameState
		currentState = GameStateEnum.StageGenerated;
	}
}

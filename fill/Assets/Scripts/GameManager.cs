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
		// add disabled guardManager as component
		GuardManager guardManager = gameObject.AddComponent<GuardManager> ();
		guardManager.enabled = false;


	}
	
	// Update is called once per frame
	void Update () {
		string filepath = "~/Desktop/temp.map";

		switch (currentState) {
		case GameStateEnum.StageSelected:
			generateStage (filepath);
			break;
		case GameStateEnum.StageGenerated:
			playGame ();
			break;
		}
	}

	void generateStage(string filepath){
		// 1. read map data from file
		FileManager fm = FileManager.getInstance ();
		MapData md = fm.readMap (filepath);

		// 2. MapGenerator.createMap(MapData) should generate the map
		new MapGenerator().createMap(md);

		// 3. update GameState
		currentState = GameStateEnum.StageGenerated;
	}

	void playGame(){
		// 1. Load play scene

		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;
	}
}

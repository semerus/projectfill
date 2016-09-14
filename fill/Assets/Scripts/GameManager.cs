using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	private static GameManager instance = null;
	private static GameStateEnum currentState;
	public static int loadLevel;

	// for saving the map information
	private static MapData md;

	/*****************************************************************/
	/* Constructor */
	// private constructor
	private GameManager(){
	}

	/*****************************************************************/
	/* Functions */
	// checks for singleton
//	void Awake () {
//		if (instance == null)
//			instance = this;
//		else if (instance != null)
//			Destroy (gameObject);
//		DontDestroyOnLoad (gameObject);
//	}

	public static GameManager getInstance(){
		if(instance == null){
			instance = new GameManager();
		}
		return instance;
	}

	/*****************************************************************/
	/*MonoBehaviour*/
	void Start () {
		currentState = GameStateEnum.StageSelected;

		// add disabled guardManager as component
		GuardManager guardManager = gameObject.AddComponent<GuardManager> ();
		guardManager.enabled = false;
	}

	void Update () {
//		string filepath = "./GameLevels/star_with_hole";
		string filepath = "./GameLevels/star";

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
		md = fm.readMap (filepath);

		// 2. MapGenerator.createMap(MapData) should generate the map
		new MapGenerator().createMap(md);

		// 3. update GameState
		currentState = GameStateEnum.StageGenerated;
	}

	void playGame(){
		// 1. Load play scene

		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;

		bool filled = DecisionAlgorithm.isFilled (GuardManager.getGuards (), md);
		if (filled)
			Debug.Log ("Filled");
		else
			Debug.Log ("Not Filled");
	}

	public MapData getMapData(){
		return md;
	}
}

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

	void Awake () {
		if(instance == null)
			instance = this;
	}
	public static GameManager getInstance(){
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
//		string filepath = "./GameLevels/Empire";
		string filepath = Application.dataPath + "/Resources/Maps.json";

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
//		FileManager fm = FileManager.getInstance ();
//		md = fm.readMap (filepath);
		JsonManager jm = JsonManager.getInstance ();
		md = jm.readMap (filepath, "Buildings", 1);

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

	public MapData getMapData(){
		return md;
	}
}

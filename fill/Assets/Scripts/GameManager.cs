using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	private static GameManager _instance = null;

	// path for json file
	private static string fileName;
	private static GameState currentState;
	public static int loadLevel;

	// for saving the map information
	private static MapData mapData;
	private static List<ThumbnailData> mapList = new List<ThumbnailData> ();

	// for decision algorithm for this map
	private static DecisionAlgorithm da;

	public static DecisionAlgorithm DA{
		get {
			return da;
		}
	}

	/*****************************************************************/
	/* Constructor */
	// private constructor
	private GameManager()
	{
	}
		
	/* Getter and Setter */
	public static GameManager GetInstance()
	{
		if( _instance == null)
		{
			_instance = new GameManager();
		}

		return _instance;
	}

	public static GameState CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
			switch (currentState) {
			case GameState.StageSelected:
				generateStage (fileName);
				break;
			case GameState.StageGenerated:
				GameManager.CurrentState = GameState.PlayGame_Playing;
				break;
			case GameState.PlayGame_Playing:
				GuardManager.Instance().enabled = true;
				break;
			}
			if (currentState != GameState.PlayGame_Playing)
				GuardManager.Instance().enabled = false;
		}
	}

	public static MapData MapData {
		get {
			return mapData;
		}
	}

	public static List<ThumbnailData> MapList {
		get {
			return mapList;
		}
	}

	/*****************************************************************/
	/*MonoBehaviour*/
	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (_instance != null)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);

		fileName = "Maps2";
	}

	void Start () {
		// readfile to make mapList
		mapList = JsonManager.readMapList(fileName, "Buildings");
	}

	void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoad;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoad;
	}

	void OnSceneLoad (Scene scene, LoadSceneMode mode) {
		if (scene.buildIndex == 0) {
			GameManager.CurrentState = GameState.StageSelected;
		}
		if (scene.buildIndex == 1) {
			GameManager.CurrentState = GameState.StageSelection;
		}
	}


	/*****************************************************************/
	/* Functions */
	static void generateStage(string filepath){
		// 1. read map data from file
//		FileManager fm = FileManager.getInstance ();
//		md = fm.readMap (filepath);
		mapData = JsonManager.readMap (filepath, "Buildings", loadLevel);

		// 2. MapGenerator.createMap(MapData) should generate the map
		new MapGenerator().createMap(mapData);

		// 3. update GameState
		GameManager.CurrentState = GameState.StageGenerated;

		// 4. create DecisionAlgorithm
		da = new DecisionAlgorithm(mapData);
	}

	void playGame(){
		// 1. update GameState
		currentState = GameState.PlayGame_Playing;
		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;

		bool filled = da.isFilled (GuardManager.getPositionList ());
		if (filled)
			Debug.Log ("Filled");
		else
			Debug.Log ("Not Filled");
	}
}

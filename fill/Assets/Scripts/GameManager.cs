using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	/*****************************************************************/
	/* Variables */
	// path for json file
	private static string filePath; 

	// for singleton design
	private static GameManager instance = null;
	private static GameStateEnum currentState;
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
	private GameManager(){
	}
		
	/* Getter and Setter */
	public static GameManager Instance {
		get {
			return instance;
		}
	}

	public static GameStateEnum CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
			switch (currentState) {
			case GameStateEnum.StageSelected:
				generateStage (filePath);
				break;
			case GameStateEnum.StageGenerated:
				GameManager.CurrentState = GameStateEnum.PlayGame_Playing;
				break;
			case GameStateEnum.PlayGame_Playing:
				GuardManager.Instance.enabled = true;
				break;
			}
			if (currentState != GameStateEnum.PlayGame_Playing)
				GuardManager.Instance.enabled = false;
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
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);

		filePath = Application.dataPath + "/StreamingAssets/Maps2.json";
	}

	void Start () {
		// readfile to make mapList
		mapList = JsonManager.readMapList(filePath, "Buildings");
	}

	void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoad;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoad;
	}

	void OnSceneLoad (Scene scene, LoadSceneMode mode) {
		if (scene.buildIndex == 0) {
			GameManager.CurrentState = GameStateEnum.StageSelected;
			GetComponent<SelectionMenu_Swipe> ().enabled = false;
		}
		if (scene.buildIndex == 1) {
			GameManager.CurrentState = GameStateEnum.StageSelection;
			GetComponent<SelectionMenu_Swipe> ().enabled = true;
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
		GameManager.CurrentState = GameStateEnum.StageGenerated;

		// 4. create DecisionAlgorithm
		da = new DecisionAlgorithm(mapData);
	}
///*
	void playGame(){
		// 1. update GameState
		currentState = GameStateEnum.PlayGame_Playing;
		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;

		bool filled = da.isFilled (GuardManager.getPositionList ());
		if (filled)
			Debug.Log ("Filled");
		else
			Debug.Log ("Not Filled");
	}
//*/
}

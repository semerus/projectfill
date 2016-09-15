using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	/*****************************************************************/
	/* Variables */
	// path for json file
	private string filePath; 

	// for singleton design
	private static GameManager instance = null;
	private static GameStateEnum currentState;
	public static int loadLevel;

	// for saving the map information
	private static MapData md;
	private static List<SimpleData> mapList = new List<SimpleData> ();

	/*****************************************************************/
	/* Constructor */
	// private constructor
	private GameManager(){
	}

	/* Getter and Setter */
	public static GameManager getInstance(){
		return instance;
	}

	public static GameStateEnum CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
			if (currentState != GameStateEnum.PlayGame_Playing)
				GameManager.getInstance ().GetComponent<GuardManager> ().enabled = false;
		}
	}

	public MapData getMapData() {
		return md;
	}

	public List<SimpleData> getMapList() {
		return mapList;
	}
	/*****************************************************************/
	/*MonoBehaviour*/
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		filePath = Application.dataPath + "/Resources/Maps.json";
		currentState = GameStateEnum.StageSelected;
		// readfile to make mapList
		mapList = JsonManager.getInstance().readMapList(filePath, "Buildings");

		// add disabled guardManager as component
		GuardManager guardManager = gameObject.AddComponent<GuardManager> ();
		guardManager.enabled = false;
	}

	void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoad;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoad;
	}

	void OnSceneLoad (Scene scene, LoadSceneMode mode) {
		if (scene.buildIndex == 1) {
			currentState = GameStateEnum.StageSelected;
		}
		if (scene.buildIndex != 1) {
			currentState = GameStateEnum.StageSelection;
			GetComponent<GuardManager> ().enabled = false;
		}
	}

	void Update () {
		switch (currentState) {
		case GameStateEnum.StageSelected:
			generateStage (filePath);
			break;
		case GameStateEnum.StageGenerated:
			playGame ();
			break;
		}
	}

	/*****************************************************************/
	/* Functions */
	void generateStage(string filepath){
		// 1. read map data from file
//		FileManager fm = FileManager.getInstance ();
//		md = fm.readMap (filepath);
		JsonManager jm = JsonManager.getInstance ();
		md = jm.readMap (filepath, "Buildings", loadLevel);

		// 2. MapGenerator.createMap(MapData) should generate the map
		new MapGenerator().createMap(md);

		// 3. update GameState
		currentState = GameStateEnum.StageGenerated;
	}

	void playGame(){
		// 1. update GameState
		currentState = GameStateEnum.PlayGame_Playing;
		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;
	}
}

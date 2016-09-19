﻿using UnityEngine;
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

		filePath = Application.dataPath + "/StreamingAssets/Maps.json";

		// add disabled guardManager as component
		GuardManager guardManager = gameObject.AddComponent<GuardManager> ();
		guardManager.enabled = false;
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
		if (scene.buildIndex == 1) 
			GameManager.CurrentState = GameStateEnum.StageSelection;
		if (scene.buildIndex == 0)
			GameManager.CurrentState = GameStateEnum.StageSelected;
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
	}
///*
	void playGame(){
		// 1. update GameState
		currentState = GameStateEnum.PlayGame_Playing;
		// 2. Enable the GuardManager
		GetComponent<GuardManager>().enabled = true;

		bool filled = DecisionAlgorithm.isFilled (GuardManager.getPositionList (), mapData);
		if (filled)
			Debug.Log ("Filled");
		else
			Debug.Log ("Not Filled");
	}
//*/
}

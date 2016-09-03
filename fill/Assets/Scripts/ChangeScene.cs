using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class ChangeScene : MonoBehaviour {

	static ChangeScene instance = null;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if (Input.GetKeyUp ("k")) {
			SceneManager.LoadScene ("room");
		}
		if (Input.GetKeyUp ("l")) {
			SceneManager.LoadScene ("StageSelect");
		}
	}
}

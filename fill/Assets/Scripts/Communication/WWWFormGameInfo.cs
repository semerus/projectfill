using UnityEngine;
using System.Collections;

public class WWWFormImage : MonoBehaviour {

	public string screenShotURL= "http://127.0.0.1:8080/submit";

	// Use this for initialization
	void Start () {
		StartCoroutine(RequestTopScore());
	}

	IEnumerator RequestTopScore() {
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		var tex = new Texture2D( width, height, TextureFormat.RGB24, false );

		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy( tex );

		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
		form.AddField("GameId", GameManager.MapData.getId());
		form.AddField("Email", SystemInfo.deviceUniqueIdentifier);
		form.AddField("NumOfGuards", GuardManager.guardCount);
		form.AddField("GuardLocation", GuardManager.guardList.ToString());
		form.AddField("GameHash", GameManager.MapData.GetHashCode());
		form.AddField("Score", GuardManager.currentScore.ToString());

		// Upload to a cgi script
		WWW w = new WWW(screenShotURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
		}
		else {
			print("Finished Uploading Screenshot");
		}
	}
}
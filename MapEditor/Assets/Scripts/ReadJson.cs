using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour {
    private string path = "/Resources/Maps.json";
	private int stage = 1;

	public MapData mapData;

	void Start () {
		mapData = ReadMapVertices (path, stage);
		Debug.Log (mapData.vertices[1]);
	}

	MapData ReadMapVertices(string path, int stage) {
		string jsonString = File.ReadAllText (Application.dataPath + path);
		JsonData data = JsonMapper.ToObject (jsonString);
		Vector2[] vertices = new Vector2[data ["Maps"] [stage - 1] ["vertices"].Count];
		for (int i = 0; i < data ["Maps"] [stage - 1] ["vertices"].Count; i++) {
			double x = (double) data ["Maps"] [stage - 1] ["vertices"] [i] ["x"];
			double y = (double)data ["Maps"] [stage - 1] ["vertices"] [i] ["y"];
			vertices [i].x = (float) x;
			vertices [i].y = (float) y; 
		}
		MapData mapData = new MapData((int)data ["Maps"] [stage - 1] ["id"], data ["Maps"] [stage - 1] ["name"].ToString (), vertices);
		return mapData;
    }

	public class MapData {
		public int id;
		public string name;
		public Vector2[] vertices;

		public MapData(int id, string name, Vector2[] vertices){
			this.id = id;
			this.name = name;
			this.vertices = vertices;
		}
	}
}

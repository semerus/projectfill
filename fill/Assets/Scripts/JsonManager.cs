using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class JsonManager {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	private static JsonManager instance;

	/*****************************************************************/
	/* Constructor */
	private JsonManager () {
	}

	/*****************************************************************/
	/* Functions */
	//getter for singleton
	public static JsonManager getInstance () {
		if (instance == null) {
			instance = new JsonManager ();
		}
		return instance;
	}

	public List<SimpleData> readMapList (string pathToFile, string theme) {
		string jsonString = File.ReadAllText (pathToFile);
		JsonData jsonData = JsonMapper.ToObject (jsonString);

		List<SimpleData> sdl = new List<SimpleData> ();
		for (int i = 0; i < jsonData [theme].Count; i++) {
			SimpleData sd = new SimpleData (readId(jsonData, theme, i), readName(jsonData, theme, i), readFilePath(jsonData, theme, i));
			sdl.Add(sd);
		}
		return sdl;
	}

	public MapData readMap (string pathToFile, string theme, int order) {
		SimplePolygon2D outer;
		SimplePolygon2D[] holes;

		string jsonString = File.ReadAllText (pathToFile);
		JsonData jsonData = JsonMapper.ToObject (jsonString);

		outer = readVertices (jsonData, theme, order, "outerVertices");
		holes = new SimplePolygon2D[jsonData [theme] [order] ["holes"].Count];
		for (int i = 0; i < holes.Length; i++) {
			holes [i] = readVertices (jsonData, theme, order, "holes", "innerVertices");
		}

		MapData mapData = new MapData(
			outer,
			holes,
			readName(jsonData, theme, order),
			readId(jsonData, theme, order),
			readColor(jsonData, theme, order, "lineColor"),
			readColor(jsonData, theme, order, "backgroundColor"),
			readColor(jsonData, theme, order, "guardBasicColor"),
			readColor(jsonData, theme, order, "guardSelectedColor"),
			readColor(jsonData, theme, order, "vgColor")
		);
		return mapData;
	}

	private string readName(JsonData data, string theme, int order) {
		string name = data [theme] [order] ["name"].ToString();
		return name;
	}

	private int readId(JsonData data, string theme, int order) {
		int id =(int)data [theme] [order] ["id"];
		return id;
	}

	private string readFilePath(JsonData data, string theme, int order) {
		string path = data [theme] [order] ["filePath"].ToString ();
		return path;
	}

	// for outer
	private SimplePolygon2D readVertices(JsonData data, string theme, int order, string outorhole) {
		SimplePolygon2D polygon = new SimplePolygon2D ();
		for (int i = 0; i < data [theme] [order] [outorhole].Count; i++) {
			double x = (double)data [theme] [order] [outorhole] [i] ["x"];
			double y = (double)data [theme] [order] [outorhole] [i] ["y"];
			polygon.addVertex(new Vector2((float) x, (float) y));
			}
		return polygon;
	}

	// overloading for holes
	private SimplePolygon2D readVertices(JsonData data, string theme, int order, string outorhole, string component) {
		SimplePolygon2D polygon = new SimplePolygon2D ();
		for (int i = 0; i < data [theme] [order] [outorhole].Count; i++) {
			for (int j = 0; j < data [theme] [order] [outorhole] [i] [component].Count; j++) {
				double x = (double)data [theme] [order] [outorhole] [i] [component] [j] ["x"];
				double y = (double)data [theme] [order] [outorhole] [i] [component] [j] ["y"];
				polygon.addVertex (new Vector2 ((float)x, (float)y));
			}
		}
		return polygon;
	}

	private Color readColor(JsonData data, string theme, int order, string component) {
		double red, green, blue, alpha;
		red = (double)data [theme] [order] [component] ["r"];
		green = (double)data [theme] [order] [component] ["g"];
		blue = (double)data [theme] [order] [component] ["b"];
		alpha = (double)data [theme] [order] [component] ["a"];
		Color color = new Color ((float)red, (float)green, (float)blue, (float)alpha);
		return color;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class JsonManager {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	//private static JsonManager instance;

	/*****************************************************************/
	/* Constructor */
	private JsonManager () {
	}

	/*****************************************************************/
	/* Functions */
	//getter for singleton

	public static List<ThumbnailData> readMapList (string fileName, string theme) {
		//string jsonString = File.ReadAllText (fileName);
		TextAsset file = Resources.Load(fileName) as TextAsset;
		string jsonString = file.ToString ();
		JsonData jsonData = JsonMapper.ToObject (jsonString);

		List<ThumbnailData> tnl = new List<ThumbnailData> ();
		for (int i = 0; i < jsonData [theme].Count; i++) {
			ThumbnailData sd = new ThumbnailData (readId(jsonData, theme, i), readName(jsonData, theme, i), readFilePath(jsonData, theme, i));
			tnl.Add(sd);
		}
		return tnl;
	}

	public static MapData readMap (string fileName, string theme, int order) {
		SimplePolygon2D outer;
		SimplePolygon2D[] holes;

		//string jsonString = File.ReadAllText (fileName);
		TextAsset file = Resources.Load(fileName) as TextAsset;
		string jsonString = file.ToString ();
		JsonData jsonData = JsonMapper.ToObject (jsonString);

		outer = readVertices (jsonData, theme, order, "outerVertices");
		holes = new SimplePolygon2D[jsonData [theme] [order] ["holes"].Count];
		for (int i = 0; i < holes.Length; i++) {
			holes [i] = readVertices (jsonData, theme, order, "holes", i, "innerVertices");
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

	private static string readName(JsonData data, string theme, int order) {
		string name = data [theme] [order] ["name"].ToString();
		return name;
	}

	private static int readId(JsonData data, string theme, int order) {
		int id =(int)data [theme] [order] ["id"];
		return id;
	}

	private static string readFilePath(JsonData data, string theme, int order) {
		string path = data [theme] [order] ["filePath"].ToString ();
		return path;
	}

	// for outer
	private static SimplePolygon2D readVertices(JsonData data, string theme, int order, string outerhole) {
		SimplePolygon2D polygon = new SimplePolygon2D ();
		for (int i = 0; i < data [theme] [order] [outerhole].Count; i++) {
			double x = (double)data [theme] [order] [outerhole] [i] ["x"];
			double y = (double)data [theme] [order] [outerhole] [i] ["y"];
			polygon.addVertex(new Vector2((float) x, (float) y));
			}
		return polygon;
	}

	// overloading for holes
	private static SimplePolygon2D readVertices(JsonData data, string theme, int order, string innerhole, int holeNum, string component) {
		SimplePolygon2D polygon = new SimplePolygon2D ();
		for (int i = 0; i < data [theme] [order] [innerhole] [holeNum] [component].Count; i++) {
			double x = (double)data [theme] [order] [innerhole] [holeNum] [component] [i] ["x"];
			double y = (double)data [theme] [order] [innerhole] [holeNum] [component] [i] ["y"];
			polygon.addVertex (new Vector2 ((float)x, (float)y));
		}
		return polygon;
	}

	private static Color readColor(JsonData data, string theme, int order, string component) {
		double red, green, blue, alpha;
		red = (double)data [theme] [order] [component] ["r"];
		green = (double)data [theme] [order] [component] ["g"];
		blue = (double)data [theme] [order] [component] ["b"];
		alpha = (double)data [theme] [order] [component] ["a"];
		Color color = new Color ((float)red, (float)green, (float)blue, (float)alpha);
		return color;
	}
}

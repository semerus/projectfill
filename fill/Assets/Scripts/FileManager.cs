using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class FileManager {
	/*****************************************************************/
	/* Variables */
	// for singleton design
	private static FileManager instance;

	/*****************************************************************/
	/* Constructor */
	// private constructor
	private FileManager(){
	}

	/*****************************************************************/
	/* Functions */
	// getter for singleton
	public static FileManager getInstance(){
		if (instance == null) {
			instance = new FileManager ();
		}
		return instance;
	}

	public MapData readMap(string pathToDirectory){
		SimplePolygon2D outer;
		SimplePolygon2D[] holes;

		//1. Find corresponding files
		string[] fullFileList = Directory.GetFiles(pathToDirectory);

		//2. verify file names
		// for outer file
		string outerFilename = "";
		Regex regexForOuterFilename = new Regex(".*_\\d\\d_Outer.txt");
		for (int i = 0; i < fullFileList.Length; i++) {
			Match matchOuter = regexForOuterFilename.Match(fullFileList[i]);
			if (matchOuter.Success) {
				outerFilename = matchOuter.Value;
				break;
			}
		}
		if (outerFilename.Equals("", StringComparison.Ordinal)) {
			Debug.LogError("No outer file found!!");
			return null;
		}

		// for hole files
		ArrayList holeFileList = new ArrayList();
		Regex regexForHoleFilename = new Regex(".*_\\d\\d_Hole\\d\\d.txt");
		for (int i = 0; i < fullFileList.Length; i++) {
			Match matchHole = regexForHoleFilename.Match(fullFileList[i]);
			if (matchHole.Success) {
				holeFileList.Add (matchHole.Value);
			}
		}
		object[] holeFilenames = holeFileList.ToArray ();

		//3. Read files
		string outerString = readFile(outerFilename);

		//     for every filename read, save to holeStrings[i]
		string[] holeStrings = new String[holeFilenames.Length];
		for (int i = 0; i < holeFilenames.Length; i++){ 
			holeStrings [i] = readFile ((string) holeFilenames[i]);
		}

		//4. Change to SimplePolygon2D
		outer = toSimplePolygon2D(outerString);
		holes = new SimplePolygon2D[holeFilenames.Length];
		for (int i = 0; i < holeFilenames.Length; i++){ 
			holes [i] = toSimplePolygon2D (holeStrings[i]);
		}

		return new MapData(outer, holes);
	}


	/**
	 * This function reads file and converts to string
	*/
	private string readFile(string filepath){
		string retString = File.ReadAllText(filepath);

		return retString;
	}

	/**
	 * This function takes mapString (e.g., 1\t2\n3\t4\n5\t6\n
	 * and turn it into SimplePolygon2D object
	*/
	private SimplePolygon2D toSimplePolygon2D(string mapString){
		SimplePolygon2D polygonToReturn = new SimplePolygon2D();
		string[] stringPerVertex = mapString.Split (new string[] { Environment.NewLine }, StringSplitOptions.None);

		for (int i = 0; i < stringPerVertex.Length; i++) {
			string[] coordinate = stringPerVertex[i].Split(new string[] { "\t" } , StringSplitOptions.None);
			Vector2 newVertex = new Vector2 (float.Parse (coordinate [0]), float.Parse (coordinate [1]));	
			Debug.Log (newVertex);
			if (!polygonToReturn.addVertex (newVertex)) {
				Debug.LogError ("polygon.addVertex(newVertex) failed!");
			}
		}

		return polygonToReturn;
	}
	/*****************************************************************/
}

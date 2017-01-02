using System;
using UnityEngine;
using System.Collections.Generic;

public class DecisionAlgorithm
{
	static int NORTH = 0, SOUTH = 1, EAST = 2, WEST = 3;
	static float scale = 0.1f;

	MapData md;
	Vector2[] vertices;
	Edge[] edges;
	List<Vector2>[] guardingInfo;

	public DecisionAlgorithm(MapData md){
		this.md = md;

		// fixed vertices to guard
		vertices = getPointsToCheck(md);

		// fixed edges to check if hindering the visibility
		edges = md.getTotalEdges ().ToArray ();

		guardingInfo = new List<Vector2>[vertices.Length];
	}

	public bool isFilled(Vector3[] guards){
		Debug.Log ("Checking " + vertices.Length + " number of points\n");
		for(int i = 0; i < vertices.Length; i++){
			bool vertexVisible = false;
			for(int j = 0; j < guards.Length; j++){
				bool visible = true;
				Edge check = new Edge (vertices [i], guards [j]);
				for(int k = 0; k < edges.Length; k++){
					if (check.isCross (edges [k])) {
						visible = false;
//						Debug.Log ("Vertex #" + i + "/" + vertices.Length + ": " + vertices [i] + " Not visible by guard " + j + ": " + guards [j] + " because of edge: " + edges [k].ToString ());

						break;
					} else {
//						Debug.DrawLine (vertices [i], guards [j], Color.green);
					}
				}

				// if a guard can see that vertex, then that vertex is visible and move on to next one
				if (visible) {
					vertexVisible = true;
					continue;
				} 
			}

			if (!vertexVisible) {
				for(int j = 0; j < guards.Length; j++){
					Debug.DrawLine (vertices [i], guards [j], Color.red);
				}
				return false;
			}
		}

		return true;
	}

	/**
	 * This function returns a Vector2[] that will be checked by the guard*/
	private static Vector2[] getPointsToCheck(MapData md){
//		int N = md.getSize ();
//		Vector2[] toRet = new Vector2[N];
		List<Vector2> toRet = new List<Vector2>();

//		int index = 0;
		Vector2[] outer = md.getOuter ().getVertices();
		for (int i = 0; i < outer.Length; i++) {
			toRet.Add (outer [i]);
//			toRet [index++] = outer [i];
		}

		SimplePolygon2D[] holes = md.getHoles ();
		for (int i = 0; i < holes.Length; i++) {
			for(int j = 0; j < holes[i].getVertices().Length; j++){
//				toRet [index++] = holes [i].getVertices () [j];
				toRet.Add (holes [i].getVertices () [j]);
			}
		}

		Vector2[] extreme = md.getExtremePoints ();
		Debug.Log ("North: " + extreme[NORTH].ToString () + "\n");
		Debug.Log ("SOUTH: " + extreme[SOUTH].ToString () + "\n");
		Debug.Log ("West: " + extreme[WEST].ToString () + "\n");
		Debug.Log ("East: " + extreme[EAST].ToString () + "\n");


		for (float y = extreme [SOUTH].y; y < extreme [NORTH].y; y += scale) {
			for (float x = extreme [WEST].x; x < extreme [EAST].x; x += scale) {
				if (md.isInsideMap (x, y))
					toRet.Add (new Vector2 (x, y));
			}
		}

		return toRet.ToArray();
	}

	/**
	 * This function returns a Vector2[] that needs to be checked by the guards
	 * Point location is depedent on the guards' location
	*/
//	private static Vector2[] getDynamicPointsToCheck(MapData md, Vector3[] guards){
//		
//	}
}


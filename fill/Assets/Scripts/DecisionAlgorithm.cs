using System;
using UnityEngine;
using System.Collections.Generic;

public class DecisionAlgorithm
{
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
		for(int i = 0; i < vertices.Length - 1; i++){
			bool vertexVisible = false;
			for(int j = 0; j < guards.Length; j++){
				bool visible = true;
				Edge check = new Edge (vertices [i], guards [j]);
				for(int k = 0; k < edges.Length; k++){
					if (check.isCrossEasy (edges [k])) {
						visible = false;
						//Debug.Log ("Vertex : " + vertices[i] + " Not visible by guard " + j + ": " + guards[j] + " because of edge: " + edges [k].ToString());
						break;
					}
				}

				// if a guard can see that vertex, then that vertex is visible and move on to next one
				if (visible) {
					vertexVisible = true;
					continue;
				}
			}

			if (!vertexVisible) {
				return false;
			}
		}

		return true;
	}

	/**
	 * This function returns a Vector2[] that will be checked by the guard*/
	private static Vector2[] getPointsToCheck(MapData md){
		int N = md.getSize ();
		Vector2[] toRet = new Vector2[N];

		int index = 0;
		Vector2[] outer = md.getOuter ().getVertices();
		for (int i = 0; i < outer.Length; i++) {
			toRet [index++] = outer [i];
		}

		SimplePolygon2D[] holes = md.getHoles ();
		for (int i = 0; i < holes.Length; i++) {
			for(int j = 0; j < holes[i].getVertices().Length; j++){
				toRet [index++] = holes [i].getVertices () [j];
			}
		}

		return toRet;
	}
}


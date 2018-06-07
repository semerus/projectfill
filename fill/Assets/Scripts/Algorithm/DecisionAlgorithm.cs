using System;
using UnityEngine;
using System.Collections.Generic;

public class DecisionAlgorithm
{
	/*****************************************************************/
	/* Variables */
	static int NORTH = 0, SOUTH = 1, EAST = 2, WEST = 3;
	static float scale = 0.1f;

	StageData stageData;
	Vector2[] visibilityCheckpoints;
	Edge[] mapEdges;
	List<Vector2>[] guardingInfo;

	/*****************************************************************/
	/* Constructor */
	public DecisionAlgorithm(StageData stageData){
		this.stageData = stageData;

		// fixed vertices to guard
		visibilityCheckpoints = GetVisibilityCheckpoints(stageData);

		// fixed edges to check if hindering the visibility
		mapEdges = stageData.GetEdges().ToArray ();

		guardingInfo = new List<Vector2>[visibilityCheckpoints.Length];
	}

	/*****************************************************************/
	/* Methods */
	public bool IsFilled(Vector3[] guards){
		Debug.Log ("Checking " + visibilityCheckpoints.Length + " number of points\n");
		for(int i = 0; i < visibilityCheckpoints.Length; i++){
			bool isCheckpointCovered = false;
			for(int j = 0; j < guards.Length; j++){
				bool isCheckpointVisible = true;
				Edge visibilityEdge = new Edge (visibilityCheckpoints [i], guards [j]);
				for(int k = 0; k < mapEdges.Length; k++){
					if (visibilityEdge.isCross (mapEdges [k])) {
						isCheckpointVisible = false;
//						Debug.Log ("Vertex #" + i + "/" + vertices.Length + ": " + vertices [i] + " Not visible by guard " + j + ": " + guards [j] + " because of edge: " + edges [k].ToString ());
						break;
					} else {
//						Debug.DrawLine (vertices [i], guards [j], Color.green);
					}
				}

				// if a guard can see that vertex, then that vertex is covered and move on to next one
				if (isCheckpointVisible) {
					isCheckpointCovered = true;
					continue;
				}
			}

			if (!isCheckpointCovered) {
				for(int j = 0; j < guards.Length; j++){
					Debug.DrawLine (visibilityCheckpoints [i], guards [j], Color.red);
				}
				return false;
			}
		}

		return true;
	}

	/**
	 * This function returns a Vector2[] that will be checked by the guard*/
	private static Vector2[] GetVisibilityCheckpoints(StageData md){
//		int N = md.getSize ();
//		Vector2[] toRet = new Vector2[N];
		List<Vector2> toRet = new List<Vector2>();

//		int index = 0;
		Vector2[] outer = md.OuterPolygon.getVertices();
		for (int i = 0; i < outer.Length; i++) {
			toRet.Add (outer [i]);
//			toRet [index++] = outer [i];
		}

		SimplePolygon2D[] holes = md.InnerPolygons.ToArray();
        for (int i = 0; i < holes.Length; i++)
        {
            for (int j = 0; j < holes[i].getVertices().Length; j++)
            {
                toRet.Add(holes[i].getVertices()[j]);
            }
        }

        Vector2[] extreme = md.GetExtremePoints ();
		Debug.Log ("North: " + extreme[NORTH].ToString () + "\n");
		Debug.Log ("South: " + extreme[SOUTH].ToString () + "\n");
		Debug.Log ("West: " + extreme[WEST].ToString () + "\n");
		Debug.Log ("East: " + extreme[EAST].ToString () + "\n");

		for (float y = extreme [SOUTH].y; y < extreme [NORTH].y; y += scale) {
			for (float x = extreme [WEST].x; x < extreme [EAST].x; x += scale) {
				if (md.IsInsideMap (x, y))
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


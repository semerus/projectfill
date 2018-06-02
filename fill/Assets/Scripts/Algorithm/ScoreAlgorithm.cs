using System;
using UnityEngine;

public class ScoreAlgorithm
{
	/*****************************************************************/
	/* Constructor */
	public ScoreAlgorithm ()
	{
	}

	/*****************************************************************/
	/* Methods */
	public static double calculateArea(Vector3[] vertices, int[] triangles){
		// remember triangle is a vector of indices that every three pair is one triangle
		double totalArea = 0;
		for (int i = 0; i < triangles.Length / 3; i++) { // for every triangle
			double area = System.Math.Abs(areaOfTriangle(vertices[triangles[i * 3]], vertices[triangles[i * 3 + 1]], vertices[triangles[i * 3 + 2]]));
			totalArea += (area * 1000);
		}

		return totalArea;
	}

	private static double areaOfTriangle(Vector2 a, Vector2 b, Vector2 c){
		return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
	}
}

using System;
using System.Collections;
using UnityEngine;

/*SimplePolygon is a polygon that has no edge crossing*/
public class SimplePolygon2D
{
	/*Variables*/
	// Vector2[] is ordered such that polygon[i] has an undirected edge to polygon[i+1]
	private Stack polygon;

	class Edge{
		Vector2 a, b;
		public Edge(Vector2 from, Vector2 to){
			a = from;
			b = to;
		}

		/**
		 * Returns
		 * 		true : if this edge crosses with target edge
		 * 		false : if this edge does not cross with target edge
		*/
		public bool isCross(Edge target){
			//TODO
			return false;
		}
	}

	/*Constructor*/
	public SimplePolygon2D ()
	{
		polygon = new Stack ();
	}

	/*Functions*/
	/**
	 * This function returns true if successfully added a new vertex to polygon.
	 * This function fails when there exists a self intersection. 
	*/
	public bool addVertex(Vector2 newVertex){
		if (validateNewVertex (newVertex)) {
			return false;
		} else {
			polygon.Push (newVertex);
			return true;
		}
	}

	/**
	 * This function returns true if adding the new Vertex does not create an intersection
	 * This function returns false if adding the new Vertex does create an intersection
	*/
	private bool validateNewVertex(Vector2 newVertex){
		// if the number of vertices currently in polygon is less than or equal to 1 return true
		// because there is no way two points can form an intersection
		// Note. three points can be collinear, which forms a sort of intersection
		if (polygon.Count <= 1) {
			return true;
		}

		// create newly added edge
		Edge newEdge = new Edge(newVertex, (Vector2) polygon.Peek());
			
		// for every edge check if the new edge forms an intersection
		object[] toArray = (object[]) polygon.ToArray();

		for (int i = 0; i < toArray.Length - 1; i++) { // Note. toArray.Length - 1 to avoid indexOutOfRange
			// check if newEdge crosses with any of the existing edges
			if (newEdge.isCross (new Edge ((Vector2)toArray [i], (Vector2)toArray [i + 1])))
				return false;
		}

		return true;
	}
}

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
			Vector2 c, d;
			c = target.a;
			d = target.b;
			if (collinear (a, b, c) ||
				collinear (a, b, d) ||
				collinear (c, d, a) ||
				collinear (c, d, b))
				return false;

			return Xor (Left (a, b, c), Left(a, b, d)) &&
				Xor (Left (c, d, a), Left(c, d, b));
		}

		private static bool collinear(Vector2 a, Vector2 b, Vector2 c){
			return false;
		}

		private static bool Left(Vector2 a, Vector2 b, Vector2 c){
			return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y) > 0;
		}

		private static bool Xor(bool x, bool y){
			return !x ^ !y;
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
		polygon.Push (newVertex);
		return true;
//		if (validateNewVertex (newVertex)) {
//			return false;
//		} else {
//			polygon.Push (newVertex);
//			return true;
//		}
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

	/**
	 * @return Returns Vector2[] of the vertices in which they form a polygon
	*/
	public Vector2[] getVertices(){
		object[] oArray = polygon.ToArray ();
		Vector2[] toRet = new Vector2[polygon.Count];

		for (int i = 0; i < polygon.Count; i++) {
			toRet [i] = (Vector2)oArray [i];
		}

		return toRet;
	}

	/**
	 * @param position : querying location
	 * @return
	 * 		true: if the position is strictly inside the polygon
	 * 		false: if the position is not strictly inside the polygon
	 */
	public bool isInsidePolygon(Vector3 position){
		Vector2[] vertices = getVertices ();
		//TODO
		// For now assume that 
		Vector2 origin = new Vector2(position.x, position.y);
		Vector2 rightInf = new Vector2(1000, position.y);

		Edge rayToRight = new Edge (origin, rightInf);
		int count = 0;
		for (int i = 0; i < vertices.Length; i++) {
			if (rayToRight.isCross (new Edge (vertices [i], vertices [(i + 1) % vertices.Length])))
				count++;
		}

		return (count % 2 == 1);
	}
}

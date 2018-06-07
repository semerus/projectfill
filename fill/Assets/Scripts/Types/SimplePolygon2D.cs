using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// SimplePolygon is a polygon that has no edge crossing
public class SimplePolygon2D
{
	/* Variables */
	// Vector2[] is ordered such that polygon[i] has an undirected edge to polygon[i+1]
	private Stack<Vector2> polygon;
	private List<Edge> edges;
	private Vector2 lastAdded = new Vector2(-100, -100);

	/*Constructor*/
	public SimplePolygon2D ()
	{
		polygon = new Stack<Vector2> ();
		edges = new List<Edge> ();
	}

    public static SimplePolygon2D Create(Vector2[] points)
    {
        if(points.Length < 3)
        {
            return null;
        }

        var result = new SimplePolygon2D();

        foreach (var point in points)
        {
            if(!result.addVertex(point))
            {
                return null;
            }
        }

        return result;
    }

	/* Functions */
	/**
	 * This function returns true if successfully added a new vertex to polygon.
	 * This function fails when there exists a self intersection. 
	*/
	/// <summary>
	/// Adds the vertex.
	/// </summary>
	/// <returns><c>true</c>, if vertex was added, <c>false</c> otherwise.</returns>
	/// <param name="newVertex">New vertex.</param>
	public bool addVertex(Vector2 newVertex){
//		if (validateNewVertex (newVertex)) {
			// must add edge first, before pushing in to stack
			if (lastAdded.x != -100 && lastAdded.y != -100) { // TODO: find better way to do this
				edges.Add(new Edge(lastAdded, newVertex));
			}
			lastAdded = newVertex;
			polygon.Push (newVertex);
			return true;
//		} else {
//			return false;
//		}
	}

	/**
	 * This function returns true if adding the new Vertex does not create an intersection
	 * This function returns false if adding the new Vertex does create an intersection
	*/
	/// <summary>
	/// Validates the new vertex.
	/// </summary>
	/// <returns><c>true</c>, if new vertex was validated, <c>false</c> otherwise.</returns>
	/// <param name="newVertex">New vertex.</param>
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
		Vector2[] toArray = polygon.ToArray();

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
	/// <summary>
	/// Gets the vertices.
	/// </summary>
	/// <returns>The vertices.</returns>
	public Vector2[] getVertices(){
		Vector2[] oArray = polygon.ToArray ();
		Vector2[] toRet = new Vector2[polygon.Count];

		for (int i = 0; i < polygon.Count; i++) {
			toRet [i] = oArray [i];
		}

		return toRet;
	}

	/**
	 * @param position : querying location
	 * @return
	 * 		true: if the position is strictly inside the polygon
	 * 		false: if the position is not strictly inside the polygon
	 */
	/// <summary>
	/// Determine whether the provided position is inside the polygon.
	/// </summary>
	/// <returns><c>true</c>, if position is inside the polygon, <c>false</c> otherwise.</returns>
	/// <param name="position">Position to check.</param>
	public bool IsInsidePolygon(Vector3 position){
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

	/**
	 * @return
	 * 		Returns edges ordered from start to end
	*/
	public List<Edge> GetEdges(){
		return edges;
	}
}

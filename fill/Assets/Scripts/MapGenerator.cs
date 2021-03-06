﻿using UnityEngine;
using System.Collections;

public class MapGenerator {

//	public GameObject linePrefab;
	public GameObject vertexPrefab;
	float scale = 0.1f;

	//creating map using the vertices
	public void createMap (MapData md) {
		// Change Background Color
		GameObject bg = GameObject.Find("Background");
		bg.GetComponent<MeshRenderer> ().material = new Material (Shader.Find ("Sprites/Default"));
		bg.GetComponent<MeshRenderer> ().material.color = md.getBackgroundColor();

		// assert there are at least 3 vertices
		Vector2[] vertices = md.getOuter().getVertices();

		if (vertices.Length < 3) {
			Debug.LogError ("not enough vertices");
		}

		GameObject temp = new GameObject("Line Renderer " + "Outer");
		LineRenderer lineRenderer = temp.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth (1f * scale, 1f * scale);
		lineRenderer.SetColors (md.getLineColor(), md.getLineColor());
		lineRenderer.SetVertexCount (vertices.Length);
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

		// add vertices
		for (int i = 0; i < vertices.Length; i++) {
			lineRenderer.SetPosition (i, vertices[i]);
		}
		CreateEdgeCollider (vertices);

		SimplePolygon2D[] holes = md.getHoles ();
		for (int i = 0; i < holes.Length; i++) {
			Vector2[] hole_vertices = holes[i].getVertices();

			if (hole_vertices.Length < 3) {
				Debug.LogError ("not enough vertices");
			}

			GameObject temp_hole = new GameObject("Line Renderer " + "Hole" + i);
			LineRenderer lineRenderer_hole = temp_hole.AddComponent<LineRenderer> ();
			lineRenderer_hole.SetWidth (1f * scale, 1f * scale);
			lineRenderer_hole.SetColors (md.getLineColor(), md.getLineColor());
			lineRenderer_hole.material = new Material (Shader.Find ("Sprites/Default"));
			lineRenderer_hole.SetVertexCount (hole_vertices.Length);

			// add vertices
			for (int j = 0; j < hole_vertices.Length; j++) {
				lineRenderer_hole.SetPosition (j, hole_vertices[j]);
			}
			CreateEdgeCollider (hole_vertices);
		}
	}

	//creating map using the vertices
	void createMap (Vector2[] vertices) {
		// assert there are at least 3 vertices
		if (vertices.Length < 3) {
			Debug.LogError ("not enough vertices");
		}

		GameObject temp = new GameObject("Line Renderer");

		LineRenderer lineRenderer = temp.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth (0.1f * scale, 0.1f * scale);
		lineRenderer.SetColors (Color.white, Color.black);
		lineRenderer.SetVertexCount (vertices.Length);

		// add vertices
		for (int i = 0; i < vertices.Length; i++) {
			lineRenderer.SetPosition (i, vertices[i]);
		}

//		CreateVertex (vertices);
		CreateEdgeCollider (vertices);
	}

//	//creating a dot sprite for each vertex to render round corners
//	void CreateVertex(Vector2[] vertices) {
//		
//		for (int i = 0; i < vertices.Length - 1; i++) {
//			GameObject vertex = Instantiate (vertexPrefab) as GameObject;
//			vertex.name = "vertex" + i;
//			vertex.transform.position = vertices [i];
//		}
//	}

	//creating EdgeCollider2D by connecting the vertices of the polygon
	void CreateEdgeCollider (Vector2[] vertices) {
		GameObject edge = new GameObject ("Edge Collider");

		EdgeCollider2D edgeCollider = edge.AddComponent<EdgeCollider2D> ();
		edgeCollider.points = vertices;
	}
}

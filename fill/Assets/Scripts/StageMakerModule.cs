using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageMakerModule : MonoBehaviour {

	GameObject dotPrefab;
	GameObject linePrefab;
	GameObject backLinePrefab;

	GameObject background;
	GameObject board;
	LineRenderer line;

	List<Vertex> dots = new List<Vertex>();

	int vertexIdCounter = 0;

	public bool dotProcessing;

	void Start()
	{
		dotPrefab = Resources.Load<GameObject>("SimpleVertex");
		linePrefab = Resources.Load<GameObject>("DrawingLine");
		backLinePrefab = Resources.Load<GameObject> ("BackgroundLine");

		board = GameObject.Find ("DrawingBoard");
		background = GameObject.Find ("Background");
		var lineGO = Instantiate (linePrefab, board.transform);
		lineGO.transform.SetAsFirstSibling ();
		line = lineGO.GetComponent<LineRenderer> ();

		CreateBackground (12f, 20f, 1f);
	}

	void Update()
	{
		if (dotProcessing) { return; }
		if (Input.GetKeyDown (KeyCode.Mouse0)) 
		{
			var mousePosInWorld = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePosInWorld = new Vector3 (mousePosInWorld.x, mousePosInWorld.y, 0f);
			CreateDot (mousePosInWorld);
		}
	}

	public void CreateDot(Vector3 position)
	{
		// when coming back to the first dot
		// end the polygon and save
		if (dots.Count > 2 && Vector2.Distance (dots [0].transform.position, position) < 0.5f) 
		{
			// create
			position = dots[0].transform.position;
			// save polygon
		}
		// coming back to already drawn dot
		else if (dots.Count > 0) 
		{
			for (int i = 0; i < dots.Count; i++) {
				if (Vector2.Distance (dots [i].transform.position, position) < 0.5f) {
					return;
				}
			}
		}


		var dot = Instantiate (dotPrefab, board.transform);
		dot.transform.position = position;

		/*
		foreach (var _dot in dots) {
			if (Vector2.Distance (_dot.transform.position, position) < 0.5f) 
			{
				dot.transform.position = _dot.transform.position;
				break;
			}
		}
		*/


		var vertex = dot.AddComponent<Vertex> ();
		vertex.SetVertex (vertexIdCounter++);

		dots.Add (vertex);
		UpdateLine ();
	}

	public void ChangeDotPosition()
	{
		UpdateLine ();
	}

	void UpdateLine()
	{
		if (dots.Count < 2) { return; }

		var posList = new List<Vector3> ();
		foreach (var _dot in dots) {
			posList.Add(_dot.transform.position);
		}
		DrawLine (posList, line);
	}

	void DrawLine(List<Vector3> points, LineRenderer renderer)
	{
		var array = points.ToArray ();
		renderer.positionCount = array.Length;
		renderer.SetPositions (array);
	}

	// -9, -5
	void CreateBackground(float height, float width, float interval)
	{
		var points = new List<Vector3> ();
		var start = new Vector3 (-10f, -6f);
		var current = start;
		while (current.x < start.x + width) 
		{
			points.Add (current);
			current += new Vector3 (0f, height);
			points.Add (current);
			current += new Vector3 (interval, 0f);
			points.Add (current);
			current += new Vector3 (0f, -height);
			points.Add (current);
			current += new Vector3 (interval, 0f);
		}

		var vertical = Instantiate (backLinePrefab, background.transform);
		vertical.transform.name = "Vertical";
		var _line = vertical.GetComponent<LineRenderer> ();
		DrawLine (points, _line);

		Debug.Log (start);

		points = new List<Vector3> ();
		current = start;
		while (current.y < start.y + height) 
		{
			points.Add (current);
			current += new Vector3 (width, 0f);
			points.Add (current);
			current += new Vector3 (0f, interval);
			points.Add (current);
			current += new Vector3 (-width, 0f);
			points.Add (current);
			current += new Vector3 (0f, interval);
		}

		var horizontal = Instantiate (backLinePrefab, background.transform);
		horizontal.transform.name = "Horizontal";
		_line = horizontal.GetComponent<LineRenderer> ();
		DrawLine (points, _line);
	}

	void Save()
	{
	}
}

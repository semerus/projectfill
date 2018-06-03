using GiraffeStar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VertexCountMessage : MessageCore
{
    public int VertexCount;
}

public class StageMakerModule : Module {

    GameObject root;
	GameObject dotPrefab;
	GameObject linePrefab;
	GameObject backLinePrefab;

	GameObject background;
    Transform basicLayer;
	GameObject board;
	LineRenderer line;
    GameObject editableSpace;

	List<Vertex> dots = new List<Vertex>();
    Stack<List<Vertex>> cachedHistory = new Stack<List<Vertex>>();

	int vertexIdCounter = 0;
    int viewState = 3;
    public bool IsSnapping { get; set; }
    bool isComplete;

	public bool dotProcessing;

    public override void OnRegister()
    {
        base.OnRegister();
        Init();
    }

    void Init()
    {
        dotPrefab = Resources.Load<GameObject>("SimpleVertex");
        linePrefab = Resources.Load<GameObject>("DrawingLine");
        backLinePrefab = Resources.Load<GameObject>("BackgroundLine");

        // draw back lines
        root = GameObject.Find("Root");
        board = GameObject.Find("DrawingBoard");
        background = root.FindChildByName("Background");
        var layer = new GameObject("BasicLayer");
        layer.transform.SetParent(background.transform);
        basicLayer = layer.transform;
        CreateBackground(1f, 0.1f, Color.black, basicLayer);
        
        // prepare dot line
        var lineGO = Object.Instantiate(linePrefab, board.transform);
        lineGO.transform.SetAsFirstSibling();
        line = lineGO.GetComponent<LineRenderer>();
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        editableSpace = GameObject.Find("EditableSpace");
        var clickable = editableSpace.GetComponent<ClickableSpace>();
        clickable.OnPointerClicked += () =>
        {
            if (dotProcessing) { return; }
            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosInWorld = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
            CreateDot(mousePosInWorld);            
        };                
    }

	public void CreateDot(Vector3 position)
	{
		// when coming back to the first dot
		// end the polygon
		if (dots.Count > 2 && Vector2.Distance (dots [0].transform.position, position) < 0.5f) 
		{			
            isComplete = true;
            return;
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


		var dot = Object.Instantiate (dotPrefab, board.transform);
        var vertex = dot.AddComponent<Vertex>();
        dot.transform.position = position;
        if(IsSnapping)
        {
            vertex.Snap();
        }

        /*
		foreach (var _dot in dots) {
			if (Vector2.Distance (_dot.transform.position, position) < 0.5f) 
			{
				dot.transform.position = _dot.transform.position;
				break;
			}
		}
		*/
        vertex.SetVertex (vertexIdCounter++);

		dots.Add (vertex);
		UpdateLine ();

        //CacheDotHistory();

        new VertexCountMessage
        {
            VertexCount = dots.Count,
        }.Dispatch();
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

        if(isComplete)
        {
            posList.Add(dots[0].transform.position);
        }

		DrawLine (posList, line);
	}

	void DrawLine(List<Vector3> points, LineRenderer renderer)
	{
		var array = points.ToArray ();
		renderer.positionCount = array.Length;
		renderer.SetPositions (array);
	}

	void CreateBackground(float interval, float width, Color color, Transform layer)
	{
        var existingLines = layer.GetComponentsInChildren<LineRenderer>();
        foreach (var line in existingLines)
        {
            Object.Destroy(line.gameObject);
        }

        var camera = Camera.main;
        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        var upRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
        var currentVertical = Mathf.Round(bottomLeft.x);
        var currentHorizontal = Mathf.Round(bottomLeft.y);

        while (currentVertical < upRight.x)
        {
            var nextLine = Object.Instantiate(linePrefab, layer.transform).GetComponent<LineRenderer>();
            var startPoint = new Vector3(currentVertical, bottomLeft.y);
            var endPoint = new Vector3(currentVertical, upRight.y);
            nextLine.positionCount = 2;
            nextLine.SetPositions(new Vector3[] { startPoint, endPoint });
            nextLine.startWidth = width;
            nextLine.endWidth = width;
            nextLine.startColor = color;
            nextLine.endColor = color;
            currentVertical += interval;
        }

        while (currentHorizontal < upRight.y)
        {
            var nextLine = Object.Instantiate(linePrefab, layer.transform).GetComponent<LineRenderer>();
            var startPoint = new Vector3(bottomLeft.x, currentHorizontal);
            var endPoint = new Vector3(upRight.x, currentHorizontal);
            nextLine.positionCount = 2;
            nextLine.SetPositions(new Vector3[] { startPoint, endPoint });
            nextLine.startWidth = width;
            nextLine.endWidth = width;
            nextLine.startColor = color;
            nextLine.endColor = color;
            currentHorizontal += interval;
        }        
	}

    public void Clear()
    {
        foreach (var dot in dots)
        {
            Object.Destroy(dot.gameObject);
        }

        dots.Clear();
        line.positionCount = 0;

        new VertexCountMessage
        {
            VertexCount = dots.Count,
        }.Dispatch();
    }

    // save stage data
	public void Save()
	{
		var previousList = JsonIO.Load<List<StageData>> (Application.persistentDataPath + "/test.json");
		var stageLists = new List<StageData> ();
		if (previousList != null) {
			stageLists.AddRange (previousList);
		}


		List<Vector2> outer = new List<Vector2> ();
		foreach (var dot in dots) {
			outer.Add (dot.transform.position);
		}
		var stageData = new StageData ("test", 1, outer, new Color(0f,0f,0f,1f));

        stageLists.Add(stageData);

		// path should be constant
		JsonIO.Save(Application.persistentDataPath + "/test.json", stageLists);
	}

    // caching
    public void CacheDotHistory()
    {
        var copy = new List<Vertex>();
        copy.AddRange(dots);
        cachedHistory.Push(dots);
    }

    public void LoadDotHistory()
    {

    }

    // magnify
    public void ChangeView(bool magnify)
    {
        // 1~5
        if(magnify && viewState < 5)
        {
            Camera.main.orthographicSize -= 1f;
            viewState++;
            CreateBackground(1f, 0.1f, Color.black, basicLayer);
        }
        else if(!magnify && viewState > 1)
        {
            Camera.main.orthographicSize += 1f;
            viewState--;
            CreateBackground(1f, 0.1f, Color.black, basicLayer);
        }
    }
}

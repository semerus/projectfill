using GiraffeStar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FillClient.UI;

public class VertexCountMessage : MessageCore
{
    public int VertexCount;
}

public class DrawData
{
    public bool IsComplete;
    public List<Vertex> vertices = new List<Vertex>();
    public LineRenderer Line;    
    public int Count
    {
        get { return vertices.Count; }
    }
    public Vertex FirstVertex
    {
        get { return vertices.Count > 0 ? vertices[0] : null; }
    }

    public DrawData(LineRenderer line)
    {
        Line = line;
    }

    public bool AddVertex(Vertex vertex)
    {
        vertices.Add(vertex);

        return true;
    }

    public void Clear()
    {
        vertices.Clear();
        Line.positionCount = 0;
        IsComplete = false;
    }

    public bool IsCloseToFirstVertex(Vector3 pos)
    {
        return Count > 2 && Vector2.Distance(FirstVertex.transform.position, pos) < 0.5f;
    }

    public List<Vector2> GetDataPoints()
    {
        var data = new List<Vector2>();
        foreach (var vertex in vertices)
        {
            data.Add(vertex.transform.position);
        }

        return data;
    }

    public List<Vector3> GetDrawPoints()
    {
        if (Count < 2) { return null; }

        var posList = new List<Vector3>();
        foreach (var _dot in vertices)
        {
            posList.Add(_dot.transform.position);
        }

        if (IsComplete)
        {
            posList.Add(FirstVertex.transform.position);
        }

        return posList;
    }
}

public class StageMakerModule : Module {

    GameObject root;
	GameObject dotPrefab;
	GameObject linePrefab;
	GameObject backLinePrefab;

	GameObject background;
    Transform basicLayer;
    GameObject board;	
    GameObject editableSpace;

    DrawData outerVertices;
    List<DrawData> innerGroups = new List<DrawData>();
    Stack<List<Vertex>> cachedHistory = new Stack<List<Vertex>>();

	int vertexIdCounter = 0;
    int viewState = 3;
    public bool IsSnapping { get; set; }
    int currentInnerIndex = 0;

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
        var line = CreateLine();
        outerVertices = new DrawData(line);

        editableSpace = GameObject.Find("EditableSpace");
        var clickable = editableSpace.GetComponent<ClickableSpace>();
        clickable.onPointerClick += (e) =>
        {            
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).OverrideZ(0f);
            CreateDot(worldPos);            
        };                
    }

    public LineRenderer CreateLine()
    {
        var lineGO = Object.Instantiate(linePrefab, board.transform);
        lineGO.transform.SetAsFirstSibling();
        var line = lineGO.GetComponent<LineRenderer>();
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        return line;
    }

	public void CreateDot(Vector3 position)
	{
        // when coming back to the first dot
        // end the polygon
        if(!outerVertices.IsComplete && outerVertices.IsCloseToFirstVertex(position))
        {
            outerVertices.IsComplete = true;
            UpdateLine();
            return;
        }

        foreach (var inner in innerGroups)
        {
            if(!inner.IsComplete && inner.IsCloseToFirstVertex(position))
            {
                inner.IsComplete = true;
                UpdateLine();
                return;
            }
        }

        // 이미 점을 컨트롤하는 있으면 패스
        if (dotProcessing) { return; }

        /*
        // 이미 존재하는 점 근처면 패스
        if (outerVertices.Count > 0) 
		{
			for (int i = 0; i < outerVertices.Count; i++) {
				if (Vector2.Distance (outerVertices [i].transform.position, position) < 0.5f) {
					return;
				}
			}
		}
        */

        // 일단 생성
        var dot = Object.Instantiate(dotPrefab, board.transform);
        var vertex = dot.AddComponent<Vertex>();
        dot.transform.position = position;
        if (IsSnapping)
        {
            vertex.Snap();
        }
        vertex.SetVertex(vertexIdCounter++);

        // 어느 도형에 들어가는지 판단
        if (!outerVertices.IsComplete)
        {
            outerVertices.AddVertex(vertex);
        }
        else
        {
            DrawData data = null;
            foreach (var inner in innerGroups)
            {
                if (!inner.IsComplete)
                {
                    data = inner;
                    break;
                }
            }

            if (data == null)
            {
                var line = CreateLine();
                data = new DrawData(line);
                innerGroups.Add(data);
            }

            data.AddVertex(vertex);
        }        

        //CacheDotHistory();

        UpdateLine();
        new VertexCountMessage
        {
            VertexCount = outerVertices.Count,
        }.Dispatch();
	}    

	public void ChangeDotPosition()
	{
		UpdateLine ();
	}

	void UpdateLine()
	{
        var outerDots = outerVertices.GetDrawPoints();
        if(outerDots != null)
        {
            DrawLines(outerDots, outerVertices.Line);
        }

        foreach (var inner in innerGroups)
        {
            var innerDots = inner.GetDrawPoints();
            if(innerDots != null)
            {
                DrawLines(innerDots, inner.Line);
            }
        }
	}

	void DrawLines(List<Vector3> points, LineRenderer renderer)
	{
        if(points.Count < 2) { return; }
		var array = points.ToArray ();
		renderer.positionCount = array.Length;
		renderer.SetPositions (array);
	}

    void SetLine(Vector2 from, Vector2 to, GameObject line)
    {
        var midPoint = (from + to) / 2f;
        var distance = Vector2.Distance(from, to);
        var angle = Vector2.SignedAngle(new Vector2(midPoint.x, distance / 2f), to);

        var renderer = line.GetOrAddComponent<LineRenderer>();
        var collider = line.GetOrAddComponent<BoxCollider2D>();

        renderer.positionCount = 2;
        renderer.SetPosition(0, new Vector3(0f, -distance / 2f, 0f));
        renderer.SetPosition(1, new Vector3(0f, distance / 2f, 0f));
        renderer.widthMultiplier = 0.1f;
        renderer.SetFullWidth(0.1f);

        collider.size = new Vector2(0.1f, distance - 0.2f);
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
        foreach (var dot in outerVertices.vertices)
        {
            Object.Destroy(dot.gameObject);
        }

        foreach (var inner in innerGroups)
        {
            foreach (var vertex in inner.vertices)
            {
                Object.Destroy(vertex.gameObject);
            }
        }

        outerVertices.Clear();
        foreach (var inner in innerGroups)
        {
            inner.Clear();
        }        

        new VertexCountMessage
        {
            VertexCount = outerVertices.Count,
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

		var outerData = new List<Vector2> ();
        outerData.AddRange(outerVertices.GetDataPoints());

        var innerData = new List<List<Vector2>>();
        foreach (var inner in innerGroups)
        {
            var data = new List<Vector2>();
            data.AddRange(inner.GetDataPoints());
            innerData.Add(data);
        }

		var stageData = new StageData ("test", 1, outerData, innerData, new Color(0f,0f,0f,1f));

        stageLists.Add(stageData);

		// path should be constant
		JsonIO.Save(Application.persistentDataPath + "/test.json", stageLists);
        new DisplayModalMessage()
        {
            message = "Saved Successfully",
        }.Dispatch();
	}

    // caching
    public void CacheDotHistory()
    {
        var copy = new List<Vertex>();
        copy.AddRange(outerVertices.vertices);
        //cachedHistory.Push(outerVertices);
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

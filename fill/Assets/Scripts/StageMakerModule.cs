using FillClient.UI;
using GiraffeStar;
using System.Collections.Generic;
using UnityEngine;

namespace FillClient
{
    public class VertexCountMessage : MessageCore
    {
        public int VertexCount;
    }

    public class StageMakerModule : Module
    {
        GameObject root;
        GameObject dotPrefab;
        GameObject linePrefab;
        GameObject backLinePrefab;

        GameObject background;
        Transform basicLayer;
        GameObject board;
        GameObject editableSpace;

        EditablePolygon outerVertices;
        List<EditablePolygon> innerGroups = new List<EditablePolygon>();
        Stack<List<Vertex>> cachedHistory = new Stack<List<Vertex>>();

        //int vertexIdCounter = 0;
        int viewState = 3;
        public bool IsSnapping { get; set; }
        //int currentInnerIndex = 0;
        EditablePolygon currentPolygon;

        public bool dotProcessing;
        Vertex selectedVertex;
        public Vertex SelectedVertex
        {
            get { return selectedVertex; }
            set
            {
                if(selectedVertex != null)
                {
                    selectedVertex.ChangeColor(false);
                }
                selectedVertex = value;
                selectedVertex.ChangeColor(true);
            }
        }

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

        void CreateDot(Vector3 position)
        {
            // 현재 진행 중인 폴리곤이 없다
            if(currentPolygon == null)
            {
                if(outerVertices == null || outerVertices.Count < 1)
                {
                    outerVertices = new EditablePolygon();
                    currentPolygon = outerVertices;
                }
                else
                {
                    var nextInner = new EditablePolygon();
                    innerGroups.Add(nextInner);
                    currentPolygon = nextInner;
                }
            }

            if(!currentPolygon.IsComplete && currentPolygon.IsCloseToFirstVertex(position))
            {
                // when coming back to the first dot
                // end the polygon
                AddNextVertex(currentPolygon, position);
                currentPolygon = null;
                return;
            }

            if(dotProcessing) { return; }

            AddNextVertex(currentPolygon, position);
        }

        void AddNextVertex(EditablePolygon polygon, Vector3 pos)
        {
            if (polygon.IsComplete) { return; }

            if (polygon.IsCloseToFirstVertex(pos))
            {
                // 마지막 연결 잘하기
                var obj = GameObject.Instantiate(linePrefab);
                var next = obj.GetOrAddComponent<Line>();
                next.SetLine(polygon.LastVertex, polygon.FirstVertex, polygon);
                polygon.IsComplete = true;
                return;
            }

            var dot = Object.Instantiate(dotPrefab, board.transform);
            var vertex = dot.AddComponent<Vertex>();
            vertex.IncludedPolygon = currentPolygon;
            dot.transform.position = pos;
            if (IsSnapping)
            {
                vertex.Snap();
            }
            var startVertex = polygon.LastVertex;
            polygon.vertices.Add(vertex);
            if(startVertex != null)
            {
                var nextLineObj = GameObject.Instantiate(linePrefab);
                var nextLine = nextLineObj.GetOrAddComponent<Line>();
                nextLine.SetLine(startVertex, polygon.LastVertex, polygon);
            }

            new VertexCountMessage
            {
                VertexCount = outerVertices.Count,
            }.Dispatch();
        }

        public void ChangeDotPosition()
        {
            UpdateLine();
        }

        void UpdateLine()
        {
            //var outerDots = outerVertices.GetDrawPoints();
            //if(outerDots != null)
            //{
            //    //DrawLines(outerDots, outerVertices.Line);
            //    DrawLines(outerDots);
            //}

            //foreach (var inner in innerGroups)
            //{
            //    var innerDots = inner.GetDrawPoints();
            //    if(innerDots != null)
            //    {
            //        //DrawLines(innerDots, inner.Line);
            //        DrawLines(innerDots);
            //    }
            //}
        }

        //void DrawLines(List<Vector3> points, LineRenderer renderer)
        //{
        //       if(points.Count < 2) { return; }
        //	var array = points.ToArray ();
        //	renderer.positionCount = array.Length;
        //	renderer.SetPositions (array);
        //}

        //void DrawLines(List<Vector3> points)
        //{
        //    if (points.Count < 2) { return; }
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        var newLine = new GameObject();
        //        newLine.transform.SetParent(board.transform);
        //        if(i == points.Count - 1)
        //        {
        //            SetLine(points[i], points[0], newLine);
        //        }
        //        else
        //        {
        //            SetLine(points[i], points[i + 1], newLine);
        //        }
        //    }
        //}

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
                if(dot.StartingLine != null)
                {
                    Object.Destroy(dot.StartingLine.gameObject);
                }
                Object.Destroy(dot.gameObject);                
            }

            foreach (var inner in innerGroups)
            {
                foreach (var vertex in inner.vertices)
                {
                    if (vertex.StartingLine != null)
                    {
                        Object.Destroy(vertex.StartingLine.gameObject);
                    }
                    Object.Destroy(vertex.gameObject);                    
                }
            }

            currentPolygon = null;
            outerVertices = null;
            innerGroups = new List<EditablePolygon>();

            new VertexCountMessage
            {
                VertexCount = 0,
            }.Dispatch();
        }

        // save stage data
        public void Save()
        {
            var previousList = JsonIO.Load<List<StageData>>(Application.persistentDataPath + "/test.json");
            var stageLists = new List<StageData>();
            if (previousList != null)
            {
                stageLists.AddRange(previousList);
            }

            var outerData = new List<Vector2>();
            outerData.AddRange(outerVertices.GetDataPoints());

            var innerData = new List<List<Vector2>>();
            foreach (var inner in innerGroups)
            {
                var data = new List<Vector2>();
                if(inner.GetDataPoints().Count < 1) { continue; }
                data.AddRange(inner.GetDataPoints());
                innerData.Add(data);
            }

            var sceneModule = GiraffeSystem.FindModule<StageMakerScene>();
            var title = sceneModule.Title.IsNullOrEmpty() ? "Test" : sceneModule.Title;
            var stageData = new StageData(title, 1, outerData, innerData);

            stageLists.Add(stageData);

            // path should be constant
            JsonIO.Save(Application.persistentDataPath + "/test.json", stageLists);
            new DisplayModalMessage()
            {
                message = "Saved Successfully",
            }.Dispatch();
        }

        public void InsertVertex(EditablePolygon polygon, Line line, Vector3 worldPos)
        {
            var dot = Object.Instantiate(dotPrefab, board.transform);
            var vertex = dot.AddComponent<Vertex>();
            vertex.IncludedPolygon = currentPolygon;
            dot.transform.position = worldPos;
            if (IsSnapping)
            {
                vertex.Snap();
            }

            // line 이 새로운 점의 ending line이 된다.
            var frontIndex = polygon.vertices.IndexOf(line.StartVertex);
            polygon.vertices.Insert(frontIndex + 1, vertex);
            var endVertex = line.EndVertex;
            line.SetLine(line.StartVertex, vertex, polygon);

            var nextLineObj = GameObject.Instantiate(linePrefab);
            var nextLine = nextLineObj.GetOrAddComponent<Line>();
            nextLine.SetLine(vertex, endVertex, polygon);
        }

        public void DeleteVertex()
        {
            if(selectedVertex != null)
            {                
                selectedVertex.DeleteVertex();
                selectedVertex = null;
            }
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
            if (magnify && viewState < 5)
            {
                Camera.main.orthographicSize -= 1f;
                viewState++;
                CreateBackground(1f, 0.1f, Color.black, basicLayer);
            }
            else if (!magnify && viewState > 1)
            {
                Camera.main.orthographicSize += 1f;
                viewState--;
                CreateBackground(1f, 0.1f, Color.black, basicLayer);
            }
        }
    }
}


using GiraffeStar;
using UnityEngine;

namespace FillClient
{
    public class Line : InputBehaviour
    {
        StageMakerModule module;

        public EditablePolygon IncludedPolygon { get; private set; }
        public Vertex StartVertex { get; private set; }
        public Vertex EndVertex { get; private set; }
        public LineRenderer LineRenderer { get; private set; }

        void Start()
        {
            module = GiraffeSystem.FindModule<StageMakerModule>();

            OnInputDown += () =>
            {
                module.dotProcessing = true;                
            };

            OnHoldEnd += () =>
            {
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).OverrideZ(0f);
                module.InsertVertex(IncludedPolygon, this, worldPos);
            };

            OnInputUp += () =>
            {
                module.dotProcessing = false;
            };
        }

        public void SetLine(Vertex startVertex, Vertex endVertex, EditablePolygon included)
        {
            SetLine(startVertex, endVertex, included, LineRenderer.startColor);
        }

        public void SetLine(Vertex startVertex, Vertex endVertex, EditablePolygon included, Color color)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            IncludedPolygon = included;

            StartVertex.StartingLine = this;
            EndVertex.EndingLine = this;

            SetLineInternal(StartVertex.transform.position, EndVertex.transform.position);

            LineRenderer.SetFullColor(color);
        }

        public void DeleteLine()
        {
            GameObject.Destroy(gameObject);
        }

        void SetLineInternal(Vector2 from, Vector2 to)
        {
            var midPoint = (from + to) / 2f;    
            var distance = Vector2.Distance(from, to);
            var angle = 0f;
            if(to.x > from.x)
            {
                angle = Vector2.Angle(Vector2.up, from - midPoint);
            }
            else
            {
                angle = Vector2.Angle(Vector2.up, to - midPoint);
            }
            

            LineRenderer = gameObject.GetOrAddComponent<LineRenderer>();
            var collider = gameObject.GetOrAddComponent<BoxCollider2D>();

            transform.position = midPoint;

            LineRenderer.useWorldSpace = false;
            LineRenderer.positionCount = 2;
            LineRenderer.SetPosition(0, new Vector3(0f, -distance / 2f, 0f));
            LineRenderer.SetPosition(1, new Vector3(0f, distance / 2f, 0f));
            LineRenderer.widthMultiplier = 0.1f;
            LineRenderer.SetFullWidth(0.1f);

            collider.size = new Vector2(0.5f, distance - 0.7f);
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}



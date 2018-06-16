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
            StartVertex = startVertex;
            EndVertex = endVertex;
            IncludedPolygon = included;

            StartVertex.StartingLine = this;
            EndVertex.EndingLine = this;

            SetLineInternal(StartVertex.transform.position, EndVertex.transform.position);
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
            

            var renderer = gameObject.GetOrAddComponent<LineRenderer>();
            var collider = gameObject.GetOrAddComponent<BoxCollider2D>();

            transform.position = midPoint;

            renderer.useWorldSpace = false;
            renderer.positionCount = 2;
            renderer.SetPosition(0, new Vector3(0f, -distance / 2f, 0f));
            renderer.SetPosition(1, new Vector3(0f, distance / 2f, 0f));
            renderer.widthMultiplier = 0.1f;
            renderer.SetFullWidth(0.1f);

            collider.size = new Vector2(0.5f, distance - 0.7f);
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}



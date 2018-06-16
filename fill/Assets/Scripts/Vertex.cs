using UnityEngine;
using GiraffeStar;

namespace FillClient
{
    public class Vertex : InputBehaviour
    {
        public int Id { get; private set; }
        //bool isDragging;
        StageMakerModule module;
        bool isInitialized;
        Color originalColor;
        public EditablePolygon IncludedPolygon { get; set; }

        public Line StartingLine { get; set; }
        public Line EndingLine { get; set; }

        void Awake()
        {
            module = GiraffeSystem.FindModule<StageMakerModule>();
            Init();
        }

        void Init()
        {
            if (isInitialized) { return; }

            OnInputDown += () =>
            {
                module.dotProcessing = true;
            };

            OnInputUp += () =>
            {
                module.dotProcessing = false;

                if (module.IsSnapping)
                {
                    Snap();
                }
            };

            OnClick += () =>
            {
                module.SelectedVertex = this;
            };

            OnDrag += () =>
            {
                Vector3 nextPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).OverrideZ(0f);
                transform.position = nextPos + Offset;
                if(StartingLine != null)
                {
                    StartingLine.SetLine(this, StartingLine.EndVertex, StartingLine.IncludedPolygon);
                }

                if(EndingLine != null)
                {
                    EndingLine.SetLine(EndingLine.StartVertex, this, EndingLine.IncludedPolygon);
                }
            };

            originalColor = transform.GetComponent<SpriteRenderer>().color;

            isInitialized = true;
        }

        public void ChangeColor(bool isSelected)
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.color = isSelected ? Color.red : originalColor;
        }

        public void DeleteVertex()
        {
            IncludedPolygon.vertices.Remove(this);

            if(StartingLine != null)
            {
                var endingVertex = StartingLine.EndVertex;
                StartingLine.DeleteLine();
                EndingLine.SetLine(EndingLine.StartVertex, endingVertex, EndingLine.IncludedPolygon);
            }
            GameObject.Destroy(gameObject);
        }

        public void Snap()
        {
            var snappedX = Mathf.Round(transform.position.x);
            var snappedY = Mathf.Round(transform.position.y);

            transform.position = new Vector3(snappedX, snappedY);
            module.ChangeDotPosition();
        }
    }
}


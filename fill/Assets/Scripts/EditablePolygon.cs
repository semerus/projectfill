using System.Collections.Generic;
using GiraffeStar;
using UnityEngine;

namespace FillClient
{
    public class EditablePolygon
    {
        public bool IsComplete;
        public List<Vertex> vertices = new List<Vertex>();
        public List<Line> lines = new List<Line>();

        public int Count
        {
            get { return vertices.Count; }
        }

        public Vertex FirstVertex
        {
            get { return vertices.Count > 0 ? vertices[0] : null; }
        }

        public Vertex LastVertex
        {
            get { return vertices.Count > 0 ? vertices[vertices.Count - 1] : null; }
        }

        public void Clear()
        {
            vertices.Clear();
            //Line.positionCount = 0;
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
}



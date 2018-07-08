using GiraffeStar;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FillClient
{
    public enum GuardState
    {
        Default,
        Selected
    }

    public class Guard : InputBehaviour
    {
        readonly float angleDelta = 0.01f;
        readonly float rayDistance = 100000f;
        readonly Color vgColor = Color.grey.OverrideAlpha(0.3f);
        readonly int layerMask = ~(1 << 8);

        public bool IsSelecting { get; private set; }
        GameObject vgMesh;
        PlayRoomModule playModule;
        SpriteRenderer guardSprite;
        bool isInitialized;

        void Awake()
        {
            Init();
        }

        void Start()
        {
            DrawArea();
        }

        // 원래 여기서 넣는게 맞는데 InputBehaviour가 FixedUpdate 이후에 돌게끔 되어있어서 지금은 드래그에 넣은 상태 @semerus
        void FixedUpdate()
        {
            //DrawArea();
        }

        void Init()
        {
            if(isInitialized) { return; }

            playModule = GiraffeSystem.FindModule<PlayRoomModule>();
            guardSprite = GetComponent<SpriteRenderer>();
            vgMesh = new GameObject("VGMesher"); // VG stands for Visibility Graph
            vgMesh.transform.SetParent(transform);
            var mRend = vgMesh.AddComponent<MeshRenderer>();
            var mat = Resources.Load<Material>("GuardMat");
            mRend.material = mat;
            mRend.material.color = mRend.material.color.OverrideAlpha(0.5f);
            //mRend.material.color = vgColor;
            //mRend.material.shader = Shader.Find("Sprites/Default");
            //mRend.material.enableInstancing = false;

            OnInputDown += () =>
            {
                playModule.InputProcessing = true;
                playModule.SelectGuard(this);
            };

            OnInputUp += () =>
            {
                playModule.InputProcessing = false;
            };

            OnDrag += () =>
            {
                var inputPosition = Input.mousePosition;
                transform.position = Camera.main.ScreenToWorldPoint(inputPosition).OverrideZ(0f) + Offset;
                DrawArea();
                playModule.CheckFill();
            };

            OnDragEnd += () =>
            {
                playModule.RecordPlayRoom();
            };

            isInitialized = true;
        }

        public void SelectStatus(GuardState state)
        {
            switch(state)
            {
                case GuardState.Default:
                    guardSprite.color = Color.blue;
                    IsSelecting = false;
                    break;
                case GuardState.Selected:
                    guardSprite.color = Color.red;
                    IsSelecting = true;
                    break;
            }
        }

        #region DrawMesh

        void DrawArea()
        {
            if (true)//GuardManager.JudgeBounds(transform.position))
            {
                vgMesh.transform.position = Vector3.zero;
                transform.GetComponentInChildren<MeshRenderer>().enabled = true;
                HashSet<Vector2> unorderedVertices = ShootRays(gameObject.transform.position, layerMask, playModule.StageData);
                var toArray = unorderedVertices.ToArray();
                Array.Sort(toArray, new ClockwiseVector2Comparer(gameObject.transform.position));
                RenderVG(toArray);
            }
            else
            {
                transform.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }

        HashSet<Vector2> ShootRays(Vector3 targetPos, int layerMask, StageData md)
        {
            HashSet<Vector2> unorderedVertices = new HashSet<Vector2>();

            // add targetPos to unorderVertices if targetPos is outside polygon
            Vector2[] outer = md.OuterPolygon.getVertices();
            var innerGroups = md.InnerGroups;
            //SimplePolygon2D[] holes = md.getHoles();

            int totalCount = outer.Length;
            foreach (var group in innerGroups)
            {
                totalCount += group.Count;
            }
            //for (int i = 0; i < holes.Length; i++)
            //{
            //    totalCount += holes[i].getVertices().Length;
            //}

            Vector2[] mapVertices2D = new Vector2[totalCount];
            int currentIndex = 0;
            for (int i = 0; i < outer.Length; i++)
            {
                mapVertices2D[currentIndex++] = outer[i];
            }

            foreach (var group in innerGroups)
            {
                foreach (var point in group)
                {
                    mapVertices2D[currentIndex++] = point;
                }
            }
            //for (int i = 0; i < holes.Length; i++)
            //{
            //    for (int j = 0; j < holes[i].getVertices().Length; j++)
            //    {
            //        mapVertices2D[currentIndex++] = holes[i].getVertices()[j];
            //    }
            //}

            const int maxCount = 10000; // max angle turn

            for (int i = 0; i < mapVertices2D.Length; i++)
            {

                /* 1. Calculate Angles */

                // Calculate standardAngle
                Vector2 standardAngle = (mapVertices2D[i] - new Vector2(targetPos.x, targetPos.y));

                // Calculate leftAngle
                Vector2 leftAngle = standardAngle;
                int count = 0;
                do
                {
                    leftAngle = Quaternion.AngleAxis(angleDelta, Vector3.forward) * leftAngle;
                    count++;
                } while (leftAngle == standardAngle && count < maxCount);

                // Calculate rightAngle
                Vector2 rightAngle = standardAngle;
                count = 0;
                do
                {
                    rightAngle = Quaternion.AngleAxis(-angleDelta, Vector3.forward) * rightAngle;
                    count++;
                } while (rightAngle == standardAngle && count < maxCount);

                /* 2. Shoot rays with the calculated angles */
                // Color : Left Blue, Right White
                //			RaycastHit2D rightHit = raycastWithDebug (targetPos, rightAngle, Color.white, layerMask);
                //			RaycastHit2D leftHit = raycastWithDebug (targetPos, leftAngle, Color.blue, layerMask);
                //			RaycastHit2D hit = raycastWithDebug (targetPos, standardAngle, Color.red, layerMask);

                RaycastHit2D rightHit = RaycastBorders(targetPos, rightAngle, layerMask, true);
                RaycastHit2D leftHit = RaycastBorders(targetPos, leftAngle, layerMask, true);
                RaycastHit2D hit = RaycastBorders(targetPos, standardAngle, layerMask, true);

                /* 3. Add hit point to unorderedVertices if the ray hits collider */
                // check if the ray is hit
                // add to the unorderedVertices only if the ray is hit
                if (leftHit.collider != null) { unorderedVertices.Add(leftHit.point); }
                if (hit.collider != null) { unorderedVertices.Add(hit.point); }
                if (rightHit.collider != null) { unorderedVertices.Add(rightHit.point); }
            }

            // Note.
            // Debug.Assert (addCounted == unorderVertices.ToArray().Length);
            // By inspecting the above Assert, which happens when the guard in on the line of Collider,
            // we should decide wheter to allow guard on edge or not
            return unorderedVertices;
        }

        RaycastHit2D RaycastBorders(Vector3 rayFrom, Vector3 shootAtAngle, int layerMask, bool debug = false)
        {
            RaycastHit2D hitPosition = Physics2D.Raycast(rayFrom, shootAtAngle, rayDistance, layerMask);

            if(debug)
            {
                if (hitPosition.collider != null)
                {
                    Debug.DrawLine(rayFrom, hitPosition.point, Color.red);
                }
                else
                {
                    Debug.DrawRay(rayFrom, shootAtAngle, Color.black);
                }
            }

            return hitPosition;
        }

        /**
	 * vertices2D : Vector2[]
	 * 		Vertices to draw a polygon
	 * 
	 * This function sets the "mesh" of vgMesh : GameObject.
	 */
        void RenderVG(Vector2[] vertices2D)
        {
            // Create the Vector3 vertices
            Vector3[] vertices3D = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices2D.Length; i++)
            {
                vertices3D[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // create polygons
            Mesh msh = vgMesh.GetOrAddComponent<MeshFilter>().mesh;
            msh.Clear();
            msh.vertices = vertices3D;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();
        }

        /**
         * This class is a IComparer<Vector2>.
         * When initializing the Comparer, send rayFrom : Vector2.	
         */
        private class ClockwiseVector2Comparer : IComparer<Vector2>
        {
            private Vector2 rayFrom;

            public ClockwiseVector2Comparer(Vector2 respectTo)
            {
                this.rayFrom = respectTo;
            }

            public int Compare(Vector2 v1, Vector2 v2)
            {
                // use cross product to calculate area
                float area2 = area(rayFrom, v1, v2);

                if (area2 > 0)
                {
                    return 1;
                }
                else if (area2 == 0)
                {
                    // distance used for tie breaker
                    float distComp = Vector2.Distance(rayFrom, v1) - Vector2.Distance(rayFrom, v2);
                    if (distComp > 0)
                    {
                        return 1;
                    }
                    else if (distComp < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
            }

            // cross product function which calculates the area bounded by 3 vertices
            private float area(Vector2 a, Vector2 b, Vector2 c)
            {
                // i.e. draw ray from a to b
                // if c is left of the extended line from a to b, then it returns a positive value
                return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
            }
        }

        #endregion
    }
}

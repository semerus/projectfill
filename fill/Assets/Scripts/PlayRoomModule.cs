using FillClient.UI;
using GiraffeStar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FillClient
{
    public class PlayRoomHistory
    {
        public List<Vector2> GuardPosition = new List<Vector2>();
    }

    public class PlayRoomModule : Module {

        // 제너릭한 풀링은 나중에 제대로 만들자 지금은 임시조치
        GameObject linePrefab;
        GameObject guardPrefab;

        GameObject root;
        GameObject drawBoard;
        GameObject colliderBoard;
        GameObject guardBoard;
        ClickableSpace clickableSpace;

        public StageData StageData { get; private set; }
        public bool InputProcessing { get; set; }
        GameObject guardPool;
        List<Guard> guardList = new List<Guard>();
        PlayRoomScene sceneModule;
        DecisionAlgorithm algorithm;
        Guard selectedGuard;
        Stack<PlayRoomHistory> records = new Stack<PlayRoomHistory>();
        PlayRoomHistory bufferRecord;

        bool isInitialized;

        public override void OnRegister()
        {
            base.OnRegister();
            Init();
        }

        void Init()
        {
            if (isInitialized) { return; }

            linePrefab = Resources.Load<GameObject>("Prefab/Line");
            guardPrefab = Resources.Load<GameObject>("Prefab/Vertex");

            root = GameObject.Find("Root");
            drawBoard = root.FindChildByName("LineContainer");
            colliderBoard = root.FindChildByName("ColliderContainer");
            guardBoard = root.FindChildByName("Guards");
            clickableSpace = root.FindChildByName("ClickableSpace").GetComponent<ClickableSpace>();

            clickableSpace.onPointerClick += OnClick;

            // prepare pooling
            guardPool = new GameObject("GuardPool");
            guardPool.transform.position = Vector3.zero;

            isInitialized = true;
        }        

        public void SetStage(StageData stageData)
        {
            sceneModule = GiraffeSystem.FindModule<PlayRoomScene>();
            var points = StageData.GetCompleteVertices(stageData.OuterVertices);
            if (points == null) { return; }

            var converted = new List<Vector3>();
            foreach (var point in points)
            {
                converted.Add(point);
            }
            CreateLine(converted, "OuterLine", stageData.LineColor);
            CreateCollider(points, "OuterCollider");

            var index = 0;
            var innerGroups = stageData.InnerGroups;
            foreach (var inner in innerGroups)
            {
                var completed = StageData.GetCompleteVertices(inner);
                if(completed.Count < 1) { continue; }
                var convert = new List<Vector3>();
                foreach (var point in completed)
                {
                    convert.Add(point);
                }
                CreateLine(convert, "InnerLine" + (++index).ToString(), stageData.LineColor);
                CreateCollider(completed, "InnerCollider" + index);
            }
            
            StageData = stageData;
            algorithm = new DecisionAlgorithm(StageData);            
        }

        LineRenderer CreateLine(List<Vector3> points, string name, Color color)
        {
            var lineObj = Object.Instantiate(linePrefab, drawBoard.transform);
            lineObj.transform.name = name;
            var line = lineObj.GetComponent<LineRenderer>();
            line.widthMultiplier = 0.15f;
            line.SetFullWidth(0.1f);
            line.SetFullColor(Color.black);
            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
            line.SetFullColor(color);

            return line;
        }

        EdgeCollider2D CreateCollider(List<Vector2> points, string name)
        {
            var colObj = new GameObject(name);
            colObj.transform.SetParent(colliderBoard.transform);
            colObj.transform.localPosition = Vector3.zero;
            var col = colObj.AddComponent<EdgeCollider2D>();   
            col.points = points.ToArray();

            return col;
        }

        void OnClick(PointerEventData e)
        {
            if(InputProcessing) { return; }
            // check bounds
            var worldPos = Camera.main.ScreenToWorldPoint(e.pressPosition);
            if (!Calculator.JudgeValidPositions(worldPos, StageData)) { return; }

            PlaceGuard(worldPos.OverrideZ(0f));
            RecordPlayRoom();
        }

        void PlaceGuard(Vector3 worldPos)
        {
            var guardObj = SpawnGuard();
            guardObj.transform.position = worldPos;
            var guard = guardObj.GetComponent<Guard>();
            guardList.Add(guard);

            SelectGuard(guard);
            sceneModule.UpdateGuardCount(guardList.Count);
            CheckFill();
        }

        public void TrashSelectedGuard()
        {
            TrashGuard(selectedGuard);
            RecordPlayRoom();
        }

        void ClearAllGuards()
        {
            foreach (var guard in guardList)
            {
                DespawnGuard(guard);
            }
            guardList.Clear();
        }

        void TrashGuard(Guard guard)
        {
            if(guard == null) { return; }

            guardList.Remove(guard);
            DespawnGuard(guard);

            sceneModule.UpdateGuardCount(guardList.Count);
            CheckFill();
        }

        public void SelectGuard(Guard guard)
        {
            if(selectedGuard != null)
            {
                if(selectedGuard.Equals(guard)) { return; }
                selectedGuard.SelectStatus(GuardState.Default);
            }
            selectedGuard = guard;
            selectedGuard.SelectStatus(GuardState.Selected);
        }

        // 스폰 부분 나중에 깔끔히 다듬기(제너릭하게)
        //=======================================================================================
        GameObject SpawnGuard()
        {
            var guards = guardPool.GetComponentsInChildren<Guard>();
            var select = guards.Length < 1 ? GameObject.Instantiate(guardPrefab, guardBoard.transform) : guards[0].gameObject;
            if(!select.activeSelf)
            {
                select.SetActive(true);
            }

            return select;
        }

        void DespawnGuard(Guard guard)
        {
            guard.transform.SetParent(guardPool.transform);
            if(guard.gameObject.activeSelf)
            {
                guard.gameObject.SetActive(false);
            }
        }
        // 히스토리 기능
        //=======================================================================================
        public void RecordPlayRoom()
        {
            var history = new PlayRoomHistory();
            foreach (var guard in guardList)
            {
                history.GuardPosition.Add(guard.transform.position);
            }
            if(bufferRecord != null)
            {
                records.Push(bufferRecord);
            }
            bufferRecord = history;
            //Debug.Log("history saved" + history.GuardPosition.Count);
        }

        public void LoadPlayRoomHistory()
        {
            ClearAllGuards();
            if(records.Count > 0)
            {
                var latest = records.Pop();
                foreach (var position in latest.GuardPosition)
                {
                    PlaceGuard(position);
                }
            }
            sceneModule.UpdateGuardCount(guardList.Count);
            bufferRecord = null;
        }
        //=======================================================================================
        public void CheckFill()
        {
            var guardPositions = new List<Vector3>();
            foreach (var guard in guardList)
            {
                guardPositions.Add(guard.transform.position);
            }

            FillSuccess(algorithm.IsFilled(guardPositions.ToArray()));            
        }

        void FillSuccess(bool isSuccess)
        {
            sceneModule.UpdateCheckDisplay(isSuccess);
        }
	}
}
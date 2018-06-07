using FillClient.UI;
using GiraffeStar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FillClient
{
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
            var points = stageData.GetCompleteVertices();
            if (points == null) { return; }

            var lineObj = Object.Instantiate(linePrefab, drawBoard.transform);
            lineObj.transform.name = "Line";
            var line = lineObj.GetComponent<LineRenderer>();
            line.widthMultiplier = 0.15f;
            line.SetFullWidth(0.1f);
            line.SetFullColor(Color.black);            
            line.positionCount = points.Count;

            var colObj = new GameObject("Edges");
            colObj.transform.SetParent(colliderBoard.transform);
            colObj.transform.localPosition = Vector3.zero;
            var col = colObj.AddComponent<EdgeCollider2D>();

            var converted = new List<Vector3>();
            foreach (var point in points) {
                converted.Add(point);
            }

            col.points = points.ToArray();
            line.SetPositions(converted.ToArray());
            StageData = stageData;
            algorithm = new DecisionAlgorithm(StageData);
        }

        void OnClick(PointerEventData e)
        {
            if(InputProcessing) { return; }
            // check bounds

            PlaceGuard(e.pressPosition);
        }

        void PlaceGuard(Vector2 screenPos)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            var guardObj = SpawnGuard();
            guardObj.transform.position = worldPos.OverrideZ(0f);
            var guard = guardObj.GetComponent<Guard>();
            guardList.Add(guard);

            SelectGuard(guard);
            sceneModule.UpdateGuardCount(guardList.Count);
            CheckFill();
        }

        public void TrashSelectedGuard()
        {
            TrashGuard(selectedGuard);
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
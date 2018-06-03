using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;

namespace FillClient.UI
{
    public class CustomStageSelectScene : Module
    {

        bool isInitialized;

        GameObject stageItemPrefab;
        GameObject root;
        GameObject stageList;
        Button backButton;


        public override void OnRegister()
        {
            base.OnRegister();
            Init();
        }

        void Init()
        {
            if (isInitialized) { return; }

            stageItemPrefab = Resources.Load<GameObject>("UI/StageItem");

            root = GameObject.Find("Root");
            stageList = root.FindChildByName("StageList");
            backButton = root.FindChildByName("BackButton").GetComponent<Button>();

            backButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
                    NextState = FillState.MainMenu,
                }.Dispatch();
            });

            LoadStages();

            isInitialized = false;
        }

        void LoadStages()
        {
            var stages = JsonIO.Load<List<StageData>>(Application.dataPath + "/test.json");
            if(stages == null)
            {
                Debug.LogError("File load failed.");
                return;
            }

            // make item script later
            foreach (var stage in stages)
            {
                var item = Object.Instantiate(stageItemPrefab, stageList.transform);
                var button = item.GetComponentInChildren<Button>();
                var title = item.GetComponentInChildren<Text>();

                title.text = stage.Name;
            }
        }
    }
}



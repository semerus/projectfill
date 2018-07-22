using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;
using FillClient;

namespace FillClient.UI
{
    public class CustomStageSelectScene : Module
    {

        bool isInitialized;

        GameObject stageItemPrefab;
        GameObject root;
        GameObject stageList;
        Button backButton;
        Button editButton;

        Button selectedButton;
        StageData selectedStage;

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
            editButton = root.FindChildByName("EditButton").GetComponent<Button>();

            backButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
                    NextState = FillState.MainMenu,
                }.Dispatch();
            });

            editButton.onClick.AddListener(() =>
            {
                if(selectedStage != null)
                {
                    new EditStageMessage()
                    {
                        StageData = selectedStage,
                    }.Dispatch();
                }
            });

            LoadStages();

            isInitialized = false;
        }

        void LoadStages()
        {
			var stages = JsonIO.Load<List<StageData>>(Application.persistentDataPath + "/test.json");
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
                var stageItem = item.AddComponent<StageItem>();

				button.onClick.AddListener (() => {

                    if(Time.realtimeSinceStartup > stageItem.PointerDownTime + stageItem.holdThreshold)
                    {
                        if(selectedButton != null)
                        {
                            // change to normal
                            var bg = selectedButton.transform.GetComponent<Image>();
                            bg.color = Color.white.OverrideAlpha(bg.color.a);
                        }
                        selectedButton = button;
                        selectedStage = stage;
                        var currentBg = button.transform.GetComponent<Image>();
                        currentBg.color = Color.black.OverrideAlpha(currentBg.color.a);
                        return;
                    }

					new StartPlayRoomMessage () {
						StageData = stage,
						NextState = FillState.PlayRoom,
					}.Dispatch ();
				});

                title.text = stage.Name;
            }
        }
    }
}



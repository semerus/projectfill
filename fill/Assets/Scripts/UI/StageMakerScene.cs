using GiraffeStar;
using UnityEngine;
using UnityEngine.UI;

namespace FillClient.UI
{
    public class StageMakerScene : Module
    {
        bool initialized;

        GameObject root;
        Text countDisplay;
        Button saveButton;
        Button clearButton;
        Button maxButton;
        Button minButton;
        Button backButton;
        Button historyButton;
        Toggle snapToggle;

        StageMakerModule stageMakerModule;

        public override void OnRegister()
        {
            base.OnRegister();
            Init();
        }

        void Init()
        {
            if (initialized) { return; }

            root = GameObject.Find("Root");
            countDisplay = root.FindChildByName("Count").GetComponent<Text>();
            saveButton = root.FindChildByName("SaveButton").GetComponent<Button>();
            maxButton = root.FindChildByName("MaxButton").GetComponent<Button>();
            minButton = root.FindChildByName("MinButton").GetComponent<Button>();
            historyButton = root.FindChildByName("HistoryButton").GetComponent<Button>();
            snapToggle = root.FindChildByName("SnapToggle").GetComponent<Toggle>();
            clearButton = root.FindChildByName("ClearButton").GetComponent<Button>();
            backButton = root.FindChildByName("BackButton").GetComponent<Button>();

            stageMakerModule = GiraffeSystem.FindModule<StageMakerModule>();

            saveButton.onClick.AddListener(OnSave);
            maxButton.onClick.AddListener(() => ChangeView(true));
            minButton.onClick.AddListener(() => ChangeView(false));
            historyButton.onClick.AddListener(OnReturnHistory);
            clearButton.onClick.AddListener(OnClear);
            backButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
                    NextState = FillState.MainMenu,
                }.Dispatch();
            });
            snapToggle.onValueChanged.AddListener((isOn) =>
            {
                stageMakerModule.IsSnapping = isOn;
            });

            UpdateCount(0);
            snapToggle.isOn = false;

            initialized = true;
        }

        void OnClear()
        {
            stageMakerModule.Clear();
        }

        void OnSave()
        {
            stageMakerModule.Save();
        }

        void OnReturnHistory()
        {

        }

        void ChangeView(bool magnify)
        {
            stageMakerModule.ChangeView(magnify);
        }

        [Subscriber]
        void UpdateCount(VertexCountMessage msg)
        {
            UpdateCount(msg.VertexCount);
        }

        void UpdateCount(int count)
        {
            countDisplay.text = "Count :" + count;
        }
    }
}



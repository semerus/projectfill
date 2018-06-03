using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;

namespace FillClient.UI
{
    public class MainMenuScene : Module
    {
        GameObject root;
        Button playButton;
        Button editorButton;
        Button customButton;

        bool isInitialized;

        public override void OnRegister()
        {
            Init();
        }

        void Init()
        {
            if (isInitialized) { return; }
            root = GameObject.Find("Root");
            playButton = root.FindChildByName("PlayButton").GetComponent<Button>();
            editorButton = root.FindChildByName("EditorButton").GetComponent<Button>();
            customButton = root.FindChildByName("CustomButton").GetComponent<Button>();

            playButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
					NextState = FillState.StageSelect,
                }.Dispatch();
            });

            editorButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
                    NextState = FillState.StageMaker,
                }.Dispatch();
            });

            customButton.onClick.AddListener(() =>
            {
                new SwitchStateMessage()
                {
                    NextState = FillState.CustomStageSelect,
                }.Dispatch();
            });

            isInitialized = true;
        }
    }
}



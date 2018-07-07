using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;
using DG.Tweening;

namespace FillClient.UI
{
    public class MainMenuScene : Module
    {
        GameObject root;
		Button startButton;
        Button playButton;
        Button editorButton;
        Button customButton;
		Text touchText;

		static bool isFirstInitial = true;
        bool isInitialized;

        public override void OnRegister()
        {
			if (isFirstInitial) 
			{
				FirstInit ();
				isFirstInitial = false;
			} else 
			{
				Init ();
			}
        }

        void FirstInit()
        {
            if (isInitialized) { return; }
            root = GameObject.Find("Root");
			startButton = root.FindChildByName ("StartButton").GetComponent<Button>();
			touchText = startButton.GetComponentInChildren<Text> ();
			playButton = root.FindChildByName("PlayButton").GetComponent<Button>();
            editorButton = root.FindChildByName("EditorButton").GetComponent<Button>();
            customButton = root.FindChildByName("CustomButton").GetComponent<Button>();

			startButton.onClick.AddListener (() => 
			{
				ShiftButtons();	
			});

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

			var touchAnim = touchText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);

            isInitialized = true;
        }

		void Init()
		{
			if (isInitialized) { return; }

			root = GameObject.Find("Root");
			startButton = root.FindChildByName ("StartButton").GetComponent<Button>();
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

			startButton.gameObject.SetActive (false);

			var logo = root.FindChildByName ("Logo").GetComponent<RectTransform>();
			var startPos = logo.anchoredPosition;
			logo.anchoredPosition = new Vector2 (350f, startPos.y);
			//logo.DOAnchorPos(new Vector2(350f, startPos.y), 1.6f);

			var menuAlpha = root.FindChildByName ("LeftMenuBar").GetComponent<AlphaGroup> ();
			menuAlpha.GroupAlpha = 1f;

			isInitialized = true;
		}

		void ShiftButtons()
		{
			var logo = root.FindChildByName ("Logo").GetComponent<RectTransform>();
			var startPos = logo.anchoredPosition;
			logo.DOAnchorPos(new Vector2(350f, startPos.y), 1.6f);

			var menuAlpha = root.FindChildByName ("LeftMenuBar").GetComponent<AlphaGroup> ();
			DOTween.To (x => menuAlpha.GroupAlpha = x, menuAlpha.GroupAlpha, 1f, 1f);

			startButton.gameObject.SetActive (false);
		}
    }
}
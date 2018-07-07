using UnityEngine;
using GiraffeStar;
using UnityEngine.UI;
using DG.Tweening;

namespace FillClient.UI
{
	public class SplashScene : Module {

		GameObject root;
		Button playButton;
        Text touchText;

		bool isInitialized;

		public override void OnRegister ()
		{
			base.OnRegister ();
			Init ();
		}

		void Init()
		{
			if (isInitialized) { return; }

			root = GameObject.Find ("Root");

            touchText = playButton.GetComponentInChildren<Text>();

            playButton.onClick.AddListener (() => 
			{
					new SwitchStateMessage()
					{
						NextState = FillState.MainMenu,
					}.Dispatch();
			});

            var touchAnim = touchText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);

			isInitialized = true;
		}
	}
}

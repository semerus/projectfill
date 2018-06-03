using UnityEngine;
using GiraffeStar;
using UnityEngine.UI;

namespace FillClient.UI
{
	public class SplashScene : Module {

		GameObject root;
		Button playButton;

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
			playButton = root.FindChildByName ("PlayButton").GetComponent<Button>();

			playButton.onClick.AddListener (() => 
			{
					new SwitchStateMessage()
					{
						NextState = FillState.MainMenu,
					}.Dispatch();
			});

			isInitialized = true;
		}
	}
}

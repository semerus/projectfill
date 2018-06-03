using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;

namespace FillClient.UI
{
	public class PlayRoomScene : Module {

		GameObject root;
		Button backButton;

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
			backButton = root.FindChildByName("BackButton").GetComponent<Button>();

			backButton.onClick.AddListener (() => {
				new SwitchStateMessage()
				{
					NextState = FillState.CustomStageSelect,
				}.Dispatch();
			});

			isInitialized = true;
		}
	}
}
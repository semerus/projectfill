using UnityEngine;
using UnityEngine.UI;
using GiraffeStar;

namespace FillClient.UI
{
	public class PlayRoomScene : Module {

		GameObject root;
		Button backButton;
        Button menuButton;
        Button trashButton;
        Button historyButton;
        Text guardCount;
        Image checkDisplay;

        PlayRoomModule playModule;
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
            menuButton = root.FindChildByName("MenuButton").GetComponent<Button>();
            trashButton = root.FindChildByName("TrashButton").GetComponent<Button>();
            historyButton = root.FindChildByName("HistoryButton").GetComponent<Button>();
            guardCount = root.FindChildByName("GuardCount").GetComponent<Text>();
            checkDisplay = root.FindChildByName("CheckDisplay").GetComponent<Image>();
            playModule = GiraffeSystem.FindModule<PlayRoomModule>();

			backButton.onClick.AddListener (() => 
            {
				new SwitchStateMessage()
				{
					NextState = FillState.CustomStageSelect,
				}.Dispatch();
			});

            menuButton.onClick.AddListener(() =>
            {

            });

            historyButton.onClick.AddListener(() =>
            {
                playModule.LoadPlayRoomHistory();
            });

            trashButton.onClick.AddListener(() =>
            {
                playModule.TrashSelectedGuard();
            });

            UpdateGuardCount(0);
            UpdateCheckDisplay(false);

			isInitialized = true;
		}

        public void UpdateGuardCount(int count)
        {
            guardCount.text = "Guards : " + count;
        }

        public void UpdateCheckDisplay(bool isComplete)
        {
            var color = checkDisplay.color;
            checkDisplay.color = isComplete ?
                color.OverrideAlpha(1f) :
                color.OverrideAlpha(0.4f);
        }
	}
}
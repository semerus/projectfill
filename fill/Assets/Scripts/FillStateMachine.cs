using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GiraffeStar;
using FillClient.UI;

namespace FillClient
{
    public enum FillState
    {
        MainMenu,
        StageMaker,
        StageSelect,
        StageRoom,
        CustomStageSelect,
		Splash,
		PlayRoom,
    }

    public class SwitchStateMessage : MessageCore
    {
        public FillState NextState;
        public bool Reoccur;
    }

	public class StartPlayRoomMessage : SwitchStateMessage
	{
		public StageData StageData;
	}

    public class FillStateMachine : StateMachine
    {
        public override void OnRegister()
        {
            base.OnRegister();
            var sceneName = SceneManager.GetActiveScene().name;
            base.SwitchTo(ConvertEnumToType(FillStateMachine.ConvertSceneToEnum(sceneName)), true);
        }

        [Subscriber]
        void SwitchTo(SwitchStateMessage msg)
        {
            base.SwitchTo(ConvertEnumToType(msg.NextState), msg.Reoccur, msg);
            Debug.Log(string.Format("Changed to {0}", msg.NextState.ToString()));
        }

        public Type ConvertEnumToType(FillState state)
        {
            switch(state)
            {
                case FillState.MainMenu:
                    return typeof(MainMenuState);
                case FillState.StageMaker:
                    return typeof(StageMakerState);
                case FillState.StageRoom:
                    return typeof(StageRoomState);
                case FillState.StageSelect:
                    return typeof(StageSelectState);
                case FillState.CustomStageSelect:
                    return typeof(CustomStageSelectState);
				case FillState.Splash:
					return typeof(SplashState);
				case FillState.PlayRoom:
					return typeof(PlayRoomState);
            }

            return null;
        }

        public static FillState ConvertSceneToEnum(string name)
        {
            switch(name)
            {
                case "MainMenu":
                    return FillState.MainMenu;
                case "room":
                    return FillState.StageRoom;
                case "StageEditor":
                    return FillState.StageMaker;
                case "StageSelect":
                    return FillState.StageSelect;
                case "CustomStageSelect":
                    return FillState.CustomStageSelect;
				case "Splash":
					return FillState.Splash;
				case "PlayRoom":
					return FillState.PlayRoom;
            }

            return FillState.MainMenu;
        }

        //public string ConvertEnumToScene(FillState state)
        //{
        //    return null;
        //}

        // add transitions and async load later
        // TODO: improve sceneloading style
        class MainMenuState : State
        {
			public override void OnEnter(object msg)
            {
                if(!SceneManager.GetActiveScene().name.Equals("MainMenu"))
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    RegisterAndHold(new MainMenuScene());
                }
            }

			public override void OnExit(object msg)
            {
                UnRegisterAll();
            }

            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                RegisterAndHold(new MainMenuScene());
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        class StageMakerState : State
        {
			public override void OnEnter(object msg)
            {
                if (!SceneManager.GetActiveScene().name.Equals("StageEditor"))
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene("StageEditor");
                }
                else
                {
                    RegisterAndHold(new StageMakerModule());
                    RegisterAndHold(new StageMakerScene());
                }
            }

			public override void OnExit(object msg)
            {
                UnRegisterAll();
            }

            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                RegisterAndHold(new StageMakerModule());
                RegisterAndHold(new StageMakerScene());
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        class StageSelectState : State
        {
			public override void OnEnter(object msg)
            {
                SceneManager.LoadScene("StageSelect");
            }

			public override void OnExit(object msg)
            {
                UnRegisterAll();
            }
        }

        class StageRoomState : State
        {

        }

        class CustomStageSelectState : State
        {
			public override void OnEnter(object msg)
            {
                if (!SceneManager.GetActiveScene().name.Equals("CustomStageSelect"))
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene("CustomStageSelect");
                }
                else
                {
                    RegisterAndHold(new CustomStageSelectScene());
                }
            }

			public override void OnExit(object msg)
            {
                UnRegisterAll();
            }

            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                RegisterAndHold(new CustomStageSelectScene());
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

		class SplashState : State
		{
			public override void OnEnter(object msg)
			{
				if (!SceneManager.GetActiveScene().name.Equals("Splash"))
				{
					SceneManager.sceneLoaded += OnSceneLoaded;
					SceneManager.LoadScene("Splash");
				}
				else
				{
					RegisterAndHold(new SplashScene());
				}
			}

			public override void OnExit(object msg)
			{
				UnRegisterAll();
			}

			void OnSceneLoaded(Scene scene, LoadSceneMode mode)
			{
				RegisterAndHold(new SplashScene());
				SceneManager.sceneLoaded -= OnSceneLoaded;
			}
		}

		class PlayRoomState : State
		{
			StartPlayRoomMessage cachedMessage;

			public override void OnEnter(object msg)
			{
				var convertedMessage = msg as StartPlayRoomMessage;
				if (convertedMessage != null) 
				{
					cachedMessage = convertedMessage;
				}

				if (!SceneManager.GetActiveScene().name.Equals("PlayRoom"))
				{
					SceneManager.sceneLoaded += OnSceneLoaded;
					SceneManager.LoadScene("PlayRoom");
				}
				else
				{
					RegisterAndHold (new PlayRoomModule ());
					RegisterAndHold(new PlayRoomScene());

					var playRoomModule = GiraffeSystem.FindModule<PlayRoomModule> ();
					playRoomModule.SetStage (cachedMessage.StageData);
				}

				Debug.Log ("Entered PlayRoom");
			}

			public override void OnExit(object msg)
			{
				UnRegisterAll();
			}

			void OnSceneLoaded(Scene scene, LoadSceneMode mode)
			{
				RegisterAndHold (new PlayRoomModule ());
				RegisterAndHold(new PlayRoomScene());

				var playRoomModule = GiraffeSystem.FindModule<PlayRoomModule> ();
				playRoomModule.SetStage (cachedMessage.StageData);

				SceneManager.sceneLoaded -= OnSceneLoaded;
			}
		}
    }
}



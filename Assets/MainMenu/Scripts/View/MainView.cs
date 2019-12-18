using UniKid.Core;
using UniKid.MainMenu.Controller;
using UniKid.SubGame.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace UniKid.MainMenu.View
{
    public sealed class MainView : EventView
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher contextDispatcher { get; set; }

        protected override void Start()
        {
            base.Start();

            ValionLog.Info("test log");
        }

        private void LoadSeaGuessingScene()
        {
            LoadParticularSubGame("SeaGuessing");
        }
        
        private void LoadTexturePaintScene()
        {
            LoadParticularSubGame("TexturePaint");
        }

        private void LoadBubbleGuessingScene()
        {
            LoadParticularSubGame("BubbleGuessing");
        }

        private void LoadHideSeekCardsScene()
        {
            LoadParticularSubGame("HideSeekCards");
        }

        private void LoadSandbox01Scene()
        {
            LoadParticularSubGame("Sandbox01");            
        }

        private void LoadLibrarySliderScene()
        {
            LoadParticularSubGame("LibrarySlider");
        }

        private void LoadWordConstructor01Scene()
        {
            LoadParticularSubGame("WordConstructor01");            
        }

        private void LoadWordConstructor02Scene()
        {
            LoadParticularSubGame("WordConstructor02");                        
        }

        private void StudyPlayWOPriority()
        {
            CoreContext.UserData.CurrentProfile.IsSubGamePriorityEnabled = false;
            StartStudyPlan();
        }

        private void StudyPlayWPriority()
        {
            CoreContext.UserData.CurrentProfile.IsSubGamePriorityEnabled = true;

            StartStudyPlan();
        }

        private void LoadParticularSubGame(string gameName)
        {
            CoreContext.PlayParticularGame = true;

            contextDispatcher.Dispatch(MainMenuEventType.LoadSubGame, gameName);   
        }


        private void StartStudyPlan()
        {
            if (CoreContext.UserData.CurrentProfile.IsGameFinished)
            {
                Debug.LogWarning("Game Is Finished!");
                return;
            }

            if (!string.IsNullOrEmpty(CoreContext.UserData.CurrentProfile.CurrentSubGameName))
            {
                contextDispatcher.Dispatch(MainMenuEventType.LoadSubGame, CoreContext.UserData.CurrentProfile.CurrentSubGameName);
            }
            else
            {
                SubGameCoreBase.StartLevelAction = (level, isLastLevelPassed) =>
                                                          {
                                                              Debug.Log(level.SubGameName); 
                                                              contextDispatcher.Dispatch(MainMenuEventType.LoadSubGame,
                                                                                  level.SubGameName);
                                                          };

                SubGameCoreBase.StartLevel();
            }
        }
		
		private void LoadBubblesScene()
        {
            contextDispatcher.Dispatch(MainMenuEventType.LoadSubGame, "BubbleGuessingGame");
			
        }
		
		
    }
}


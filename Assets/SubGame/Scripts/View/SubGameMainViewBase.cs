using UniKid.SubGame.Model;
using UniKid.SubGame.View.Character;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.View
{
    public abstract class SubGameMainViewBase : EventView
    {
        public static readonly float DEFAULT_SECONDS_BEFORE_START_NEW_LEVEL = 3.0f;

        public abstract SubGameCoreBase SubGameCoreBase { get; }

        public virtual float SecondsBeforeLoadNewLevel { get { return DEFAULT_SECONDS_BEFORE_START_NEW_LEVEL; } }
        

        public enum SubGameViewEvent
        {
            ExitToMainMenu,
            GameIsPassed
        }
        
        public abstract void LoadStage();

        protected override void Start()
        {
            base.Start();

        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
            }
        }

        private void OnApplicationQuit()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }
    }
}
using System;
using UniKid.Core;
using UniKid.Core.Controller;
using UniKid.Core.Service;
using UniKid.SubGame.Model;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.SubGame.Controller
{
    public abstract class SubGameExitCommandBase<TSettings, TUserData> : SubGameExitCommandBase<TSettings, TUserData, Level<Stage>, Stage>
        where TSettings : SubGameSettingsBase<Level<Stage>, Stage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
    {
        protected abstract SubGameCoreBase<TSettings, TUserData> SubGameCoreSimple { get; }

        protected override SubGameCoreBase<TSettings, TUserData, Level<Stage>, Stage> SubGameCore { get { return SubGameCoreSimple; } }
    }

    public abstract class SubGameExitCommandBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> : EventCommand
        where TSettings : SubGameSettingsBase<TSettingsLevel, TSettingsStage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
        where TSettingsLevel : Level<TSettingsStage>, new()
        where TSettingsStage : Stage, new()
    {
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        protected abstract SubGameCoreBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> SubGameCore { get; }

        public override void Execute()
        {
            CoreContext.SetDebugInfoActive(false);
            
            SubGameCore.LevelQueue.Save();

            CoreContext.PlayParticularGame = false;

            foreach (var tag in SubGameCore.CurrentLevelTagList)
            {
                if (tag.SpentTimeArray[tag.SpentTimeArray.Length - 1].To.ToOADate() - 1 < 0)
                    tag.SpentTimeArray[tag.SpentTimeArray.Length - 1].To = DateTime.UtcNow;
            }

            CoreContext.Save();

            crossContextDispatcher.Dispatch(CoreEventType.DbUpdateUserData);

            //GameObject.Destroy(contextView);

            if (evt.data == null)
            {
                crossContextDispatcher.Dispatch(CoreEventType.LoadMainMenu);
            }
            else
            {
                crossContextDispatcher.Dispatch(CoreEventType.LoadScene, evt.data);
            }
        }
    }
}
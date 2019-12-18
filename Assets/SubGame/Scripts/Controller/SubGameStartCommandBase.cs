using System;
using UniKid.Core;
using UniKid.SubGame.Model;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace UniKid.SubGame.Controller
{
    public abstract class SubGameStartCommandBase<TSettings, TUserData> : SubGameStartCommandBase<TSettings, TUserData, Level<Stage>, Stage>
        where TSettings : SubGameSettingsBase<Level<Stage>, Stage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
    {
        protected abstract SubGameCoreBase<TSettings, TUserData> SubGameCoreSimple { get; }

        protected override SubGameCoreBase<TSettings, TUserData, Level<Stage>, Stage> SubGameCore { get { return SubGameCoreSimple; } }
    }

    public abstract class SubGameStartCommandBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> : EventCommand
        where TSettings : SubGameSettingsBase<TSettingsLevel, TSettingsStage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
        where TSettingsLevel : Level<TSettingsStage>, new()
        where TSettingsStage : Stage, new()
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }


        protected abstract SubGameCoreBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> SubGameCore { get; }

        public override void Execute()
        {
            CoreContext.SetDebugInfoActive(true);
        }
    }
}

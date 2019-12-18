using System;
using System.Collections;
using UniKid.Core;
using UniKid.Core.Controller;
using UniKid.SubGame.Controller;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;


namespace UniKid.MainMenu.Controller
{
    public sealed class LoadSubGameCommand : EventCommand
    {
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }


        public override void Execute()
        {
            CoreContext.LoadedSubGameName = (SubGameName)Enum.Parse(typeof(SubGameName), evt.data.ToString());

            //crossContextDispatcher.Dispatch(CoreEventType.LoadScene, new LoadSceneCommandArgument { SceneName = "SubGameUI", LoadAdditive = true });

            crossContextDispatcher.Dispatch(CoreEventType.LoadScene, new LoadSceneCommandArgument { SceneName = evt.data.ToString(), IsSubGameScene = true, WithFade = true, LoadAdditive = true });

        }

    }
}

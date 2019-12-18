using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.Core.Controller
{
    public class LoadMainMenuCommand : EventCommand
    {
        public static readonly string MainMenuSceneName = "MainMenu";

        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher contextDispatcher { get; set; }

        public override void Execute()
        {
            dispatcher.Dispatch(CoreEventType.LoadScene, MainMenuSceneName);
        }
    }
}
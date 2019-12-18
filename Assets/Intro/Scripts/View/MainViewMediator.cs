using System.Collections;
using UniKid.Core.Controller;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.Intro.View
{
    public sealed class MainViewMediator : EventMediatorBase
    {
        [Inject]
        public MainView MainView { get; set; }
        
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            MainView.dispatcher.UpdateListener(toAdd, MainView.ViewEventType.IntroIsFinished, () => crossContextDispatcher.Dispatch(CoreEventType.LoadMainMenu));
        }
    }
}
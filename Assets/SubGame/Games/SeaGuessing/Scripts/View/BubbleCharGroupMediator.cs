using System;
using UniKid.SubGame.Games.SeaGuessing.Controller;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class BubbleCharGroupMediator : EventMediatorBase
    {
        [Inject]
        public BubbleCharGroup View { get; set; }
    
        protected override void UpdateListeners(bool toAdd)
        {
            View.dispatcher.UpdateListener(toAdd, BubbleCharGroup.EventType.CharIsFound, OnCharIsFound);
            View.dispatcher.UpdateListener(toAdd, BubbleCharGroup.EventType.CharIsNotFound, OnCharIsNotFound);
        }

        private void OnCharIsFound()
        {
            dispatcher.Dispatch(SeaGuessingEventType.CharIsFound);
        }

        private void OnCharIsNotFound()
        {
            dispatcher.Dispatch(SeaGuessingEventType.CharIsNotFound);
        }
    }
}
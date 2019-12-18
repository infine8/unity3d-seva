using UniKid.SubGame.Games.WordConstructor02.Controller;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class ScrollMediator : EventMediatorBase
    {
        [Inject]
        public Scroll Scroll { get; set; }

        [Inject]
        public NewItemDetectedSignal NewItemDetectedSignal { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            Scroll.dispatcher.UpdateListener(toAdd, Scroll.ViewEventType.CurrentItemDetected, () => NewItemDetectedSignal.Dispatch(Scroll.Index, Scroll.CurrentItem.Syllable));
        }
    }
}
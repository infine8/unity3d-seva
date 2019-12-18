using UniKid.SubGame.Games.SeaGuessing.Controller;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class WaterTopBorderMediator : EventMediatorBase
    {
        [Inject]
        public WaterTopBorder WaterTopBorder { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            WaterTopBorder.dispatcher.UpdateListener(toAdd, WaterTopBorder.EventType.BubbleGroupReachedTopBorder, BubbleGroupReachedTopBorder);
        }

        private void BubbleGroupReachedTopBorder()
        {
            dispatcher.Dispatch(SeaGuessingEventType.CharIsNotFound);
        }
    }
}
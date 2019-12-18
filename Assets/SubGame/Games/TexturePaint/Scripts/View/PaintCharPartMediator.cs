using UniKid.SubGame.Games.TexturePaintGame.Controller;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class PaintCharPartMediator : EventMediatorBase
    {
        [Inject]
        public PaintCharPart PaintCharPart { get; set; }

        [Inject]
        public BrushPaintCharColliderTriggerSignal BrushPaintCharColliderTriggerSignal { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            PaintCharPart.dispatcher.UpdateListener(toAdd, PaintCharPart.PaintCharPartEventType.OnBrushTriggerEnter, OnBrushTriggerEnter);
            PaintCharPart.dispatcher.UpdateListener(toAdd, PaintCharPart.PaintCharPartEventType.OnBrushTriggerExit, OnBrushTriggerExit);
        }


        private void OnBrushTriggerEnter()
        {
            BrushPaintCharColliderTriggerSignal.Dispatch(true);
        }

        private void OnBrushTriggerExit()
        {
            BrushPaintCharColliderTriggerSignal.Dispatch(false);

        }
        
    }
}
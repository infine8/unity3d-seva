using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.TexturePaintGame.Controller;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class BrushMediator : EventMediatorBase
    {
        [Inject]
        public Brush Brush { get; set; }

        [Inject]
        public BrushPaintCharColliderTriggerSignal BrushPaintCharColliderTriggerSignal { get; set; }

        [Inject]
        public PaintCharPartIsCompletedSignal PaintCharPartIsCompletedSignal { get; set; }

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            Brush.dispatcher.UpdateListener(toAdd, Brush.BrushEventType.PaintCharIsCompleted, PaintCharIsCompleted);
            Brush.dispatcher.UpdateListener(toAdd, Brush.BrushEventType.PaintCharPartIsCompleted, PaintCharPartIsCompleted);

            if (toAdd)
            {
                BrushPaintCharColliderTriggerSignal.AddListener(BrushPaintCharColliderTrigger);
            }
            else
            {
                BrushPaintCharColliderTriggerSignal.RemoveListener(BrushPaintCharColliderTrigger);
            }
        }

        private void BrushPaintCharColliderTrigger(bool isEnter)
        {
            Brush.BrushPaintCharColliderTrigger(isEnter);
        }

        private void PaintCharIsCompleted()
        {
            StagePassedSignal.Dispatch();
        }

        private void PaintCharPartIsCompleted()
        {
            PaintCharPartIsCompletedSignal.Dispatch();
        }
    }
}
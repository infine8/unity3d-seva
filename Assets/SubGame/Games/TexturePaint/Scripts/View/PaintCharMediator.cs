using UniKid.SubGame.Games.TexturePaintGame.Controller;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public class PaintCharMediator : EventMediator
    {
        [Inject]
        public PaintChar PaintChar { get; set; }

        [Inject]
        public PaintCharPartIsCompletedSignal PaintCharPartIsCompletedSignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            PaintCharPartIsCompletedSignal.AddListener(OnPaintCharPartIsCompletedSignal);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            PaintCharPartIsCompletedSignal.RemoveListener(OnPaintCharPartIsCompletedSignal);
        }

        private void OnPaintCharPartIsCompletedSignal()
        {
            PaintChar.MoveNextCharPart();
        }
    }
}
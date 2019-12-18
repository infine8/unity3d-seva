using UniKid.SubGame.Games.TexturePaintGame.Controller;
using UniKid.SubGame.Games.TexturePaintGame.Model;
using UniKid.SubGame.Games.TexturePaintGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.TexturePaintGame
{
    public class TexturePaintContext : SubGameContextBase<TexturePaintCore, MainView, MainViewMediator, StartCommand, ExitCommand>
    {
        public TexturePaintContext()
        {

        }

        public TexturePaintContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<BrushPaintCharColliderTriggerSignal>().ToSingleton();
            injectionBinder.Bind<PaintCharPartIsCompletedSignal>().ToSingleton();
            
            mediationBinder.Bind<Brush>().To<BrushMediator>();
            mediationBinder.Bind<PaintChar>().To<PaintCharMediator>();
            mediationBinder.Bind<PaintCharPart>().To<PaintCharPartMediator>();
        }
    }
}
using UniKid.SubGame.Games.SeaGuessing.Controller;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UniKid.SubGame.Games.SeaGuessing.View;
using UnityEngine;

namespace UniKid.SubGame.Games.SeaGuessing
{
    public class SeaGuessingContext : SubGameContextBase<SeaGuessingCore, MainView, MainViewMediator, StartCommand, ExitCommand>
    {
        public SeaGuessingContext()
        {

        }

        public SeaGuessingContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<MainView>().To<MainViewMediator>();

            mediationBinder.Bind<BubbleCharGroup>().To<BubbleCharGroupMediator>();
            mediationBinder.Bind<WaterTopBorder>().To<WaterTopBorderMediator>();
        }
    }
}
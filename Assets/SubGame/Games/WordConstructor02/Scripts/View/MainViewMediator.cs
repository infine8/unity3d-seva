using UniKid.SubGame.Games.WordConstructor02.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<WordConstructor02Settings, WordConstructor02UserData, WordConstructor02Level, WordConstructor02Stage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public WordConstructor02Core WordConstructor02Core { get; set; }

        public override SubGameMainViewBase SubGameViewBase
        {
            get { return MainView; }
        }

        protected override SubGameCoreBase<WordConstructor02Settings, WordConstructor02UserData, WordConstructor02Level, WordConstructor02Stage> SubGameCore
        {
            get { return WordConstructor02Core; }
        }
    }
}
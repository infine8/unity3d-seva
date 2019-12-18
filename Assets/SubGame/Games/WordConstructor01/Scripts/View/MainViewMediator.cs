using UniKid.SubGame.Games.WordConstructor01.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<WordConstructor01Settings, WordConstructor01UserData, WordConstructor01Level, WordConstructor01Stage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public WordConstructor01Core WordConstructor01Core { get; set; }

        public override SubGameMainViewBase SubGameViewBase
        {
            get { return MainView; }
        }

        protected override SubGameCoreBase<WordConstructor01Settings, WordConstructor01UserData, WordConstructor01Level, WordConstructor01Stage> SubGameCore
        {
            get { return WordConstructor01Core; }
        }
    }
}
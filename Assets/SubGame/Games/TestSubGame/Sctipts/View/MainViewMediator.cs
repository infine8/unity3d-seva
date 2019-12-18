using System;
using UniKid.Core.Model;
using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.TestSubGame.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<TestSubGameSettings, TestSubGameUserData>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public TestSubGameCore TestSubGameCore { get; set; }

        public override SubGameMainViewBase SubGameViewBase { get { return MainView; } }

        protected override SubGameCoreBase<TestSubGameSettings, TestSubGameUserData> SubGameCoreSimple
        {
            get { return TestSubGameCore; }
        }
    }
}
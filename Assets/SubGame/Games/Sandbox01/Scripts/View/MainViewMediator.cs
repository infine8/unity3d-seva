using System;
using UniKid.SubGame.Games.Sandbox01.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.Sandbox01.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<Sandbox01Settings, Sandbox01UserData, Sandbox01Level, Sandbox01Stage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public Sandbox01Core Sandbox01Core { get; set; }
        
        public override SubGameMainViewBase SubGameViewBase { get { return MainView; } }

        protected override SubGameCoreBase<Sandbox01Settings, Sandbox01UserData, Sandbox01Level, Sandbox01Stage> SubGameCore
        {
            get { return Sandbox01Core; }
        }

    }
}
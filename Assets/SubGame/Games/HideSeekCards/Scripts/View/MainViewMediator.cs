using System;
using System.Collections;
using UniKid.Core.Model;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.HideSeekCards.Controller;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<HideSeekCardsSettings, HideSeekCardsUserData, HideSeekCardsLevel, HideSeekCardsStage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public HideSeekCardsCore HideSeekCardsCore { get; set; }

        public override SubGameMainViewBase SubGameViewBase { get { return MainView; } }
		
		

        protected override SubGameCoreBase<HideSeekCardsSettings, HideSeekCardsUserData, HideSeekCardsLevel, HideSeekCardsStage> SubGameCore
        {
            get { return HideSeekCardsCore; }
        }
		

    }
}
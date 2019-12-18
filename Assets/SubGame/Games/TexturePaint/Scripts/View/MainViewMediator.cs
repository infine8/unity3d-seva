using System;
using System.Collections;
using UniKid.Core.Model;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.TexturePaintGame.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public class MainViewMediator : SubGameMainViewMediatorBase<TexturePaintSettings, TexturePaintUserData, TexturePaintLevel, TexturePaintStage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public TexturePaintCore TexturePaintCore { get; set; }

        public override SubGameMainViewBase SubGameViewBase { get { return MainView; } }

        protected override SubGameCoreBase<TexturePaintSettings, TexturePaintUserData, TexturePaintLevel, TexturePaintStage> SubGameCore
        {
            get { return TexturePaintCore; }
        }
    }
}
using System;
using UniKid.Core.Model;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.LibrarySlider.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<LibrarySliderSettings, LibrarySliderUserData, LibrarySliderLevel, LibrarySliderStage>
    {
        [Inject]
        public MainView MainView { get; set; }


        [Inject]
        public LibrarySliderCore LibrarySliderCore { get; set; }

        public override SubGameMainViewBase SubGameViewBase
        {
            get { return MainView; }
        }

        protected override SubGameCoreBase<LibrarySliderSettings, LibrarySliderUserData, LibrarySliderLevel, LibrarySliderStage> SubGameCore
        {
            get { return LibrarySliderCore; }
        }
    }
}
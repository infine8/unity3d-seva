using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.LibrarySlider.Controller
{
    public sealed class StartCommand : SubGameStartCommandBase<LibrarySliderSettings, LibrarySliderUserData, LibrarySliderLevel, LibrarySliderStage>
    {
        [Inject]
        public LibrarySliderCore LibrarySliderCore { get; set; }

        protected override SubGameCoreBase<LibrarySliderSettings, LibrarySliderUserData, LibrarySliderLevel, LibrarySliderStage> SubGameCore
        {
            get { return LibrarySliderCore; }
        }
    }
}
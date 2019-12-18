using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.LibrarySlider.Model
{
    public sealed class LibrarySliderCore : SubGameCoreBase<LibrarySliderSettings, LibrarySliderUserData, LibrarySliderLevel, LibrarySliderStage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.LibrarySlider; }
        }

    }
}
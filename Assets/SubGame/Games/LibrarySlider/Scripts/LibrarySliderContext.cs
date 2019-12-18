using UniKid.SubGame.Games.LibrarySlider.Controller;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UniKid.SubGame.Games.LibrarySlider.View;
using UnityEngine;

namespace UniKid.SubGame.Games.LibrarySlider
{
    public sealed class LibrarySliderContext : SubGameContextBase<LibrarySliderCore, MainView, MainViewMediator, StartCommand, ExitCommand>
    {

        public LibrarySliderContext()
        {

        }

        public LibrarySliderContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }
    }
}
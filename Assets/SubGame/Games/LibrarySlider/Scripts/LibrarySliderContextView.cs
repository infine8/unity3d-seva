using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.LibrarySlider
{
    public sealed class LibrarySliderContextView : ContextView
    {
        void Awake()
        {
            context = new LibrarySliderContext(this, true);
            context.Start();
        }
    }
}
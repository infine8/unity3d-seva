using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.TexturePaintGame
{
    public sealed class TexturePaintContextView : ContextView
    {
        void Awake()
        {
            context = new TexturePaintContext(this, true);
            context.Start();
        }
    }

}



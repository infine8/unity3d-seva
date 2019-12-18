using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.HideSeekCards
{
    public sealed class HideSeekCardsContextView : ContextView
    {

        void Awake()
        {
            context = new HideSeekCardsContext(this, true);
            context.Start();
        }
    }
}
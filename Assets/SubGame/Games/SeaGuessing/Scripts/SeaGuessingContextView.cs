using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.SeaGuessing
{
    public sealed class SeaGuessingContextView : ContextView
    {
        void Awake()
        {
            context = new SeaGuessingContext(this, true);
            context.Start();
        }
    }
}
using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.WordConstructor01
{
    public sealed class WordConstructor01ContextView : ContextView
    {
        void Awake()
        {
            context = new WordConstructor01Context(this, true);
            context.Start();
        }
    }
}
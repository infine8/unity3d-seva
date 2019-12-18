using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.WordConstructor02
{
    public sealed class WordConstructor02ContextView : ContextView
    {
        void Awake()
        {
            context = new WordConstructor02Context(this, true);
            context.Start();
        }
    }
}
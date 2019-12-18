using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.BubbleGuessing
{
    public sealed class BubbleGuessingContextView : ContextView
    {
        void Awake()
        {
            context = new BubbleGuessingContext(this, true);
            context.Start();
        }
		
    }
}
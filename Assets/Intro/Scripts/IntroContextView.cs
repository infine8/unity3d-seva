using strange.extensions.context.impl;

namespace UniKid.Intro
{
    public sealed class IntroContextView : ContextView
    {
        private void Awake()
        {
            context = new IntroContext(this, true);
            context.Start();
        }
    }
}
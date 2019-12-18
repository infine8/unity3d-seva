using strange.extensions.context.impl;

namespace UniKid.SubGame.UI
{
    public sealed class SubGameUIContextView : ContextView
    {
        private void Awake()
        {
            context = new SubGameUIContext(this, true);
            context.Start();

            DontDestroyOnLoad(this);
        }
    }
}
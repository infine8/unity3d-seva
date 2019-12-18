using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.Sandbox01
{
    public sealed class Sandbox01ContextView : ContextView
    {
        void Awake()
        {
            context = new Sandbox01Context(this, true);
            context.Start();
        }
    }
}
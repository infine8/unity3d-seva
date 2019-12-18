using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.TestSubGame
{
    public class TestSubGameContextView : ContextView
    {
        void Awake()
        {
            UnityEngine.Debug.Log("init context");

            context = new TestSubGameContext(this, true);
            context.Start();
        }
    }
}
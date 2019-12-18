using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Games.TestSubGame.View;
using UniKid.SubGame.Model;
using UnityEngine;

namespace UniKid.SubGame.Games.TestSubGame
{
    public class TestSubGameContext : 
        SubGameContextBase<TestSubGameCore, MainView, MainViewMediator, Controller.StartCommand, Controller.ExitCommand>
    {
        public TestSubGameContext()
        {

        }

        public TestSubGameContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<IQueue<GameObject>>().To<LeitnerQueue<GameObject>>();
        }
    }
}

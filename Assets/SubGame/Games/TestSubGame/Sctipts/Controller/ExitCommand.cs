using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Model;
using UnityEngine;
using strange.extensions.context.api;

namespace UniKid.SubGame.Games.TestSubGame.Controller
{
    public sealed class ExitCommand : SubGameExitCommandBase<TestSubGameSettings, TestSubGameUserData>
    {
        [Inject]
        public TestSubGameCore TestSubGameCore { get; set; }

        protected override SubGameCoreBase<TestSubGameSettings, TestSubGameUserData> SubGameCoreSimple
        {
            get { return TestSubGameCore; }
        }

        public override void Execute()
        {
            base.Execute();

        }
    }
}
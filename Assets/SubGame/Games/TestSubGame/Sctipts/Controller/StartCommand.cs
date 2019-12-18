using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Model;
using UnityEngine;

namespace UniKid.SubGame.Games.TestSubGame.Controller
{
    public class StartCommand : SubGameStartCommandBase<TestSubGameSettings, TestSubGameUserData>
    {
        [Inject]
        public TestSubGameCore TestSubGameCore { get; set; }

        [Inject]
        public IQueue<GameObject> Queue { get; set; }

        protected override SubGameCoreBase<TestSubGameSettings, TestSubGameUserData> SubGameCoreSimple
        {
            get { return TestSubGameCore; }
        }

        public override void Execute()
        {
            base.Execute();
            Debug.Log(TestSubGameCore);
            //dispatcher.Dispatch(SubGameEventType.CreateDbSubGame, TestSubGameCore.UserData);

            //var goList = new List<GameObject>();

            //for (var i = 0; i < 10; i++) goList.Add(new GameObject(string.Format("object {0}", i)));
            //LeitnerQueue.Init(goList, TestSubGameCore.UserData);
        }
    }

}



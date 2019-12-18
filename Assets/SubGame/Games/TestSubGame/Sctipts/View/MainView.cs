using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.TestSubGame.View
{
    public class MainView : SubGameMainViewBase
    {

        [Inject]
        public TestSubGameCore TestSubGameCore { get; set; }

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return TestSubGameCore; }
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void LoadStage()
        {
            throw new System.NotImplementedException();
        }


        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
            }
        }
    }

}


using System.Collections;
using UniKid.SubGame.Games.Sandbox01.Model;
using UniKid.SubGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.Sandbox01.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        [Inject]
        public Sandbox01Core Sandbox01Core { get; set; }


        public override SubGame.Model.SubGameCoreBase SubGameCoreBase
        {
            get { return Sandbox01Core; }
        }

        protected override void Start()
        {
            base.Start();

            LoadStage();
        }

        public override void LoadStage()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitForSeconds(0.5f);

            var tapObjects = GetComponentsInChildren<TapAnimatedObject>();

            foreach (var tapObj in tapObjects)
            {
                tapObj.Init();
            }
        }
		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }
    }
}
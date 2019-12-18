using UniKid.Core;
using UniKid.Core.Controller;
using UniKid.SubGame.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.View
{
    public sealed class SubGameDebugView : EventView
    {
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher CrossContextDispatcher { get; set; }

        [SerializeField] private tk2dTextMesh _gameInfoText;

        [SerializeField] private tk2dButton _prevLevelButton;
        [SerializeField] private tk2dButton _nextLevelButton;

        private string _gameInfoInitialText;

        private SubGameCoreBase SubGameCore { get { return MainView.SubGameCoreBase; } }

        private SubGameMainViewBase MainView { get; set; }

        protected override void Start()
        {
            base.Start();

            _gameInfoInitialText = _gameInfoText.text;

            _prevLevelButton.gameObject.SetActive(CoreContext.PlayParticularGame);
            _nextLevelButton.gameObject.SetActive(CoreContext.PlayParticularGame);
        }

        private void MoveNextLevel()
        {
            SubGameCore.ForceMoveNextLevel();
        }

        private void MovePrevLevel()
        {
            SubGameCore.ForceMovePrevLevel();
        }

        private void ExitToMenu()
        {
            CoreContext.SetDebugInfoActive(false);
            CrossContextDispatcher.Dispatch(CoreEventType.LoadMainMenu);
        }

        private void OnEnable()
        {
            MainView = (SubGameMainViewBase)FindObjectOfType(typeof(SubGameMainViewBase));

            var buttons = FindObjectsOfType(typeof(tk2dButton));

            foreach (var button in buttons)
            {
                ((tk2dButton)button).viewCamera = Camera.main;
            }

            if (MainView == null) gameObject.SetActive(false);
        }

        private void Update()
        {
            if (SubGameCore == null) return;

            if (Input.GetKeyDown(KeyCode.DownArrow)) SubGameCore.ForceMovePrevLevel();
            if (Input.GetKeyDown(KeyCode.UpArrow)) SubGameCore.ForceMoveNextLevel();

            _gameInfoText.text = string.Format(_gameInfoInitialText, SubGameCore.SubGameName,
                SubGameCore.CurrentLevelId + 1, SubGameCore.CurrentStageId + 1, (int)Time.timeSinceLevelLoad);

            _gameInfoText.Commit();
        }
    }
}

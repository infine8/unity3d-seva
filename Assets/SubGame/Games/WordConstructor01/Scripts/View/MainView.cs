using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class MainView : SubGameMainViewBase
    {
        public static float HIDE_FIELD_DURATION = 1f;

        [Inject]
        public WordConstructor01Core WordConstructor01Core { get; set; }

        [SerializeField]
        private Transform _boardField;

        public List<SourceBoardField> SourceFieldList;
        public List<TargetBoardField> TargetFieldList;

        private SourceBoardField _lastSourceField;
        private TargetBoardField _lastTargetField;

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return WordConstructor01Core; }
        }

        protected override void Start()
        {
            base.Start();

            LoadStage();
        }


        public override void LoadStage()
        {
            var loadStageAct = new Action(() =>
            {
                _lastSourceField = PoolManager.Spawn(SourceFieldList.Find(x => x.name.EndsWith(WordConstructor01Core.CurrentStage.ShowedWord.Length.ToString())).gameObject).GetComponent<SourceBoardField>();
                _lastTargetField = PoolManager.Spawn(TargetFieldList.Find(x => x.name.EndsWith(WordConstructor01Core.CurrentStage.GuessingWord.Length.ToString())).gameObject).GetComponent<TargetBoardField>();

                _lastSourceField.transform.parent = _lastTargetField.transform.parent = _boardField;

                _lastSourceField.Init(WordConstructor01Core.CurrentStage.ShowedWord);
                _lastTargetField.Init(WordConstructor01Core.CurrentStage.GuessingWord);
            });


            if (_lastSourceField == null)
            {
                loadStageAct();
            }
            else
            {
                StartCoroutine(DestroyOldFieldAndSpawnNew(loadStageAct));
            }
        }

        private IEnumerator DestroyOldFieldAndSpawnNew(Action onDestroy)
        {
            foreach (var board in _lastSourceField.BoardList)
            {
                HOTween.To(board.CharSprite, HIDE_FIELD_DURATION, "scale", Vector3.zero);
            }

            yield return new WaitForSeconds(HIDE_FIELD_DURATION);

            _lastTargetField.Despawn();
            _lastSourceField.Despawn();

            onDestroy();
        }
		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu);
        }

    }
}
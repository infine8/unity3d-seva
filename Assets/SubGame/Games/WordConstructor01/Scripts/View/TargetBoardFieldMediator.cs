using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor01.Controller;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class TargetBoardFieldMediator : EventMediatorBase
    {
        [Inject]
        public TargetBoardField TargetBoardField { get; set; }

        [Inject]
        public CharIsFoundSignal CharIsFoundSignal { get; set; }

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        [Inject]
        public TargetBoardIsInitedSignal TargetBoardIsInitedSignal { get; set; }

        private int _foundCharCount;

        public override void OnRegister()
        {
            base.OnRegister();

            CharIsFoundSignal.AddListener(OnCharIsFound);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            CharIsFoundSignal.RemoveListener(OnCharIsFound);
        }

        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);

            TargetBoardField.dispatcher.UpdateListener(toAdd, TargetBoardField.ViewEventType.BoardIsInited, TargetBoardIsInitedSignal.Dispatch);
        }

        private void OnCharIsFound(TargetBoard targetBoard)
        {
            _foundCharCount++;

            if(_foundCharCount == TargetBoardField.Word.Length)
            {
                _foundCharCount = 0;
                StagePassedSignal.Dispatch();
            }
        }
    }
}
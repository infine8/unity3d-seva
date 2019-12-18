using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor01.Controller;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class SourceBoardFieldMediator : EventMediatorBase
    {
        [Inject]
        public SourceBoardField SourceBoardField { get; set; }

        [Inject]
        public CharIsFoundSignal CharIsFoundSignal { get; set; }

        [Inject]
        public TargetBoardIsInitedSignal TargetBoardIsInitedSignal { get; set; }

        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            CharIsFoundSignal.AddListener(OnCharIsFound);

            TargetBoardIsInitedSignal.AddListener(OnTargetBoardIsInited);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            CharIsFoundSignal.RemoveListener(OnCharIsFound);
            TargetBoardIsInitedSignal.RemoveListener(OnTargetBoardIsInited);
        }

        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);

            SourceBoardField.dispatcher.UpdateListener(toAdd, SourceBoardField.ViewEventType.DragIsBegan, ResetDoingNothingCoroutine);
            SourceBoardField.dispatcher.UpdateListener(toAdd, SourceBoardField.ViewEventType.DragIsFinished, ResetDoingNothingCoroutine);
        }


        private void OnCharIsFound(TargetBoard targetBoard)
        {
            SourceBoardField.MoveDragObjectOnTarget(targetBoard.CharSprite.transform.position);
        }

        private void OnTargetBoardIsInited()
        {
            StartCoroutine(SourceBoardField.ShowChars(ResetDoingNothingCoroutine));
        }
    }
}
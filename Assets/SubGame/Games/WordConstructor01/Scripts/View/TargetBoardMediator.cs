using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor01.Controller;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class TargetBoardMediator : EventMediatorBase
    {
        [Inject]
        public TargetBoard TargetBoard { get; set; }

        [Inject]
        public CharIsFoundSignal CharIsFoundSignal { get; set; }
        
        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);

            TargetBoard.dispatcher.UpdateListener(toAdd, TargetBoard.ViewEventType.CharIsFound, CharIsFound);
            TargetBoard.dispatcher.UpdateListener(toAdd, TargetBoard.ViewEventType.CharIsNotFound, CharIsNotFound);
        }

        private void CharIsFound()
        {
            CharIsFoundSignal.Dispatch(TargetBoard);
            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Delight);
        }

        private void CharIsNotFound()
        {
            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Denial);            
        }

    }
}
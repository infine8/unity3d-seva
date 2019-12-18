using System.Collections;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.HideSeekCards.Controller;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.View.Character;
using UnityEngine;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class FieldMediator : EventMediatorBase
    {
        public static readonly float DOING_NOTHING_TIMEOUT = 10f;

        [Inject]
        public Field Field { get; set; }

        [Inject]
        public HideSeekCardsCore HideSeekCardsCore { get; set; }

        [Inject]
        public CardOpenedSignal CardOpenedSignal { get; set; }

        [Inject]
        public CardSequenceFoundSignal CardSequenceFoundSignal { get; set; }

        [Inject]
        public CardSequenceLooseSignal CardSequenceLooseSignal { get; set; }

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }


        protected override void UpdateListeners(bool toAdd)
        {
            if(toAdd)
            {
                CardOpenedSignal.AddListener(CardOpened);
            }
            else
            {
                CardOpenedSignal.RemoveListener(CardOpened);
            }

            Field.dispatcher.UpdateListener(toAdd, Field.FieldEventType.CardSequenceFound, OnCardSequenceFound);
            Field.dispatcher.UpdateListener(toAdd, Field.FieldEventType.CardSequenceLoose, OnCardSequenceLoose);
            Field.dispatcher.UpdateListener(toAdd, Field.FieldEventType.FieldPassed, OnFieldPassed);
            Field.dispatcher.UpdateListener(toAdd, Field.FieldEventType.FirstCardShowFinished, OnFirstCardShowFinished);
        }


        private void CardOpened(Card card)
        {
            Field.OnCardOpened(card);
            ResetDoingNothingCoroutine();
        }

        private void OnCardSequenceFound()
        {
            CardSequenceFoundSignal.Dispatch();
        }

        private void OnCardSequenceLoose()
        {
            CardSequenceLooseSignal.Dispatch();

            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Denial);

            ResetDoingNothingCoroutine();
        }

        private void OnFieldPassed()
        {
            StagePassedSignal.Dispatch();
            
            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Delight);
        }

        private void OnFirstCardShowFinished()
        {
            ResetDoingNothingCoroutine();
        }

    }
}
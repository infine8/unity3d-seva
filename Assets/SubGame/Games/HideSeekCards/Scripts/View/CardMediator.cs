using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.HideSeekCards.Controller;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class CardMediator : EventMediatorBase
    {
        [Inject]
        public Card Card { get; set; }

        [Inject]
        public CardOpenedSignal CardOpenedSignal { get; set; }

        [Inject]
        public CardSequenceFoundSignal CardSequenceFoundSignal { get; set; }

        [Inject]
        public CardSequenceLooseSignal CardSequenceLooseSignal { get; set; }

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        protected override void UpdateListeners(bool toAdd)
        {
            Card.dispatcher.UpdateListener(toAdd, Card.CardEventType.CardOpened, OnCardOpened);

            if (toAdd)
            {
                CardSequenceFoundSignal.AddListener(OnPairFound);
                CardSequenceLooseSignal.AddListener(OnPairLoose);
                StagePassedSignal.AddListener(OnFieldPassedSignal);
            }
            else
            {
                CardSequenceFoundSignal.RemoveListener(OnPairFound);
                CardSequenceLooseSignal.RemoveListener(OnPairLoose);
                StagePassedSignal.RemoveListener(OnFieldPassedSignal);
            }
        }

        private void OnFieldPassedSignal()
        {
            Card.OnShowCardFinished = Card.OnHideCardFinished = null;
        }

        private void OnCardOpened()
        {
            CardOpenedSignal.Dispatch(Card);
        }

        private void OnPairFound()
        {
        }

        private void OnPairLoose()
        {
            Card.Collider.enabled = false;
        }

        private void OnDisable()
        {
            Card.OnShowCardFinished = Card.OnHideCardFinished = null;
        }
    }
}
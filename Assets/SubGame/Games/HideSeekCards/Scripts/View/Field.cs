using System;
using System.Collections.Generic;
using System.Linq;
using UniKid.SubGame.View.Character;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using Random = UnityEngine.Random;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class Field : EventView
    {
        public static readonly float CARD_MEMORY_DURATION = 2f;


        public enum FieldEventType
        {
            CardSequenceFound,
            CardSequenceLoose,
            FieldPassed,
            FirstCardShowFinished
        }


        public bool IsSingleMode { get; private set; }
        private DreamingBaloon _baloon;

        private Card _singleModeGuessingCard;

        private readonly Dictionary<Card, bool> _cardPassDict = new Dictionary<Card, bool>();

        private readonly List<Card> _lastOpenedCardList = new List<Card>();

        protected override void Start()
        {
            base.Start();

            IsSingleMode = true;
            _singleModeGuessingCard = null;

            var cardList = new List<Card>(GetComponentsInChildren<Card>());

            var maxCardId = cardList.FindMax(x => x.Id);

            foreach (var card in cardList)
            {
                if (IsSingleMode && _cardPassDict.Keys.FirstOrDefault(x => x.PictureSprite.spriteId == card.PictureSprite.spriteId) != null) IsSingleMode = false;

                card.Init(maxCardId * Card.SHOW_FIRST_TIME_DELAY + CARD_MEMORY_DURATION);
                _cardPassDict.Add(card, false);
            }

            ShuffleCards();

            _baloon = transform.root.GetComponentsInChildren<DreamingBaloon>(true)[0];

            _baloon.gameObject.SetActive(IsSingleMode);

            cardList[0].OnHideCardFinished += () => { dispatcher.Dispatch(FieldEventType.FirstCardShowFinished); if (IsSingleMode) _baloon.Show(); };

            if (IsSingleMode)
            {
                _singleModeGuessingCard = cardList[Random.Range(0, cardList.Count)];
                _baloon.Picture.SetSprite(_singleModeGuessingCard.PictureSprite.spriteId);

                foreach (var card in cardList) if (!card.Equals(_singleModeGuessingCard)) _cardPassDict[card] = true;
            }

        }


        public void OnCardOpened(Card card)
        {
            if (_lastOpenedCardList.Count > 0)
            {
                var firstSpriteId = _lastOpenedCardList[0].PictureSprite.spriteId;
                var totalSameSpriteCount = _cardPassDict.Keys.Count(x => x.PictureSprite.spriteId == firstSpriteId);

                card.Collider.enabled = false;
                _lastOpenedCardList.Add(card);

                if (firstSpriteId == card.PictureSprite.spriteId)
                {
                    if(_lastOpenedCardList.Count == totalSameSpriteCount) CardSequenceFound();
                }
                else
                {
                    CardSequenceLoose();
                }
            }
            else
            {
                _lastOpenedCardList.Add(card);
                card.Collider.enabled = false;

                if (IsSingleMode)
                {
                    if(_singleModeGuessingCard.PictureSprite.spriteId == card.PictureSprite.spriteId)
                    {
                        CardSequenceFound();
                    }
                    else
                    {
                        CardSequenceLoose();
                    }

                }
            }

        }

        private void FieldPassed()
        {
            SetEnabledAllCards(false); 

            dispatcher.Dispatch(FieldEventType.FieldPassed);
        }

        private void CardSequenceFound()
        {
            foreach (var card in _lastOpenedCardList)
            {
                card.Collider.enabled = false;
                _cardPassDict[card] = true;
            }


            _lastOpenedCardList.Clear();

            dispatcher.Dispatch(FieldEventType.CardSequenceFound);
            
            var isFieldPassed = _cardPassDict.All(item => item.Value);

            if (isFieldPassed) FieldPassed();

        }

        private void CardSequenceLoose()
        {            
            StartCoroutine(HideLostSequence());

            dispatcher.Dispatch(FieldEventType.CardSequenceLoose);
        }


        private IEnumerator HideLostSequence()
        {
            yield return new WaitForSeconds(1.5f);

            var onHideAct = new Action(() => { foreach (var card in _cardPassDict) if (!card.Value || IsSingleMode) card.Key.Collider.enabled = true; });

            StartCoroutine(PerformActionShowHide(() => _lastOpenedCardList.ForEach(x => x.ShowHideCard(true)), onHideAct));

            _lastOpenedCardList.Clear();

        }

        private IEnumerator PerformActionShowHide(Action showHideAct, Action afterAct)
        {
            showHideAct();
            yield return new WaitForSeconds(Card.SHOW_HIDE_DURATION);
            afterAct();
        }

        private void SetEnabledAllCards(bool isEnabled)
        {
            foreach (var card in _cardPassDict) card.Key.Collider.enabled = isEnabled;
        }

        private void ShuffleCards()
        {
            var pictureList = _cardPassDict.Select(item => item.Key.PictureSprite).ToList();

            var newPictureList = new List<string>();
            var pictureCount = pictureList.Count;

            for (var i = 0; i < pictureCount; i++)
            {
                var randomPictureIndex = Random.Range(0, pictureList.Count);
                newPictureList.Add(pictureList[randomPictureIndex].CurrentSprite.name);

                pictureList.RemoveAt(randomPictureIndex);
            }

            var cardIndex = 0;
            foreach (var item in _cardPassDict)
            {
                item.Key.PictureSprite.SetSprite(newPictureList[cardIndex]);
                cardIndex++;
            }
        }
    }
}
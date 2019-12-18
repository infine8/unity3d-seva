using System;
using System.Collections;
using Holoville.HOTween;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.HideSeekCards.View
{
    public sealed class Card : EventView
    {
        public enum CardEventType
        {
            CardOpened
        }

        public static readonly float SHOW_FIRST_TIME_DELAY = 0.6f;
        public static readonly float SHOW_HIDE_DURATION = 0.7f;

        public tk2dSprite PictureSprite;

        [SerializeField] private int _id;
        [SerializeField] private tk2dSprite _lampOff;
        [SerializeField] private tk2dSprite _lampOn;

        public int Id { get { return _id; } }
        
        private float _cardMemoryDuration;

        public BoxCollider Collider { get; private set; }
        public Action OnHideCardFinished { get; set; }
        public Action OnShowCardFinished { get; set; }

        public void Init(float cardMemoryDuration)
        {
            OnHideCardFinished = OnShowCardFinished = null;

            Collider = GetComponentInChildren<BoxCollider>();

            _cardMemoryDuration = cardMemoryDuration;
        }
        

        private void OnClick()
        {
            Collider.enabled = false;

            ShowHideCard(false);

            dispatcher.Dispatch(CardEventType.CardOpened);
        }

        #region Show Hide Cards
        
        public void ShowCardHideFirstTime()
        {
            StartCoroutine(ShowCardsFirstTime());
            StartCoroutine(HideCardsFirstTime());
        }

        private IEnumerator HideCardsFirstTime()
        {
            yield return new WaitForSeconds(_cardMemoryDuration);

            ShowHideCard(true);
        }

        private IEnumerator ShowCardsFirstTime()
        {
            yield return new WaitForSeconds(_id * SHOW_FIRST_TIME_DELAY);

            Collider.enabled = false;

            OnHideCardFinished += () => Collider.enabled = true;

            ShowHideCard(false);
        }

        public void ShowHideCard(bool hide)
        {
            var colliderEnabled = Collider.enabled;
 
            Collider.enabled = false;

            var param = new TweenParms();

            param.Prop("color", new Color(1, 1, 1, hide ? 1 : 0));

            param.OnComplete(() =>
                                 {
                                     Collider.enabled = colliderEnabled;
                                     
                                     if (OnShowCardFinished != null && !hide)
                                     {
                                         OnShowCardFinished();
                                         OnShowCardFinished = null;
                                     }
                                     if (OnHideCardFinished != null && hide)
                                     {
                                         OnHideCardFinished();
                                         OnHideCardFinished = null;
                                     }
                                 });


            HOTween.To(_lampOff, SHOW_HIDE_DURATION, param); 
        }

        #endregion

    }
}
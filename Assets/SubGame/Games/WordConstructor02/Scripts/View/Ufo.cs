using System;
using Holoville.HOTween;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class Ufo : EventView
    {
        public static readonly float SHOW_HIDE_DURATION = 1.0f;

        public enum AnimationType
        {
            UfoUp,
            UfoDown
        }

        [SerializeField] private tk2dSprite _light;
        [SerializeField] private tk2dSprite _guessingPicture;

        private Action OnHideComplete { get; set; }

        public void ShowPicture(bool isFirstTime)
        {
            OnHideComplete = null;

            _light.color = _guessingPicture.color = Const.TransperentWhiteColor;

            if (isFirstTime)
            {
                animation.Play(AnimationType.UfoUp.ToString());
            }
            else
            {
                OnReachedUp();
            }
        }

        public void HidePicture(Action onComplete)
        {
            OnHideComplete = onComplete;

            ShowHideSprite(_guessingPicture, false, () => ShowHideSprite(_light, false, onComplete));
        }

        public void OnReachedUp()
        {
            ShowHideSprite(_light, true, () => ShowHideSprite(_guessingPicture, true, null));
        }

        public void OnReachedDown()
        {
            if (OnHideComplete != null) OnHideComplete();
        }


        private void ShowHideSprite(tk2dSprite sprite, bool isShow, Action onComplete)
        {
            var param = new TweenParms();

            param.Prop("color", isShow ? Const.WhiteColor : Const.TransperentWhiteColor);

            if (onComplete != null) param.OnComplete(() => onComplete());

            HOTween.To(sprite, SHOW_HIDE_DURATION, param);
        }

    }
}
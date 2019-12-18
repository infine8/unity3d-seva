using Holoville.HOTween;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.LibrarySlider.View
{
    public sealed class Pot : EventView
    {
        [Inject]
        public LibrarySliderCore LibrarySliderCore { get; set; }

        public int Index;
        [SerializeField] private tk2dSprite _charSprite;

        private Vector3 _initScale;

        protected override void Start()
        {
            base.Start();

            _initScale = _charSprite.scale;
        }

        public void UpdateChar(string langLibraryNameSequence, string charName, float tweenDuration, bool isInitiazation)
        {
            if(isInitiazation)
            {
                UpdateChar(langLibraryNameSequence, charName);
            }
            else
            {
                var param = new TweenParms();
                param.Prop("scale", Vector3.zero);
                param.OnComplete(() =>
                                     {
                                         UpdateChar(langLibraryNameSequence, charName);
                                         HOTween.To(_charSprite, tweenDuration, "scale", _initScale);
                                     });

                HOTween.To(_charSprite, tweenDuration, param);
            }

        }


        private void OnTouch()
        {

        }

        private void UpdateChar(string langLibraryNameSequence, string charName)
        {
            _charSprite.gameObject.SetActive(true);

            if (!string.IsNullOrEmpty(charName))
            {
                _charSprite.SetSprite(Utils.GetCharSpriteName(langLibraryNameSequence, charName));
            }
            else
            {
                _charSprite.gameObject.SetActive(false);
            }
        }
    }
}
using Holoville.HOTween;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class TargetBoard : BoardBase
    {
        public static readonly float CHAR_HIDE_DURATION = 3f;

        public enum ViewEventType
        {
            CharIsFound,
            CharIsNotFound
        }

        [Inject]
        public WordConstructor01Core WordConstructor01Core { get; set; }

        public tk2dSprite CharOutlineSprite;


        public void Init(char c)
        {
            collider.enabled = true;

            CharOutlineSprite.color = new Color(1, 1, 1, 1);
            
            CharOutlineSprite.SetSprite(Utils.GetCharSpriteName(WordConstructor01Core.CurrentLevel.LangOutlineLibraryNameSequence, c));
            
            CharOutlineSprite.gameObject.SetActive(true);

            AnimateInit();
        }

        private void AnimateInit()
        {
            CharOutlineSprite.color = Const.TransperentWhiteColor;
            
            HOTween.To(CharOutlineSprite, CHAR_HIDE_DURATION, "color", Const.WhiteColor);

            HOTween.To(CharSprite, CHAR_HIDE_DURATION, "color", Const.TransperentWhiteColor);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.GetComponent<SourceBoard>().Char.Equals(Char))
            {
                collider.enabled = false;

                HOTween.To(CharOutlineSprite, SourceBoardField.MOVE_DURATION, "color", new Color(1, 1, 1, 0));

                dispatcher.Dispatch(ViewEventType.CharIsFound);
            }
            else
            {
                dispatcher.Dispatch(ViewEventType.CharIsNotFound);                
            }
        }
    }
}
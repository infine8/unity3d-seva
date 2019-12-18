using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public sealed class TargetBoardField : BoardFieldBase
    {
        public enum ViewEventType
        {
            BoardIsInited
        }

        public override void Init(string word)
        {
            base.Init(word);

            var onCharsShowedAct = new Action(() => StartCoroutine(InitBordList()));

            BoardList.ForEach(x => ((TargetBoard)x).CharOutlineSprite.gameObject.SetActive(false));

            StartCoroutine(ShowChars(onCharsShowedAct));
        }

        private IEnumerator InitBordList()
        {
            for (var i = 0; i < Word.Length; i++)
            {
                ((TargetBoard)BoardList[i]).Init(Word[i]);
            }

            yield return new WaitForSeconds(TargetBoard.CHAR_HIDE_DURATION);

            dispatcher.Dispatch(ViewEventType.BoardIsInited);
        }
    }
}
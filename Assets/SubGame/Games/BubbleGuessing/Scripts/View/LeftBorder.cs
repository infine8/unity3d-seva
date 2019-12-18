using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class LeftBorder : EventView
    {
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        [SerializeField]
        private tk2dTextMesh _missedManyLettersLabel;

        [System.NonSerialized]
        public int MissedCharCount;

        private int _missedLetterCounter = 0;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Const.TAG_BubbleGuessing_UfoChar)) return;
            
            var ufoChar = other.transform.parent.gameObject.GetComponent<UfoChar>();

            if (BubbleGuessingCore.Expression != null && BubbleGuessingCore.CurrentFindingCharIndex < BubbleGuessingCore.Expression.Text.Length)
            {
                var currentRightChar = BubbleGuessingCore.Expression.Text[BubbleGuessingCore.CurrentFindingCharIndex];
                if (ufoChar.BubbleChar.Equals(currentRightChar)) _missedLetterCounter++;
            }

            //if (_missedLetterCounter >= _findingWord.CurrentStage.MaxMissedCharCount) StartCoroutine(ShowLabel());
            PoolManager.Despawn(ufoChar.gameObject);
        }


        private IEnumerator ShowLabel()
        {
            _missedManyLettersLabel.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            _missedManyLettersLabel.gameObject.SetActive(false);
        }

        private void OnWin()
        {
            _missedLetterCounter = 0;
        }
    }
}
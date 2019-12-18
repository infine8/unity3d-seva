using System.Collections.Generic;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor02.Controller;
using UniKid.SubGame.Games.WordConstructor02.Model;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class ScrollFieldMediator : EventMediatorBase
    {
        [Inject]
        public ScrollField ScrollField { get; set; }

        [Inject]
        public NewItemDetectedSignal NewItemDetectedSignal { get; set; }

        [Inject]
        public WordConstructor02Core WordConstructor02Core { get; set; }

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }

        private Dictionary<int, string> _scrollSyllableDict = new Dictionary<int, string>();

        private List<string> _syllableList; 

        public override void OnRegister()
        {
            base.OnRegister();

            NewItemDetectedSignal.AddListener(OnNewItemDetected);

            _syllableList = WordConstructor02Core.CurrentStage.SyllableSequence.SplitSequence(true);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            NewItemDetectedSignal.RemoveListener(OnNewItemDetected);
        }

        private void OnNewItemDetected(int scrollIndex, string syllable)
        {
            syllable = syllable.ToUpper();

            if (_scrollSyllableDict.ContainsKey(scrollIndex))
            {
                _scrollSyllableDict[scrollIndex] = syllable;
            }
            else
            {
                _scrollSyllableDict.Add(scrollIndex, syllable);
            }

            var isStagePassed = true;

            for (var i = 0; i < _syllableList.Count; i++)
            {
                if (!_scrollSyllableDict.ContainsKey(i))
                {
                    ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Denial);
                    
                    return;
                }

                isStagePassed = isStagePassed && _syllableList[i].Equals(_scrollSyllableDict[i]);
            }

            if (isStagePassed)
            {
                StagePassedSignal.Dispatch();
                ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Delight);
            }
            else
            {
                ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Denial);                
            }
        }
    }
}
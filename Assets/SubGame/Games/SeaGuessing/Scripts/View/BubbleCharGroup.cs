using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UniKid.SubGame.Games.SeaGuessing.Model;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class BubbleCharGroup : PoolObject
    {
        public enum EventType
        {
            CharIsFound,
            CharIsNotFound
        }

        [Inject]
        public SeaGuessingCore SeaGuessingCore { get; set; }

        [SerializeField] private GameObject _fxFound;

        private List<BubbleChar> _bubbleCharList;

        private List<string> _possibleCharNameList;

        private MainView _mainView;
        private tk2dSprite _guessingChar;
        private GameObject _scoreSpriteFull;
        private Transform _guessingCharTransform;
        private List<ScoreItem> _scoreItemList;
        private float _scoreItemMoveDuration;

        protected override void OnPoolInitialize()
        {
            _bubbleCharList = new List<BubbleChar>(GetComponentsInChildren<BubbleChar>(true));

            _mainView = ((MainView) FindObjectOfType(typeof (MainView)));

            _guessingChar = _mainView.GuessingChar;
            _scoreSpriteFull = _mainView.ScoreItemFull;
            _scoreItemList = _mainView.ScoreItemList;
            _scoreItemMoveDuration = _mainView.ScoreItemMoveDuration;
        }

        protected override void OnPoolActivation()
        {
            _guessingChar.SetSprite(Utils.GetCharSpriteName(SeaGuessingCore.CurrentStage.CharLibraryNameSequence, SeaGuessingCore.CurrentStage.CharName));
            ShowHideGuessingChar(true);

            _possibleCharNameList = SeaGuessingCore.PossibleCharList;
            
            foreach (var item in _bubbleCharList)
            {
                var randomCharNameIndex = Random.Range(0, _possibleCharNameList.Count);
                var possibleCharName = _possibleCharNameList[randomCharNameIndex];

                if (possibleCharName.Equals(SeaGuessingCore.CurrentStage.CharName)) _guessingCharTransform = item.transform;

                item.Init(SeaGuessingCore, possibleCharName, OnCharIsFound, OnCharIsNotFound);
                
                _possibleCharNameList.RemoveAt(randomCharNameIndex);
            }
        }
        
        public IEnumerator DespawnGroup(Action onDespawn)
        {
            //var delay = _bubbleCharList.Max(x => x.ScaleDuration);
			var delay = _bubbleCharList.FindMax(x => x.ScaleDuration);
            _bubbleCharList.ForEach(x => x.Despawn());

            ShowHideGuessingChar(false);

            yield return new WaitForSeconds(delay);

            PoolManager.Despawn(gameObject);

            if (onDespawn != null) onDespawn();
        }

        private void OnCharIsFound()
        {
            SpawnFxFound();

            dispatcher.Dispatch(EventType.CharIsFound);
        }

        private void OnCharIsNotFound()
        {
            dispatcher.Dispatch(EventType.CharIsNotFound);            
        }

        private void ShowHideGuessingChar(bool isShow)
        {
            _guessingChar.transform.localScale = isShow ? Vector3.zero : Vector3.one;
			
            var param = new TweenParms();
            param.Prop("localScale", isShow ? Vector3.one : Vector3.zero);
            param.Ease(EaseType.Linear);
//			float max = _bubbleCharList[0].ScaleDuration;
//			for (int i=0; i < _bubbleCharList.Count; i++) {
//				if (max < _bubbleCharList[i].ScaleDuration)
//					max = _bubbleCharList[i].ScaleDuration;
//			}
			//float bb = _bubbleCharList.Max(x => x.ScaleDuration);
			float max = _bubbleCharList.FindMax(x => x.ScaleDuration);
            HOTween.To(_guessingChar.transform, max , param);
        }

        private void SpawnFxFound()
        {
            var fx = PoolManager.Spawn(_fxFound, _guessingCharTransform);

            fx.transform.parent = transform.parent;
        }

        private void SpawnScoreItem()
        {
            var scoreItem = PoolManager.Spawn(_scoreSpriteFull, _guessingCharTransform);
            scoreItem.transform.parent = transform.parent;

            var targetEmptyScoreItemIndex = _scoreItemList.FindIndex(x => !x.IsFilled);
            var targetEmptyScoreItem = _scoreItemList[targetEmptyScoreItemIndex < 0 ? 0 : targetEmptyScoreItemIndex];
            targetEmptyScoreItem.IsFilled = true;

            var endPos = targetEmptyScoreItem.EmptySprite.transform.position;

            endPos = new Vector3(endPos.x, endPos.y, endPos.z - 1);

            var startPos = scoreItem.transform.position;
            startPos = new Vector3(startPos.x, startPos.y, endPos.z);
            scoreItem.transform.position = startPos;
            
            var pathVectors = new List<Vector3>();

//            var center = (endPos + startPos)/2;
//            center = center * 1.1f;
//            center.z = endPos.z;
//            
//            pathVectors.Add(center);
            pathVectors.Add(endPos); 


            var param = new TweenParms();
            param.Prop("position", new PlugVector3Path(pathVectors.ToArray()));
            param.Ease(EaseType.Linear); 

            HOTween.To(scoreItem.transform, _scoreItemMoveDuration, param);
			Debug.Log("SpawnScoreItem Done");
        }
    }
}
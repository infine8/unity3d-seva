using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public abstract class BoardFieldBase : PoolObject
    {
        public static float SHOW_ITEM_DURATION = 1f;
        public static float SHOW_ITEM_DELAY = 0.5f;

        [Inject]
        public WordConstructor01Core WordConstructor01Core { get; set; }

        public List<BoardBase> BoardList;

        public string Word { get; private set; }

        private readonly List<Vector3> _initCharScaleList = new List<Vector3>(); 
        private readonly List<Vector3> _initCharPositionList = new List<Vector3>(); 

        protected override void OnPoolInitialize()
        {
            BoardList = new List<BoardBase>(GetComponentsInChildren<BoardBase>(true));
            BoardList.Sort((obj1, obj2) => obj1.Index - obj2.Index);

            BoardList.ForEach(x => { _initCharScaleList.Add(x.CharSprite.scale); _initCharPositionList.Add(x.CharSprite.transform.localPosition); });
        }
        
        public virtual void Init(string word)
        {
            Word = word;

            for (var i = 0; i < word.Length; i++)
            {
                BoardList[i].CharSprite.scale = Vector3.zero;

                BoardList[i].CharSprite.color = new Color(1, 1, 1, 1);
                BoardList[i].CharSprite.transform.localPosition = _initCharPositionList[i];

                BoardList[i].Char = word[i];
                BoardList[i].CharSprite.SetSprite(Utils.GetCharSpriteName(WordConstructor01Core.CurrentLevel.LangLibraryNameSequence, word[i]));

                if (BoardList[i].CharSprite.collider != null) BoardList[i].CharSprite.collider.enabled = true;
            }
        }

        public IEnumerator ShowChars(Action onComplete)
        {
            for (var i = 0; i < BoardList.Count; i++)
            {
                var param = new TweenParms();
                param.Prop("scale", _initCharScaleList[i]);
                param.Delay(SHOW_ITEM_DELAY*BoardList[i].Index);

                HOTween.To(BoardList[i].CharSprite, SHOW_ITEM_DURATION, param);
            }

            yield return new WaitForSeconds(BoardList.FindMax(x => x.Index)*SHOW_ITEM_DELAY);

            if (onComplete != null) onComplete();
        }

    }
}
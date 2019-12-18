using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UnityEngine;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class BubbleChar : MonoBehaviour
    {
        public static readonly float JUMP_RADIUS = 8f;
        public static readonly float JUMP_DURATION = 0.6f;
        public static readonly int JUMP_POSITION_COUNT = 50;
        public static readonly float SCALE_DURATION = 0.6f;

        [SerializeField]
        public tk2dSprite CharSprite;
        [SerializeField]
        private GameObject _jellyfish;
        [SerializeField]
        private Vector2 _scaleDelayRange = new Vector2(0, 1.5f);

        public float ScaleDuration { get { return SCALE_DURATION; } }

        private Action _charIsFoundAction;
        private Action _charIsNotFoundAction;
	
        private SeaGuessingCore Core { get; set; }
        private string CharName { get; set; }

        private Vector3 _initialPosition;

        private readonly List<tk2dSpriteAnimator> _jellyfishAnimList = new List<tk2dSpriteAnimator>();
        private readonly List<Vector3> _jumpPositionList = new List<Vector3>(); 

        public void Init(SeaGuessingCore core, string charName, Action onCharIsFound, Action onCharIsNotFound)
        {
            CharSprite.gameObject.SetActive(true);

            _jellyfishAnimList.AddRange(GetComponentsInChildren<tk2dSpriteAnimator>(true));

            if (_jellyfishAnimList.Count > 0)
            {
                var startFrameIndex = UnityEngine.Random.Range(0, _jellyfishAnimList[0].DefaultClip.frames.Length);
                _jellyfishAnimList.ForEach(x => x.PlayFromFrame(startFrameIndex));
            }

            transform.localScale = Vector3.zero;

            Core = core;
            CharName = charName;

            _charIsFoundAction = onCharIsFound;
            _charIsNotFoundAction = onCharIsNotFound;
            _initialPosition = transform.localPosition;

            CharSprite.SetSprite(Utils.GetCharSpriteName(Core.CurrentStage.CharLibraryNameSequence, CharName));

            ScaleRoot(false);

            for (var i = 0; i < JUMP_POSITION_COUNT; i++)
            {
                var offset = UnityEngine.Random.insideUnitCircle * JUMP_RADIUS;
                _jumpPositionList.Add(new Vector3(offset.x, offset.y, 0) + _initialPosition);
            }
        }

        public void Despawn()
        {
            ScaleRoot(true);
        }

        private void TouchBubble()
        {
            if (CharName.ToUpper().Equals(Core.CurrentStage.CharName.ToUpper()))
            {
                if (_charIsFoundAction != null) _charIsFoundAction();

            }
            else
            {
                if (_charIsNotFoundAction != null) _charIsNotFoundAction(); 

                Jump();
            }
        }

        private void ScaleRoot(bool isOut)
        {
            var param = new TweenParms();

            if (!isOut) param.Delay(UnityEngine.Random.Range(_scaleDelayRange.x, _scaleDelayRange.y));
            param.Prop("localScale", isOut ? Vector3.zero : Vector3.one);
            param.Ease(EaseType.Linear);
            param.OnComplete(() => transform.localPosition = _initialPosition);

            HOTween.To(transform, SCALE_DURATION, param);
        }

        private void Jump()
        {
            HOTween.To(transform, JUMP_DURATION, "localPosition", _jumpPositionList[UnityEngine.Random.Range(0, _jumpPositionList.Count)]);
        }
    }
}
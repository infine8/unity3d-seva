using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using UniKid.SubGame.Games.BubbleGuessing.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class FindingExpression : SubGameMainViewBase
    {
        public enum EventType
        {
            ExpressionFound
        }

        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        [SerializeField] private GameObject _findingCharObject;
        [SerializeField] private Color32 _unfoundCharColor;
        [SerializeField] private float _charShowDuration = 1.0f;
        [SerializeField] private GameObject _finishWindow;
        [SerializeField] private Pipe _pipe;
        [SerializeField] private UfoSpawner _ufoSpawner;
        [SerializeField] private tk2dTextMesh _levelNumText;
        [SerializeField] private tk2dTextMesh _stageNumText;
        [SerializeField] private tk2dTextMesh _errorCounterText;
        [SerializeField] private float _centerPositionX;
		
		private readonly List<tk2dSprite> _charSpriteList = new List<tk2dSprite>();

        public override SubGameCoreBase SubGameCoreBase
        {
            get { return BubbleGuessingCore; }
        }

        public override float SecondsBeforeLoadNewLevel
        {
            get
            {
                return 2f;
            }
        }
        
        #region Initialization
		
        protected override void Start()
        {
            base.Start();

            LoadStage();
        }

        public override void LoadStage()
        {
            UpdateCurrentLevelNumText();
            UpdateCurrentStageNumText();
            UpdateErrorNumText();

            SetNewWord();
        }

        #endregion

        private void SetNewWord()
        {

            if (BubbleGuessingCore.CurrentStage == null) return;

            #region init settings

            BubbleGuessingCore.CurrentCharSequence = BubbleGuessingCore.CurrentStage.CharSequence.SplitSequence(true).ToArray();
            BubbleGuessingCore.CurrentCharLibraryNameSequence = BubbleGuessingCore.CurrentStage.CharLibraryNameSequence;

            BubbleGuessingCore.CurrentFindingCharIndex = 0;
            _charSpriteList.ForEach(x => { x.color = Color.white; PoolManager.Despawn(x.gameObject); });
            _charSpriteList.Clear();
            BubbleGuessingCore.UserData.ErrorCount = 0;

            #endregion

            #region init finding word with template

            string findingWord;

            if (!BubbleGuessingCore.UseTemplate)
            {
                findingWord = BubbleGuessingCore.RealFindingWord = BubbleGuessingCore.Expression.Text;
            }
            else
            {
                findingWord = BubbleGuessingCore.Expression.Template;
                BubbleGuessingCore.RealFindingWord = string.Empty;

                if (BubbleGuessingCore.Expression.Template.Length != BubbleGuessingCore.Expression.Text.Length)
                {
                    Debug.LogError("Word teplate is not suitable with word + " + BubbleGuessingCore.Expression.Text +
                                   " : " + BubbleGuessingCore.Expression.Template);
                    return;
                }

                for (var i = 0; i < findingWord.Length; i++)
                {
                    if (char.ToUpper(findingWord[i]).Equals(char.ToUpper(Const.UNKNOWN_CHAR)))
                    {
                        BubbleGuessingCore.RealFindingWord += BubbleGuessingCore.Expression.Text[i];
                    }
                }
            }

            #endregion

            #region init finding word sprites

            var wordLength = 0f;

            for (var i = 0; i < BubbleGuessingCore.Expression.Text.Length; i++)
            {
                var go = PoolManager.Spawn(_findingCharObject, transform);
                var charSprite = go.GetComponent<tk2dSprite>();
                charSprite.SetSprite(Utils.GetCharSpriteName(BubbleGuessingCore.CurrentCharLibraryNameSequence,findingWord[i]));

                if (!BubbleGuessingCore.UseTemplate) charSprite.color = _unfoundCharColor;

                _charSpriteList.Add(charSprite);

                go.transform.localPosition = new Vector3(0, 0, -3);

                if (i == 0) continue;

                var lastSprite = _charSpriteList[i - 1];
                var lb = lastSprite.GetBounds();
                var cb = charSprite.GetBounds();
                var charLength = 0.5f + lb.extents.x + lb.center.x + cb.extents.x + cb.center.x;

                var offset = BubbleGuessingCore.UseTemplate && findingWord[i].Equals(Const.UNKNOWN_CHAR) ? Vector3.zero : GetCharOffset(BubbleGuessingCore.Expression.Text[i]);

                go.transform.localPosition += offset + new Vector3(lastSprite.gameObject.transform.localPosition.x, 0, 0) + new Vector3(charLength, 0, 0);

                wordLength += charLength;

            }

            transform.localPosition = new Vector3(_centerPositionX - wordLength/2, transform.localPosition.y, transform.localPosition.z);

            #endregion

            #region init game objects
            
            UfoChar.Init();
            _ufoSpawner.StartSwawn();
            _pipe.Init();

            #endregion

        }


        public bool CheckChar(char c)
        {
            if (BubbleGuessingCore.CurrentStage == null) return false;

            var rightChar = BubbleGuessingCore.RealFindingWord[BubbleGuessingCore.CurrentFindingCharIndex];

            if (!char.ToUpper(c).Equals(char.ToUpper(rightChar)))
            {
                BubbleGuessingCore.UserData.ErrorCount++;

                if (BubbleGuessingCore.UserData.ErrorCount >= BubbleGuessingCore.CurrentStage.MaxErrorCount)
                {
                    //StartCoroutine(MoveNextStage(false));
                }

                UpdateErrorNumText();

                return false;
            }

            var tweenCharIndex = BubbleGuessingCore.UseTemplate ? BubbleGuessingCore.Expression.Template.IndexOf(Const.UNKNOWN_CHAR, BubbleGuessingCore.CurrentFindingCharIndex + 1) : BubbleGuessingCore.CurrentFindingCharIndex;

            if (tweenCharIndex < 0) tweenCharIndex = BubbleGuessingCore.CurrentFindingCharIndex;

            if (!BubbleGuessingCore.UseTemplate)
            {
                HOTween.To(_charSpriteList[tweenCharIndex], _charShowDuration, "color", Color.white);
            }
            else
            {
                var param = new TweenParms();
                param.Prop("scale", Vector3.zero);

                param.OnComplete(() =>
                {
                    var pos = _charSpriteList[tweenCharIndex].gameObject.transform.localPosition;
                    PoolManager.Despawn(_charSpriteList[tweenCharIndex].gameObject);

                    var go = PoolManager.Spawn(_findingCharObject, transform);
                    go.transform.localPosition = pos;
                    var charSprite = go.GetComponent<tk2dSprite>();
                    charSprite.SetSprite(Utils.GetCharSpriteName(BubbleGuessingCore.CurrentCharLibraryNameSequence, c));
                    charSprite.scale = Vector3.zero;
                    HOTween.To(charSprite, _charShowDuration, "scale", _findingCharObject.GetComponent<tk2dSprite>().scale);
                });

                HOTween.To(_charSpriteList[tweenCharIndex], _charShowDuration, param);
            }

            BubbleGuessingCore.CurrentFindingCharIndex++;

            return true;
        }


        public bool CheckWin(bool isCharFound)
        {
            if (BubbleGuessingCore.CurrentFindingCharIndex < BubbleGuessingCore.RealFindingWord.Length) return false;
            
            dispatcher.Dispatch(EventType.ExpressionFound);

            BubbleGuessingCore.CurrentFindingCharIndex = 0;

            return true;
        }

        private Vector3 GetCharOffset(char c)
        {
            var fc = _findingCharObject.GetComponent<FindingChar>();

            var lib = fc.LibraryCharOffsets[0];

            var charOffset = lib.CharOffsets.FirstOrDefault(x => x.Char.ToUpper().Equals(c.ToString().ToUpper()));
            return charOffset != null ? new Vector2(0, charOffset.YOffset) : Vector2.zero;
        }

        private IEnumerator ShowLabel(tk2dTextMesh label, float forSeconds)
        {
            label.gameObject.SetActive(true);
            yield return new WaitForSeconds(forSeconds);
            label.gameObject.SetActive(false);
        }

        private void UpdateCurrentLevelNumText()
        {
            //_levelNumText.text = string.Format("Current Level: {0} / {1}", BubbleGuessingCore.CurrentLevel.Name, BubbleGuessingCore.Settings.LevelController.LevelList.Count);
            //_levelNumText.Commit();
        }

        private void UpdateCurrentStageNumText()
        {
            //_stageNumText.text = string.Format("Current Stage: {0} / {1}", BubbleGuessingCore.CurrentLevel.Name, BubbleGuessingCore.CurrentLevel.StageController.StageList.Count);
            //_stageNumText.Commit();
        }

        private void UpdateErrorNumText()
        {
            //_errorCounterText.text = string.Format("Errors: {0} / {1}", BubbleGuessingCore.UserData.LevelSnapshot.ErrorCount, BubbleGuessingCore.CurrentStage.MaxErrorCount);
            //_errorCounterText.Commit();
        }
		
		private void ExitGame()
        {
            dispatcher.Dispatch(SubGameViewEvent.ExitToMainMenu); 
        }
    }
}
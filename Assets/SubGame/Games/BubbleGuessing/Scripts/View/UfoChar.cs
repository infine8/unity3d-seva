using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class UfoChar : PoolObject
    {
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        public tk2dSprite CharSprite;
        
        [SerializeField] private tk2dSpriteAnimator _bubbleBoomAnimation;
        [SerializeField] private tk2dSprite _bubbleSprite;
        [SerializeField] private Vector2 _forcePeriod;

        [System.NonSerialized]
        public char BubbleChar;

        private Pipe _pipe;
        private FindingExpression _findingExp;

        private Vector3 _baseVelocity;

        private int _rotate;
        private float _nextTime;
        private Vector3 _force;
        private AnimationManager _animationManager;

        private static int _currentCharIndex;

        [System.NonSerialized]
        public static int Counter = 0;

        public static void Init()
        {
            _currentCharIndex = 0;
        }

        protected override void OnPoolInitialize()
        {
            _pipe = FindObjectOfType(typeof(Pipe)) as Pipe;
            _findingExp = FindObjectOfType(typeof(FindingExpression)) as FindingExpression;
            Counter = 0;
        }

        protected override void OnPoolActivation()
        {
            Counter++;

            CharSprite.gameObject.SetActive(true);
            _bubbleSprite.gameObject.SetActive(true);

            if (_currentCharIndex == BubbleGuessingCore.CurrentCharSequence.Length) _currentCharIndex = 0;

            BubbleChar = BubbleGuessingCore.CurrentCharSequence[_currentCharIndex++][0];

            CharSprite.SetSprite(Utils.GetCharSpriteName(BubbleGuessingCore.CurrentCharLibraryNameSequence, BubbleChar));

            var fs = BubbleGuessingCore.CurrentLevel.SpawnSettings;
            _baseVelocity = rigidbody.velocity = Random.Range(fs.VelocityRange.From, fs.VelocityRange.To) * Vector3.left;
        }


        protected override void OnPoolDeactivation()
        {
            Counter--;
        }


        void FixedUpdate()
        {
            if (BubbleGuessingCore.CurrentStage == null) return;

            AddForce();
            FixVelocity();

            if (Time.fixedTime > _nextTime)
            {
                var fs = BubbleGuessingCore.CurrentLevel.SpawnSettings;
                _nextTime = Time.fixedTime + Random.Range(_forcePeriod.x, _forcePeriod.y);

                _force = new Vector3(0, Random.Range(fs.ForceRange.YFrom, fs.ForceRange.YTo), 0);
            }
        }
        
        private void AddForce()
        {
            rigidbody.AddForce(_force, ForceMode.Force);
        }

        private void FixVelocity()
        {
            if(Mathf.Abs(rigidbody.velocity.sqrMagnitude - _baseVelocity.sqrMagnitude) > 1)
            {
                rigidbody.velocity = _baseVelocity;
            }
        }

        private void TouchBubble()
        {
            var isRightChar = _findingExp.CheckChar(BubbleChar);

            if (!isRightChar)
            {
				
				
				
                //Core.AudioController.Play(Const.Sound.WrongTap);
                //return;
            }
			

			AudioController.Play("bubble_splash");
		
			
			
            _bubbleSprite.gameObject.SetActive(false);
            _bubbleBoomAnimation.gameObject.SetActive(true);
            _bubbleBoomAnimation.Play();
            _bubbleBoomAnimation.AnimationCompleted += (animator, clip) =>
            {
                _bubbleBoomAnimation.gameObject.SetActive(false);
                CharSprite.gameObject.SetActive(false);
                _bubbleBoomAnimation.SetFrame(0);
            };

            if (isRightChar) _pipe.UpdateLetters(); 

            _findingExp.CheckWin(isRightChar);
			
			
        }
    }
}
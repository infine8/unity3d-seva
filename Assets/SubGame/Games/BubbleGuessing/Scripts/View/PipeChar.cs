using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class PipeChar : EventView
    {
        [SerializeField] private float _scale = 1;
        [SerializeField] private Pipe _pipe;
        
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }


        [System.NonSerialized] public char? Char;

        private tk2dSprite _sprite;

        private bool IsVisible
        {
            get { return _sprite.color.Equals(new Color(1, 1, 1, 1)); }
            set { _sprite.color = new Color(1, 1, 1, value ? 1 : 0); }
        }

        protected override void Awake()
        {
            base.Awake();

            _sprite = GetComponent<tk2dSprite>();
        }

        void Update()
        {
            _sprite.scale = new Vector3(_scale, _scale, _scale);
        }

        public void UpdateChar()
        {
            if (Char.HasValue)
            {
                _sprite.SetSprite(Utils.GetCharSpriteName(BubbleGuessingCore.CurrentCharLibraryNameSequence, Char.Value));
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }

        public void OnInPipe()
        {
            UpdateChar(); 
            _pipe.PlayInAnimation();
        }

        public void OnOutPipe()
        {
            if (IsVisible) _pipe.PlayOutAnimation();
        }
    }
}
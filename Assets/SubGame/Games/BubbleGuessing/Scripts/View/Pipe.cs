using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class Pipe : EventView
    {
        private static readonly string IN_ANIM_NAME = "grab";
        private static readonly string OUT_ANIM_NAME = "out";

        public LWFAnimation LeftPipe;
        public LWFAnimation RightPipe;

        private enum PipeCharAnimation
        {
            PipeChar1,
            PipeChar12,
            PipeChar23,
            PipeChar3,
        }


        [SerializeField] private PipeChar _char0;
        [SerializeField] private PipeChar _char1;
        [SerializeField] private PipeChar _char2;

        
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        private readonly PipeChar[] _chars = new PipeChar[3]; //the last letter is current 

        public void Init()
        {
            UpdateLetters();

            //LeftPipe.OnInit = PlayOutAnimation;

            if (BubbleGuessingCore.CurrentStage.NoHelp) return;

            foreach (var c in _chars) c.UpdateChar();
        }

        public void UpdateLetters()
        {
            if (BubbleGuessingCore.CurrentStage == null) return;

            _char0.gameObject.SetActive(!BubbleGuessingCore.CurrentStage.NoHelp);
            _char1.gameObject.SetActive(!BubbleGuessingCore.CurrentStage.NoHelp);
            _char2.gameObject.SetActive(!BubbleGuessingCore.CurrentStage.NoHelp);

            if (BubbleGuessingCore.CurrentStage.NoHelp) return;
            
            UpdateLetterArray();

            _chars[0].Char = GetChar(0);
            _chars[1].Char = GetChar(1);
            _chars[2].Char = GetChar(2);
            
            StartCoroutine(PlaySound());
        }

        private char? GetChar(int letterIndex)
        {
            var charIndex = BubbleGuessingCore.CurrentFindingCharIndex + 2 - letterIndex;
            return BubbleGuessingCore.RealFindingWord.Length > charIndex ? (char?)BubbleGuessingCore.RealFindingWord[charIndex] : null;
        }

        private void UpdateLetterArray()
        {
            switch (BubbleGuessingCore.CurrentFindingCharIndex % 3)
            {
                case 0:
                    _chars[0] = _char0;
                    _chars[1] = _char1;
                    _chars[2] = _char2;

                    _char1.gameObject.animation.Play(PipeCharAnimation.PipeChar12.ToString());
                    _char2.gameObject.animation.Play(PipeCharAnimation.PipeChar23.ToString());
                    if (BubbleGuessingCore.CurrentFindingCharIndex > 0)
                    {
                        _char0.gameObject.animation.Play(PipeCharAnimation.PipeChar3.ToString());
                        _char0.gameObject.animation.PlayQueued(PipeCharAnimation.PipeChar1.ToString());
                        
                        PlayInAnimation();
                    }
                    else
                    {
                        _char0.gameObject.animation.Play(PipeCharAnimation.PipeChar1.ToString());
                    }
                    
                    break;
                case 1:
                    _chars[0] = _char2;
                    _chars[1] = _char0;
                    _chars[2] = _char1;

                    _char0.gameObject.animation.Play(PipeCharAnimation.PipeChar12.ToString());
                    _char1.gameObject.animation.Play(PipeCharAnimation.PipeChar23.ToString());
                    _char2.gameObject.animation.Play(PipeCharAnimation.PipeChar3.ToString());
                    _char2.gameObject.animation.PlayQueued(PipeCharAnimation.PipeChar1.ToString());

                    break;
                case 2:
                    _chars[0] = _char1;
                    _chars[1] = _char2;
                    _chars[2] = _char0;

                    _char2.gameObject.animation.Play(PipeCharAnimation.PipeChar12.ToString());
                    _char0.gameObject.animation.Play(PipeCharAnimation.PipeChar23.ToString());
                    _char1.gameObject.animation.Play(PipeCharAnimation.PipeChar3.ToString());
                    _char1.gameObject.animation.PlayQueued(PipeCharAnimation.PipeChar1.ToString());

                    break;
            }
        }

        private IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(0.5f);

            //Core.AudioController.Play(Const.Sound.CurrentCharComing); 
        }

        public void PlayOutAnimation()
        {
            LeftPipe.Play(OUT_ANIM_NAME);
        }

        public void PlayInAnimation()
        {
            RightPipe.Play(IN_ANIM_NAME);
        }
    }
}
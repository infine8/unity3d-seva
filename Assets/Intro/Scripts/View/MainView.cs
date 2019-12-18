using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.Intro.View
{
    public sealed class MainView : EventView
    {
        public enum ViewEventType
        {
            IntroIsFinished
        }

        [SerializeField] private tk2dSpriteAnimator _animator;
        [SerializeField] private AudioSource _introSound;


        protected override void Start()
        {
            base.Start();

            StartCoroutine(PlaySound());

            _animator.Play();
        }

        private IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(3.7f); 
            
            _introSound.Play();
            yield return new WaitForSeconds(3.0f);
            dispatcher.Dispatch(ViewEventType.IntroIsFinished);
        }

    }
}
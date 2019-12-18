using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid
{
    public sealed class TapAnimatedObject : EventView
    {
        [SerializeField] private List<string> _animationList = new List<string>();
		//[SerializeField] private List<string> _audioList = new List<string>();
		[SerializeField] private List<AudioSource> _audioList = new List<AudioSource>();

        private int _currentAnimationIndex;

        private LWFAnimation _lwf;

        private void LWFAnimationHasBeenInited(LWFAnimation lwf)
        {
            _lwf = lwf;
            _lwf.OnPlayFinished = UpdateCollider;
        }

        public void Init()
        {
            UpdateCollider();
            AddButtonComponent();
        }

        private void AddButtonComponent()
        {
            var btn = gameObject.AddComponent<tk2dButton>();
            btn.buttonDownSprite = btn.buttonUpSprite = btn.buttonPressedSprite = null;
            btn.targetScale = 1;
            btn.messageName = "OnTouch";
            btn.targetObject = gameObject;
        }

        private void UpdateCollider()
        {
            var c = GetComponent<BoxCollider>();
            if (c != null) Destroy(c);

            StartCoroutine(AddCollider(c != null));
        }

        private IEnumerator AddCollider(bool withDelay)
        {
            if (withDelay) yield return new WaitForSeconds(0.01f);

            var c = gameObject.AddComponent<BoxCollider>();
            c.size = new Vector3(c.size.x, c.size.y, 0.2f);
        }

        private void OnTouch()
        {
            if (_lwf.Movie.playing && _animationList.Count == 1)
            {
                _lwf.Stop();
                return;
            }

            _lwf.Play(_animationList[_currentAnimationIndex]);
			if (_currentAnimationIndex < _audioList.Count)
				_audioList[_currentAnimationIndex].Play();
				//AudioController.Play(_audioList[_currentAnimationIndex]);
            
            _currentAnimationIndex++;

            if (_currentAnimationIndex == _animationList.Count) _currentAnimationIndex = 0;
        }
    }
}
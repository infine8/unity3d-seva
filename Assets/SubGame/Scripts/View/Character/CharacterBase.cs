using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.View.Character
{
    public abstract class CharacterBase : EventView
    {
        public static readonly float SHOW_DOING_NOTHING_ANIMATION_DURATION = 30f;

        public enum ViewEventType
        {
            PlayDoingNothingAnimation
        }

        protected abstract string AnimationFileName { get; }

        protected override void Start()
        {
            base.Start();

            var lwf = gameObject.AddComponent<LWFAnimation>();
            lwf.isAlive = true;
            lwf.lwfName = AnimationFileName;
        }


        public void ResetDoingNothingCoroutine()
        {
            StopCoroutine("PlayDoingNothingAnimation");
            StartCoroutine("PlayDoingNothingAnimation");
        }

        private IEnumerator PlayDoingNothingAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(SHOW_DOING_NOTHING_ANIMATION_DURATION);

                dispatcher.Dispatch(ViewEventType.PlayDoingNothingAnimation);
            }
        }

    }
}
using System.Collections;
using UniKid.SubGame.Controller;
using UnityEngine;

namespace UniKid.SubGame.View.Character
{
    public class CharacterMediator : EventMediatorBase
    {
        public CharacterBase Character { get; private set; }

        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }

        [Inject]
        public TouchCharacterSignal TouchCharacterSignal { get; set; }

        private LWFAnimation Animation { get; set; }

        public override void OnRegister()
        {

            ChangeCharacterEmotionSignal.AddListener(ChangeCharacterEmotion);

            var btn = gameObject.AddComponent<tk2dButton>();
            btn.buttonDownSprite = btn.buttonUpSprite = btn.buttonPressedSprite = null;
            btn.targetScale = 1;
            btn.messageName = "OnTouch";
            btn.targetObject = gameObject;

            Character = GetComponent<CharacterBase>();

            Character.ResetDoingNothingCoroutine();

            base.OnRegister();
        }

        public override void OnRemove()
        {
            base.OnRemove();

            ChangeCharacterEmotionSignal.RemoveListener(ChangeCharacterEmotion);
        }

        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);

            Character.dispatcher.UpdateListener(toAdd, CharacterBase.ViewEventType.PlayDoingNothingAnimation, PlayDoingNothingAnimation);
        }

        public void ResetDoingNothingCoroutine()
        {
            Character.ResetDoingNothingCoroutine();            
        }


        private void LWFAnimationHasBeenInited(LWFAnimation lwf)
        {
            Animation = lwf;
        }

        private void ChangeCharacterEmotion(CharacterEmotionType emotionType)
        {
            Animation.Play(emotionType.ToString());
        }

        private void OnTouch()
        {
            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Incompr);
            TouchCharacterSignal.Dispatch(Character);

            ResetDoingNothingCoroutine();
        }

        private void PlayDoingNothingAnimation()
        {
            ChangeCharacterEmotion(CharacterEmotionType.Incompr);            
        }

    }
}
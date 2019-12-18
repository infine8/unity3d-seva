using UniKid.SubGame.View.Character;
using strange.extensions.mediation.impl;

namespace UniKid
{
    public abstract class EventMediatorBase : EventMediator
    {
        public CharacterBase Character { get; private set; }

        public override void OnRegister()
        {
            UpdateListeners(true);

            Character = transform.root.GetComponentInChildren<CharacterBase>();
        }

        public override void OnRemove()
        {
            UpdateListeners(false);
        }

        protected virtual void UpdateListeners(bool toAdd) { }
        

        public void ResetDoingNothingCoroutine()
        {
            if (Character == null) return;

            Character.ResetDoingNothingCoroutine();
        }
    }
}
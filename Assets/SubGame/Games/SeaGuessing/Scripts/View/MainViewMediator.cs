using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.SeaGuessing.Controller;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;

namespace UniKid.SubGame.Games.SeaGuessing.View
{
    public sealed class MainViewMediator : SubGameMainViewMediatorBase<SeaGuessingSettings, SeaGuessingUserData, SeaGuessinggLevel, SeaGuessingStage>
    {
        [Inject]
        public MainView MainView { get; set; }

        [Inject]
        public SeaGuessingCore SeaGuessingCore { get; set; }

        [Inject]
        public ChangeCharacterEmotionSignal ChangeCharacterEmotionSignal { get; set; }


        public override SubGameMainViewBase SubGameViewBase { get { return MainView; } }

        protected override SubGameCoreBase<SeaGuessingSettings, SeaGuessingUserData, SeaGuessinggLevel, SeaGuessingStage> SubGameCore
        {
            get { return SeaGuessingCore; }
        }

        private BubbleCharGroup _lastSpawnedObject;

        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);
            
            dispatcher.UpdateListener(toAdd, SeaGuessingEventType.CharIsFound, CharIsFound);
            dispatcher.UpdateListener(toAdd, SeaGuessingEventType.CharIsNotFound, CharIsNotFound);
        }

        private void CharIsFound()
        {

            ResetDoingNothingCoroutine();
            
            StagePassedSignal.Dispatch();

            MainView.DespawnLastGroup();

            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Delight);
        }

        private void CharIsNotFound()
        {
            ResetDoingNothingCoroutine();

            ChangeCharacterEmotionSignal.Dispatch(CharacterEmotionType.Denial);
        }


    }
}

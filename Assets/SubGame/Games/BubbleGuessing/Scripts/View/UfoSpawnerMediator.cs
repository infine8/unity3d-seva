using UniKid.SubGame.Controller;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class UfoSpawnerMediator : EventMediatorBase
    {
        [Inject]
        public UfoSpawner UfoSpawner { get; set; }
        
        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            StagePassedSignal.AddListener(UfoSpawner.StopSpawn);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            StagePassedSignal.RemoveListener(UfoSpawner.StopSpawn);
        }

    }
}
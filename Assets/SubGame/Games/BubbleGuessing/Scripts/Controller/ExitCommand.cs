using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.BubbleGuessing.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.BubbleGuessing.Controller
{
    public sealed class ExitCommand : SubGameExitCommandBase<BubbleGuessingSettings, BubbleGuessingUserData, BubbleGuessingLevel, BubbleGuessingStage>
    {
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        protected override SubGameCoreBase<BubbleGuessingSettings, BubbleGuessingUserData, BubbleGuessingLevel, BubbleGuessingStage> SubGameCore
        {
            get { return BubbleGuessingCore; }
        }
		
		public override void Execute()
        {
            base.Execute();
        }
    }
}
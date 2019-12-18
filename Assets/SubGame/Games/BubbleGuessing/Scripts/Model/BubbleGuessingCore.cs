using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.BubbleGuessing.Model
{
    public sealed class BubbleGuessingCore : SubGameCoreBase<BubbleGuessingSettings, BubbleGuessingUserData, BubbleGuessingLevel, BubbleGuessingStage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.BubbleGuessing; }
        }

        public BubbleGuessingStage.ExpressionElement Expression { get { return CurrentStage != null ? CurrentStage.Expression : null; } }
        public int CurrentFindingCharIndex;
        public string[] CurrentCharSequence;
        public string CurrentCharLibraryNameSequence;
        public string RealFindingWord;

        public bool UseTemplate { get { return !string.IsNullOrEmpty(CurrentStage.Expression.Template); } }
		
    }
}
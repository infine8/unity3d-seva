using System.Collections;
using UniKid.SubGame.Games.BubbleGuessing.Controller;
using UniKid.SubGame.Games.BubbleGuessing.Model;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class FindingExpressionMediator : SubGameMainViewMediatorBase<BubbleGuessingSettings, BubbleGuessingUserData, BubbleGuessingLevel, BubbleGuessingStage>
    {
        [Inject]
        public FindingExpression FindingExpression { get; set; }

        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        
        public override SubGameMainViewBase SubGameViewBase { get { return FindingExpression; } }

        protected override SubGameCoreBase<BubbleGuessingSettings, BubbleGuessingUserData, BubbleGuessingLevel, BubbleGuessingStage> SubGameCore
        {
            get { return BubbleGuessingCore; }
        }
        
        protected override void UpdateListeners(bool toAdd)
        {
            base.UpdateListeners(toAdd);

            FindingExpression.dispatcher.UpdateListener(toAdd, FindingExpression.EventType.ExpressionFound, () => StagePassedSignal.Dispatch());
        }
    }
}
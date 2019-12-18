using UniKid.SubGame.Games.BubbleGuessing.Model;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class PipeMediator : EventMediatorBase
    {
        [Inject]
        public BubbleGuessingCore BubbleGuessingCore { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
        }
    }
}
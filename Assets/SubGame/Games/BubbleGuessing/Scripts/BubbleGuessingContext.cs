using UniKid.SubGame.Games.BubbleGuessing.Controller;
using UniKid.SubGame.Games.BubbleGuessing.Model;
using UniKid.SubGame.Games.BubbleGuessing.View;
using UnityEngine;

namespace UniKid.SubGame.Games.BubbleGuessing
{
    public sealed class BubbleGuessingContext : SubGameContextBase<BubbleGuessingCore, FindingExpression, FindingExpressionMediator, StartCommand, ExitCommand>
    {
        public BubbleGuessingContext()
        {

        }

        public BubbleGuessingContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<UfoChar>().To<UfoCharMediator>();
            mediationBinder.Bind<Pipe>().To<PipeMediator>();
            mediationBinder.Bind<UfoSpawner>().To<UfoSpawnerMediator>();
        }
    }
}
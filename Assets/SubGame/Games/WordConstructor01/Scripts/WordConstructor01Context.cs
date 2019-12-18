using UniKid.SubGame.Games.WordConstructor01.Controller;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UniKid.SubGame.Games.WordConstructor01.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor01
{
    public sealed class WordConstructor01Context : SubGameContextBase<WordConstructor01Core, MainView, MainViewMediator, StartCommand, ExitCommand>
    {
        public WordConstructor01Context()
        {

        }

        public WordConstructor01Context(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<CharIsFoundSignal>().ToSingleton();
            injectionBinder.Bind<TargetBoardIsInitedSignal>().ToSingleton();

            mediationBinder.Bind<TargetBoard>().To<TargetBoardMediator>();
            mediationBinder.Bind<SourceBoard>().To<SourceBoardMediator>();
            mediationBinder.Bind<SourceBoardField>().To<SourceBoardFieldMediator>();
            mediationBinder.Bind<TargetBoardField>().To<TargetBoardFieldMediator>();
        }
    }
}
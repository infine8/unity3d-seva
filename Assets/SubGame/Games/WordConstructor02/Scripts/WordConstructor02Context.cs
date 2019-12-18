using UniKid.SubGame.Games.WordConstructor02.Controller;
using UniKid.SubGame.Games.WordConstructor02.Model;
using UniKid.SubGame.Games.WordConstructor02.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.WordConstructor02
{
    public sealed class WordConstructor02Context : SubGameContextBase<WordConstructor02Core, MainView, MainViewMediator, StartCommand, ExitCommand>
    {
        public WordConstructor02Context()
        {

        }

        public WordConstructor02Context(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<NewItemDetectedSignal>().ToSingleton();

            mediationBinder.Bind<Scroll>().To<ScrollMediator>();
            mediationBinder.Bind<ScrollField>().To<ScrollFieldMediator>();
        }
    }
}
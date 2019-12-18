using UniKid.SubGame.Games.HideSeekCards.Controller;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.Games.HideSeekCards.View;
using UnityEngine;

namespace UniKid.SubGame.Games.HideSeekCards
{
    public sealed class HideSeekCardsContext : SubGameContextBase<HideSeekCardsCore, MainView, MainViewMediator, StartCommand, ExitCommand>
    {
        public HideSeekCardsContext()
        {

        }

        public HideSeekCardsContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<Card>().To<CardMediator>();
            mediationBinder.Bind<Field>().To<FieldMediator>();

            injectionBinder.Bind<CardOpenedSignal>().ToSingleton();
            injectionBinder.Bind<CardSequenceLooseSignal>().ToSingleton();
            injectionBinder.Bind<CardSequenceFoundSignal>().ToSingleton();
        }
    }
}


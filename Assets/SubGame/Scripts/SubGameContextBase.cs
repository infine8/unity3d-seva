using UniKid.MainMenu.Controller;
using UniKid.SubGame.Controller;
using UniKid.SubGame.View.Character;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace UniKid.SubGame
{
    public abstract class SubGameContextBase<TCore, TMainView, TMainViewMediator, TStartCommand, TExitCommand> : MVCSContext
    {
        protected SubGameContextBase()
        {

        }

        protected SubGameContextBase(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {
             
        }

        protected override void mapBindings()
        {
            injectionBinder.Bind<ChangeCharacterEmotionSignal>().ToSingleton();
            injectionBinder.Bind<SubGameIsPassedSignal>().ToSingleton();
            injectionBinder.Bind<MoveNextLevelSignal>().ToSingleton();
            injectionBinder.Bind<TouchCharacterSignal>().ToSingleton();
            injectionBinder.Bind<StagePassedSignal>().ToSingleton();

            injectionBinder.Bind<TCore>().To<TCore>().ToSingleton();

            commandBinder.Bind(ContextEvent.START).To<TStartCommand>().Once();
            commandBinder.Bind(SubGameEventType.Exit).To<TExitCommand>().Once();
            commandBinder.Bind(SubGameEventType.CreateDbSubGame).To<CreateDbSubGameCommand>().Once();

            mediationBinder.Bind<TMainView>().To<TMainViewMediator>();

            mediationBinder.Bind<SevaCharacter>().To<CharacterMediator>();
        }
    }
}
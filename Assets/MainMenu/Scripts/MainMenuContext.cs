using UniKid.Core.Service;
using UniKid.MainMenu.Controller;
using UniKid.MainMenu.View;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace UniKid.MainMenu
{
    public class MainMenuContext : MVCSContext
    {
        public MainMenuContext() : base()
        {

        }

        public MainMenuContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            injectionBinder.Bind<IDbService>().To<ParseDbService>().ToSingleton();

            commandBinder.Bind(ContextEvent.START).To<StartCommand>().Once();
            commandBinder.Bind(MainMenuEventType.LoadSubGame).To<LoadSubGameCommand>();
            commandBinder.Bind(MainMenuEventType.RemoveContext).To<RemoveContextCommand>();

            commandBinder.Bind(MainMenuEventType.AuthenticateUser).To<AuthenticateDbUserCommand>();
            commandBinder.Bind(MainMenuEventType.CreateDbUser).To<CreateDbUserCommand>();
            commandBinder.Bind(MainMenuEventType.CreateDbProfile).To<CreateDbProfileCommand>();
            commandBinder.Bind(MainMenuEventType.CreateDbSubGameLevelTag).To<CreateDbSubGameLevelTagCommand>();
            commandBinder.Bind(MainMenuEventType.CreateDbSubGame).To<CreateDbSubGameCommand>();

            commandBinder.Bind(MainMenuEventType.UpdateDbSubGameLevelTag).To<UpdateDbSubGameLevelTagCommand>();
            commandBinder.Bind(MainMenuEventType.CreateInitDbInfo).To<CreateInitDbInfoCommand>();

            mediationBinder.Bind<MainView>().To<MainViewMediator>();
        }
    }
}
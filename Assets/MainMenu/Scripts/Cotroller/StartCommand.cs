using UniKid.Core;
using strange.extensions.command.impl;

namespace UniKid.MainMenu.Controller
{
    public sealed class StartCommand : EventCommand
    {
        public override void Execute()
        {
            dispatcher.Dispatch(MainMenuEventType.CreateInitDbInfo);
        }
    }
}

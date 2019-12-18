using UniKid.Core.Service;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.Core.Controller
{
    public sealed class DbUpdateUserDataCommand : EventCommand
    {
        [Inject]
        public IDbService DbService { get; set; }


        public override void Execute()
        {
            if (!NetworkingCore.IsThereConnection) return;

            DbService.dispatcher.AddListener(DbService.UserDataHasBeenSavedCommandName, OnComplete);

            DbService.SaveUserData();

            Retain();
        }

        private void OnComplete(IEvent evt)
        {
            DbService.dispatcher.RemoveListener(DbService.UserDataHasBeenSavedCommandName, OnComplete);
            
            Release();
        }
    }
}
using UniKid.Core;
using UniKid.Core.Service;
using UniKid.Core.Service.DbServiceType;
using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Linq;

namespace UniKid.MainMenu.Controller
{
    public sealed class CreateInitDbInfoCommand : EventCommand
    {
        [Inject]
        public IDbService DbService { get; set; }

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }

        public override void Execute()
        {
            CoreContext.StartCoroutine(CreateDbInitData(NetworkingCore.IsThereConnection ? 0 : NetworkingCore.PING_DELTA_TIME + 1));
        }

        private IEnumerator CreateDbInitData(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (NetworkingCore.IsThereConnection) CreateDbInitData();
        }

        private void CreateDbInitData()
        {
            DbService.dispatcher.AddListener(((IDbUser)DbService).ItemHasBeenCreatedCommandName, UserHasBeenCreated);

            if (string.IsNullOrEmpty(CoreContext.UserData.Common.User.DbId))
            {
                dispatcher.Dispatch(MainMenuEventType.CreateDbUser);
            }
            else
            {
                UserHasBeenCreated();
            }
        }

        private void UserHasBeenCreated()
        {

            DbService.dispatcher.RemoveListener(((IDbUser)DbService).ItemHasBeenCreatedCommandName, UserHasBeenCreated);

            DbService.dispatcher.AddListener(((IDbProfile)DbService).ItemHasBeenCreatedCommandName, ProfileHasBeenCreated);

            if (string.IsNullOrEmpty(CoreContext.UserData.Common.User.DbId)) CoreContext.UserData.Common.User.DbId = ((IDbUser)DbService).Id;

            if (string.IsNullOrEmpty(CoreContext.UserData.CurrentProfile.DbId))
            {
                dispatcher.Dispatch(MainMenuEventType.CreateDbProfile, CoreContext.UserData.CurrentProfile);
            }
            else
            {
                ProfileHasBeenCreated();
            }

        }

        private void ProfileHasBeenCreated()
        {
            DbService.dispatcher.RemoveListener(((IDbProfile)DbService).ItemHasBeenCreatedCommandName, ProfileHasBeenCreated);

            if (string.IsNullOrEmpty(CoreContext.UserData.CurrentProfile.DbId)) CoreContext.UserData.CurrentProfile.DbId = ((IDbProfile)DbService).Id;

            CreateDbSubGameLevelTag();
        }

        private void CreateDbSubGameLevelTag()
        {
            DbService.dispatcher.RemoveListener(((IDbSubGameLevelTag)DbService).ItemHasBeenCreatedCommandName, CreateDbSubGameLevelTag);

            var tag = CoreContext.UserData.CurrentProfile.SubGameLevelTagArray.FirstOrDefault(x => string.IsNullOrEmpty(x.DbId));

            if (tag != null)
            {
                dispatcher.Dispatch(MainMenuEventType.CreateDbSubGameLevelTag, tag);

                DbService.dispatcher.AddListener(((IDbSubGameLevelTag)DbService).ItemHasBeenCreatedCommandName, CreateDbSubGameLevelTag);
            }
        }
    }
}
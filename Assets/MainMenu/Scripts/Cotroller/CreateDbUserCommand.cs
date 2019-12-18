using System;
using UniKid.Core;
using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Service.DbServiceType;

namespace UniKid.MainMenu.Controller
{
    public class CreateDbUserCommand : DbCreateItemCommand<IDbUser>
    {
        protected override void PrepareCommand()
        {
            DbService.UserLogin = DbService.Password = Guid.NewGuid().ToString("N");

        }

        protected override void OnCommandExecuted(IDbUser obj, string error)
        {
            CoreContext.UserData.Common.User.DbId = obj.Id;
            CoreContext.UserData.Common.User.Login = obj.UserLogin;
            CoreContext.UserData.Common.User.LastSyncDate = DateTime.UtcNow;
        }
    }
}
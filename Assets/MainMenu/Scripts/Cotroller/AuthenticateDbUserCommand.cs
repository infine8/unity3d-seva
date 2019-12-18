using UniKid.Core;
using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Service.DbServiceType;

namespace UniKid.MainMenu.Controller
{
    public sealed class AuthenticateDbUserCommand : DbCommandBase<IDbUser>
    {
        protected override string DbCommandName
        {
            get { return ((IDbUser)DbService).UserHasBeenAuthenticatedCommandName; }
        }

        protected override void PrepareCommand()
        {
            ((IDbUser) DbService).UserLogin = ((IDbUser) DbService).Password = CoreContext.UserData.Common.User.Login;
        }

        public override void Execute()
        {
            base.Execute();

            DbService.AuthenticateUser();
        }
    }
}
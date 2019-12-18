using System.Collections.Generic;
using UniKid.Core.Model;

namespace UniKid.Core.Service.DbServiceType
{
    public interface IDbUser : IDbType<IDbUser>
    {
        string UserHasBeenAuthenticatedCommandName { get; set; }

        List<IDbProfile> DbProfileList { get; set; }

        string UserLogin { get; set; }
        string Password { get; set; }

        void AuthenticateUser();
    }
}
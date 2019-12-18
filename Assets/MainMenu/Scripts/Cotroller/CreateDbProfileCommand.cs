using System;
using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Model;
using UniKid.Core.Service.DbServiceType;

namespace UniKid.MainMenu.Controller
{
    public sealed class CreateDbProfileCommand : DbCreateItemCommand<IDbProfile>
    {
        private Profile _profile;

        protected override void PrepareCommand()
        {
            _profile = evt.data as Profile;

            if (_profile == null) throw new Exception("CreateDbProfileCommand: profile arg is null");

            ((IDbProfile) DbService).Name = _profile.Name;
            ((IDbProfile) DbService).DbUserId = _profile.User.DbId;
        }

        protected override void OnCommandExecuted(IDbProfile obj, string error)
        {
            _profile.LastSyncDate = DateTime.UtcNow;
            _profile.DbId = obj.Id;
        }
    }
}
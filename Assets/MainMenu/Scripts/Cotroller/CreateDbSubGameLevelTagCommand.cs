using System;
using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Model;
using UniKid.Core.Service.DbServiceType;
using UniKid.SubGame.Model;

namespace UniKid.MainMenu.Controller
{
    public sealed class CreateDbSubGameLevelTagCommand : DbCreateItemCommand<IDbSubGameLevelTag>
    {
        private SubGameLevelTagUserData _subGameLevelTag;

        protected override void PrepareCommand()
        {
            _subGameLevelTag = evt.data as SubGameLevelTagUserData;

            if (_subGameLevelTag == null) throw new Exception("CreateDbSubGameTagCommand: subgametag arg is null");

            ((IDbSubGameLevelTag)DbService).Name = _subGameLevelTag.Name;
            ((IDbSubGameLevelTag)DbService).DbProfileId = _subGameLevelTag.CurrentProfile.DbId;
            ((IDbSubGameLevelTag)DbService).IsEnabled = _subGameLevelTag.IsEnabled;
            ((IDbSubGameLevelTag)DbService).Priority = _subGameLevelTag.Priority;
            ((IDbSubGameLevelTag)DbService).SetSpentTimeJson(_subGameLevelTag.SpentTimeArray);
        }

        protected override void OnCommandExecuted(IDbSubGameLevelTag obj, string error)
        {
            _subGameLevelTag.LastSyncDate = DateTime.UtcNow;
            _subGameLevelTag.DbId = obj.Id;
        }
    }
}
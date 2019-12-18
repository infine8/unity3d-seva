using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Service.DbServiceType;
using UniKid.SubGame.Model;
using UnityEngine;
using System.Collections;

namespace UniKid.MainMenu.Controller
{
    public sealed class UpdateDbSubGameLevelTagCommand : DbUpdateItemCommand<IDbSubGameLevelTag>
    {
        private SubGameLevelTagUserData _subGameLevelTag;

        protected override void PrepareCommand()
        {
            _subGameLevelTag = evt.data as SubGameLevelTagUserData;

            ((IDbSubGameLevelTag) DbService).Id = _subGameLevelTag.DbId;
            ((IDbSubGameLevelTag) DbService).SetSpentTimeJson(_subGameLevelTag.SpentTimeArray);
        }

        protected override void OnCommandExecuted(IDbSubGameLevelTag obj, string error)
        {
            Debug.Log(_subGameLevelTag.DbId);
        }
    }
}
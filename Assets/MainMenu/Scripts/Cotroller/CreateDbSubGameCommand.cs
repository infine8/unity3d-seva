using System;
using UniKid.Core.Controller.DbCommand;
using UniKid.Core.Service.DbServiceType;
using UniKid.SubGame.Model;

namespace UniKid.MainMenu.Controller
{
    public sealed class CreateDbSubGameCommand : DbCreateItemCommand<IDbSubGame>
    {
        private SubGameUserDataBase _subGame;

        protected override void PrepareCommand()
        {
            _subGame = evt.data as SubGameUserDataBase;

            if (_subGame == null) throw new Exception("CreateDbSubGameCommand: subgame arg is null");

            ((IDbSubGame) DbService).Name = _subGame.Name.ToString();
            ((IDbSubGame) DbService).DbProfileId = _subGame.CurrentProfile.DbId;
            ((IDbSubGame) DbService).IsEnabled = _subGame.IsEnabled;
        }

        protected override void OnCommandExecuted(IDbSubGame obj, string error)
        {
            _subGame.LastSyncDate = Utils.UtcNowAO;
            _subGame.DbId = obj.Id;
        }
    }
}
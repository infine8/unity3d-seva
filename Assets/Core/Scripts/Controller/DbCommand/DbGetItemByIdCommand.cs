using UniKid.Core.Service.DbServiceType;

namespace UniKid.Core.Controller.DbCommand
{
    public class DbGetItemByIdCommand<T> : DbCommandBase<T> where T : IDbType<T>
    {
        protected override string DbCommandName
        {
            get { return ((T)DbService).ItemHasBeenGottenCommandName; }
        }

        public override void Execute()
        {
            base.Execute();

            ((T)DbService).GetById(evt.data.ToString());
        }
    }
}
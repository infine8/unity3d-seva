using UniKid.Core.Service.DbServiceType;

namespace UniKid.Core.Controller.DbCommand
{
    public class DbDeleteItemCommand<T> : DbCommandBase<T> where T : IDbType<T>
    {
        protected override string DbCommandName
        {
            get { return ((T)DbService).ItemHasBeenDeletedCommandName; }
        }

        public override void Execute()
        {
            base.Execute();

            ((T)DbService).Delete();
        }
    }
}
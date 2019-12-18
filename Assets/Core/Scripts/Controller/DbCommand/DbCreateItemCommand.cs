using UniKid.Core.Service.DbServiceType;

namespace UniKid.Core.Controller.DbCommand
{
    public class DbCreateItemCommand<T> : DbCommandBase<T> where T : IDbType<T>
    {
        protected override string DbCommandName
        {
            get { return ((T) DbService).ItemHasBeenCreatedCommandName; }
        }

        public override void Execute()
        {
            base.Execute();

            ((T)DbService).Create();
        }
    }
}
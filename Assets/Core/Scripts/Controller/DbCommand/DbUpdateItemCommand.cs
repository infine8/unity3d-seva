using UniKid.Core.Service.DbServiceType;

namespace UniKid.Core.Controller.DbCommand
{
    public class DbUpdateItemCommand<T> : DbCommandBase<T> where T : IDbType<T>
    {
        protected override string DbCommandName
        {
            get { return ((T)DbService).ItemHasBeenUpdatedCommandName; }
        }

        public override void Execute()
        {
            base.Execute();

            ((T)DbService).Update();
        }
    }
}
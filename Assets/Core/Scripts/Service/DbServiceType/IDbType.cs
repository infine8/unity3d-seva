namespace UniKid.Core.Service.DbServiceType
{
    public interface IDbType<T>
    {
        string ItemHasBeenCreatedCommandName { get; set; }
        string ItemHasBeenUpdatedCommandName { get; set; }
        string ItemHasBeenDeletedCommandName { get; set; }
        string ItemHasBeenGottenCommandName { get; set; }

        string Id { get; set; }

        void Create();
        void GetById(string id);
        void Update();
        void Delete();
    }

}

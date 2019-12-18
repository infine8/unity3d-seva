namespace UniKid.Core.Service.DbServiceType
{
    public interface IDbSubGame : IDbType<IDbSubGame>
    {
        string DbProfileId { get; set; }

        string Name { get; set; }

        bool IsEnabled { get; set; }
    }
}
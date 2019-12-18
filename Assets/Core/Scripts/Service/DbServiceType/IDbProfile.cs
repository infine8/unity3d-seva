using System.Collections.Generic;

namespace UniKid.Core.Service.DbServiceType
{
    public interface IDbProfile : IDbType<IDbProfile>
    {
        string DbUserId { get; set; }
        string Name { get; set; }

        List<IDbSubGame> DbSubGameList { get; set; }
        List<IDbSubGameLevelTag> DbSubGameLevelTagList { get; set; }
    }
}
using System.Collections.Generic;
using UniKid.SubGame.Model;
using System.Collections;

namespace UniKid.Core.Service.DbServiceType
{
    public interface IDbSubGameLevelTag : IDbType<IDbSubGameLevelTag>
    {
        string DbProfileId { get; set; }

        string Name { get; set; }

        bool IsEnabled { get; set; }

        int Priority { get; set; }

        List<string> SpentTimeList { get; set; }

        void SetSpentTimeJson(SpentTimeItem[] spentTimeList);

        List<SpentTimeItem> GetSpentTimeList();
    }
}
using System.Collections.Generic;
using UniKid.Core.Service.DbServiceType;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.Core.Service
{
    public interface IDbService : IDbUser, IDbProfile, IDbSubGame, IDbSubGameLevelTag
    {
        string UserDataHasBeenSavedCommandName { get; }

        IEventDispatcher dispatcher { get; set; }
        IEventDispatcher crossContextDispatcher { get; set; }

        IDbUser DbUser { get; set; }

        void SaveUserData();
    }
}

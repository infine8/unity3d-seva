using System;
using System.Collections.Generic;
using System.Linq;
using UniKid.Core.Service.DbServiceType;
using UniKid.SubGame.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Parse;

namespace UniKid.Core.Service
{
    public class ParseDbService : IDbService
    {
        private static readonly string PARSE_APP_ID = "vQwI7Ln0XUFWYABmBhcuU6tQgJR7UHc1mP0JncWG";
        private static readonly string PARSE_DOTNET_KEY = "PHDBI9FGfYm8K12Ee7I5Pghdq4DTi4Ep9clflsjb";

        private static readonly string PROFILE_PARSE_CLASS_NAME = "Profile";
        private static readonly string SUBGAME_PARSE_CLASS_NAME = "SubGame";
        private static readonly string SUBGAMELEVELTAG_PARSE_CLASS_NAME = "SubGameLevelTag";

        private static readonly string UPDATE_USERDATA_FUNC_NAME = "UpdateData";

        [Inject]
        public IEventDispatcher dispatcher { get; set; }

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }

        public IDbUser DbUser { get; set; }

        string IDbUser.UserLogin { get; set; }
        string IDbUser.Password { get; set; }
        public List<IDbProfile> DbProfileList { get; set; }

        string IDbProfile.DbUserId { get; set; }
        string IDbProfile.Name { get; set; }
        public List<IDbSubGame> DbSubGameList { get; set; }
        public List<IDbSubGameLevelTag> DbSubGameLevelTagList { get; set; }

        string IDbSubGame.DbProfileId { get; set; }
        string IDbSubGame.Name { get; set; }
        bool IDbSubGame.IsEnabled { get; set; }

        string IDbSubGameLevelTag.DbProfileId { get; set; }
        string IDbSubGameLevelTag.Name { get; set; }
        bool IDbSubGameLevelTag.IsEnabled { get; set; }
        int IDbSubGameLevelTag.Priority { get; set; }
        public ArrayList SpentTimeArray { get; set; }
        public List<string> SpentTimeList { get; set; }

        #region Command Name Properties

        public string UserDataHasBeenSavedCommandName { get { return "UserDataHasBeenSavedCommand"; } }

        string IDbType<IDbUser>.ItemHasBeenCreatedCommandName { get { return "DbUserHasBeenCreatedCommand"; } set { } }
        string IDbType<IDbUser>.ItemHasBeenUpdatedCommandName { get { return "DbUserHasBeenUpdatedCommand"; } set { } }
        string IDbType<IDbUser>.ItemHasBeenDeletedCommandName { get { return "DbUserHasBeenDeletedCommand"; } set { } }
        string IDbType<IDbUser>.ItemHasBeenGottenCommandName { get { return "DbUserHasBeenGottenCommand"; } set { } }
        string IDbUser.UserHasBeenAuthenticatedCommandName { get { return "UserHasBeenAuthenticated"; } set { } }

        string IDbType<IDbProfile>.ItemHasBeenUpdatedCommandName { get { return "DbProfileHasBeenUpdatedCommand"; } set { } }
        string IDbType<IDbProfile>.ItemHasBeenDeletedCommandName { get { return "DbProfileHasBeenDeletedCommand"; } set { } }
        string IDbType<IDbProfile>.ItemHasBeenCreatedCommandName { get { return "DbProfileHasBeenCreatedCommand"; } set { } }
        string IDbType<IDbProfile>.ItemHasBeenGottenCommandName { get { return "DbProfileHasBeenGottenCommand"; } set { } }

        string IDbType<IDbSubGame>.ItemHasBeenUpdatedCommandName { get { return "DbSubGameHasBeenUpdatedCommand"; } set { } }
        string IDbType<IDbSubGame>.ItemHasBeenDeletedCommandName { get { return "DbSubGameHasBeenDeletedCommand"; } set { } }
        string IDbType<IDbSubGame>.ItemHasBeenCreatedCommandName { get { return "DbSubGameHasBeenCreatedCommand"; } set { } }
        string IDbType<IDbSubGame>.ItemHasBeenGottenCommandName { get { return "DbSubGameHasBeenGottenCommand"; } set { } }

        string IDbType<IDbSubGameLevelTag>.ItemHasBeenUpdatedCommandName { get { return "DbSubGameLevelTagHasBeenUpdatedCommand"; } set { } }
        string IDbType<IDbSubGameLevelTag>.ItemHasBeenDeletedCommandName { get { return "DbSubGameLevelTagHasBeenDeletedCommand"; } set { } }
        string IDbType<IDbSubGameLevelTag>.ItemHasBeenCreatedCommandName { get { return "DbSubGameLevelTagHasBeenCreatedCommand"; } set { } }
        string IDbType<IDbSubGameLevelTag>.ItemHasBeenGottenCommandName { get { return "DbSubGameLevelTagHasBeenGottenCommand"; } set { } }

        #endregion

        string IDbType<IDbProfile>.Id { get; set; }
        string IDbType<IDbUser>.Id { get; set; }
        string IDbType<IDbSubGame>.Id { get; set; }
        string IDbType<IDbSubGameLevelTag>.Id { get; set; }

        private static bool _isInited = false;

        [PostConstruct]
        public void Init()
        {

            DbProfileList = new List<IDbProfile>();
            DbSubGameList = new List<IDbSubGame>();
            DbSubGameLevelTagList = new List<IDbSubGameLevelTag>();

            if (_isInited) return;

            var initObj = CoreContext.ContextView.gameObject.AddComponent<ParseInitializeBehaviour>();
            initObj.applicationID = PARSE_APP_ID;
            initObj.dotnetKey = PARSE_DOTNET_KEY;
                
            ParseClient.Initialize(PARSE_APP_ID, PARSE_DOTNET_KEY);

            _isInited = true;
        }

        #region IDbUser

        void IDbType<IDbUser>.Create()
        {
            var user = new ParseUser { Username = ((IDbUser)this).UserLogin, Password = ((IDbUser)this).Password };

            var callback = new Action(() => { ((IDbUser)this).Id = user.ObjectId; DbUser = this; });

            CoreContext.StartCoroutine(RunRequest<IDbUser>(user.SignUpAsync(), ((IDbUser)this).ItemHasBeenCreatedCommandName, callback));
        }

        void IDbType<IDbUser>.Update()
        {
            UpdateItemRequest<IDbUser>();
        }

        void IDbType<IDbUser>.Delete()
        {
            DeleteItemRequest<IDbUser>();
        }

        void IDbType<IDbUser>.GetById(string id)
        {
            ((IDbUser)this).Id = id;
            var callback = new Action(() => DbUser = this);

            GetItemRequest<IDbUser>(callback);
        }

        public void AuthenticateUser()
        {
            if (string.IsNullOrEmpty(((IDbUser)this).UserLogin)) throw new Exception("There is no saved user credentials");

            //var authUserRequest = ParseClass.Authenticate(((IDbUser) this).UserLogin, ((IDbUser) this).Password);

            //_root.StartCoroutine(RunRequest<IDbUser>(authUserRequest, ((IDbUser) this).UserHasBeenAuthenticatedCommandName));
        }

        #endregion

        #region IDbProfile

        void IDbType<IDbProfile>.GetById(string id)
        {
            ((IDbProfile)this).Id = id;
            var item = DbProfileList.Find(x => x.Id == ((IDbProfile)this).Id);
            if (item != null) DbProfileList.Remove(item);

            var callback = new Action(() => DbProfileList.Add(this));

            GetItemRequest<IDbUser>(callback);
        }

        void IDbType<IDbProfile>.Update()
        {
            UpdateItemRequest<IDbProfile>();
        }

        void IDbType<IDbProfile>.Delete()
        {
            DeleteItemRequest<IDbProfile>();
        }

        void IDbType<IDbProfile>.Create()
        {
            var callback = new Action(() => DbProfileList.Add(this));

            CreateItemRequest<IDbProfile>(callback);
        }

        #endregion

        #region IDbSubGameLevelTag

        void IDbType<IDbSubGameLevelTag>.GetById(string id)
        {
            ((IDbSubGameLevelTag)this).Id = id;
            var item = DbSubGameLevelTagList.Find(x => x.Id == ((IDbSubGameLevelTag)this).Id);
            if (item != null) DbSubGameLevelTagList.Remove(item);

            var callback = new Action(() => DbSubGameLevelTagList.Add(this));

            GetItemRequest<IDbSubGameLevelTag>(callback);
        }

        void IDbType<IDbSubGameLevelTag>.Update()
        {
            UpdateItemRequest<IDbSubGameLevelTag>();
        }

        void IDbType<IDbSubGameLevelTag>.Delete()
        {
            DeleteItemRequest<IDbSubGameLevelTag>();
        }

        void IDbType<IDbSubGameLevelTag>.Create()
        {
            var callback = new Action(() => DbSubGameList.Add(this));

            CreateItemRequest<IDbSubGameLevelTag>(callback);
        }

        public void SetSpentTimeJson(SpentTimeItem[] spentTimeList)
        {
            ((IDbSubGameLevelTag)this).SpentTimeList = new List<string>();

            foreach (var stItem in spentTimeList)
            {
                var json = GetSpentTimeJson(stItem);

                if (!string.IsNullOrEmpty(json)) ((IDbSubGameLevelTag)this).SpentTimeList.Add(json);
            }
        }

        public List<SpentTimeItem> GetSpentTimeList()
        {
            var spentTimeList = new List<SpentTimeItem>();

            foreach (var seq in ((IDbSubGameLevelTag)this).SpentTimeList)
            {
                var array = seq.SplitSequence();
                if (array.Count != 2) throw new Exception("SpentTime array item is not valid");

                spentTimeList.Add(new SpentTimeItem { From = DateTime.Parse(array[0]), To = DateTime.Parse(array[1]) });
            }

            return spentTimeList;
        }

        private string GetSpentTimeJson(SpentTimeItem stItem)
        {
            if (stItem.To.ToOADate() - 1 < 0) return null;

            return string.Format("{0}{1}{2}", stItem.From.ToOADate(), Const.SEPARATE_CHAR, stItem.To.ToOADate());
        }

        #endregion

        #region IDbSubGame

        void IDbType<IDbSubGame>.GetById(string id)
        {
            ((IDbSubGame)this).Id = id;
            var item = DbSubGameList.Find(x => x.Id == ((IDbSubGame)this).Id);
            if (item != null) DbSubGameList.Remove(item);

            var callback = new Action(() => DbSubGameList.Add(this));

            GetItemRequest<IDbSubGame>(callback);
        }

        void IDbType<IDbSubGame>.Update()
        {
            UpdateItemRequest<IDbSubGame>();
        }

        void IDbType<IDbSubGame>.Delete()
        {
            DeleteItemRequest<IDbSubGame>();
        }

        void IDbType<IDbSubGame>.Create()
        {
            var callback = new Action(() => DbSubGameList.Add(this));

            CreateItemRequest<IDbSubGame>(callback);

        }

        #endregion

        public void SaveUserData()
        {
            var tagIdList = new List<string>();
            var tagEnabledList = new List<bool>();
            var tagLastSyncDateList = new List<double>();
            var tagPriorityList = new List<int>();
            var tagSpentTimeList = new List<List<string>>();

            foreach (var tag in CoreContext.UserData.CurrentProfile.SubGameLevelTagArray)
            {
                if (string.IsNullOrEmpty(tag.DbId)) continue;
                
                var spentTimeList = new List<string>();

                tag.SpentTimeArray.Where(item => item.To > tag.LastSyncDate).ToList().ForEach(x => spentTimeList.Add(GetSpentTimeJson((x))));
                //Debug.Log(tag.Name + " - " + spentTimeList.Count + "  - " + tag.LastSyncDate);
                if(spentTimeList.Count < 1) continue;

                tagSpentTimeList.Add(spentTimeList);
                
                tagIdList.Add(tag.DbId);
                tagEnabledList.Add(tag.IsEnabled);
                tagPriorityList.Add(tag.Priority);

                tagLastSyncDateList.Add(tag.LastSyncDate.ToOADate());
            }

            var paramDict = new Dictionary<string, object>()
                                {
                                    {"tag_id_list", tagIdList},
                                    {"tag_enabled_list", tagEnabledList},
                                    {"tag_priority_list", tagPriorityList},
                                    {"tag_last_sync_date", tagLastSyncDateList},
                                    {"tag_spent_time_lists", tagSpentTimeList}
                                };

            var task = ParseCloud.CallFunctionAsync<string>(UPDATE_USERDATA_FUNC_NAME, paramDict);

            var act = new Action(() =>
                                     {
                                         CoreContext.UserData.CurrentProfile.SubGameLevelTagArray.ForEach(x => x.LastSyncDate = DateTime.UtcNow);

                                         var resDict = JsonFx.Json.JsonReader.Deserialize<Dictionary<string, object>>(task.Result);

                                         if (((object[])resDict["tag_id_list"]).Length < 1) return;
                                         
                                         tagIdList = new List<string>((string[]) resDict["tag_id_list"]);
                                         tagEnabledList = new List<bool>((bool[]) resDict["tag_enabled_list"]);
                                         tagPriorityList = new List<int>((int[]) resDict["tag_priority_list"]);

                                         for (var i = 0; i < tagIdList.Count; i++)
                                         {
                                             var tag = CoreContext.UserData.CurrentProfile.SubGameLevelTagArray.FirstOrDefault(x => x.DbId == tagIdList[i]);
                                             if (tag == null) throw new Exception("Tag doesn't exist: " + tagIdList[i]);

                                             tag.IsEnabled = tagEnabledList[i];
                                             tag.Priority = tagPriorityList[i];
                                         }

                                     });

            CoreContext.StartCoroutine(RunRequest<int>(task, UserDataHasBeenSavedCommandName, act));
        }

        #region Helpers

        private string GetParseClassName<T>() where T : IDbType<T>
        {
            if (typeof(T) == typeof(IDbProfile)) return PROFILE_PARSE_CLASS_NAME;
            if (typeof(T) == typeof(IDbSubGame)) return SUBGAME_PARSE_CLASS_NAME;
            if (typeof(T) == typeof(IDbSubGameLevelTag)) return SUBGAMELEVELTAG_PARSE_CLASS_NAME;

            return null;
        }

        private void UpdateFields<T>(ParseObject parseObject, Action callback = null, bool updateInstanse = true) where T : IDbType<T>
        {
            string fieldName;

            if (typeof(T) == typeof(IDbUser))
            {
                fieldName = "username";
                if (updateInstanse) parseObject[fieldName] = ((IDbUser)this).UserLogin; else ((IDbUser)this).UserLogin = parseObject.Get<string>(fieldName);
                fieldName = "password";
                if (updateInstanse) parseObject[fieldName] = ((IDbUser)this).Password; else ((IDbUser)this).Password = parseObject.Get<string>(fieldName);

                if (callback != null) callback();
            }

            if (typeof(T) == typeof(IDbProfile))
            {
                fieldName = "name";
                if (updateInstanse) parseObject[fieldName] = ((IDbProfile)this).Name; else ((IDbProfile)this).Name = parseObject.Get<string>(fieldName);
                fieldName = "userId";
                if (updateInstanse) parseObject[fieldName] = ((IDbProfile)this).DbUserId; else ((IDbProfile)this).DbUserId = parseObject.Get<string>(fieldName);

                //if (callback != null)
                //{
                //    var upi = GetParseClass<IDbUser>().Get(((IDbService)this).DbUserId);

                //    var name = fieldName;
                //    var onReceiveUserAct = new Action(() => { parseInstance.Set(name, upi); callback(); });
                //    _root.StartCoroutine(RunRequest<T>(upi, null, onReceiveUserAct));
                //}

                if (callback != null) callback();
            }

            if (typeof(T) == typeof(IDbSubGameLevelTag))
            {
                fieldName = "name";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGameLevelTag)this).Name; else ((IDbSubGameLevelTag)this).Name = parseObject.Get<string>(fieldName);
                fieldName = "profileId";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGameLevelTag)this).DbProfileId; else ((IDbSubGameLevelTag)this).DbProfileId = parseObject.Get<string>(fieldName);
                fieldName = "priority";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGameLevelTag)this).Priority; else ((IDbSubGameLevelTag)this).Priority = parseObject.Get<int>(fieldName);
                fieldName = "isEnabled";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGameLevelTag)this).IsEnabled; else ((IDbSubGameLevelTag)this).IsEnabled = parseObject.Get<bool>(fieldName);
                fieldName = "spentTimeArray";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGameLevelTag)this).SpentTimeList;

                if (callback != null) callback();
            }

            if (typeof(T) == typeof(IDbSubGame))
            {
                fieldName = "name";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGame)this).Name; else ((IDbSubGame)this).Name = parseObject.Get<string>(fieldName);
                fieldName = "profileId";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGame)this).DbProfileId; else ((IDbSubGame)this).DbProfileId = parseObject.Get<string>(fieldName);
                fieldName = "isEnabled";
                if (updateInstanse) parseObject[fieldName] = ((IDbSubGame)this).IsEnabled; else ((IDbSubGame)this).IsEnabled = parseObject.Get<bool>(fieldName);

                if (callback != null) callback();
            }
        }

        private void DispatchEvent(string commandName, object data = null)
        {
            dispatcher.Dispatch(commandName, data);
            crossContextDispatcher.Dispatch(commandName, data);
        }

        private void CreateItemRequest<T>(Action callback = null) where T : IDbType<T>
        {
            var parseObject = new ParseObject(GetParseClassName<T>());

            UpdateFields<T>(parseObject);

            var act = new Action(() => { ((T)(IDbService)this).Id = parseObject.ObjectId; if (callback != null) callback(); });

            CoreContext.StartCoroutine(RunRequest<T>(parseObject.SaveAsync(), ((T)(IDbService)this).ItemHasBeenCreatedCommandName, act));
        }

        private void GetItemRequest<T>(Action callback = null) where T : IDbType<T>
        {
            var query = ParseObject.GetQuery(GetParseClassName<T>());

            var task = query.GetAsync(((T)(IDbService)this).Id);

            var act = new Action(() => { UpdateFields<T>(task.Result); if (callback != null) callback(); });

            CoreContext.StartCoroutine(RunRequest<T>(task, ((T)(IDbService)this).ItemHasBeenGottenCommandName, act));
        }

        private void UpdateItemRequest<T>(Action callback = null) where T : IDbType<T>
        {

            var query = ParseObject.GetQuery(GetParseClassName<T>());

            var task = query.GetAsync(((T)(IDbService)this).Id);

            var act = new Action(() =>
            {
                var parseObject = task.Result;
                UpdateFields<T>(parseObject);
                CoreContext.StartCoroutine(RunRequest<T>(parseObject.SaveAsync(), ((T)(IDbService)this).ItemHasBeenUpdatedCommandName, callback));
            });

            CoreContext.StartCoroutine(RunRequest<T>(task, null, act));
        }

        private void DeleteItemRequest<T>(Action callback = null) where T : IDbType<T>
        {
            var query = ParseObject.GetQuery(GetParseClassName<T>());

            var task = query.GetAsync(((T)(IDbService)this).Id);

            var act = new Action(() =>
            {
                var parseObject = task.Result;
                CoreContext.StartCoroutine(RunRequest<T>(parseObject.DeleteAsync(), ((T)(IDbService)this).ItemHasBeenDeletedCommandName, callback));
            });

            CoreContext.StartCoroutine(RunRequest<T>(task, null, act));
        }

        private IEnumerator RunRequest<T>(System.Threading.Tasks.Task task, string commandName, Action callback = null)
        {
            while (!task.IsCompleted) yield return null;

            if (task.Exception != null) Debug.LogError(string.Format("Query of {0} is not finished successefuly. Exception: {1}", typeof(T).Name, task.Exception));

            if (callback != null) callback();

            if (!string.IsNullOrEmpty(commandName)) DispatchEvent(commandName, new CallbackObject { Object = this, Error = task.Exception != null ? task.Exception.Message : null });
        }

        #endregion
    }
}
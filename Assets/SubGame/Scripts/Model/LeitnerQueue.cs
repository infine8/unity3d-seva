using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniKid.SubGame.Model
{
    public class LeitnerQueue<T> : IQueue<T> where T : class, new()
    {
        public class QueueObject
        {
            public int Id;
            public T Obj;
            public bool? IsFound;
            public int AttemptNum;
        }


        public QueueInfo UserDataQueueInfo { get; private set; }

        public T CurrentItem { get { return _currentObj.Obj; } }

        public int CurrentItemIndex { get { return _currentObj.Id; } }

        public int AttemptNum { get { return UserDataQueueInfo.AttemptNum; } }

        public int QueueId { get; private set; }

        public bool IsPassed
        {
            get
            {
                if (UserDataQueueInfo.IsPassed) return true;

                UserDataQueueInfo.IsPassed = _queue.TrueForAll(x => x.IsFound == true);

                return UserDataQueueInfo.IsPassed;
            }
            set { UserDataQueueInfo.IsPassed = value; }
        }
        
        public event EventHandler QueueIsPassed;
        public event EventHandler QueueIsFinishedFirstTime;

        private int LastItemIndex { get; set; }

        private readonly List<QueueObject> _queue = new List<QueueObject>();
        private QueueObject _currentObj;

        public void Init(T[] objList, QueueInfo userDataQueueInfo)
        {
            _queue.Clear();

            UserDataQueueInfo = userDataQueueInfo;

            QueueId = UserDataQueueInfo.Id;

            if (UserDataQueueInfo.Queue == null || UserDataQueueInfo.Queue.Length < 1)
            {
                var index = 0;
                objList.ForEach(x => _queue.Add(new QueueObject { Id = index++, IsFound = null, Obj = x }));

                UserDataQueueInfo.Queue = new QueueItem[0];
                return;
            }

            if (UserDataQueueInfo.Queue.Length != objList.Length) throw new Exception("Leitner queue count is not mutch to object list count");

            for (var i = 0; i < UserDataQueueInfo.Queue.Length; i++)
            {
                var item = UserDataQueueInfo.Queue[i];

                if (objList[item.Id] == null) throw new Exception(string.Format("Object with index {0} is not found", item.Id));
                
                _queue.Add(new QueueObject { Id = item.Id, IsFound = item.IsFound, Obj = objList[item.Id] });
            }
        }

        public void SetCurrentObjectStatus(bool isFound = true)
        {
            if (_currentObj == null) return;

            _currentObj.IsFound = isFound;

            IsPassed = _queue.TrueForAll(x => x.IsFound == true);

            if (_queue.TrueForAll(x => x.IsFound == true) && QueueIsPassed != null) { QueueIsPassed(this, new QueueEventArgs { QueueId = QueueId }); }

            if (_queue.TrueForAll(x => x.IsFound.HasValue) && QueueIsFinishedFirstTime != null) QueueIsFinishedFirstTime(this, new QueueEventArgs { QueueId = QueueId });
        }

        public T GetNext()
        {
            if (_currentObj == null)
            {
                _currentObj = _queue[0];

                _currentObj.AttemptNum++;
                LastItemIndex = _currentObj.Id;
                
                return _currentObj.Obj;
            }

            if (!_currentObj.IsFound.HasValue) throw new Exception("Call SetCurrentObjectStatus function first!");

            _queue.RemoveAt(0);

            if (_currentObj.IsFound.Value)
            {
                _queue.Insert(_queue.Count, _currentObj);
            }
            else
            {
                var lastNotFoundObjIndex = _queue.FindLastIndex(x => x.IsFound == false);

                _queue.Insert(lastNotFoundObjIndex > -1 ? lastNotFoundObjIndex + 1 : _queue.Count, _currentObj);
            }

            _currentObj = _queue[0];

            _currentObj.AttemptNum++;


            //if (CurrentItemIndex == LastItemIndex && _queue.FindAll(x => x.IsFound == false).Count == 1)
            //{
            //    _queue.RemoveAt(0);
            //    _queue.Insert(_queue.Count, _currentObj);
            //    _currentObj = _queue[0];
            //}

            LastItemIndex = _currentObj.Id;

            //DubugQueue();

            return _currentObj.Obj;
        }

        public void Save()
        {
            UserDataQueueInfo.Queue = UserDataQueueInfo.Queue.Clear();

            _queue.ForEach(x => UserDataQueueInfo.Queue = UserDataQueueInfo.Queue.Add(new QueueItem { Id = x.Id, IsFound = x.IsFound, AttemptNum = x.AttemptNum }));
        }

        public void ResetQueue()
        {
            _currentObj = null;

            _queue.Sort((queueObject1, queueObject2) => queueObject1.Id - queueObject2.Id);

            _queue.ForEach(x => x.IsFound = null);
        }

        private void DubugQueue()
        {
            var debugStr = string.Empty;
            _queue.ForEach(x => debugStr += x.Id + ";");
            UnityEngine.Debug.Log(debugStr);
        }
    }
}



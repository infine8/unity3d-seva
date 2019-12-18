using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniKid.SubGame.Model
{
    public sealed class QueueEventArgs : EventArgs
    {
        public int? QueueId { get; set; }
    }

    public class QueueList<T, TZ> : IQueueList<T, TZ> where T : class, new() where TZ : IQueue<T>, new()
    {
        private readonly List<IQueue<T>> _list = new List<IQueue<T>>();
        private SubGameQueueUserDataBase _userData;

        public event EventHandler<EventArgs> QueueIsPassed;
        public event EventHandler<EventArgs> QueueIsFinishedFirstTime;

        public int CurrentQueueId { get; private set; }
        public IQueue<T> CurrentQueue { get; private set; }
        
        public void Init(SubGameQueueUserDataBase userData)
        {
            _userData = userData;

            _list.Clear();
        }

        public void AddQueue( int id, T[] objList)
        {
            var userDataQueue = _userData.QueueInfoArray.FirstOrDefault(x => id < 0 || x.Id == id) ?? new QueueInfo();

            userDataQueue.Id = id;

            var queue = new TZ();

            queue.Init(objList, userDataQueue);

            queue.QueueIsFinishedFirstTime += OnQueueIsFinishedFirstTime;
            queue.QueueIsPassed += OnQueueIsPassed;

            _list.Add(queue);
        }

        private void OnQueueIsFinishedFirstTime(object sender, EventArgs e)
        {
            if (QueueIsFinishedFirstTime != null) QueueIsFinishedFirstTime(this, e);
        }

        private void OnQueueIsPassed(object sender, EventArgs e)
        {
            if (QueueIsPassed != null) QueueIsPassed(this, e);
        }
        
        public T GetFirstItem()
        {
            return GetNextItem();
        }
        
        public T GetNextItem(bool isPreviosFound = true)
        {
            if (CurrentQueue == null) MoveNextQueue();
            if (CurrentQueue == null) throw new Exception("Current queue is null");

            var queueId = CurrentQueueId;

            CurrentQueue.SetCurrentObjectStatus(isPreviosFound);

            if (CurrentQueue == null || queueId != CurrentQueueId) return null;
            
            if (!CurrentQueue.IsPassed) return CurrentQueue.GetNext();

            MoveNextQueue();

            if (CurrentQueue == null) return null;
            

            return CurrentQueue.GetNext();
        }

        public T GetNextItem(int queueId, bool isPreviosFound = true)
        {
            if (CurrentQueueId != queueId || CurrentQueue == null) MoveToQueue(queueId);

            if (CurrentQueue == null) throw new Exception("Current queue is null");


            CurrentQueue.SetCurrentObjectStatus(isPreviosFound);

            if (CurrentQueue == null || queueId != CurrentQueueId) return null;

            return CurrentQueue.GetNext();
        }

        private void MoveNextQueue()
        {
            if (CurrentQueueId < 0 || CurrentQueueId >= _list.Count) return;

            var firstNotPassedQueueIndex = _list.FindIndex(x => !x.IsPassed);

            if (firstNotPassedQueueIndex < 0)
            {
                CurrentQueueId = -1;
                CurrentQueue = null;
                return;
            }

            MoveToQueue(firstNotPassedQueueIndex);
        }

        public void MoveToQueue(int queueId)
        {
            CurrentQueueId = queueId;

            CurrentQueue = _list.FirstOrDefault(x => x.QueueId == queueId);
            
            if (CurrentQueue == null) throw new Exception("Queue is not found. Id: " + queueId);

            if (!CurrentQueue.UserDataQueueInfo.IsPassed) CurrentQueue.UserDataQueueInfo.AttemptNum++;

            CurrentQueue.ResetQueue();
        }


        public void Save()
        {
            _userData.QueueInfoArray = new QueueInfo[0];

            foreach (var queue in _list)
            {
                queue.Save();

                _userData.QueueInfoArray = _userData.QueueInfoArray.Add(queue.UserDataQueueInfo);
            }

        }

        public void ResetQueueList()
        {
            CurrentQueue = null;
            _list.ForEach(x => x.ResetQueue());
        }
    }
}
using System;
using System.Collections.Generic;

namespace UniKid.SubGame.Model
{
    public interface IQueueList<T, TZ> where T : class, new() where TZ : IQueue<T>, new()
    {
        event EventHandler<EventArgs> QueueIsPassed;
        event EventHandler<EventArgs> QueueIsFinishedFirstTime;

        int CurrentQueueId { get; }
        IQueue<T> CurrentQueue { get; }

        void Init(SubGameQueueUserDataBase userData);
        void AddQueue(int id, T[] objList);
        void Save();
        T GetNextItem(bool isPreviosFound = true);
        T GetNextItem(int queueId, bool isPreviosFound = true);
        T GetFirstItem();
        void ResetQueueList();
        void MoveToQueue(int queueId);
    }
}
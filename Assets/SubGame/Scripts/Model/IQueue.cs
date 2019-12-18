using System;
using System.Collections.Generic;

namespace UniKid.SubGame.Model
{
    public interface IQueue<T> where T : class, new()
    {
        event EventHandler QueueIsPassed;
        event EventHandler QueueIsFinishedFirstTime;

        T CurrentItem { get; }

        int CurrentItemIndex { get; }

        int AttemptNum { get; }

        int QueueId { get; }

        bool IsPassed { get; }

        QueueInfo UserDataQueueInfo { get; }

        void Init(T[] objList, QueueInfo userDataQueueInfo);

        T GetNext();

        void SetCurrentObjectStatus(bool isFound = true);

        void Save();

        void ResetQueue();
    }
}
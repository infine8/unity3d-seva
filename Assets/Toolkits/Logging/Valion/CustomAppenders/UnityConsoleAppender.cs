using UnityEngine;
using log4net.Core;

namespace log4net.Appender
{
    public sealed class UnityConsoleAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = this.RenderLoggingEvent(loggingEvent);
            if (loggingEvent.Level == Level.Error)
            {
                Debug.LogError(message);
            }

            else if (loggingEvent.Level == Level.Warn)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }


        protected override void PassCheckpoint(string checkPointName)
        {
            Debug.Log("CHECKPOINT PASSED - " + checkPointName);
        }
    }
}
using log4net.Core;

namespace log4net.Appender
{
    public sealed class TestFlightAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            TestFlight.log(string.Format("{0} - {1}", loggingEvent.Level, RenderLoggingEvent(loggingEvent)));
        }

        protected override void OpenFeedbackView()
        {
            TestFlight.openFeedbackView();
        }

        protected override void PassCheckpoint(string checkPointName)
        {
            TestFlight.passCheckpoint(checkPointName);
        }
    }
}
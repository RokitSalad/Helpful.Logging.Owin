using Helpful.Logging;
using Helpful.Logging.Owin;
using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;

namespace TestApplication
{
    public class Appender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = JsonConvert.SerializeObject(((DynamicDictionary) loggingEvent.MessageObject).AsDictionary);
            LogStore.LogEntries.Add(message);
        }
        override protected bool RequiresLayout
        {
            get { return false; }
        }
    }
}

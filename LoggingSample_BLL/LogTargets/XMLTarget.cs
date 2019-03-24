using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggingSample_BLL.LogTargets
{
    [Target("XMLTarget")]
    public sealed class XMLTarget : AsyncTaskTarget
    {
        private readonly object _lockObject = new object();
        private readonly string _fileName = "";

        public XMLTarget()
        {
            this.Host = Environment.MachineName;
        }

        [RequiredParameter]
        public string Host { get; set; }

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            var target = LogManager.Configuration.Variables;

            lock (_lockObject)
            {
                
            }


            //using (var logContext = new LogContext())
            //{
            //    logContext.LogMessages.Add(new LogMessage
            //    {
            //        MachineName = this.Host,
            //        Exception = logEvent.Exception?.ToString(),
            //        LoggerName = logEvent.LoggerName,
            //        Level = logEvent.Level.ToString(),
            //        Message = logEvent.Message,
            //        MessageSource = logEvent.CallerFilePath,
            //        TimeStamp = logEvent.TimeStamp
            //    });

            //    await logContext.SaveChangesAsync(cancellationToken);
            //}

        }
    }
}
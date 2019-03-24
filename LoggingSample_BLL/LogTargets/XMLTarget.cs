using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggingSample_BLL.LogTargets
{
    [Target("XMLTarget")]
    public sealed class XMLTarget : AsyncTaskTarget
    {
        private readonly object _lockObject = new object();

        public XMLTarget()
        {
            this.Host = Environment.MachineName;
        }

        [RequiredParameter]
        public string Host { get; set; }

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            var xmlFilePath = LogManager.Configuration.Variables["logFilePath"].Render(logEvent) + ".xml";
            
            lock (_lockObject)
            {
                if (!File.Exists(xmlFilePath))
                {
                    XmlWriter writer = XmlWriter.Create(xmlFilePath);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("logs");
                    writer.Dispose();
                }

                var doc = XDocument.Load(xmlFilePath);

                var newLog = new XElement("log",
                        new XAttribute("MachineName", this.Host),
                        new XAttribute("Exception", logEvent.Exception?.ToString() ?? "null"),
                        new XAttribute("LoggerName", logEvent.LoggerName),
                        new XAttribute("Level", logEvent.Level.ToString()),
                        new XAttribute("Message", logEvent.Message),
                        new XAttribute("MessageSource", logEvent.CallerFilePath),
                        new XAttribute("TimeStamp", logEvent.TimeStamp));

                    doc.Root?.Add(newLog);
                
                doc.Save(xmlFilePath);
            }
        }
    }
}
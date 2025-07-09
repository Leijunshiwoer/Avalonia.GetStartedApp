using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack.Logging;

namespace GetStartedApp.Utils
{
    public class LogUpdatedEventArgs : EventArgs
    {
        public string Text { get; set; }
    }

    public class MemoryLogger : IVelopackLogger
    {
        public event EventHandler<LogUpdatedEventArgs> LogUpdated;
        private readonly StringBuilder _sb = new StringBuilder();

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public override string ToString()
        {
            lock (_sb)
            {
                return _sb.ToString();
            }
        }

        public void Log(VelopackLogLevel logLevel, string message, Exception exception)
        {
            if (logLevel < VelopackLogLevel.Information) return;
            lock (_sb)
            {
                message = $"{logLevel}: {message}";
                if (exception != null) message += "\n" + exception.ToString();
                Console.WriteLine("log: " + message);
                _sb.AppendLine(message);
                LogUpdated?.Invoke(this, new LogUpdatedEventArgs { Text = _sb.ToString() });
            }
        }
    }
}

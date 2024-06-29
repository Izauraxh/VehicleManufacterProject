using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model.Log
{
    public interface ILogClient
    {
        void Debug(string v);
        void Info(string v);
        void Warning(string v);
        void Warning(string v, Exception exception);
        void Error(string v, Exception exception);
        void Error(string v, Exception exception, object dumpData);
    }

    public class LogClient : ILogClient, ILogger
    {
        public object Context { get; set; }

        public void Warning(string textMessage, Exception ex)
        {
            return;
        }

        public Task ErrorAsync(string message, Exception ex)
        {
            return Task.FromResult(0);
        }

        public Task DebugAsync(string v)
        {
            return Task.FromResult(0);
        }

        public Task WarningAsync(string v)
        {
            return Task.FromResult(0);
        }

        public void Info(string v)
        {
            return;
        }

        void ILogClient.Debug(string v)
        {
            return;
        }

        void ILogClient.Info(string v)
        {
            //return;
            return;
        }

        void ILogClient.Warning(string v)
        {
            //return;
            return;
        }

        void ILogClient.Error(string v, Exception exception)
        {
            return;
        }

        void ILogClient.Error(string v, Exception exception, object dumpData)
        {
            return;
        }

        public void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.Extensions.Logging;

namespace Common
{
    public class LoggerFactoryWrapper
    {
        public ILoggerFactory LoggerFactory { get; private set; }

        public LoggerFactoryWrapper(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }
    }
}

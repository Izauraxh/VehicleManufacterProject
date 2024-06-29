using Common.Model.Log;
using System;

namespace Common
{
    public static class LogManager
    {
        public static LogClient GetLogger<T>()
        {
            return new LogClient();
        }

        public static LogClient GetLogger(Type type)
        {
            return new LogClient();
        }
    }
}

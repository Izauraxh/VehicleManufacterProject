using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Common.Serilog
{
    public static class CommonLogHeper 
    {
        public static IDisposable AddLogProperty(string key, object value)
        {
            try
            {
                return LogContext.PushProperty(key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
            }
            catch (Exception ex) { return null; }
        }
    }
}

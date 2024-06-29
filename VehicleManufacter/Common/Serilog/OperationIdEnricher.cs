using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Serilog
{
    public class OperationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("RequestId", out var requestId))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("operationId", requestId));
            }
        }
    }
}

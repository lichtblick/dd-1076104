using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace OtlpClient;

public class DatadogEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        if (activity == null) return;
        var stringTraceId = activity.TraceId.ToString();
        var stringSpanId = activity.SpanId.ToString();

        var ddTraceId = Convert.ToUInt64(stringTraceId.Substring(16), 16).ToString();
        var ddSpanId = Convert.ToUInt64(stringSpanId, 16).ToString();

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("dd.trace_id", ddTraceId));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("dd.span_id", ddSpanId));
    }
}
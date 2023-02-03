using OpenTelemetry;
using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtlpClient;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.With<DatadogEnricher>()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Error)
    .MinimumLevel.Override("Azure", LogEventLevel.Warning)
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(loggingBuilder => loggingBuilder
        .ClearProviders()
        .AddSerilog(Log.Logger, true))
    .ConfigureServices((context, services) =>
    {
        services
            .AddHostedService<TraceGenerator>();

        services
            .AddOpenTelemetry()
            .WithTracing(otBuilder =>
            {
                otBuilder
                    .ConfigureResource(resourceBuilder => resourceBuilder
                        .AddService("datadog-eval-otlp-client", serviceVersion: "0.1",
                            serviceInstanceId: Environment.MachineName)
                        .AddDetector(new DockerResourceDetector())
                        .AddDetector(new KubernetesResourceDetector())
                    )
                    .AddSource("Azure.*")
                    .AddSource("OtlpClient.*")
                    .AddOtlpExporter();
            }).StartWithHost();
    })
    .Build();


await host.RunAsync();
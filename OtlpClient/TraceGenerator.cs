using System.Diagnostics;

namespace OtlpClient;

public class TraceGenerator : BackgroundService
{
    private readonly ActivitySource _activitySource;
    private readonly ILogger<TraceGenerator> _logger;

    public TraceGenerator(ILogger<TraceGenerator> logger)
    {
        _logger = logger;
        _activitySource = new ActivitySource(typeof(TraceGenerator).FullName!);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        do
        {
            await GenerateTrace(stoppingToken);
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }


    private async Task GenerateTrace(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity();
        _logger.LogInformation("Generate trace {TraceId}", activity?.TraceId);
        await Task.Delay(100, stoppingToken);
    }
}
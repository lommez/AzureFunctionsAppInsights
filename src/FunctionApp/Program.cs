using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.Configure<TelemetryConfiguration>(config =>
                    {
                        var instrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
                        config.InstrumentationKey = instrumentationKey;
                        config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                    });
                })
                .Build();

            host.Run();
        }
    }
}
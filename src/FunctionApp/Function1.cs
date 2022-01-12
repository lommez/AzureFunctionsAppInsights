using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly TelemetryClient _telemetryClient;

        public Function1(
            IOptions<TelemetryConfiguration> telemetryConfiguration,
            ILogger<Function1> logger)
        {
            _logger = logger;
            _telemetryClient = new TelemetryClient(telemetryConfiguration.Value);
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            using var operation = _telemetryClient.StartOperation<RequestTelemetry>("TestOperation");

            _logger.LogInformation("*** Logger *** C# HTTP trigger function processed a request.");
            _telemetryClient.TrackTrace("*** TelemetryClient *** C# HTTP trigger function processed a request.");
            _logger.LogError(new Exception("Some exception"), "Another exception message!");

            _telemetryClient.StopOperation(operation);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
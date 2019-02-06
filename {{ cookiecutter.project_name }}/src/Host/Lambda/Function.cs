using Amazon.Lambda.Core;
{% if cookiecutter.lambda_trigger_type == "SQS" -%}
using Amazon.Lambda.SQSEvents;
{% elif cookiecutter.lambda_trigger_type == "API" -%}
using Amazon.Lambda.APIGatewayEvents;
{%- endif %}
using Coolblue.Utilities.MonitoringEvents;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using {{ cookiecutter.project_name }}.Core;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace {{ cookiecutter.project_name }}.Host.Lambda
{
    public class Function
    {
        private readonly Container _container;
        private readonly MonitoringEvents _monitoringEvents;
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function() : this(Bootstrapper.CreateDefaultApplication(Bootstrapper.GetLambdaSettings()))
        {

        }

        public Function(Container container)
        {
            container.Verify();
            _container = container;
            _monitoringEvents = container.GetInstance<MonitoringEvents>();
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        {% if cookiecutter.lambda_trigger_type == "SQS" -%}
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            try
            {
                foreach (var message in evnt.Records)
                {
                    using (_monitoringEvents.LogContext.PushProperty(new LogContextProperty("MessageId", message.MessageId)))
                    {
                        await ProcessMessageAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                _monitoringEvents.Logger.Error(ex, "Exception during execution for event {@Event}", evnt);
                throw; //Exceptions have to be rethrown to signal the lambda runtime in AWS to requeue the message(s)
            }
        }
        {% elif cookiecutter.lambda_trigger_type == "API" -%}
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            await ProcessMessageAsync(apigProxyEvent.Body);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
        {%- endif %}
        private async Task ProcessMessageAsync(object message)
        {
            var correlationId = GetCorrelationId();
            using (_monitoringEvents.LogContext.PushProperty(new LogContextProperty("CorrelationId", correlationId.ToString())))
            using (Scope scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                _monitoringEvents.Logger.Information("Processed message {message}", message);
                var useCase = scope.GetInstance<{{ cookiecutter.project_name }}UseCase>();

                await useCase.Execute();
            }
        }

        private Guid GetCorrelationId()
        {
            return Guid.NewGuid();
        }
    }
}

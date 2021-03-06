using Coolblue.Utilities.MonitoringEvents;
using Coolblue.Utilities.MonitoringEvents.Aws.Lambda.Datadog;
using Coolblue.Utilities.MonitoringEvents.SimpleInjector;
using {{ cookiecutter.project_slug }}.Core;
using Microsoft.Extensions.Configuration;
{% if cookiecutter.use_kms =="YES" -%}
using SecretManagement.Adapter;
{%- endif %}
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using SimpleInjector;
using System.IO;

namespace {{ cookiecutter.project_slug }}.Host.Lambda
{
    public static class Bootstrapper
    {
        private static IConfigurationRoot GetConfiguration()
            => new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddEnvironmentVariables()
               .Build();

        internal static LambdaSettings GetLambdaSettings() => GetConfiguration().Get<LambdaSettings>();

        public static Container CreateDefaultApplication(LambdaSettings lambdaSettings)
        {
            var log = new LoggerConfiguration()
                              .Enrich.FromLogContext()
                              .MinimumLevel.Verbose()
                              .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                              .WriteTo.Console(new JsonFormatter())
                              .CreateLogger();

            var monitoringEvents = CreateMonitoringEvents(log);
            var container = new Container();

            container.Options.PropertySelectionBehavior = new MonitoringEventsPropertySelectionBehavior();

            container.RegisterInstance(monitoringEvents);

            container.Register<{{ cookiecutter.project_slug }}UseCase>();

            {% if cookiecutter.use_kms =="YES" -%}
            SecretManagementAdapter.AddSecretManagementAdapter(container, lambdaSettings.Environment);
            {%- endif %}

            return container;
        }

        private static MonitoringEvents CreateMonitoringEvents(ILogger logger)
        {
            return new MonitoringEvents(logger, new AwsLambdaDatadogMetrics());
        }
    }
}

using Coolblue.Utilities.MonitoringEvents;
using System.Threading.Tasks;

namespace {{ cookiecutter.project_name }}.Core
{
    public sealed class {{ cookiecutter.project_name }}UseCase
    {
        public MonitoringEvents MonitoringEvents { get; set; }

        public {{ cookiecutter.project_name }}UseCase()
        {
        }

        public Task Execute()
        {
            MonitoringEvents.Logger.Information("Use case entered");
            // TODO: Add the business logic
            MonitoringEvents.Logger.Information("Use case finished");
            return Task.CompletedTask;
        }
    }
}

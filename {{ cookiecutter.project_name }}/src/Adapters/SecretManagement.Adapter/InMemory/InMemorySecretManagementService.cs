using {{ cookiecutter.project_slug }}.Core.Ports;
using System.Threading.Tasks;

namespace SecretManagement.Adapter.InMemory
{
    internal sealed class InMemorySecretManagementService : ISecretManagementService
    {
        public Task<string> DecryptString(string value)
        {
            return Task.FromResult(value);
        }
    }
}

using System.Threading.Tasks;

namespace {{ cookiecutter.project_slug }}.Core.Ports
{
    public interface ISecretManagementService
    {
        Task<string> DecryptString(string value);
    }
}

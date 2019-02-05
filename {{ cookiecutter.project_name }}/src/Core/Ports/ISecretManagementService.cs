using System.Threading.Tasks;

namespace {{ cookiecutter.project_name }}.Core.Ports
{
    public interface ISecretManagementService
    {
        Task<string> DecryptString(string value);
    }
}

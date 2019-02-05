using Amazon.KeyManagementService;
using {{ cookiecutter.project_name }}.Core.Ports;
using {{ cookiecutter.project_name }}.Core.ValueTypes;
using SecretManagement.Adapter.InMemory;
using SecretManagement.Adapter.Kms;
using SimpleInjector;
using System;

namespace SecretManagement.Adapter
{
    public static class SecretManagementAdapter
    {
        public static void AddSecretManagementAdapter(Container serviceCollection, ApplicationEnvironment environment)
        {
            switch (environment)
            {
                case ApplicationEnvironment.Testing:
                case ApplicationEnvironment.Acceptance:
                case ApplicationEnvironment.Production:
                    serviceCollection.Register<ISecretManagementService, KmsSecretManagementService>();
                    serviceCollection.Register(() => new AmazonKeyManagementServiceClient());
                    break;
                case ApplicationEnvironment.Development:
                    serviceCollection.Register<ISecretManagementService, InMemorySecretManagementService>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(environment), environment, null);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Coolblue.Serilog.Sinks.InMemory;

using {{ cookiecutter.project_slug }}.Core;
using {{ cookiecutter.project_slug }}.Core.Ports;
using {{ cookiecutter.project_slug }}.Core.ValueTypes;

using FluentAssertions;
using Moq;
using SimpleInjector;
using Xunit;

{% if cookiecutter.use_kms =="YES" -%}
using SecretManagement.Adapter.InMemory;
using SecretManagement.Adapter.Kms;
{% endif -%}


namespace {{ cookiecutter.project_slug }}.Host.Lambda.Tests
{
    public sealed class BootstrapperTests
    {
        private LambdaSettings TestSettings(ApplicationEnvironment environment) =>
            new LambdaSettings {
                Environment = environment
            };

        {% if cookiecutter.use_kms =="YES" -%}
        [Fact]
        public void GivenADevelopmentEnvironment_WhenContainerResolvesISecretManagementService_GetsInstanceOfInMemoryService()
        {
            var sut = CreateSut(TestSettings(ApplicationEnvironment.Development));
            var instance = sut.GetInstance<ISecretManagementService>();

            instance.Should().BeOfType<InMemorySecretManagementService>();
        }

        [Theory]
        [InlineData(ApplicationEnvironment.Testing)]
        [InlineData(ApplicationEnvironment.Acceptance)]
        [InlineData(ApplicationEnvironment.Production)]
        public void GivenTAPEnvironment_WhenContainerResolvesISecretManagementService_GetsInstanceOfKmsService(ApplicationEnvironment environment)
        {
            var sut = CreateSut(TestSettings(environment));
            var instance = sut.GetInstance<ISecretManagementService>();

            instance.Should().BeOfType<KmsSecretManagementService>();
        }
        {% endif -%}

        [Theory]
        [InlineData(ApplicationEnvironment.Development)]
        [InlineData(ApplicationEnvironment.Testing)]
        [InlineData(ApplicationEnvironment.Acceptance)]
        [InlineData(ApplicationEnvironment.Production)]
        public void ContainerResolvesUseCase(ApplicationEnvironment environment)
        {
            var sut = CreateSut(TestSettings(environment));

            Action func = () => sut.GetInstance<{{ cookiecutter.project_slug }}UseCase>();

            func.Should().NotThrow();
        }

        private Container CreateSut(LambdaSettings settings) => Bootstrapper.CreateDefaultApplication(settings);
    }
}

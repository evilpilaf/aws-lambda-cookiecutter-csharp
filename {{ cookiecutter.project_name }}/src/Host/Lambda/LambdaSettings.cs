using {{ cookiecutter.project_name }}.Core.ValueTypes;
using System;

namespace {{ cookiecutter.project_name }}.Host.Lambda
{
    public sealed class LambdaSettings
    {
        public ApplicationEnvironment Environment { get; set; }
    }
}

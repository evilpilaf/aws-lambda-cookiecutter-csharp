using {{ cookiecutter.project_slug }}.Core.ValueTypes;
using System;

namespace {{ cookiecutter.project_slug }}.Host.Lambda
{
    public sealed class LambdaSettings
    {
        public ApplicationEnvironment Environment { get; set; }
    }
}

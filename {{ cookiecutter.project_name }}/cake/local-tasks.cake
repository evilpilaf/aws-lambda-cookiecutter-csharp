using Newtonsoft.Json;
using System.IO;

var templateFilePath = "./cloudformation/parameters.json.template";
var targetFile = "./cloudformation/development.parameters.json";

Task("Configure-Local")
    .Does(() => {
        var jsonSettings = Newtonsoft.Json.Linq.JArray.Parse(System.IO.File.ReadAllText(templateFilePath));
        foreach (var setting in jsonSettings)
        {
            var value = setting["ParameterValue"].Value<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.Write($"Value for {setting["ParameterKey"]}: ");
                setting["ParameterValue"] = Console.ReadLine();
            }
        }
        System.IO.File.WriteAllText(targetFile, jsonSettings.ToString());
    });

Task("Deploy-Local")
	.Description("Runs all the acceptance tests locally.")
    .WithCriteria(BuildSystem.IsLocalBuild)
	.Does(() =>
    {
        var parameterOverrides = BuildParameterOverrides();
        var settings = new ProcessSettings
        {
            Arguments = $"local invoke \"{functionName}\" -e event.json --template cloudformation.yaml --parameter-overrides \"{parameterOverrides}\" ",
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            Information("Starting the SAM local...");
            using(var process = StartAndReturnProcess("C:/Program Files/Amazon/AWSSAMCLI/bin/sam.cmd", settings))
            {
                process.WaitForExit();
                foreach(var o in process.GetStandardOutput())
                {
                    Information(o);
                }             
                foreach(var o in process.GetStandardError())
                {
                    Information(o);
                }
                Information($"Exit code: {process.GetExitCode()}");
            }
            Information("SAM local has finished.");
        }
        catch(Exception ex)
        {
            Error($"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
    });

Task("Run")
    .Description("This is the task which will run if target Run is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .IsDependentOn("Deploy-Local")
    .Does(() => { Information("Run target ran."); });

private string BuildParameterOverrides()
{
    if(!System.IO.File.Exists(targetFile)) {
        throw new Exception("The development parameters file does not exist");
    }
    var jsonSettings = Newtonsoft.Json.Linq.JArray.Parse(System.IO.File.ReadAllText(targetFile));

    return string.Join(" ", jsonSettings.Select(s => $"ParameterKey={s["ParameterKey"]},ParameterValue={s["ParameterValue"]}"));
}
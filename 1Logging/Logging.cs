using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;

namespace _1Logging;

/// <summary>
/// This is the replacement for the former package.json data. This is required for all mods.
///
/// This is where we define all the metadata associated with this mod.
/// You don't have to do anything with it, other than fill it out.
/// All properties must be overriden, properties you don't use may be left null.
/// It is read by the mod loader when this mod is loaded.
/// </summary>
public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; set; } = "LoggingExample";
    public override string Author { get; set; } = "SPTarkov";
    public override List<string>? Contributors { get; set; }
    public override string Version { get; set; } = "1.0.0";
    public override string SptVersion { get; set; } = "4.0.0";
    public override List<string>? LoadBefore { get; set; } = ["EditDatabaseExample"];
    public override List<string>? LoadAfter { get; set; }
    public override List<string>? Incompatibilities { get; set; }
    public override Dictionary<string, string>? ModDependencies { get; set; }
    public override string? Url { get; set; } = "https://github.com/sp-tarkov/server-mod-examples";
    public override bool? IsBundleMod { get; set; } = false;
    public override string? Licence { get; set; } = "MIT";
}

// We want to load after PreSptModLoader is complete, so we set our type priority to that, plus 1.
[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class Logging(
    ISptLogger<Logging> logger) // We inject a logger for use inside our class, it must have the class inside the diamond <> brackets
    : IOnLoad // Implement the IOnLoad interface so that this mod can do something on server load
{
    public Task OnLoad()
    {
        // We can access the logger and call its methods to log to the server window and the server log file
        logger.Success("This is a success message");
        logger.Warning("This is a warning message");
        logger.Error("This is an error message");
        logger.Info("This is an info message");
        logger.Critical("This is a critical message");

        // Logging with colors requires you to 'pass' the text color and background color
        logger.LogWithColor("This is a message with custom colors", LogTextColor.Red, LogBackgroundColor.Black);
        logger.Debug("This is a debug message that gets written to the log file, not the console");
        
        // Inform the server our mod has finished doing work
        return Task.CompletedTask;
    }
}

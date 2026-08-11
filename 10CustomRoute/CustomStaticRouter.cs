using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Common.Models.Logging;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Models.Utils;

namespace _10CustomRoute;

/// <summary>
/// This is the replacement for the former package.json data. This is required for all mods.
///
/// This is where we define all the metadata associated with this mod.
/// You don't have to do anything with it, other than fill it out.
/// All properties must be overriden, properties you don't use may be left null.
/// It is read by the mod loader when this mod is loaded.
/// </summary>
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.sp-tarkov.examples.customroute";
    public string Name { get; init; } = "CustomStaticRouterExample";
    public string Author { get; init; } = "SPTarkov";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";
    public bool HasPrepatcher { get; init; } = false;
}

/// <summary>
///  This class registers a new static router in SPT, you can register as many routes as you want here
/// </summary>
[Injectable(TypePriority = OnLoadOrder.Routers)]
public class CustomStaticRouter(JsonUtil jsonUtil, CustomStaticRouterCallback customStaticRouterCallback)
    : StaticRouter(jsonUtil, [
            new RouteAction<ExampleStaticRequestData>(
                "/example/route/static",
                async (
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await customStaticRouterCallback.HandleExampleStaticRoute(url, info, sessionId)
            ),
            // There are cases where you dont want to send data to the server, in that case you can ignore ExampleStaticRequestData and use EmptyRequestData
            new RouteAction<EmptyRequestData>(
                "/example/route/emptystatic",
                async (
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await customStaticRouterCallback.HandleEmptyExampleStaticRoute(url, info, sessionId)
            )
        ])
{ }

/// <summary>
/// This class handles callbacks that are sent to your route, you can run code both synchronously here as well as asynchronously
/// </summary>
[Injectable]
public class CustomStaticRouterCallback(ISptLogger<CustomStaticRouterCallback> logger, HttpResponseUtil httpResponseUtil)
{
    public ValueTask<string> HandleEmptyExampleStaticRoute(string url, EmptyRequestData info, MongoId sessionId)
    {
        // Your mods code goes here
        logger.Info($"Callback on {url} route received!");
        return new ValueTask<string>(httpResponseUtil.NullResponse());
    }

    public ValueTask<string> HandleExampleStaticRoute(string url, ExampleStaticRequestData info, MongoId sessionId)
    {
        // Your mods code goes here
        logger.Info($"Callback on {url} route received!");
        return new ValueTask<string>(httpResponseUtil.NullResponse());
    }
}

/// <summary>
/// This record represents your incoming data model, any data you are sending to the server you will need to have in here.
/// </summary>
public record ExampleStaticRequestData : IRequestData
{
}
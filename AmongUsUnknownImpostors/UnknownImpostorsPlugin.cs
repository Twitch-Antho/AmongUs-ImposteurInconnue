using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Utilities;

namespace AmongUsUnknownImpostors;

[BepInPlugin(Id, "Among Us Imposteur Inconnu", PluginVersion)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public class UnknownImpostorsPlugin : BasePlugin
{
    public const string Id = "com.herysia.amongusunkimpostor";
    public const string PluginVersion = "2.0.0";

    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        UnknownImpostorsSettings.Init(Config);

        ReactorCredits.Register("Imposteur Inconnu", PluginVersion, false, ReactorCredits.AlwaysShow);

        Harmony.PatchAll();

        Log.LogInfo($"{Id} v{PluginVersion} chargé.");
    }
}

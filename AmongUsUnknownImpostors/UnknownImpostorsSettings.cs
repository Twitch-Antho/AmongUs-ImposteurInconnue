using System;
using System.Globalization;
using BepInEx.Configuration;
using HarmonyLib;
using InnerNet;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace AmongUsUnknownImpostors;

/// <summary>
/// Réglages du mod. L'hôte les lit dans sa config BepInEx (modifiable via les commandes du lobby)
/// et les envoie aux autres joueurs. Un client n'active le mode que s'il a reçu les réglages
/// d'un hôte qui a le mod : sans le mod chez l'hôte, tout reste vanilla.
/// </summary>
public static class UnknownImpostorsSettings
{
    public const float MinVision = 0.25f;
    public const float MaxVision = 5f;

    private static ConfigEntry<bool> _enabled = null!;
    private static ConfigEntry<float> _impostorLightOffVision = null!;

    private static bool _receivedFromHost;
    private static bool _hostEnabled;
    private static float _hostVision = 2f;

    public static void Init(ConfigFile config)
    {
        _enabled = config.Bind("Partie", "ImposteursInconnus", true,
            "Active le mode Imposteur Inconnu quand vous êtes l'hôte.");
        _impostorLightOffVision = config.Bind("Partie", "VisionImposteurLumieresCoupees", 2f,
            new ConfigDescription("Multiplicateur de vision des imposteurs quand les lumières sont sabotées.",
                new AcceptableValueRange<float>(MinVision, MaxVision)));
    }

    private static bool IsHostOrLocal =>
        AmongUsClient.Instance && (AmongUsClient.Instance.AmHost || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay);

    /// <summary>Le mode est-il actif pour la partie en cours ?</summary>
    public static bool Active
    {
        get
        {
            if (!AmongUsClient.Instance) return false;
            return IsHostOrLocal ? _enabled.Value : _receivedFromHost && _hostEnabled;
        }
    }

    public static float ImpostorLightOffVision => IsHostOrLocal ? _impostorLightOffVision.Value : _hostVision;

    public static bool LocalEnabledSetting
    {
        get => _enabled.Value;
        set { _enabled.Value = value; Broadcast(); }
    }

    public static float LocalVisionSetting
    {
        get => _impostorLightOffVision.Value;
        set { _impostorLightOffVision.Value = Math.Clamp(value, MinVision, MaxVision); Broadcast(); }
    }

    public static string Describe() =>
        $"Imposteur Inconnu : {(Active ? "ACTIVÉ" : "désactivé")} | vision imposteur lumières coupées : " +
        ImpostorLightOffVision.ToString("0.##", CultureInfo.InvariantCulture) + "x";

    /// <summary>Envoie les réglages de l'hôte à tous les joueurs.</summary>
    public static void Broadcast()
    {
        if (!AmongUsClient.Instance || !AmongUsClient.Instance.AmHost || !PlayerControl.LocalPlayer) return;
        if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay) return;
        RpcSyncSettings(PlayerControl.LocalPlayer, _enabled.Value, _impostorLightOffVision.Value);
    }

    [MethodRpc((uint) RpcIds.SyncSettings, LocalHandling = RpcLocalHandling.None)]
    public static void RpcSyncSettings(PlayerControl sender, bool enabled, float vision)
    {
        // On n'accepte les réglages que de l'hôte.
        if (!AmongUsClient.Instance || sender.OwnerId != AmongUsClient.Instance.HostId) return;

        _receivedFromHost = true;
        _hostEnabled = enabled;
        _hostVision = Math.Clamp(vision, MinVision, MaxVision);
    }

    public static void ResetHostSettings()
    {
        _receivedFromHost = false;
        _hostEnabled = false;
        _hostVision = 2f;
    }

    private enum RpcIds : uint
    {
        SyncSettings = 0,
    }

    // Quand un joueur apparaît dans le lobby, l'hôte renvoie les réglages pour qu'il les reçoive.
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    internal static class PlayerPhysics_CoSpawnPlayer
    {
        public static void Postfix() => Broadcast();
    }

    // Retour au menu principal : on oublie les réglages de l'ancien hôte.
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    internal static class MainMenuManager_Start
    {
        public static void Postfix() => ResetHostSettings();
    }
}

using System;
using System.Globalization;
using HarmonyLib;
using InnerNet;

namespace AmongUsUnknownImpostors.Patches;

/// <summary>
/// Commandes du lobby (hôte uniquement) :
///   /inconnu            affiche les réglages
///   /inconnu on|off     active ou désactive le mode
///   /inconnu vision 1.5 règle la vision des imposteurs quand les lumières sont coupées
/// </summary>
[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
internal static class ChatCommands
{
    public static bool Prefix(ChatController __instance)
    {
        var text = __instance.freeChatField.Text?.Trim() ?? string.Empty;
        if (!text.StartsWith("/inconnu", StringComparison.OrdinalIgnoreCase)) return true;

        var args = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string reply;

        if (args.Length == 1)
        {
            reply = UnknownImpostorsSettings.Describe();
        }
        else if (!AmongUsClient.Instance.AmHost && AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay)
        {
            reply = "Seul l'hôte peut modifier les réglages d'Imposteur Inconnu.";
        }
        else if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
        {
            reply = "Les réglages se modifient dans le lobby, pas pendant la partie.";
        }
        else
        {
            switch (args[1].ToLowerInvariant())
            {
                case "on":
                    UnknownImpostorsSettings.LocalEnabledSetting = true;
                    reply = UnknownImpostorsSettings.Describe();
                    break;
                case "off":
                    UnknownImpostorsSettings.LocalEnabledSetting = false;
                    reply = UnknownImpostorsSettings.Describe();
                    break;
                case "vision" when args.Length >= 3 &&
                                   float.TryParse(args[2].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var vision):
                    UnknownImpostorsSettings.LocalVisionSetting = vision;
                    reply = UnknownImpostorsSettings.Describe();
                    break;
                default:
                    reply = "Usage : /inconnu [on|off|vision <0.25-5>]";
                    break;
            }
        }

        __instance.AddChat(PlayerControl.LocalPlayer, reply, false);
        __instance.freeChatField.Clear();
        __instance.quickChatMenu.Clear();
        return false;
    }
}

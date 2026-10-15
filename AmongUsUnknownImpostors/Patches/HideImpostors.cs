using HarmonyLib;
using UnityEngine;

namespace AmongUsUnknownImpostors.Patches;

internal static class HideImpostors
{
    private static bool LocalIsImpostor
    {
        get
        {
            var local = PlayerControl.LocalPlayer;
            return local && local.Data != null && local.Data.Role && local.Data.Role.IsImpostor;
        }
    }

    private static bool IsOtherImpostor(NetworkedPlayerInfo info)
    {
        return info != null && info.Role && info.Role.IsImpostor && info.PlayerId != PlayerControl.LocalPlayer.PlayerId;
    }

    // Intro : l'imposteur se voit seul dans son équipe.
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    internal static class IntroCutscene_BeginImpostor
    {
        public static void Prefix([HarmonyArgument(0)] ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            if (!UnknownImpostorsSettings.Active) return;

            yourTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            yourTeam.Add(PlayerControl.LocalPlayer);
        }
    }

    // Noms au-dessus des joueurs et dans le vote : les autres imposteurs restent en blanc.
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    internal static class HudManager_Update
    {
        public static void Postfix()
        {
            if (!UnknownImpostorsSettings.Active || !LocalIsImpostor) return;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (!player || player == PlayerControl.LocalPlayer || !IsOtherImpostor(player.Data)) continue;
                if (player.cosmetics && player.cosmetics.nameText)
                    player.cosmetics.nameText.color = Color.white;
            }

            if (!MeetingHud.Instance) return;

            foreach (var voteArea in MeetingHud.Instance.playerStates)
            {
                if (!voteArea || !voteArea.NameText) continue;
                if (IsOtherImpostor(GameData.Instance.GetPlayerById(voteArea.TargetPlayerId)))
                    voteArea.NameText.color = Color.white;
            }
        }
    }

    // Chat : le nom des autres imposteurs n'est plus rouge.
    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
    internal static class ChatBubble_SetName
    {
        public static void Postfix(ChatBubble __instance, [HarmonyArgument(0)] string playerName)
        {
            if (!UnknownImpostorsSettings.Active || !LocalIsImpostor || !__instance.NameText) return;
            if (playerName == PlayerControl.LocalPlayer.Data.PlayerName) return;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player && player.Data != null && player.Data.PlayerName == playerName && IsOtherImpostor(player.Data))
                {
                    __instance.NameText.color = Color.white;
                    return;
                }
            }
        }
    }

    // Les imposteurs peuvent se cibler (et donc s'entre-tuer).
    [HarmonyPatch(typeof(RoleBehaviour), nameof(RoleBehaviour.IsValidTarget))]
    internal static class RoleBehaviour_IsValidTarget
    {
        public static bool Prefix(RoleBehaviour __instance, NetworkedPlayerInfo target, ref bool __result)
        {
            if (!UnknownImpostorsSettings.Active || !__instance.IsImpostor || !__instance.Player) return true;

            __result = target != null
                       && !target.Disconnected
                       && !target.IsDead
                       && target.PlayerId != __instance.Player.PlayerId
                       && target.Role
                       && target.Object
                       && !target.Object.inVent
                       && !target.Object.inMovingPlat
                       && target.Object.Visible;
            return false;
        }
    }
}

using AmongUs.GameOptions;
using HarmonyLib;
using UnityEngine;

namespace AmongUsUnknownImpostors.Patches;

/// <summary>
/// Les imposteurs subissent aussi le sabotage des lumières, avec leur propre multiplicateur
/// de vision quand tout est coupé (réglable avec /inconnu vision).
/// </summary>
[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
internal static class LightSabotageFix
{
    public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player, ref float __result)
    {
        if (!UnknownImpostorsSettings.Active) return true;
        if (player == null || player.IsDead || !player.Role || !player.Role.IsImpostor) return true;

        if (!__instance.Systems.ContainsKey(SystemTypes.Electrical)) return true;
        var switchSystem = __instance.Systems[SystemTypes.Electrical].TryCast<SwitchSystem>();
        if (switchSystem == null) return true;

        float lightFactor = switchSystem.Value / 255f;
        float impostorLightMod = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.ImpostorLightMod);

        __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, lightFactor)
                   * Mathf.Lerp(UnknownImpostorsSettings.ImpostorLightOffVision, impostorLightMod, lightFactor);
        return false;
    }
}

using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Teleport))]
public class Teleportable {
  static int Hash = "override_restrict".GetStableHashCode();
  static void Prefix(TeleportWorld __instance) {
    if (!Configuration.configTeleportable.Value) return;
    if (!__instance.m_nview || !__instance.m_nview.IsValid()) return;
    ForceTeleportable.Force = !__instance.m_nview.GetZDO().GetBool(Hash, true);
  }
  static void Postfix() {
    ForceTeleportable.Force = false;
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsTeleportable))]
public class ForceTeleportable {
  public static bool Force = false;
  static bool Prefix(ref bool __result) {
    __result = Force;
    return !Force;
  }
}
using HarmonyLib;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(StaticPhysics), nameof(StaticPhysics.Awake))]
public class Fall
{
  static int Hash = "override_fall".GetStableHashCode();
  static void Postfix(StaticPhysics __instance)
  {
    if (!Configuration.configFalling.Value) return;
    if (!__instance.m_nview || !__instance.m_nview.IsValid()) return;
    var fall = __instance.m_nview.GetZDO().GetInt(Hash, -1);
    if (fall < 0) return;
    __instance.m_fall = fall > 0;
    __instance.m_pushUp = fall > 0;
    __instance.m_checkSolids = fall > 1;
  }
}
[HarmonyPatch(typeof(StaticPhysics), nameof(StaticPhysics.SUpdate))]
public class GlobalFall
{
  static bool Prefix() => !Configuration.configDisableFalling.Value;
}

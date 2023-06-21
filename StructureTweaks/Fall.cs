using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(StaticPhysics), nameof(StaticPhysics.Awake))]
public class Fall
{
  static void Postfix(StaticPhysics __instance)
  {
    if (!Configuration.configFalling.Value) return;
    Helper.Int(__instance.m_nview, Hash.Fall, value =>
    {
      __instance.m_fall = value == 1 || value == 2;
      __instance.m_pushUp = value == 1 || value == 2;
      __instance.m_checkSolids = value == 2;
    });
  }
}
[HarmonyPatch(typeof(StaticPhysics), nameof(StaticPhysics.SUpdate))]
public class GlobalFall
{
  static bool Prefix() => !Configuration.configDisableFalling.Value;
}

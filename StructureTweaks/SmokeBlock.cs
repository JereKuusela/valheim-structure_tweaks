using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.IsBlocked))]
public class SmokeBlock
{
  static bool Postfix(bool result, SmokeSpawner __instance)
  {
    if (!Configuration.configSmokeBlock.Value) return result;
    if (!result) return result;
    var view = __instance.GetComponentInParent<ZNetView>();
    return Helper.Bool(view, Hash.Smoke);
  }
}
[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.Spawn))]
public class SmokeSpawn
{
  static bool Prefix(SmokeSpawner __instance)
  {
    if (!Configuration.configSmokeBlock.Value) return true;
    var view = __instance.GetComponentInParent<ZNetView>();
    return Helper.Bool(view, Hash.Smoke); ;
  }
}

using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.IsBlocked))]
public class SmokeBlock
{
  static int Hash = "override_smoke".GetStableHashCode();
  static bool Postfix(bool result, SmokeSpawner __instance)
  {
    if (!Configuration.configSmokeBlock.Value) return result;
    var view = __instance.GetComponentInParent<ZNetView>();
    Helper.Int(view, Hash, value =>
    {
      result = false;
    });
    return result;
  }
}
[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.Spawn))]
public class SmokeSpawn
{
  static int Hash = "override_smoke".GetStableHashCode();
  static bool Prefix(SmokeSpawner __instance)
  {
    if (!Configuration.configSmokeBlock.Value) return true;
    var view = __instance.GetComponentInParent<ZNetView>();
    var ret = true;
    Helper.Bool(view, Hash, value =>
    {
      if (!value) ret = false;
    });
    return ret;
  }
}

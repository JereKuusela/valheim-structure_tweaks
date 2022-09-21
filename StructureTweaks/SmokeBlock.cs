using HarmonyLib;
using Service;

namespace Plugin;

[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.IsBlocked))]
public class SmokeBlock {
  static int Hash = "override_restrict".GetStableHashCode();
  static bool Postfix(bool result, SmokeSpawner __instance) {
    if (!Configuration.configSmokeBlock.Value) return result;
    var view = __instance.GetComponentInParent<ZNetView>();
    Helper.Bool(view, Hash, value => {
      if (!value) result = false;
    });
    return result;
  }
}

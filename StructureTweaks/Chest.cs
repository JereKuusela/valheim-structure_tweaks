using HarmonyLib;
using Service;

namespace Plugin;

[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
public class ChestAwake {
  private static int Name = "override_name".GetStableHashCode();
  // string

  static void Postfix(Container __instance) {
    if (!Configuration.configRuneStone.Value) return;
    Helper.String(__instance.m_nview, Name, value => __instance.m_name = value);
  }
}

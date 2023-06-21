using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
public class ChestAwake
{
  static void Postfix(Container __instance)
  {
    if (!Configuration.configRuneStone.Value) return;
    Helper.String(__instance.m_nview, Hash.Name, value => __instance.m_name = value);
  }
}

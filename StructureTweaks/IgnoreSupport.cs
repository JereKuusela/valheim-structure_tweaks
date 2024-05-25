using HarmonyLib;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(WearNTear))]
public class IgnoreSupport
{
  public const float INFITE = 1E19F;

  [HarmonyPatch(nameof(WearNTear.UpdateWear)), HarmonyPrefix]
  static bool UpdateWear() => !Configuration.configDisableStructureSystems.Value;

  [HarmonyPatch(nameof(WearNTear.UpdateSupport)), HarmonyPrefix]
  static bool UpdateSupport(WearNTear __instance)
  {
    if (!Configuration.configIgnoreSupport.Value) return true;
    if (!__instance || !__instance.m_nview.IsValid()) return true;
    var view = __instance.m_nview;
    if (view.GetZDO().GetFloat(ZDOVars.s_health, __instance.m_health) < INFITE) return true;
    if (__instance.m_support == INFITE) return false;
    __instance.m_support = INFITE;
    view.GetZDO().Set(ZDOVars.s_support, INFITE);
    return false;
  }
}

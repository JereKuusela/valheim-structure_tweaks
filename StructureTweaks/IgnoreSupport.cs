using HarmonyLib;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.UpdateWear))]
public class IgnoreSupport
{
  public const float INFITE = 1E19F;
  private static readonly int HashHealth = "health".GetStableHashCode();
  private static readonly int HashSupport = "support".GetStableHashCode();
  private static bool Check(ZNetView view, float defaultValue) => !Configuration.configIgnoreSupport.Value || view.GetZDO().GetFloat(HashHealth, defaultValue) < INFITE;
  static bool Prefix(WearNTear __instance)
  {
    if (Configuration.configDisableStructureSystems.Value) return false;
    if (!__instance || !__instance.m_nview.IsValid()) return true;
    var check = Check(__instance.m_nview, __instance.m_health);
    if (!check && __instance.m_nview.IsOwner() && __instance.ShouldUpdate())
    {
      // Copy pasted from the base game (not related to wear).
      if (__instance.m_wet)
      {
        var isWet = EnvMan.instance.IsWet() && !__instance.HaveRoof();
        __instance.m_wet.SetActive(isWet);
      }
      var zdo = __instance.m_nview.GetZDO();
      if (ZDOExtraData.s_floats.TryGetValue(zdo.m_uid, out var data) && data.ContainsKey(ZDOVars.s_support))
        zdo.Set(ZDOVars.s_support, __instance.GetMaxSupport());
    }
    if (!check)
    {
      __instance.m_support = INFITE;
      __instance.UpdateVisual(false);
    }
    return check;
  }
}
using HarmonyLib;

namespace Plugin;

[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.UpdateWear))]
public class IgnoreSupport {
  public const float INFITE = 1E19F;
  private static int HashHealth = "health".GetStableHashCode();
  private static int HashSupport = "support".GetStableHashCode();
  private static bool Check(ZNetView view, float defaultValue) => !Configuration.configIgnoreSupport.Value || view.GetZDO().GetFloat(HashHealth, defaultValue) < INFITE;
  static bool Prefix(WearNTear __instance) {
    if (!__instance || !__instance.m_nview.IsValid()) return true;
    var check = Check(__instance.m_nview, __instance.m_health);
    if (!check) {
      __instance.m_support = INFITE;
      __instance.UpdateVisual(false);
    }
    if (!check && __instance.m_nview.IsOwner() && __instance.ShouldUpdate()) {
      // Copy pasted from the base game (not related to wear).
      if (__instance.m_wet) {
        var isWet = EnvMan.instance.IsWet() && !__instance.HaveRoof();
        __instance.m_wet.SetActive(isWet);
      }
      if (__instance.m_nview.GetZDO().m_floats.Remove(HashSupport))
        __instance.m_nview.GetZDO().IncreseDataRevision();
    }
    return check;
  }
}
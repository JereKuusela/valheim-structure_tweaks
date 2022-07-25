using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch]
public class IgnoreDamage {
  public const float INFITE = 1E19F;
  private static int HashHealth = "health".GetStableHashCode();
  private static bool Check(ZNetView view, float defaultValue) => !Configuration.configIgnoreDamage.Value || view.IsOwner() && view.GetZDO().GetFloat(HashHealth, defaultValue) < INFITE;
  [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.RPC_Damage)), HarmonyPrefix]
  static bool TreeLog_RPC_Damage(MineRock5 __instance) => Check(__instance.m_nview, 0f);

  [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.RPC_Damage)), HarmonyPrefix]
  static bool WearNTear_RPC_Damage(WearNTear __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.RPC_Damage)), HarmonyPrefix]
  static bool TreeBase_RPC_Damage(TreeBase __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(Destructible), nameof(Destructible.RPC_Damage)), HarmonyPrefix]
  static bool Destructible_RPC_Damage(Destructible __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage)), HarmonyPrefix]
  static bool Character_RPC_Damage(Character __instance) => Check(__instance.m_nview, __instance.GetMaxHealth());
}
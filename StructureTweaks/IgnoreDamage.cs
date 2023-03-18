using HarmonyLib;

namespace StructureTweaksPlugin;

[HarmonyPatch]
public class IgnoreDamage
{
  public const float INFITE = 1E19F;
  private static int HashHealth = "health".GetStableHashCode();
  private static bool Check(ZNetView view, float defaultValue) => !Configuration.configIgnoreDamage.Value || !view.IsValid() || view.GetZDO().GetFloat(HashHealth, defaultValue) < INFITE;
  [HarmonyPatch(typeof(Character), nameof(Character.IsDodgeInvincible)), HarmonyPostfix]
  static bool Character_IsDodgeInvincible(bool result, Character __instance) => Check(__instance.m_nview, 0f) ? result : true;

  [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.RPC_Damage)), HarmonyPrefix]
  static bool MineRock5_RPC_Damage(MineRock5 __instance) => Check(__instance.m_nview, 0f);

  [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.RPC_Damage)), HarmonyPrefix]
  static bool TreeLog_RPC_Damage(TreeLog __instance) => Check(__instance.m_nview, 0f);

  [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.RPC_Damage)), HarmonyPrefix]
  static bool WearNTear_RPC_Damage(WearNTear __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.RPC_Damage)), HarmonyPrefix]
  static bool TreeBase_RPC_Damage(TreeBase __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(Destructible), nameof(Destructible.RPC_Damage)), HarmonyPrefix]
  static bool Destructible_RPC_Damage(Destructible __instance) => Check(__instance.m_nview, __instance.m_health);

  [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage)), HarmonyPrefix]
  static bool Character_RPC_Damage(Character __instance) => Check(__instance.m_nview, __instance.GetMaxHealth());


  [HarmonyPatch(typeof(MineRock), nameof(MineRock.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType MineRock_GetDestructibleType(DestructibleType type, MineRock __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType MineRock5_GetDestructibleType(DestructibleType type, MineRock5 __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType TreeLog_GetDestructibleType(DestructibleType type, TreeLog __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType WearNTear_GetDestructibleType(DestructibleType type, WearNTear __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType TreeBase_GetDestructibleType(DestructibleType type, TreeBase __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(Destructible), nameof(Destructible.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType Destructible_GetDestructibleType(DestructibleType type, Destructible __instance) => Check(__instance.m_nview, __instance.m_health) ? type : DestructibleType.None;

  [HarmonyPatch(typeof(Character), nameof(Character.GetDestructibleType)), HarmonyPostfix]
  static DestructibleType Character_GetDestructibleType(DestructibleType type, Character __instance) => Check(__instance.m_nview, __instance.GetMaxHealth()) ? type : DestructibleType.None;
}
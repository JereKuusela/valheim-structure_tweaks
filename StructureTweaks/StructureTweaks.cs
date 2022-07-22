using BepInEx;
using HarmonyLib;
using ServerSync;

namespace StructureTweaks;
[HarmonyPatch]
[BepInPlugin(GUID, NAME, VERSION)]
public class StructureTweaks : BaseUnityPlugin {
  public const float INFITE = 1E19F;
  const string GUID = "structure_tweaks";
  const string NAME = "Structure Tweaks";
  const string VERSION = "1.0";
  public void Awake() {
    new VersionCheck(GUID)
    {
      CurrentVersion = VERSION,
      DisplayName = NAME
    };
    new Harmony(GUID).PatchAll();
  }
  private static int HashHealth = "health".GetStableHashCode();
  public static bool Check(ZNetView view, float defaultValue) => view.IsOwner() && view.GetZDO().GetFloat(HashHealth, defaultValue) < INFITE;

  [HarmonyPatch(typeof(Piece), nameof(Piece.Awake)), HarmonyPostfix]
  static void ForceTargetNonPlayerBuilt(Piece __instance) {
    __instance.m_targetNonPlayerBuilt = false;
  }
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

using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(Player), nameof(Player.UpdateHover))]
public class NoHover {
  public static int Hash = "override_interact".GetStableHashCode();
  static void Postfix(Player __instance) {
    if (!Configuration.configInteract.Value) return;
    var go = __instance.m_hovering;
    if (!go) return;
    var view = go.transform.root.GetComponent<ZNetView>();
    if (!view) return;
    var zdo = view.GetZDO();
    if (zdo == null || zdo.GetBool(Hash, true)) return;
    __instance.m_hovering = null;
  }
}
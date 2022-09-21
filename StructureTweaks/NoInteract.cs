using HarmonyLib;
using Service;

namespace Plugin;

[HarmonyPatch(typeof(Player), nameof(Player.UpdateHover))]
public class NoHover {
  public static int Hash = "override_interact".GetStableHashCode();
  static void Postfix(Player __instance) {
    if (!Configuration.configInteract.Value) return;
    var go = __instance.m_hovering;
    if (!go) return;
    var view = go.transform.root.GetComponent<ZNetView>();
    Helper.Bool(view, Hash, value => {
      if (!value) __instance.m_hovering = null;
    });
  }
}
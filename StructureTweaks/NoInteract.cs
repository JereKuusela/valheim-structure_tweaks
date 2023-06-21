using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Player), nameof(Player.UpdateHover))]
public class NoHover
{
  static void Postfix(Player __instance)
  {
    if (!Configuration.configInteract.Value) return;
    var go = __instance.m_hovering;
    if (!go) return;
    var view = go.transform.root.GetComponent<ZNetView>();
    Helper.Bool(view, Hash.NoInteract, () =>
    {
      __instance.m_hovering = null;
    });
  }
}
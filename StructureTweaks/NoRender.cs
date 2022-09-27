using HarmonyLib;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.CreateObject))]
public class NoRender {
  public static int Hash = "override_render".GetStableHashCode();
  static void Postfix(ZDO zdo, GameObject __result) {
    if (!__result || !Configuration.configRendering.Value) return;
    if (zdo == null) return;
    if (zdo.GetBool(Hash, true)) return;
    var renderers = __result.GetComponentsInChildren<Renderer>();
    foreach (var renderer in renderers)
      renderer.enabled = false;
  }
}

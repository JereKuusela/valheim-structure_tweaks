using HarmonyLib;
using UnityEngine;

namespace Plugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.CreateObject))]
public class NoRender {
  public static int Hash = "override_render".GetStableHashCode();
  static void Postfix(ZDO zdo, GameObject __result) {
    if (!__result || !Configuration.configRendering.Value) return;
    if (zdo == null) return;
    if (zdo.GetInt(Hash, -1) != 0) return;
    var renderers = __result.GetComponentsInChildren<Renderer>();
    foreach (var renderer in renderers)
      renderer.enabled = false;
  }
}

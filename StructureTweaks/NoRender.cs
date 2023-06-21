using HarmonyLib;
using Service;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.CreateObject))]
public class NoRender
{
  static void Postfix(ZDO zdo, GameObject __result)
  {
    if (!__result || !Configuration.configRendering.Value) return;
    if (zdo == null) return;
    if (zdo.GetBool(Hash.NoRender))
    {
      var renderers = __result.GetComponentsInChildren<Renderer>();
      foreach (var renderer in renderers)
        renderer.enabled = false;
    }
  }
}

using HarmonyLib;
using UnityEngine;

namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.CreateObject))]
public class NoCollision {
  public static int Hash = "override_collision".GetStableHashCode();
  static void Postfix(ZDO zdo, GameObject __result) {
    if (!__result || !Configuration.configCollision.Value) return;
    if (zdo == null) return;
    if (zdo.GetBool(Hash, true)) return;
    var colliders = __result.GetComponentsInChildren<Collider>();
    foreach (var collider in colliders)
      collider.enabled = false;
  }
}

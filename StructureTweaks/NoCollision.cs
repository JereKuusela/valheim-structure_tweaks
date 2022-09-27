using HarmonyLib;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.CreateObject))]
public class NoCollision {
  public static int Hash = "override_collision".GetStableHashCode();
  public static int PortalHash = "portal_wood".GetStableHashCode();
  static void Postfix(ZDO zdo, GameObject __result) {
    if (!__result || !Configuration.configCollision.Value) return;
    if (zdo == null) return;
    if (zdo.GetBool(Hash, true)) return;
    var colliders = __result.GetComponentsInChildren<Collider>();
    // For unknown reason, isTrigger doesn't turn off portal colliding.
    // Totally turning colliders off also allows creating one way portals.
    if (zdo.GetPrefab() == PortalHash) {
      foreach (var collider in colliders)
        collider.enabled = false;

    } else {
      foreach (var collider in colliders)
        collider.isTrigger = true;
    }
  }
}

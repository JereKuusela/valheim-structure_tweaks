using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetView), nameof(Piece.Awake))]
public class AllScalable {
  static void Postfix(ZNetView __instance) {
    if (!Configuration.configAllScalable.Value) return;
    if (__instance) __instance.m_syncInitialScale = true;
  }
}
[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class AllScalable2 {
  static void Postfix(ZNetScene __instance) {
    if (!Configuration.configAllScalable.Value) return;
    foreach (var prefab in __instance.m_namedPrefabs.Values) {
      if (prefab.GetComponent<ZNetView>() is { } view)
        view.m_syncInitialScale = true;
    }
  }
}
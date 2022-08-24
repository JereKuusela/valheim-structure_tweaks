using System.Collections.Generic;
using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class AllScalable {
  static Dictionary<int, bool> Originals = new();
  public static void Update() => Update(ZNetScene.instance);
  public static void Update(int prefab, ZNetView obj, bool enable) {
    if (enable)
      obj.m_syncInitialScale = true;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_syncInitialScale = value;
  }
  public static void Update(ZNetScene scene) {
    var enable = Configuration.configAllScalable.Value;
    foreach (var kvp in scene.m_namedPrefabs) {
      if (kvp.Value.GetComponent<ZNetView>() is { } view)
        Update(kvp.Key, view, enable);
    }
    foreach (var kvp in scene.m_instances) {
      Update(kvp.Key.GetPrefab(), kvp.Value, enable);
    }
  }
  [HarmonyPriority(Priority.Last)]
  static void Postfix(ZNetScene __instance) {
    foreach (var kvp in __instance.m_namedPrefabs) {
      if (Originals.ContainsKey(kvp.Key)) continue;
      if (kvp.Value.GetComponent<ZNetView>() is { } view)
        Originals[kvp.Key] = view.m_syncInitialScale;
    }
    Update(__instance);
  }
}
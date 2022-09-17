using System.Collections.Generic;
using HarmonyLib;

namespace Plugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class AllScalable {
  static Dictionary<int, bool> Originals = new();
  public static void Update() => Update(ZNetScene.instance);
  public static void Update(int prefab, ZNetView obj) {
    if (Configuration.configAllScalable.Value)
      obj.m_syncInitialScale = true;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_syncInitialScale = value;
  }
  public static void Update(ZNetScene scene) {
    Dictionary<int, bool> Values = new();
    foreach (var kvp in scene.m_namedPrefabs) {
      if (kvp.Value.GetComponent<ZNetView>() is { } view) {
        Update(kvp.Key, view);
        Values[kvp.Key] = view.m_syncInitialScale;
      }
    }
    foreach (var kvp in scene.m_instances) {
      var prefab = kvp.Key.GetPrefab();
      if (Values.ContainsKey(prefab) && kvp.Value.GetComponent<ZNetView>() is { } view)
        view.m_syncInitialScale = Values[prefab];
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
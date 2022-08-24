using System.Collections.Generic;
using HarmonyLib;
namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class NoSuppression {
  static Dictionary<int, bool> Originals = new();
  public static void Update() => Update(ZNetScene.instance);
  public static void Update(int prefab, CreatureSpawner obj, bool enable) {
    if (enable)
      obj.m_spawnInPlayerBase = obj.m_spawnInPlayerBase || obj.m_respawnTimeMinuts <= 0f;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_spawnInPlayerBase = value;
  }
  public static void Update(ZNetScene scene) {
    var enable = Configuration.configNoSpawnPointSuppression.Value;
    Dictionary<int, bool> Values = new();
    foreach (var kvp in scene.m_namedPrefabs) {
      if (kvp.Value.GetComponent<CreatureSpawner>() is { } spawner) {
        Update(kvp.Key, spawner, enable);
        Values[kvp.Key] = spawner.m_spawnInPlayerBase;
      }
    }
    foreach (var kvp in scene.m_instances) {
      var prefab = kvp.Key.GetPrefab();
      if (Values.ContainsKey(prefab) && kvp.Value.GetComponent<CreatureSpawner>() is { } spawner)
        spawner.m_spawnInPlayerBase = Values[prefab];
    }
  }
  [HarmonyPriority(Priority.Last)]
  static void Postfix(ZNetScene __instance) {
    foreach (var kvp in __instance.m_namedPrefabs) {
      if (Originals.ContainsKey(kvp.Key)) continue;
      if (kvp.Value.GetComponent<CreatureSpawner>() is { } spawner)
        Originals[kvp.Key] = spawner.m_spawnInPlayerBase;
    }
    Update(__instance);
  }
}
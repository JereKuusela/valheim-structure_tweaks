using System.Collections.Generic;
using HarmonyLib;
namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class NoTargeting {
  static Dictionary<int, bool> Originals = new();
  public static void Update() => Update(ZNetScene.instance);
  public static void Update(int prefab, Piece obj, bool enable) {
    if (enable)
      obj.m_targetNonPlayerBuilt = true;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_targetNonPlayerBuilt = value;
  }
  public static void Update(ZNetScene scene) {
    var enable = Configuration.configNoTargeting.Value;
    foreach (var kvp in scene.m_namedPrefabs) {
      if (kvp.Value.GetComponent<Piece>() is { } piece)
        Update(kvp.Key, piece, enable);
    }
    foreach (var kvp in scene.m_instances) {
      if (kvp.Value.GetComponent<Piece>() is { } piece)
        Update(kvp.Key.GetPrefab(), piece, enable);
    }
  }
  static void Postfix(ZNetScene __instance) {
    foreach (var kvp in __instance.m_namedPrefabs) {
      if (Originals.ContainsKey(kvp.Key)) continue;
      if (kvp.Value.GetComponent<Piece>() is { } piece)
        Originals[kvp.Key] = piece.m_targetNonPlayerBuilt;
    }
    Update(__instance);
  }
}
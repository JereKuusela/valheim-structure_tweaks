using System.Collections.Generic;
using HarmonyLib;
namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class NoTargeting
{
  static readonly Dictionary<int, bool> Originals = new();
  public static void Update() => Update(ZNetScene.instance);
  public static void Update(int prefab, Piece obj)
  {
    if (Configuration.configNoTargeting.Value)
      obj.m_targetNonPlayerBuilt = false;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_targetNonPlayerBuilt = value;
  }
  public static void Update(ZNetScene scene)
  {
    Dictionary<int, bool> Values = new();
    foreach (var kvp in scene.m_namedPrefabs)
    {
      if (kvp.Value.GetComponent<Piece>() is { } piece)
      {
        Update(kvp.Key, piece);
        Values[kvp.Key] = piece.m_targetNonPlayerBuilt;
      }
    }
    foreach (var kvp in scene.m_instances)
    {
      var prefab = kvp.Key.GetPrefab();
      if (Values.ContainsKey(prefab) && kvp.Value.GetComponent<Piece>() is { } piece)
        piece.m_targetNonPlayerBuilt = Values[prefab];
    }
  }
  [HarmonyPriority(Priority.Last)]
  static void Postfix(ZNetScene __instance)
  {
    foreach (var kvp in __instance.m_namedPrefabs)
    {
      if (Originals.ContainsKey(kvp.Key)) continue;
      if (kvp.Value.GetComponent<Piece>() is { } piece)
        Originals[kvp.Key] = piece.m_targetNonPlayerBuilt;
    }
    Update(__instance);
  }
}
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
public class AllScalable
{
  static readonly Dictionary<int, bool> Originals = [];
  public static readonly HashSet<int> HasSystems = [];
  public static void Update() => Update(ZNetScene.instance);
  public static void UpdateParticles(ZNetScene scene)
  {
    var objs = scene.m_namedPrefabs.Values.Where(v => v.GetComponent<Fireplace>()).ToArray();
    List<ParticleSystem> results = [];
    foreach (var obj in objs)
    {
      obj.GetComponentsInChildren(true, results);
      foreach (var system in results)
      {
        if (system.main.scalingMode != ParticleSystemScalingMode.Local) continue;
        var main = system.main;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
      }
    }
  }
  public static void Update(int prefab, ZNetView obj)
  {
    if (Configuration.configAllScalable.Value)
      obj.m_syncInitialScale = true;
    else if (Originals.TryGetValue(prefab, out var value))
      obj.m_syncInitialScale = value;
  }
  public static void Update(ZNetScene scene)
  {
    Dictionary<int, bool> Values = [];
    foreach (var kvp in scene.m_namedPrefabs)
    {
      if (kvp.Value.GetComponent<ZNetView>() is { } view)
      {
        Update(kvp.Key, view);
        Values[kvp.Key] = view.m_syncInitialScale;
      }
    }
    foreach (var kvp in scene.m_instances)
    {
      var prefab = kvp.Key.GetPrefab();
      if (Values.ContainsKey(prefab) && kvp.Value.GetComponent<ZNetView>() is { } view)
        view.m_syncInitialScale = Values[prefab];
    }
  }
  [HarmonyPriority(Priority.Last)]
  static void Postfix(ZNetScene __instance)
  {
    foreach (var kvp in __instance.m_namedPrefabs)
    {
      if (Originals.ContainsKey(kvp.Key)) continue;
      if (kvp.Value.GetComponent<ZNetView>() is { } view)
        Originals[kvp.Key] = view.m_syncInitialScale;
    }
    Update(__instance);
    UpdateParticles(__instance);
  }
}
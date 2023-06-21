using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(MusicLocation), nameof(MusicLocation.Awake))]
public class MusicLocationAwake
{
  private static Dictionary<string, AudioClip> Clips = new();


  static void Postfix(MusicLocation __instance)
  {
    if (!Configuration.configMusic.Value) return;
    var obj = __instance;
    var view = obj.m_nview;
    Helper.Float(view, Hash.Radius, value => obj.m_radius = value);
    Helper.Int(view, Hash.Condition, value =>
    {
      obj.m_oneTime = (value & 1) > 0;
      obj.m_notIfEnemies = (value & 2) > 0;
      obj.m_forceFade = (value & 3) > 0;
    });
    Helper.String(view, Hash.Audio, value =>
    {
      if (Clips.Count == 0)
        Clips = Resources.FindObjectsOfTypeAll<AudioClip>().ToDictionary(x => x.name, x => x);
      if (Clips.TryGetValue(value, out var clip))
        obj.m_audioSource.clip = clip;
      else
        Debug.LogWarning($"Audio clip {value} not found");
    });

  }
}

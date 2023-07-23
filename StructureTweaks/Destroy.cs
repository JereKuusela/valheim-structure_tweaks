using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(TimedDestruction), nameof(TimedDestruction.DestroyNow))]
public class Destroy
{
  public static void Handle(ZNetView view)
  {
    Helper.Float(view, Hash.Destroy, value =>
    {
      var td = Helper.Get<TimedDestruction>(view);
      td.m_triggerOnAwake = true;
      td.m_timeout = value;
      td.CancelInvoke("DestroyNow");
      td.Trigger();
    });

  }
  static void Prefix(TimedDestruction __instance)
  {
    Helper.String(__instance.m_nview, Hash.DestroyEffect, value =>
    {
      var effects = Helper.ParseEffects(value);
      effects.Create(__instance.transform.position, __instance.transform.rotation, __instance.transform);
    });
  }
}

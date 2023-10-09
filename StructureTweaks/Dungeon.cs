using System.Linq;
using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;


[HarmonyPatch(typeof(LocationProxy), nameof(LocationProxy.SpawnLocation))]
public class LocationAwake
{
  static void Postfix(LocationProxy __instance)
  {
    if (!__instance.m_instance) return;

    Helper.String(__instance.m_nview, Hash.DungeonWeather, value =>
    {
      var zone = __instance.m_instance.GetComponentInChildren<EnvZone>();
      if (zone)
        zone.m_environment = value;
    });
    Teleport[] portals = [];
    Helper.String(__instance.m_nview, Hash.EnterText, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y <= 3000);
      if (portal)
        portal.m_enterText = value;
    });
    Helper.String(__instance.m_nview, Hash.EnterHover, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y <= 3000);
      if (portal)
        portal.m_hoverText = value;
    });
    Helper.String(__instance.m_nview, Hash.ExitText, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y > 3000);
      if (portal)
        portal.m_enterText = value;
    });
    Helper.String(__instance.m_nview, Hash.ExitHover, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y > 3000);
      if (portal)
        portal.m_hoverText = value;
    });
  }
}

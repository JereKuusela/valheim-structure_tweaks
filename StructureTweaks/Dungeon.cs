using System.Linq;
using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;


[HarmonyPatch(typeof(LocationProxy), nameof(LocationProxy.SpawnLocation))]
public class LocationAwake
{
  static int HashWeather = "override_dungeon_weather".GetStableHashCode();
  static int HashEnterText = "override_dungeon_enter_text".GetStableHashCode();
  static int HashEnterHover = "override_dungeon_enter_hover".GetStableHashCode();
  static int HashExitText = "override_dungeon_exit_text".GetStableHashCode();
  static int HashExitHover = "override_dungeon_exit_hover".GetStableHashCode();
  static void Postfix(LocationProxy __instance)
  {
    if (!__instance.m_instance) return;

    Helper.String(__instance.m_nview, HashWeather, value =>
    {
      var zone = __instance.m_instance.GetComponentInChildren<EnvZone>();
      if (zone)
        zone.m_environment = value;
    });
    Teleport[] portals = new Teleport[0];
    Helper.String(__instance.m_nview, HashEnterText, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y <= 3000);
      if (portal)
        portal.m_enterText = value;
    });
    Helper.String(__instance.m_nview, HashEnterHover, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y <= 3000);
      if (portal)
        portal.m_hoverText = value;
    });
    Helper.String(__instance.m_nview, HashExitText, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y > 3000);
      if (portal)
        portal.m_enterText = value;
    });
    Helper.String(__instance.m_nview, HashExitHover, value =>
    {
      if (portals.Length == 0)
        portals = __instance.m_instance.GetComponentsInChildren<Teleport>();
      var portal = portals.FirstOrDefault(p => p.transform.position.y > 3000);
      if (portal)
        portal.m_hoverText = value;
    });
  }

}
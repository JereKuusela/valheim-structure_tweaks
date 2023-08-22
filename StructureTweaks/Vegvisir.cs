using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Vegvisir))]
public class VegvisirText
{
  private static void Setup(Vegvisir obj)
  {
    if (!Configuration.configRuneStone.Value) return;
    var view = obj.GetComponentInParent<ZNetView>();
    Helper.String(view, Hash.Name, value => obj.m_name = value);
    Helper.String(view, Hash.Discover, value =>
    {
      var discovery = Helper.ParseDiscovery(value);
      if (obj.m_locations.Count == 0)
        obj.m_locations.Add(new());
      var location = obj.m_locations[0];
      location.m_locationName = discovery.name;
      location.m_pinName = discovery.pin;
      location.m_pinType = discovery.type;
    });
  }

  [HarmonyPatch(typeof(Vegvisir), nameof(Vegvisir.GetHoverText)), HarmonyPrefix]
  static void GetHoverText(Vegvisir __instance) => Setup(__instance);
  [HarmonyPatch(typeof(Vegvisir), nameof(Vegvisir.GetHoverName)), HarmonyPrefix]
  static void GetHoverName(Vegvisir __instance) => Setup(__instance);
  [HarmonyPatch(typeof(Vegvisir), nameof(Vegvisir.Interact)), HarmonyPrefix]
  static void Interact(Vegvisir __instance) => Setup(__instance);
}

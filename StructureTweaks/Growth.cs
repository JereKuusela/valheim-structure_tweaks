using System;
using System.Collections.Generic;
using HarmonyLib;
using Service;
namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Plant))]
public class Growth
{
  public static int Hash = "override_growth".GetStableHashCode();
  public static int PlantHash = "plantTime".GetStableHashCode();
  public static int Number(string value)
  {
    if (value == "small") return 0;
    if (value == "small_bad") return 1;
    if (value == "big") return 2;
    if (value == "big_bad") return 3;
    return -1;
  }
  [HarmonyPatch(typeof(Plant), nameof(Plant.SUpdate)), HarmonyPrefix]
  static void StoreTime(Plant __instance, ref float __state)
  {
    __state = __instance.m_updateTime;
  }
  [HarmonyPatch(typeof(Plant), nameof(Plant.SUpdate)), HarmonyPostfix]
  static void OverrideGrowth(Plant __instance, float __state)
  {
    if (!Configuration.configGrowth.Value) return;
    if (!__instance || !__instance.m_nview.IsValid()) return;
    if (__state == __instance.m_updateTime) return;
    Helper.Int(__instance.m_nview, Hash, growth =>
    {
      var healthy = growth == 0;
      var unhealthy = growth == 1;
      var healthyGrown = growth == 2;
      var unhealthyGrown = growth == 3;
      if (__instance.m_healthyGrown)
      {
        __instance.m_healthy.SetActive(healthy);
        __instance.m_unhealthy.SetActive(unhealthy);
        __instance.m_healthyGrown.SetActive(healthyGrown);
        __instance.m_unhealthyGrown.SetActive(unhealthyGrown);
      }
      else
      {
        __instance.m_healthy.SetActive(healthy || healthyGrown);
        __instance.m_unhealthy.SetActive(unhealthy || unhealthyGrown);
      }
    });
  }

  public static void SetGrowth(ZNetView view, string value)
  {
    if (!view.IsOwner()) return;
    var number = Number(value);
    view.GetZDO().Set(Hash, number);
    var date = number < 0 ? ZNet.instance.GetTime().Ticks : DateTime.MaxValue.Ticks / 2L;
    view.GetZDO().Set(PlantHash, date);
  }
  static void Register(ZNetView view)
  {
    if (view)
    {
      view.Register<string>("SetGrowth", (uid, value) => SetGrowth(view, value));
    }
  }
  [HarmonyPatch(typeof(Plant), nameof(Plant.Awake)), HarmonyPostfix]
  static void RegisterRPC(Plant __instance)
  {
    Register(__instance.m_nview);
  }
}
public class GrowthCommand
{

  private void Execute(Terminal terminal, ZNetView view, string value)
  {
    if (view.IsOwner())
      Growth.SetGrowth(view, value);
    else
      view.InvokeRPC("SetWear", value);
    if (view.GetComponent<Plant>() is { } plant) plant.m_updateTime = 0f;
    var number = Growth.Number(value);
    if (number < 0)
      Helper.AddMessage(terminal, "Removed growth override.");
    else
      Helper.AddMessage(terminal, $"Growth set to {value}.");
  }

  public GrowthCommand()
  {
    List<string> values = new() {
      "big",
      "big_bad",
      "small",
      "small_bad"
    };

    Helper.Command("growth", "[big/big_bad/small/small_bad] - Overrides the plant growth.", (args) =>
    {
      var view = Helper.GetHover(Configuration.configGrowthEditing.Value);
      Execute(args.Context, view, args.Length > 1 ? args[1] : "");
    }, () => values);
  }
}

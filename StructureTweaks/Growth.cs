using System;
using System.Collections.Generic;
using HarmonyLib;
using Service;
namespace StructureTweaks;

[HarmonyPatch(typeof(Plant), nameof(Plant.SUpdate))]
public class SUpdate {
  static void Prefix(Plant __instance, ref float __state) {
    __state = __instance.m_updateTime;
  }
  static void Postfix(Plant __instance, float __state) {
    if (!Configuration.configGrowth.Value) return;
    if (__state == __instance.m_updateTime) return;
    var growth = __instance.m_nview.GetZDO().GetInt(GrowthCommand.Hash, -1);
    if (growth < 0) return;
    var healthy = growth == 0;
    var unhealthy = growth == 1;
    var healthyGrown = growth == 2;
    var unhealthyGrown = growth == 3;
    if (__instance.m_healthyGrown) {
      __instance.m_healthy.SetActive(healthy);
      __instance.m_unhealthy.SetActive(unhealthy);
      __instance.m_healthyGrown.SetActive(healthyGrown);
      __instance.m_unhealthyGrown.SetActive(unhealthyGrown);
    } else {
      __instance.m_healthy.SetActive(healthy || healthyGrown);
      __instance.m_unhealthy.SetActive(unhealthy || unhealthyGrown);
    }
  }
}
public class GrowthCommand {
  public static int Hash = "override_growth".GetStableHashCode();
  public static int PlantHash = "plantTime".GetStableHashCode();
  private int Number(string value) {
    if (value == "small") return 0;
    if (value == "small_bad") return 1;
    if (value == "big") return 2;
    if (value == "big_bad") return 3;
    return -1;
  }
  private void Execute(Terminal terminal, ZNetView view, string value) {
    var number = Number(value);
    if (number < 0) {
      view.GetZDO().m_ints.Remove(Hash);
      view.GetZDO().Set(PlantHash, ZNet.instance.GetTime().Ticks);
    } else {
      view.GetZDO().Set(Hash, number);
      view.GetZDO().Set(PlantHash, DateTime.MaxValue.Ticks / 2L);
    }
    if (view.GetComponent<Plant>() is { } plant) plant.m_updateTime = 0f;
    if (number < 0)
      Helper.AddMessage(terminal, "Removed growth override.");
    else
      Helper.AddMessage(terminal, $"Growth set to {value}.");
  }

  public GrowthCommand() {
    List<string> values = new() {
      "big",
      "big_bad",
      "small",
      "small_bad"
    };

    Helper.Command("growth", "[big/big_bad/small/small_bad] - Overrides the plant growth.", (args) => {
      if (!Configuration.configCommandGrowth.Value)
        throw new InvalidOperationException("This command is disabled.");
      var view = Helper.GetHover();
      Execute(args.Context, view, args.Length > 1 ? args[1] : "");
    }, () => values);
  }
}

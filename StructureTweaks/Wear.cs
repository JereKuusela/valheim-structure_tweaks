using System;
using System.Collections.Generic;
using HarmonyLib;
using Service;

namespace Plugin;


[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.SetHealthVisual))]
public class SetHealthVisual {
  private static float Convert(int value, float defaultValue) {
    if (value == 0) return 0.1f;
    if (value == 1) return 0.5f;
    if (value == 2) return 1f;
    return defaultValue;
  }
  static void Prefix(WearNTear __instance, ref float health) {
    if (!Configuration.configWear.Value) return;
    if (!__instance.m_nview) return;
    var value = __instance.m_nview.GetZDO().GetInt(WearCommand.Hash, -1);
    health = Convert(value, health);
  }
}

public class WearCommand {
  public static int Hash = "override_wear".GetStableHashCode();
  private int Number(string value) {
    if (value == "broken") return 0;
    if (value == "damaged") return 1;
    if (value == "healthy") return 2;
    return -1;
  }
  private void Execute(Terminal terminal, ZNetView view, string value) {
    var number = Number(value);
    if (number < 0) {
      view.GetZDO().m_ints.Remove(Hash);
      view.GetZDO().IncreseDataRevision();
    } else {
      view.GetZDO().Set(Hash, number);
    }
    if (number < 0)
      Helper.AddMessage(terminal, "Removed wear override.");
    else
      Helper.AddMessage(terminal, $"Wear set to {value}.");
  }
  public WearCommand() {
    List<string> values = new() {
      "broken",
      "damaged",
      "healthy"
    };

    Helper.Command("wear", "[broken/damaged/healthy] - Overrides the wear health.", (args) => {
      if (!Configuration.configCommandGrowth.Value)
        throw new InvalidOperationException("This command is disabled.");
      var view = Helper.GetHover();
      Execute(args.Context, view, args.Length > 1 ? args[1] : "");
    }, () => values);
  }
}

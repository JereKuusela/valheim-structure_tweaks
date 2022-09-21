using System.Collections.Generic;
using HarmonyLib;
using Service;

namespace Plugin;

[HarmonyPatch(typeof(WearNTear))]
public class Wear {
  public static int Hash = "override_wear".GetStableHashCode();
  public static int Number(string value) {
    if (value == "broken") return 0;
    if (value == "damaged") return 1;
    if (value == "healthy") return 2;
    return -1;
  }

  public static void SetWear(ZNetView view, string value) {
    if (!view.IsOwner()) return;
    var number = Number(value);
    view.GetZDO().Set(Hash, number);
  }
  static void Register(ZNetView view) {
    if (view) {
      view.Register<string>("SetWear", (uid, value) => SetWear(view, value));
    }
  }
  [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake)), HarmonyPostfix]
  static void RegisterRPC(WearNTear __instance) {
    Register(__instance.m_nview);
  }

  private static float Convert(int value, float defaultValue) {
    if (value == 0) return 0.1f;
    if (value == 1) return 0.5f;
    if (value == 2) return 1f;
    return defaultValue;
  }
  [HarmonyPatch(nameof(WearNTear.SetHealthVisual)), HarmonyPrefix]
  static void OverrideWear(WearNTear __instance, ref float health) {
    if (!Configuration.configWear.Value) return;
    if (!__instance.m_nview) return;
    var value = __instance.m_nview.GetZDO().GetInt(Hash, -1);
    health = Convert(value, health);
  }
}

public class WearCommand {

  private void Execute(Terminal terminal, ZNetView view, string value) {
    Wear.SetWear(view, value);
    if (view.IsOwner())
      Wear.SetWear(view, value);
    else
      view.InvokeRPC("SetWear", value);
    var number = Wear.Number(value);
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
      var view = Helper.GetHover(Configuration.configGrowthEditing.Value);
      Execute(args.Context, view, args.Length > 1 ? args[1] : "");
    }, () => values);
  }
}

using System;
using HarmonyLib;
using Service;
using UnityEngine;

namespace StructureTweaksPlugin;

public class ZdoTextReceiver : MonoBehaviour, TextReceiver
{
  private static int Name = "override_name".GetStableHashCode();
  // string
  private static int Text = "override_text".GetStableHashCode();
  // string
  private static int Topic = "override_topic".GetStableHashCode();
  // string
  private static int Compendium = "override_compendium".GetStableHashCode();
  // string
  private int Index = 0;
  private int[] Hashes = new int[] { Name, Text, Topic, Compendium };
  private string[] Topics = new string[] { "Enter name", "$piece_sign_input", "Enter topic", "Enter compendium topic" };
  // string
  private ZNetView? m_nview;
  private bool Queued = false;
  public void Awake()
  {
    m_nview = GetComponentInParent<ZNetView>();
  }
  public void LateUpdate()
  {
    if (Queued)
      TextInput.instance.RequestText(this, Topics[Index], 1000);
    Queued = false;
  }
  public void Show()
  {
    Index = 0;
    Queued = true;
  }
  public string GetText()
  {
    var text = "";
    Helper.String(m_nview, Hashes[Index], value =>
    {
      text = value;
    });
    return text;
  }

  public void SetText(string text)
  {
    m_nview?.GetZDO().Set(Hashes[Index], text);
    Index += 1;
    if (Index == Hashes.Length)
    {
      Index = 0;
      return;
    }
    Queued = true;
  }
}

[HarmonyPatch(typeof(RuneStone))]
public class RuneStoneText
{
  private static int Name = "override_name".GetStableHashCode();
  // string
  private static int Text = "override_text".GetStableHashCode();
  // string
  private static int Topic = "override_topic".GetStableHashCode();
  // string
  private static int Compendium = "override_compendium".GetStableHashCode();
  // string
  private static int Discover = "override_discover".GetStableHashCode();
  // string

  [HarmonyPatch(typeof(RuneStone), nameof(RuneStone.GetHoverText)), HarmonyPrefix]
  static void GetHoverText(RuneStone __instance)
  {
    if (!Configuration.configRuneStone.Value) return;
    Helper.String(__instance.GetComponentInParent<ZNetView>(), Name, value => __instance.m_name = value);
  }
  [HarmonyPatch(typeof(RuneStone), nameof(RuneStone.GetHoverText)), HarmonyPostfix]
  static string GetHoverTextUseKey(string result, RuneStone __instance)
  {
    if (!Configuration.configRuneStone.Value) return result;
    if (!Helper.CanEdit(__instance.GetComponentInParent<ZNetView>(), Configuration.configRuneStoneEditing.Value)) return result;
    return result + Localization.instance.Localize("\n[<color=yellow><b>$KEY_AltPlace + $KEY_Use</b></color>] Edit");
  }
  [HarmonyPatch(typeof(RuneStone), nameof(RuneStone.GetHoverName)), HarmonyPrefix]
  static void GetHoverName(RuneStone __instance)
  {
    if (!Configuration.configRuneStone.Value) return;
    Helper.String(__instance.GetComponentInParent<ZNetView>(), Name, value => __instance.m_name = value);
  }
  [HarmonyPatch(typeof(RuneStone), nameof(RuneStone.Interact)), HarmonyPrefix]
  static bool Interact(RuneStone __instance, bool alt, ref bool __result)
  {
    if (!Configuration.configRuneStone.Value) return true;
    var view = __instance.GetComponentInParent<ZNetView>();
    var canEdit = Helper.CanEdit(view, Configuration.configRuneStoneEditing.Value);
    if (alt && canEdit)
    {
      __result = true;
      var receiver = __instance.GetComponent<ZdoTextReceiver>();
      if (!receiver)
        receiver = __instance.gameObject.AddComponent<ZdoTextReceiver>();
      receiver.Show();
      return false;
    }
    if (__instance.m_randomTexts == null)
      __instance.m_randomTexts = new();
    Helper.String(view, Text, value =>
    {
      __instance.m_randomTexts = new();
      __instance.m_text = value;
    });
    Helper.String(view, Topic, value =>
    {
      __instance.m_randomTexts = new();
      __instance.m_topic = value;
    });
    Helper.String(view, Compendium, value =>
    {
      __instance.m_randomTexts = new();
      __instance.m_label = value;
    });
    Helper.String(view, Discover, value =>
    {
      var discovery = Helper.ParseDiscovery(value);
      __instance.m_locationName = discovery.name;
      __instance.m_pinName = discovery.pin;
      __instance.m_pinType = discovery.type;
      __instance.m_showMap = discovery.showMap;
    });
    return true;
  }
}

[HarmonyPatch(typeof(Player), nameof(Player.AddKnownText))]
public class AddKnownText
{
  static bool Prefix(Player __instance, string label, string text)
  {
    var labels = label.Split('|');
    if (labels.Length > 1)
    {
      foreach (var key in labels) __instance.AddKnownText(key, text);
      return false;
    }
    if (label.StartsWith("-", StringComparison.Ordinal))
    {
      __instance.m_knownTexts.Remove(label.Substring(1));
      return false;
    }
    return true;
  }
}

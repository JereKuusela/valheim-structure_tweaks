using HarmonyLib;

namespace Plugin;

[HarmonyPatch(typeof(RuneStone), nameof(RuneStone.Interact))]
public class RuneStoneText {
  static void Prefix(RuneStone __instance) {
    return;
    //__instance.m_randomTexts = new();
    //__instance.m_topic = "Great piece of information";
    //__instance.m_text = "This is a wood wall.";
  }
}

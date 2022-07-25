using HarmonyLib;
namespace StructureTweaks;

[HarmonyPatch(typeof(Piece), nameof(Piece.Awake))]
public class NoTargeting {
  static void Postfix(Piece __instance) {
    if (!Configuration.configNoTargeting.Value) return;
    __instance.m_targetNonPlayerBuilt = false;
  }
}
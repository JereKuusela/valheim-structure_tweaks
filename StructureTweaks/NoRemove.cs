using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(Player), nameof(Player.CheckCanRemovePiece))]
public class NoRemove {
  static int Hash = "override_remove".GetStableHashCode();
  static void Postfix(Piece piece, ref bool __result) {
    if (!__result) return;
    if (!piece || !piece.m_nview.IsValid()) return;
    var remove = piece.m_nview.GetZDO().GetInt(Hash, -1);
    if (remove < 0) return;
    __result = false;
  }
}
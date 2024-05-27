using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Player), nameof(Player.CheckCanRemovePiece))]
public class NoRemove
{
  static bool Postfix(bool result, Piece piece)
  {
    if (!Configuration.configIgnoreRemove.Value) return result;
    if (!result) return result;
    if (!piece || !piece.m_nview.IsValid()) return true;
    var id = Game.instance.GetPlayerProfile().GetPlayerID();
    if (id == piece.GetCreator()) return true;
    return Helper.IsFinite(piece.m_nview, piece.TryGetComponent<WearNTear>(out var wear) ? wear.m_health : 0f);
  }
}
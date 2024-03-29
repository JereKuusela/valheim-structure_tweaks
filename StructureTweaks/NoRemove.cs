using HarmonyLib;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(Player), nameof(Player.CheckCanRemovePiece))]
public class NoRemove
{
  public const float INFITE = 1E19F;
  private static bool Check(ZNetView view, float defaultValue) => !Configuration.configIgnoreRemove.Value || view.GetZDO().GetFloat(ZDOVars.s_health, defaultValue) < INFITE;
  static void Postfix(Piece piece, ref bool __result)
  {
    if (!__result) return;
    if (!piece || !piece.m_nview.IsValid()) return;
    var id = Game.instance.GetPlayerProfile().GetPlayerID();
    if (id == piece.GetCreator()) return;
    __result = Check(piece.m_nview, 0f);
  }
}
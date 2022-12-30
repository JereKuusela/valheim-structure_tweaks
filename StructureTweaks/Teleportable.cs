using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Teleport))]
public class Teleportable
{
  static int Hash = "override_restrict".GetStableHashCode();
  static void Prefix(TeleportWorld __instance)
  {
    if (!Configuration.configTeleportable.Value) return;
    Helper.Bool(__instance.m_nview, Hash, value =>
    {
      if (!value) ForceTeleportable.Force = true;
    });
  }
  static void Postfix()
  {
    ForceTeleportable.Force = false;
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsTeleportable))]
public class ForceTeleportable
{
  public static bool Force = false;
  static bool Postfix(bool result) => result || Force;
}
using System.Collections.Generic;
using HarmonyLib;
using Service;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Awake))]
public class FixStonePortal {
  static void Postfix(TeleportWorld __instance) {
    if (!__instance.m_proximityRoot)
      __instance.m_proximityRoot = __instance.transform;
    if (!__instance.m_target_found) {
      var tr = __instance.transform.Find("_target_found");
      tr.gameObject.SetActive(true);
      var fade = tr.gameObject.AddComponent<EffectFade>();
      fade.m_fadeDuration = 1f;
      __instance.m_target_found = fade;
    }
  }
}
[HarmonyPatch(typeof(ZDOMan), nameof(ZDOMan.GetAllZDOsWithPrefabIterative))]
public class FixStonePortalConnect {
  static void Prefix(ZDOMan __instance, string prefab, List<ZDO> zdos, int index) {
    if (prefab == Game.instance.m_portalPrefab.name) {
      __instance.GetAllZDOsWithPrefabIterative("portal", zdos, ref index);
    }
  }
}
[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Teleport))]
public class Teleportable {
  static int Hash = "override_restrict".GetStableHashCode();
  static void Prefix(TeleportWorld __instance) {
    if (!Configuration.configTeleportable.Value) return;
    Helper.Bool(__instance.m_nview, Hash, value => {
      if (!value) ForceTeleportable.Force = true;
    });
  }
  static void Postfix() {
    ForceTeleportable.Force = false;
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsTeleportable))]
public class ForceTeleportable {
  public static bool Force = false;
  static bool Postfix(bool result) => result || Force;
}
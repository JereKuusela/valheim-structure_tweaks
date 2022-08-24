using HarmonyLib;

namespace StructureTweaks;

[HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Awake))]
public class CreatureSpawnerAwake {
  static int HashSpawn = "override_spawn".GetStableHashCode();
  static int HashRespawn = "override_respawn".GetStableHashCode();
  static int HashMinLevel = "override_minimum_level".GetStableHashCode();
  static int HashMaxLevel = "override_maximum_level".GetStableHashCode();
  static void HandleSpawn(CreatureSpawner obj) {
    var hash = obj.m_nview.GetZDO().GetInt(HashSpawn, 0);
    if (hash == 0) return;
    var prefab = ZNetScene.instance.GetPrefab(hash);
    if (!prefab) return;
    obj.m_creaturePrefab = prefab;
  }
  static void HandleRespawn(CreatureSpawner obj) {
    var value = obj.m_nview.GetZDO().GetFloat(HashRespawn, -1f);
    if (value < 0) return;
    obj.m_respawnTimeMinuts = value;
  }
  static void HandleMinLevel(CreatureSpawner obj) {
    var value = obj.m_nview.GetZDO().GetInt(HashMinLevel, -1);
    if (value < 0) return;
    obj.m_minLevel = value;
  }
  static void HandleMaxLevel(CreatureSpawner obj) {
    var value = obj.m_nview.GetZDO().GetInt(HashMaxLevel, -1);
    if (value < 0) return;
    obj.m_maxLevel = value;
  }
  static void Postfix(CreatureSpawner __instance) {
    if (!Configuration.configSpawnPoint.Value) return;
    if (!__instance.m_nview || !__instance.m_nview.IsValid()) return;
    HandleRespawn(__instance);
    HandleSpawn(__instance);
    HandleMaxLevel(__instance);
    HandleMinLevel(__instance);
  }
}

[HarmonyPatch(typeof(Pickable), nameof(Pickable.Awake))]
public class PickableAwake {
  static int HashSpawn = "override_spawn".GetStableHashCode();
  static int HashRespawn = "override_respawn".GetStableHashCode();
  static int HashAmount = "override_amount".GetStableHashCode();
  static void HandleSpawn(Pickable obj) {
    var hash = obj.m_nview.GetZDO().GetInt(HashSpawn, -1);
    if (hash < 0) return;
    var prefab = ZNetScene.instance.GetPrefab(hash);
    if (!prefab) return;
    obj.m_itemPrefab = prefab;
  }
  static void HandleRespawn(Pickable obj) {
    var value = obj.m_nview.GetZDO().GetFloat(HashRespawn, -1f);
    if (value < 0) return;
    obj.m_respawnTimeMinutes = (int)value;
  }
  static void HandleAmount(Pickable obj) {
    var value = obj.m_nview.GetZDO().GetFloat(HashRespawn, -1f);
    if (value < 0) return;
    obj.m_amount = (int)value;
  }
  static void Postfix(Pickable __instance) {
    if (!Configuration.configPickable.Value) return;
    if (!__instance.m_nview || !__instance.m_nview.IsValid()) return;
    HandleRespawn(__instance);
    HandleSpawn(__instance);
    HandleAmount(__instance);
  }
}
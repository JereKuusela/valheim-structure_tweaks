using System;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetView), nameof(ZNetView.Awake))]
public class ZNetViewAwake
{
  static int HashWeather = "override_weather".GetStableHashCode();
  static int HashEvent = "override_event".GetStableHashCode();
  static int HashEffect = "override_effect".GetStableHashCode();
  static int HashStatus = "override_status".GetStableHashCode();
  static int HashComponent = "override_component".GetStableHashCode();
  static int HashWater = "override_water".GetStableHashCode();
  static void HandleWeather(ZNetView view)
  {
    var str = view.GetZDO().GetString(HashWeather, "");
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(values[0]);
    var zone = obj.AddComponent<EnvZone>();
    zone.m_force = values.Length > 2;
    zone.m_environment = values[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleEvent(ZNetView view)
  {
    var str = view.GetZDO().GetString(HashEvent, "");
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(values[0]);
    obj.AddComponent<EventZone>().m_event = values[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleEffect(ZNetView view)
  {
    var str = view.GetZDO().GetString(HashEffect, "");
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(values[0]);
    var effect = obj.AddComponent<EffectArea>();
    effect.m_collider = collider;
    effect.m_type = (EffectArea.Type)0;
    foreach (var value in values.Skip(1))
    {
      if (Enum.TryParse<EffectArea.Type>(value, true, out var type) && type != EffectArea.Type.None)
        effect.m_type |= type;
    }
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleStatus(ZNetView view)
  {
    var str = view.GetZDO().GetString(HashStatus, "");
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(values[0]);
    var effect = obj.AddComponent<EffectArea>();
    effect.m_collider = collider;
    effect.m_type = (EffectArea.Type)0;
    effect.m_statusEffect = values[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleComponent(ZNetView view)
  {
    var str = view.GetZDO().GetString(HashComponent, "").ToLower(); ;
    if (str == "") return;
    var values = str.Split(',');
    foreach (var value in values)
    {
      if (value == "runestone" && !view.gameObject.GetComponent<RuneStone>()) view.gameObject.AddComponent<RuneStone>();
      if (value == "chest" && !view.gameObject.GetComponent<Container>()) view.gameObject.AddComponent<Container>();
      if (value == "door" && !view.gameObject.GetComponent<Door>()) view.gameObject.AddComponent<Door>();
      if (value == "portal" && !view.gameObject.GetComponent<TeleportWorld>()) view.gameObject.AddComponent<TeleportWorld>();
    }
  }
  static int RoomCrypt = "sunkencrypt_WaterTunnel".GetStableHashCode();
  static string WaterCrypt = "WaterCube_sunkencrypt";
  static int RoomCave = "cave_new_deeproom_bottom_lake".GetStableHashCode();
  static string WaterCave = "WaterCube_cave";
  static void HandleWater(ZNetView view)
  {
    Helper.String(view, HashWater, value =>
    {
      var split = value.Split(',');
      var scale = Helper.TryScale(split, 1);
      var roomHash = 0;
      var water = "";
      if (split[0] == "crypt")
      {
        roomHash = RoomCrypt;
        water = WaterCrypt;
        scale.x *= 2;
        scale.z *= 2;
      }
      if (split[0] == "cave")
      {
        roomHash = RoomCave;
        water = WaterCave;
      }
      if (DungeonDB.instance.m_roomByHash.TryGetValue(roomHash, out var room))
      {
        var tr = room.m_room.transform.Find(water);
        if (tr)
        {
          var obj = UnityEngine.Object.Instantiate(tr.gameObject, view.transform);
          obj.transform.localPosition = Vector3.zero;
          obj.transform.localRotation = Quaternion.identity;
          obj.transform.localScale = scale;
        }
      }
    });
  }
  static void Postfix(ZNetView __instance)
  {
    if (!Configuration.configEffects.Value) return;
    if (!__instance || !__instance.IsValid()) return;
    HandleWeather(__instance);
    HandleEvent(__instance);
    HandleEffect(__instance);
    HandleStatus(__instance);
    HandleComponent(__instance);
    HandleWater(__instance);
  }
}

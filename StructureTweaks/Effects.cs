using System;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Service;
using UnityEngine;

namespace StructureTweaksPlugin;

[HarmonyPatch(typeof(ZNetView), nameof(ZNetView.Awake))]
public class ZNetViewAwake
{
  static void HandleWeather(ZNetView view)
  {
    var str = view.GetZDO().GetString(Hash.Weather);
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    var size = Helper.Float(values[0]);
    var force = values.Length > 2 && !Helper.IsFalsy(values[2]);
    if (view.TryGetComponent<EnvZone>(out var zone))
    {
      if (size != 0f) zone.transform.localScale = Vector3.one * size;
      zone.m_force = force;
      zone.m_environment = values[1];
      return;
    }
    if (size == 0f) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = size;
    zone = obj.AddComponent<EnvZone>();
    zone.m_force = force;
    zone.m_environment = values[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleEvent(ZNetView view)
  {
    var str = view.GetZDO().GetString(Hash.Event);
    if (str == "") return;
    var values = str.Split(',');
    if (values.Length < 2) return;
    var size = Helper.Float(values[0]);
    if (view.TryGetComponent<EventZone>(out var zone))
    {
      if (size != 0f) zone.transform.localScale = Vector3.one * size;
      zone.m_event = values[1];
      return;
    }
    if (size == 0f) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = size;
    obj.AddComponent<EventZone>().m_event = values[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  private static EffectArea.Type ParseType(string[] values)
  {
    var parsed = (EffectArea.Type)0;
    foreach (var value in values)
    {
      if (Enum.TryParse<EffectArea.Type>(value, true, out var type) && type != EffectArea.Type.None)
        parsed |= type;
    }
    return parsed;
  }
  static void HandleEffect(ZNetView view)
  {
    var component = view.GetComponentInChildren<EffectArea>();
    if (component)
    {
      UnityEngine.Object.Destroy(component.m_collider);
      UnityEngine.Object.Destroy(component);
    }

    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    var effect = obj.AddComponent<CustomEffectArea>();
    effect.m_collider = collider;
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;

    var str = view.GetZDO().GetString(Hash.Status);
    if (str != "")
    {
      var values = str.Split(',');
      if (values.Length > 1)
      {
        collider.radius = Math.Max(collider.radius, Helper.Float(values[0]));
        effect.m_playerOnly = effect.m_playerOnly || values.Length > 2 && Helper.IsTruthy(values[values.Length - 1]);
        effect.m_statusEffect = values[1];
        effect.m_statusEffectHash = effect.m_statusEffect.GetStableHashCode();
        effect.m_type = 0;
      }
    }

    str = view.GetZDO().GetString(Hash.Effect);
    if (str != "")
    {
      var values = str.Split(',');
      if (values.Length > 1)
      {
        collider.radius = Math.Max(collider.radius, Helper.Float(values[0]));
        effect.m_type = ParseType(values.Skip(1).ToArray());
        effect.m_playerOnly = effect.m_playerOnly || values.Length > 2 && Helper.IsTruthy(values[values.Length - 1]);
      }
    }
  }

  static void HandleComponent(ZNetView view)
  {
    // Adding components to dungeons wouldn't really work.
    if (view.gameObject.GetComponent<DungeonGenerator>()) return;
    var str = view.GetZDO().GetString(Hash.Component).ToLower();
    if (str == "") return;
    var values = str.Split(',');
    foreach (var value in values)
    {
      if (value == "runestone" && !view.gameObject.GetComponent<RuneStone>()) view.gameObject.AddComponent<RuneStone>();
      if (value == "chest" && !view.gameObject.GetComponent<Container>()) view.gameObject.AddComponent<Container>();
      if (value == "door" && !view.gameObject.GetComponent<Door>()) view.gameObject.AddComponent<Door>();
      if (value == "destroy" && !view.gameObject.GetComponent<TimedDestruction>())
      {
        UnityEngine.Object.Destroy(view.gameObject.GetComponent<Growup>());
        view.gameObject.AddComponent<TimedDestruction>();
      }
      if (value == "portal" && !view.gameObject.GetComponent<TeleportWorld>()) view.gameObject.AddComponent<TeleportWorld>();
      /*if (value == "music" && !view.gameObject.GetComponent<MusicLocation>())
      {
        //var music = view.gameObject.AddComponent<MusicLocation>();
        //music.m_music = Music.MusicType.None;
      }*/
    }
  }
  static readonly int RoomCrypt = "sunkencrypt_WaterTunnel".GetStableHashCode();
  static readonly string WaterCrypt = "WaterCube_sunkencrypt";
  static readonly int RoomCave = "cave_new_deeproom_bottom_lake".GetStableHashCode();
  static readonly string WaterCave = "WaterCube_cave";
  static void HandleWater(ZNetView view)
  {
    Helper.String(view, Hash.Water, value =>
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
          if (Configuration.configWaterHideParent.Value)
          {
            var renderers = view.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
              renderer.enabled = false;
            var colliders = view.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
              collider.isTrigger = true;
          }
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
    HandleComponent(__instance);
    HandleWater(__instance);
    Destroy.Handle(__instance);
  }
}

public class CustomEffectArea : EffectArea
{
  public void OnTriggerExit(Collider collider)
  {
    if (ZNet.instance == null) return;
    if (string.IsNullOrEmpty(m_statusEffect)) return;
    var component = collider.GetComponent<Character>();
    if (!component) return;
    if (!component.IsOwner()) return;
    if (m_playerOnly && !component.IsPlayer()) return;
    var seMan = component.GetSEMan();
    var se = seMan.GetStatusEffect(m_statusEffectHash);
    if (se && se.m_ttl == 0f) seMan.RemoveStatusEffect(m_statusEffectHash);
  }
}

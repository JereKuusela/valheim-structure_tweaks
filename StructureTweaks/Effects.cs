using System;
using HarmonyLib;
using Service;
using UnityEngine;

namespace StructureTweaks;

[HarmonyPatch(typeof(ZNetView), nameof(ZNetView.Awake))]
public class ZNetViewAwake {
  static int HashWeather = "override_weather".GetStableHashCode();
  static int HashEvent = "override_event".GetStableHashCode();
  static int HashEffect = "override_effect".GetStableHashCode();
  static int HashStatus = "override_status".GetStableHashCode();
  static void HandleWeather(ZNetView view) {
    var value = view.GetZDO().GetString(HashWeather, "").Split(',');
    if (value.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(value[0]);
    var zone = obj.AddComponent<EnvZone>();
    zone.m_force = value.Length > 2;
    zone.m_environment = value[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleEvent(ZNetView view) {
    var value = view.GetZDO().GetString(HashEvent, "").Split(',');
    if (value.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(value[0]);
    obj.AddComponent<EventZone>().m_event = value[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleEffect(ZNetView view) {
    var value = view.GetZDO().GetString(HashEffect, "").Split(',');
    if (value.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(value[0]);
    var effect = obj.AddComponent<EffectArea>();
    effect.m_collider = collider;
    if (Enum.TryParse<EffectArea.Type>(value[1], true, out var type))
      effect.m_type = type;
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void HandleStatus(ZNetView view) {
    var value = view.GetZDO().GetString(HashStatus, "").Split(',');
    if (value.Length < 2) return;
    GameObject obj = new();
    var collider = obj.AddComponent<SphereCollider>();
    collider.isTrigger = true;
    collider.radius = Helper.Float(value[0]);
    var effect = obj.AddComponent<EffectArea>();
    effect.m_collider = collider;
    effect.m_statusEffect = value[1];
    obj.transform.parent = view.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
  static void Postfix(ZNetView __instance) {
    if (!Configuration.configEffects.Value) return;
    if (!__instance || !__instance.IsValid()) return;
    HandleWeather(__instance);
    HandleEvent(__instance);
    HandleEffect(__instance);
    HandleStatus(__instance);
  }
}

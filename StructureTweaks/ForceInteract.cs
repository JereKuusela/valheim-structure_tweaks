using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Service;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace StructureTweaksPlugin;

[HarmonyPatch]
public class Unlock
{

  private static bool CheckAccess(Component obj, float radius, bool flash, bool wardCheck)
  {
    var view = obj.GetComponentInParent<ZNetView>();
    var force = false;
    if (Configuration.configWardUnlock.Value)
    {
      Helper.Bool(view, Hash.Unlock, () =>
      {
        force = true;
      });
    }
    return force || PrivateArea.CheckAccess(obj.transform.position, radius, flash, wardCheck);

  }

  private static IEnumerable<CodeInstruction> ReplacePrivateCheck(IEnumerable<CodeInstruction> instructions)
  {
    return new CodeMatcher(instructions).MatchForward(false, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(PrivateArea), nameof(PrivateArea.CheckAccess))))
      .Set(OpCodes.Call, Transpilers.EmitDelegate(CheckAccess).operand)
      .MatchBack(false, new CodeMatch(OpCodes.Ldarg_0))
      .Advance(1)
      .SetOpcodeAndAdvance(OpCodes.Nop)
      .SetOpcodeAndAdvance(OpCodes.Nop)
      .InstructionEnumeration();
  }

  [HarmonyPatch(typeof(Door), nameof(Door.Interact)), HarmonyTranspiler]
  static IEnumerable<CodeInstruction> DoorInteract(IEnumerable<CodeInstruction> instructions) => ReplacePrivateCheck(instructions);
  [HarmonyPatch(typeof(Container), nameof(Container.Interact)), HarmonyTranspiler]
  static IEnumerable<CodeInstruction> ContainerInteract(IEnumerable<CodeInstruction> instructions) => ReplacePrivateCheck(instructions);
  [HarmonyPatch(typeof(Door), nameof(Door.GetHoverText)), HarmonyTranspiler]
  static IEnumerable<CodeInstruction> DoorGetHoverText(IEnumerable<CodeInstruction> instructions) => ReplacePrivateCheck(instructions);
  [HarmonyPatch(typeof(Container), nameof(Container.GetHoverText)), HarmonyTranspiler]
  static IEnumerable<CodeInstruction> ContainerGetHoverText(IEnumerable<CodeInstruction> instructions) => ReplacePrivateCheck(instructions);


  static string OverrideHoverText(string result, ZNetView view, Vector3 point)
  {
    if (!Configuration.configWardUnlock.Value) return result;
    var canEdit = PrivateArea.CheckAccess(point, 0f, false, false);
    if (!canEdit) return result;

    var zdo = view.GetZDO();
    var value = zdo.GetBool(Hash.Unlock);
    if (value)
      return result + Localization.instance.Localize("\n[<color=yellow><b>$KEY_AltPlace + $KEY_Use</b></color>] Remove unlock");
    else
      return result + Localization.instance.Localize("\n[<color=yellow><b>$KEY_AltPlace + $KEY_Use</b></color>] Force unlock");
  }
  [HarmonyPatch(typeof(Door), nameof(Door.GetHoverText)), HarmonyPostfix]
  static string DoorGetHoverText(string result, Door __instance)
  {
    if (!Configuration.configToggleDoorUnlock.Value || !__instance.CanInteract()) return result;
    if (string.IsNullOrEmpty(result)) return result;
    return OverrideHoverText(result, __instance.m_nview, __instance.transform.position);
  }
  [HarmonyPatch(typeof(Container), nameof(Container.GetHoverText)), HarmonyPostfix]
  static string ContainerGetHoverText(string result, Container __instance)
  {
    if (!Configuration.configToggleContainerUnlock.Value || !__instance.m_checkGuardStone) return result;
    if (string.IsNullOrEmpty(result)) return result;
    return OverrideHoverText(result, __instance.m_nview, __instance.transform.position);
  }

  private static bool OverrideInteract(ZNetView view, Vector3 point, bool alt, bool hold, ref bool __result)
  {
    if (!Configuration.configWardUnlock.Value || !alt) return true;
    if (hold) return false;
    var canEdit = PrivateArea.CheckAccess(point, 0f, false, false);
    if (!canEdit) return true;
    if (!view.HasOwner()) view.ClaimOwnership();
    view.InvokeRPC("ST_ForceUnlock", !view.GetZDO().GetBool(Hash.Unlock));
    __result = true;
    return false;
  }
  [HarmonyPatch(typeof(Door), nameof(Door.Interact)), HarmonyPrefix]
  static bool DoorInteract(Door __instance, bool alt, bool hold, ref bool __result)
  {
    if (!Configuration.configToggleDoorUnlock.Value || !__instance.CanInteract()) return true;
    return OverrideInteract(__instance.m_nview, __instance.transform.position, alt, hold, ref __result);
  }
  [HarmonyPatch(typeof(Container), nameof(Container.Interact)), HarmonyPrefix]
  static bool ContainerInteract(Container __instance, bool alt, bool hold, ref bool __result)
  {
    if (!Configuration.configToggleContainerUnlock.Value || !__instance.m_checkGuardStone) return true;
    return OverrideInteract(__instance.m_nview, __instance.transform.position, alt, hold, ref __result);
  }
  static void ForceUnlock(ZNetView view, bool value)
  {
    view.GetZDO().Set(Hash.Unlock, value);
  }
  static void Register(ZNetView view)
  {
    if (!view) return;
    view.Unregister("ST_ForceUnlock");
    view.Register<bool>("ST_ForceUnlock", (uid, value) => ForceUnlock(view, value));
  }
  [HarmonyPatch(typeof(Door), nameof(Door.Awake)), HarmonyPostfix]
  static void DoorAwake(Door __instance)
  {
    Register(__instance.m_nview);
    Helper.Item(__instance.m_nview, Hash.DoorKey, item =>
    {
      __instance.m_keyItem = item;
    });
    Helper.Int(__instance.m_nview, Hash.DoorNoClose, value =>
    {
      __instance.m_canNotBeClosed = value > 0;
    });
    Helper.String(__instance.m_nview, Hash.DoorOpenEffect, value =>
    {
      __instance.m_openEffects = Helper.ParseEffects(value);
    });
    Helper.String(__instance.m_nview, Hash.DoorCloseEffect, value =>
    {
      __instance.m_closeEffects = Helper.ParseEffects(value);
    });
    Helper.String(__instance.m_nview, Hash.DoorLockedEffect, value =>
    {
      __instance.m_lockedEffects = Helper.ParseEffects(value);
    });

  }
  [HarmonyPatch(typeof(Container), nameof(Container.Awake)), HarmonyPostfix]
  static void ContainerAwake(Container __instance)
  {
    Register(__instance.m_nview);
  }
  [HarmonyPatch(typeof(Door), nameof(Door.Open)), HarmonyPostfix]
  static void DoorOpen(Door __instance)
  {
    Helper.Bool(__instance.m_nview, Hash.DoorConsume, () =>
    {
      var key = __instance.m_keyItem?.m_itemData?.m_shared.m_name;
      if (key != null)
        Player.m_localPlayer?.GetInventory().RemoveItem(key, 1);
    });
  }
}
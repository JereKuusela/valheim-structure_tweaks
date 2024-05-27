using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Service;

public struct Discovery
{
  public string name;
  public string pin;
  public Minimap.PinType type;
  public bool showMap;
}
public class Helper
{
  public static bool IsFinite(ZNetView view, float defaultValue) => view.IsValid() && IsFinite(view.GetZDO().GetFloat(ZDOVars.s_health, defaultValue));
  public static bool IsFinite(float value) => 0f <= value && value < 1E19F;
  public static void AddMessage(Terminal context, string message, bool priority = true)
  {
    context.AddString(message);
    var hud = MessageHud.instance;
    if (!hud) return;
    if (priority)
    {
      var items = hud.m_msgQeue.ToArray();
      hud.m_msgQeue.Clear();
      Player.m_localPlayer?.Message(MessageHud.MessageType.TopLeft, message);
      foreach (var item in items)
        hud.m_msgQeue.Enqueue(item);
      hud.m_msgQueueTimer = 10f;
    }
    else
    {
      Player.m_localPlayer?.Message(MessageHud.MessageType.TopLeft, message);
    }
  }
  public static ZNetView GetHover(string mode)
  {
    var hovered = Player.m_localPlayer?.m_hovering;
    if (!hovered || hovered == null) throw new InvalidOperationException("Not hovering anything.");
    var piece = hovered.GetComponent<Piece>();
    if (!piece) throw new InvalidOperationException("Not hovering anything.");
    var view = piece.m_nview;
    if (!view) throw new InvalidOperationException("Not hovering anything.");
    var canEdit = Helper.CanEdit(view, mode);
    if (!canEdit)
      throw new InvalidOperationException("Not allowed to edit.");
    return view;
  }
  public static void Command(string name, string description, Terminal.ConsoleEvent action, Terminal.ConsoleOptionsFetcher? fetcher = null)
  {
    new Terminal.ConsoleCommand(name, description, Helper.Catch(action), optionsFetcher: fetcher);
  }
  public static void AddError(Terminal context, string message, bool priority = true)
  {
    AddMessage(context, $"Error: {message}", priority);
  }
  public static Terminal.ConsoleEvent Catch(Terminal.ConsoleEvent action) =>
    (args) =>
    {
      try
      {
        if (!Player.m_localPlayer) throw new InvalidOperationException("Player not found.");
        action(args);
      }
      catch (InvalidOperationException e)
      {
        Helper.AddError(args.Context, e.Message);
      }
    };

  public static float Float(string arg, float defaultValue = 0f)
  {
    if (!float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
      return defaultValue;
    return result;
  }
  public static int Int(string arg, int defaultValue = 0)
  {
    if (!int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
      return defaultValue;
    return result;
  }

  public static GameObject? GetPrefab(string hashStr)
  {
    if (int.TryParse(hashStr, out var hash)) return GetPrefab(hash);
    return GetPrefab(hashStr.GetStableHashCode());
  }
  public static GameObject? GetPrefab(int hash)
  {
    if (hash == 0) return null;
    var prefab = ZNetScene.instance.GetPrefab(hash);
    if (!prefab) return null;
    return prefab;
  }
  public static Discovery ParseDiscovery(string data)
  {
    var split = data.Split(',');
    Discovery discovery = new()
    {
      name = split[0],
      pin = "",
      type = Minimap.PinType.None
    };
    if (split.Length > 1)
      discovery.pin = split[1];
    if (split.Length > 2 && Enum.TryParse<Minimap.PinType>(split[2], true, out var type))
      discovery.type = type;
    if (split.Length > 3)
      discovery.showMap = Int(split[3], 0) > 0;
    return discovery;
  }

  public static void Float(ZNetView? view, int hash, Action<float> action)
  {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetFloat(hash);
    if (value == 0f) return;
    action(value);
  }
  public static void Int(ZNetView? view, int hash, Action<int> action)
  {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetInt(hash);
    if (value == 0) return;
    action(value);
  }
  public static void Bool(ZNetView? view, int hash, Action action)
  {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetBool(hash);
    if (!value) return;
    action();
  }
  public static bool Bool(ZNetView? view, int hash)
  {
    if (view == null || !view.IsValid()) return false;
    return view.GetZDO().GetBool(hash);
  }
  public static int Int(ZNetView? view, int hash)
  {
    if (view == null || !view.IsValid()) return 0;
    return view.GetZDO().GetInt(hash);
  }
  public static void String(ZNetView? view, int hash, Action<string> action)
  {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetString(hash);
    if (value == "") return;
    action(value);
  }
  public static bool Prefab(ZNetView? view, int hash, Action<GameObject> action)
  {
    if (view == null || !view.IsValid()) return false;
    var value = view.GetZDO().GetInt(hash);
    if (value == 0)
      value = view.GetZDO().GetString(hash).GetStableHashCode();
    var prefab = GetPrefab(value);
    if (prefab == null) return false;
    action(prefab);
    return true;
  }
  public static ItemDrop? GetItem(int hash)
  {
    return GetPrefab(hash)?.GetComponent<ItemDrop>();
  }
  public static void Item(ZNetView view, int hash, Action<ItemDrop> action)
  {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetInt(hash);
    if (value == 0)
      value = view.GetZDO().GetString(hash).GetStableHashCode();
    var item = GetItem(value);
    if (item == null) return;
    action(item);
  }

  public static bool CanEdit(ZNetView? view, string mode)
  {
    if (view == null || !view.IsValid()) return false;
    if (ZNet.instance.IsServer() || StructureTweaksPlugin.Plugin.ConfigSync.IsAdmin || mode == "All") return true;
    var id = Game.instance.GetPlayerProfile().GetPlayerID();
    return view.GetZDO().GetLong(ZDOVars.s_creator) == id;
  }

  public static float TryFloat(string[] args, int index, float defaultValue = 1f)
  {
    if (args.Length <= index) return defaultValue;
    return Float(args[index], defaultValue);
  }
  public static Vector3 TryVectorXZY(string[] args, int index, Vector3 defaultValue)
  {
    var vector = Vector3.zero;
    vector.x = TryFloat(args, index, defaultValue.x);
    vector.y = TryFloat(args, index + 2, defaultValue.y);
    vector.z = TryFloat(args, index + 1, defaultValue.z);
    return vector;
  }
  public static Vector3 TryScale(string[] args, int index) => SanityCheck(TryVectorXZY(args, index, Vector3.zero));
  private static Vector3 SanityCheck(Vector3 scale)
  {
    // Sanity check and also adds support for setting all values with a single number.
    if (scale.x == 0) scale.x = 1;
    if (scale.y == 0) scale.y = scale.x;
    if (scale.z == 0) scale.z = scale.x;
    return scale;
  }

  private static readonly HashSet<string> Truthies = new() {
    "1",
    "true",
    "yes",
    "on"
  };
  public static bool IsTruthy(string value) => Truthies.Contains(value);
  private static readonly HashSet<string> Falsies = new() {
    "0",
    "false",
    "no",
    "off",
    ""
  };
  public static bool IsFalsy(string value) => Falsies.Contains(value);

  public static T Get<T>(ZNetView view) where T : Component
  {
    if (view.TryGetComponent<T>(out var component)) return component;
    return view.gameObject.AddComponent<T>();
  }

  public static EffectList ParseEffects(string data)
  {
    var effects = data.Split('|').Select(effect => ParseEffect(effect)!).Where(effect => effect != null);
    EffectList list = new()
    {
      m_effectPrefabs = effects.ToArray()
    };
    return list;
  }
  public static EffectList.EffectData? ParseEffect(string data)
  {
    var split = data.Split(',');
    EffectList.EffectData effect = new()
    {
      m_prefab = GetPrefab(split[0])
    };
    if (effect.m_prefab == null) return null;
    if (split.Length > 1 && int.TryParse(split[1], out var flag))
    {
      effect.m_randomRotation = (flag & 1) > 0;
      effect.m_inheritParentRotation = (flag & 2) > 0;
      effect.m_scale = (flag & 4) > 0;
      effect.m_inheritParentScale = (flag & 8) > 0;
      effect.m_attach = (flag & 16) > 0;
    }
    if (split.Length > 2 && int.TryParse(split[2], out var variant))
      effect.m_variant = variant;
    if (split.Length > 3)
      effect.m_childTransform = split[3];
    return effect;
  }
}
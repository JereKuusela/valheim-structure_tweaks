using BepInEx;
using HarmonyLib;
using Service;

namespace Plugin;
[HarmonyPatch]
[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
  const string GUID = "structure_tweaks";
  const string NAME = "Structure Tweaks";
  const string VERSION = "1.5";
  public static ServerSync.ConfigSync ConfigSync = new(GUID)
  {
    DisplayName = NAME,
    CurrentVersion = VERSION,
    ModRequired = true,
    IsLocked = true
  };
  public void Awake() {
    ConfigWrapper wrapper = new("structure_config", Config, ConfigSync);
    Configuration.Init(wrapper);
    new Harmony(GUID).PatchAll();
  }
}

[HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
public class SetCommands {
  static void Postfix() {
    new GrowthCommand();
    new WearCommand();
  }
}

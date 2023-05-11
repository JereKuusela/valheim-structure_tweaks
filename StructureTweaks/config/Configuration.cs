using BepInEx.Configuration;
using Service;
namespace StructureTweaksPlugin;
public class Configuration
{
#nullable disable
  public static ConfigEntry<bool> configAllScalable;
  public static ConfigEntry<bool> configNoTargeting;
  public static ConfigEntry<bool> configDisableStructureSystems;
  public static ConfigEntry<bool> configDisableFalling;
  public static ConfigEntry<bool> configGrowth;
  public static ConfigEntry<bool> configWear;
  public static ConfigEntry<bool> configCollision;
  public static ConfigEntry<bool> configRuneStone;
  public static ConfigEntry<bool> configMusic;
  public static ConfigEntry<string> configRuneStoneEditing;
  public static ConfigEntry<bool> configInteract;
  public static ConfigEntry<bool> configWardUnlock;
  public static ConfigEntry<bool> configToggleContainerUnlock;
  public static ConfigEntry<bool> configToggleDoorUnlock;
  public static ConfigEntry<bool> configRendering;
  public static ConfigEntry<bool> configFalling;
  public static ConfigEntry<bool> configEffects;
  public static ConfigEntry<bool> configTeleportable;
  public static ConfigEntry<bool> configSmokeBlock;
  public static ConfigEntry<string> configGrowthEditing;
  public static ConfigEntry<string> configWearEditing;
  public static ConfigEntry<bool> configIgnoreDamage;
  public static ConfigEntry<bool> configIgnoreRemove;
  public static ConfigEntry<bool> configIgnoreSupport;
#nullable enable
  public static void Init(ConfigWrapper wrapper)
  {
    var section = "1. General";
    configAllScalable = wrapper.Bind(section, "All objects can be scaled", true, "Scaling works for every object.");
    configAllScalable.SettingChanged += (s, e) => AllScalable.Update();
    configIgnoreDamage = wrapper.Bind(section, "Ignore damage when infinite health", true, "Damage is fully ignored for objects with infinite health.");
    configIgnoreRemove = wrapper.Bind(section, "Protect pieces with infinite health", true, "Pieces with infinite health can't be deconstructed with the hammer.");
    configIgnoreSupport = wrapper.Bind(section, "Max support with infinite health", true, "Pieces with infinite health have max structure support.");
    configDisableStructureSystems = wrapper.Bind(section, "Disable structure system", false, "Structure systems are disabled for all pieces.");
    configDisableFalling = wrapper.Bind(section, "Disable falling", false, "Falling is disabled for all static objects.");
    configNoTargeting = wrapper.Bind(section, "No enemy targeting when no creator", true, "Enemies won't target neutral structure.");
    configNoTargeting.SettingChanged += (s, e) => NoTargeting.Update();
    configCollision = wrapper.Bind(section, "Override collision", true, "Collision can be overridden (requires reloading the area).");
    configGrowth = wrapper.Bind(section, "Override growth", true, "Growth visual can be overridden.");
    configRuneStone = wrapper.Bind(section, "Override runestones", true, "Runestone properties can be overridden.");
    configMusic = wrapper.Bind(section, "Override music", true, "Music properties can be overridden.");
    configInteract = wrapper.Bind(section, "Override interact", true, "Interactability can be overridden.");
    configRendering = wrapper.Bind(section, "Override rendering", true, "Rendering can be overridden (requires reloading the area).");
    configFalling = wrapper.Bind(section, "Override falling", true, "Falling can be overridden (requires reloading the area).");
    configTeleportable = wrapper.Bind(section, "Override portal restrictions", true, "Teleporting with restricted items can be overridden.");
    configSmokeBlock = wrapper.Bind(section, "Override smoke restrictions", true, "Fireplaces going out can be overridden.");
    configWear = wrapper.Bind(section, "Override wear", true, "Wear visual can be overridden.");
    configEffects = wrapper.Bind(section, "Override effects", true, "New area effects can be added.");
    configWardUnlock = wrapper.Bind(section, "Override unlock", true, "Chests and doors can be force unlocked.");
    section = "2. Commands";
    var editingModes = new string[] { "Admin only", "Owned", "All" };
    configGrowthEditing = wrapper.Bind(section, "Command growth", "Owned", new ConfigDescription("Growth editing", new AcceptableValueList<string>(editingModes)));
    configWearEditing = wrapper.Bind(section, "Command wear", "Owned", new ConfigDescription("Wear editing", new AcceptableValueList<string>(editingModes)));
    configRuneStoneEditing = wrapper.Bind(section, "Allow editing runestones", "Admin only", new ConfigDescription("Runestone editing", new AcceptableValueList<string>(editingModes)));
    configToggleContainerUnlock = wrapper.Bind(section, "Allow unlocking chests", true, "Players can unlock chests to ignore wards.");
    configToggleDoorUnlock = wrapper.Bind(section, "Allow unlocking doors", true, "Players can unlock doors to ignore wards.");

  }
}

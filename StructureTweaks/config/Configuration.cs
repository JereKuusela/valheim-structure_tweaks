using BepInEx.Configuration;
using Service;
namespace StructureTweaks;
public class Configuration {
#nullable disable
  public static ConfigEntry<bool> configAllScalable;
  public static ConfigEntry<bool> configNoTargeting;
  public static ConfigEntry<bool> configNoSpawnPointSuppression;
  public static ConfigEntry<bool> configGrowth;
  public static ConfigEntry<bool> configWear;
  public static ConfigEntry<bool> configCollision;
  public static ConfigEntry<bool> configInteract;
  public static ConfigEntry<bool> configRendering;
  public static ConfigEntry<bool> configFalling;
  public static ConfigEntry<bool> configSpawnPoint;
  public static ConfigEntry<bool> configEffects;
  public static ConfigEntry<bool> configPickable;
  public static ConfigEntry<bool> configTeleportable;
  public static ConfigEntry<bool> configCommandGrowth;
  public static ConfigEntry<bool> configCommandWear;
  public static ConfigEntry<bool> configIgnoreDamage;
  public static ConfigEntry<bool> configIgnoreRemove;
  public static ConfigEntry<bool> configIgnoreSupport;
#nullable enable
  public static void Init(ConfigWrapper wrapper) {
    var section = "1. General";
    configAllScalable = wrapper.Bind(section, "All objects can be scaled", true, "Scaling works for every object.");
    configAllScalable.SettingChanged += (s, e) => AllScalable.Update();
    configIgnoreDamage = wrapper.Bind(section, "Ignore damage when infinite health", true, "Damage is fully ignored for objects with infinite health.");
    configIgnoreRemove = wrapper.Bind(section, "Protect pieces with infinite health", true, "Pieces with infinite health can't be deconstructed with the hammer.");
    configIgnoreSupport = wrapper.Bind(section, "Max support with infinite health", true, "Pieces with infinite health have max structure support.");
    configNoTargeting = wrapper.Bind(section, "No enemy targeting when no creator", true, "Enemies won't target neutral structure.");
    configNoTargeting.SettingChanged += (s, e) => NoTargeting.Update();
    configNoSpawnPointSuppression = wrapper.Bind(section, "No spawn point suppression", true, "Spawn points can't be suppressed with player base (excludes normally respawning).");
    configNoSpawnPointSuppression.SettingChanged += (s, e) => NoSuppression.Update();
    configCollision = wrapper.Bind(section, "Override collision", true, "Collision can be overridden (requires reloading the area).");
    configGrowth = wrapper.Bind(section, "Override growth", true, "Growth visual can be overridden.");
    configInteract = wrapper.Bind(section, "Override interact", true, "Interactability can be overridden.");
    configRendering = wrapper.Bind(section, "Override rendering", true, "Rendering can be overridden (requires reloading the area).");
    configFalling = wrapper.Bind(section, "Override falling", true, "Falling can be overridden (requires reloading the area).");
    configTeleportable = wrapper.Bind(section, "Override portal restrictions", true, "Teleporting with restricted items can be overridden.");
    configSpawnPoint = wrapper.Bind(section, "Override spawn points", true, "Spawn point creature, respawn time and stars can be overridden.");
    configPickable = wrapper.Bind(section, "Override pickables", true, "Pickable drops, respawn and amount can be overridden.");
    configWear = wrapper.Bind(section, "Override wear", true, "Wear visual can be overridden.");
    configEffects = wrapper.Bind(section, "Override effects", true, "New area effects can be added.");
    section = "2. Commands";
    configCommandGrowth = wrapper.Bind(section, "Command growth", true, "Allow players to override growth for their own plants.");
    configCommandWear = wrapper.Bind(section, "Command wear", true, "Allow players to override wear for their own structures.");
  }
}

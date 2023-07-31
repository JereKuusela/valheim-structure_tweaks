namespace Service;

public static class Hash
{
  public static readonly int Name = "override_name".GetStableHashCode();
  // string
  public static readonly int Destroy = "override_destroy".GetStableHashCode();
  public static readonly int DestroyEffect = "override_destroy_effect".GetStableHashCode();
  // prefab,flags,variant,childTransform|prefab,flags,variant,childTransform|...
  public static readonly int DungeonWeather = "override_dungeon_weather".GetStableHashCode();
  public static readonly int EnterText = "override_dungeon_enter_text".GetStableHashCode();
  public static readonly int EnterHover = "override_dungeon_enter_hover".GetStableHashCode();
  public static readonly int ExitText = "override_dungeon_exit_text".GetStableHashCode();
  public static readonly int ExitHover = "override_dungeon_exit_hover".GetStableHashCode();
  public static readonly int Weather = "override_weather".GetStableHashCode();
  public static readonly int Event = "override_event".GetStableHashCode();
  public static readonly int Effect = "override_effect".GetStableHashCode();
  public static readonly int Status = "override_status".GetStableHashCode();
  public static readonly int Component = "override_component".GetStableHashCode();
  public static readonly int Water = "override_water".GetStableHashCode();
  public static readonly int Fall = "override_fall".GetStableHashCode();
  public static readonly int Unlock = "override_unlock".GetStableHashCode();
  public static readonly int Growth = "override_growth".GetStableHashCode();

  public static readonly int Radius = "override_music_radius".GetStableHashCode();
  // float
  public static readonly int Condition = "override_music_condition".GetStableHashCode();
  // int (1 = one time, 2 = not if enemies, 3 = force fade)
  public static readonly int Audio = "override_music_audio".GetStableHashCode();
  // string

  public static readonly int NoCollision = "override_collision".GetStableHashCode();
  public static readonly int Portal = "portal_wood".GetStableHashCode();
  public static readonly int NoInteract = "override_interact".GetStableHashCode();
  public static readonly int NoRender = "override_render".GetStableHashCode();
  public static readonly int Text = "override_text".GetStableHashCode();
  // string
  public static readonly int Topic = "override_topic".GetStableHashCode();
  // string
  public static readonly int Compendium = "override_compendium".GetStableHashCode();
  // string
  public static readonly int Discover = "override_discover".GetStableHashCode();
  // string
  public static readonly int Smoke = "override_smoke".GetStableHashCode();
  // 0 = on, 1 = off, 2 = no block
  public static readonly int Restrict = "override_restrict".GetStableHashCode();
  public static readonly int Wear = "override_wear".GetStableHashCode();
}
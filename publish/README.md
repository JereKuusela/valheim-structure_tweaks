# Structure Tweaks

Changes the game logic to allow invisible, invulnerable, non-colliding, non-interactable or untargetable objects. Also allow overriding visual wear and plant growth.

Install on all clients and on the server (modding [guide](https://youtu.be/L9ljm2eKLrk)).

# Config

- All objects can be scaled : Scaling works for every object.
- Disable structure system: Structure systems are disabled for all pieces. Not enabled by default.
- Ignore damage when infinite health: Damage is fully ignored for objects with infinite health.
- Protect pieces with infinite health: Pieces with infinite health can't be deconstructed (except by the creator).
- Max support with infinite health: Pieces with infinite health have max structure support.
- No enemy targeting when no creator: Enemies won't target neutral structures.
- Override collision: Collision can be overridden (requires reloading the area).
- Override effects: New area effects can be added.
- Override falling: Object fall behavior can be overridden.
- Override growth: Growth visual can be overridden.
- Override interact: Interactability can be overridden.
- Override portal restrictions: Teleporting with restricted items can be overridden.
- Override smoke restrictions: Fireplaces going out can be overridden.
- Override rendering: Rendering can be overridden (requires reloading the area).
- Override runestones: Runestone properties can be overridden.
- Override unlock: Chests and doors can be force unlocked.
- Override wear: Wear visual can be overridden.
- Allow unlocking chests: Players can unlock chests to ignore wards.
- Allow unlocking doors: Players can unlock doors to ignore wards.
- Allow editing runestones: Runestone editing (Admin only, Owned, All).
- Command growth: Growth editing (Admin only, Owned, All).
- Command wear: Wear editing (Admin only, Owned, All).

# Commands

Two commands that allow changing visual growth or wear of own plants and structures. This can be disabled from the config.

- `growth=[big/big_bad/small/small_bad]`: Overrides the plant growth. Prevents growing up.
- `wear=[broken/damaged/healthy]`: Overrides the wear health.

For other changes you need mods like [Infinity Hammer](https://valheim.thunderstore.io/package/JereKuusela/Infinity_Hammer/) or [World Edit Commands](https://valheim.thunderstore.io/package/JereKuusela/World_Edit_Commands/).

# Credits

Thanks for Azumatt for creating the mod icon!

Sources: [GitHub](https://github.com/JereKuusela/valheim-structure_tweaks)

Donations: [Buy me a computer](https://www.buymeacoffee.com/jerekuusela)

# Changelog

- v1.11
  - Fixes experience gain from infinite health object.
  - Fixes error when using mods that add multiple chests per object.

- v1.10
  - Fixes null error with some mods.

- v1.9
  - Adds support for adding a custom sized water surface.
  - Fixes the black screen.

- v1.8
  - `Ignore damage when infinite health` makes creatures permanently dodging to prevent farming experience.
  - Fixes compatibility issue with Hidden Doors mod pieces.

- v1.7
  - Adds a new setting to entirely disable structure systems.
  - Fixes compatibility issue with VALKEA mod pieces.

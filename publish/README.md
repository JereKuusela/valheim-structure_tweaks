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

- v1.9
	- Adds support for adding a custom sized water surface.
	- Fixes the black screen.

- v1.8
	- `Ignore damage when infinite health` makes creatures permanently dodging to prevent farming experience.
	- Fixes compatibility issue with Hidden Doors mod pieces.

- v1.7
	- Adds a new setting to entirely disable structure systems.
	- Fixes compatibility issue with VALKEA mod pieces.

- v1.6
	- Fixes the version check failing.

- v1.5
	- Adds support for multiple effect areas.
	- Adds support for runestones.
	- Adds support for overriding fireplace smoke.
	- Adds support for force unlocking chests and doors (overrides wards).
	- Removes pickables and spawn points (split off to Spawner Tweaks mod).

- v1.4
	- Adds overriding for spawn points.
	- Adds overriding for pickables.
	- Adds adding of effect, event, status and weather areas.
	- Adds no spawn point suppression.
	- Fixes all scaling being (usually) always enabled (now for real).
	- Fixes no targeting being (usually) always enabled (now for real).

- v1.3
	- Adds hammer protection to pieces with infinite health.
	- Adds fall override.
	- Adds portal item restrictions override.
	- Fixes all scaling being (usually) always enabled.
	- Fixes no targeting being (usually) always enabled.

- v1.2
	- Fixes plants not growing up.

- v1.1
	- Adds commands `growth` and `wear`.
	- Adds scaling to all objects.
	- Adds collision override.
	- Adds growth override.
	- Adds interaction override.
	- Adds render override.
	- Adds wear override.

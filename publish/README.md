# Structure Tweaks

Changes the game logic to allow invisible, invulnerable, non-colliding, non-interactable or untargetable objects. Also allow overriding visual wear and plant growth.

Install on all clients and on the server (modding [guide](https://youtu.be/L9ljm2eKLrk)).

# Features

Each feature can be disabled from the config.

- Objects with more than 10 000 000 000 000 000 000 (1E19) health fully ignore any damage (status effects are not applied and damage numbers are not shown).
- Pieces without creator won't be directly targeted by the enemies.
- Objects can be modified to be invisible.
- Objects can be modified to remove interaction.
- Objects can be modified to have no collision.
- Object visual wear can be overridden.
- Plant visual growth can be overridden.

# Commands

Two commands that allow changing visual growth or wear of own plants and structures. This can be disabled from the config.

- `growth=[big/big_bad/small/small_bad`: Overrides the plant growth. Prevents growing up.
- `wear=[broken/damaged/healthy`: Overrides the wear health.

For other changes you need mods like [Infinity Hammer](https://valheim.thunderstore.io/package/JereKuusela/Infinity_Hammer/) or [World Edit Commands](https://valheim.thunderstore.io/package/JereKuusela/World_Edit_Commands/).

# Config

- All objects can be scaled : Scaling works for every object (requires restart).
- Ignore damage when infinite health : amage is fully ignored for objects with infinite health.
- No enemy targeting when no creator : Enemies won't target neutral structure (requires restart).
- Override collision : Collision can be overridden (requires reloading the area).
- Override growth : Growth visual can be overridden.
- Override interact : Interactability can be overridden.
- Override rendering : Rendering can be overridden (requires reloading the area).
- Override wear : Wear visual can be overridden.
- Command growth : Allow players to override growth for their own plants.
- Command wear : Allow players to override wear for their own structures.

# Changelog

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

- v1.0
	- Initial release.

Thanks for Azumatt for creating the mod icon!
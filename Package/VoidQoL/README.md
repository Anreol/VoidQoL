# VoidQoL

(Mostly!) Server side and Vanilla compatible.

If clients don't have the mod, the Signal charge zone in Void Locus type will always be a sphere for them.

Based on Rob's VoidFieldsQoL!

## Features
- Increase Void Cell charge with enemy kills.
- Heal everyone within the Void Cell each round start.
- Option to revive every dead player on each round start.
- Option to revive every dead player after the arena has been beaten.
- Option to multiply the Void Cell's charging radius.
- Option to give enemies a speed boost when spawning.

And for Void Locus:
- Increase Signal charge with enemy kills. (D: False)
- Ban drones and other NPCs from entering Void Locus, so they don't get killed by the fog. (D: False)
- Option to decrease signal radius if an enemy steps inside. (D: True)
- Option to change the charge zone from a sphere to a tube (like Moon's escape sequence or Simulacrum). (D: True)
- Option to give players inside the fog a speed boost whenever a signal gets activated. (D: True)
- Option to increase or decrease the radius in meters. (D: +5M) 
- Option to increase charge automatically every second. (D: 0/s)
- Option to change the discharge rate of the zone. (D: -0.005)
- Option to change the player scaling of the zone. (D: 0.75)

## Planned Features
- Add options for Survivors of the Void Void Camps, such as minimum stage for void camps to appear, count, and others.

### Contact
You can contact me by messaging to Anreol#8231 on Discord chat web application.

## Changelog
**1.1.6**
- Recompiled for ver. 1.3.1#274

**1.1.5**
- Changed enemy haste check to only apply to bodies in teams enemy to players, instead of everything but players.
- Added a couple of user-requested features, might need a config regeneration, no idea:
- Should charge on kill increase depending on enemy size / is champion or not. (voidFieldsIncreaseChargeBasedOnSize)
- Flat charge percentage per kill. (voidFieldsIncreaseChargePercentagePerKill)
- Duration of the cloak speed buff. Default value has been increased from 12 to 16 (voidFieldsEnemyHasteDuration)

**1.1.4**
- Updated libraries to avoid the missing tier field error.
- That's the update.

**1.1.3**
- Fixed all void items getting removed upon arriving on void locus if the remove void items from monsters config was enabled
- Added a config to ban NPCs from entering Void Locus

**1.1.2**
- Void enemies in Void Locus now give less charge on kill as those void enemies are bigger in comparison to the small ones in Void Fields.
- Void Locus charge on kill now should no longer instantly charge the cell.
- FOR THE LOVE OF GOD, MAY THE REVIVAL ON ROUND END FINALLY WORK IN VOID FIELDS. GOD.

**1.1.1**
- Fixed clients not seeing the changed Void Locus zones
- Fixed charge on death applying in Void Locus regardless of configuration
- Added a configuration to remove void items from all monsters inside void locus.
- Players in void fog now get 5 stacks of haste, instead of just 1.

**1.1.0**
Void Locus Update
- Added config for the following:
	- Increase signal charge with kills, default to false.
	- Give players that are in the fog as a signal gets activated a speed boost, default to true.
	- Decrease signal radius if a enemy steps inside, default to true.
	- Change zone type from a sphere to a tube, default to true.
	- Additional signal radius size, default to +5m.
	- Auto charge every second, default to zero.
	- Signal charge rate player scaling, default to 0.75.
	- Signal discharge rate, default to -0.005.
	
*All these features, with its default values, are meant to change Void Locus into something more interesting that isn't stand inside small sphere for 2 minutes. Anybody can do anything with these to fit their playstyle.*

- Void Fields
	- Void cell healing now heal a tiny amount for every player, inside void vent or not.
	- Added a config to give slow enemies a speed boost when spawning.
	- Kills add a 0.5 more charge to the void vent charge.

**1.0.3**
- Fixed the config sometimes not generating (wtf?)
- This should address non-default configs like revives so they work now

**1.0.2**
- Changed some logic in the script in an attempt to make sure that on cell activation effects got executed once.

**1.0.1**
Quick Patch
- Fixed charge amounts gained from enemies getting floored to integer.
- Increased void vent healing on round start.

**1.0.0**
* Initial Release
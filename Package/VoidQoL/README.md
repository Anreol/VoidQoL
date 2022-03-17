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
You can contact me by messaging to Anreol#8231 or @anreol:poa.st

## Changelog
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
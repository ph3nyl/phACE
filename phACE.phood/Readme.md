# phACE.phood

## description:
adds three "vital saturation" pools to player characters that add to passive vital regen while they have value. 
saturation is increased by eating food, with better food being necessary to reach higher saturation levels. 
additionally has a "well fed" spell that selfcasts when all saturation pools are above 25%.

**wip**: *adds new recipes for cook-only food/drinks*

## settings:
|setting                        |default    |range  |   notes
|-------------------------------|-----------|-------|--------
|EnableVitalSaturation          |true       |t/f    |
|BonusRegenHealth               |2.5d       |0-#?   |hp per tick (currently unaffected by regen multipliers)
|BonusRegenStamina              |2.5d       |0-#?   |sp per tick (currently unaffected by regen multipliers)
|BonusRegenMana                 |2.5d       |0-#?   |mp per tick (currently unaffected by regen multipliers)
|MaxRegenPoolValue              |3600.0d    |0-#?   |well fed ticks on/off at 25% of this
|BoostToSatConversionFactor     |6.0d       |0-#?   |converts vital boost on food to vital saturation
|SpiceOfLife                    |10.0d      |0-#?   |max saturation contribution of a specific food item
|                               |           |       |
|EnableScalingAttributeBeers    |false      |t/f    |wip

## commands:
- @sat @saturation @food @hunger
	- displays character's current saturation pool levels in chat log.

## special thanks:
- **aquafir**. extremely helpful in pointing my brute-force haphazard energy in the right direction to actually make progress. thanks, green bro.
- **hells**. you keep trying to talk me into making things with trevismagic imgui and instead i made a bunch of chat log messages. deal with it nerd.
- **dekaru**. your implementation of a similar feature in classicACE/customDM is what pushed me to make this. hope you come back from your bike ride.

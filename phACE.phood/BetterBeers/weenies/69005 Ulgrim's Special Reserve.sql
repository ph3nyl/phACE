DELETE FROM `weenie` WHERE `class_Id` = 69005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69005, 'phACE.phood-UlgrimsSpecialReserveT6', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69005,   1,         32) /* ItemType - Food */
     , (69005,   5,         50) /* EncumbranceVal */
     , (69005,  11,        100) /* MaxStackSize */
     , (69005,  12,          1) /* StackSize */
     , (69005,  13,         50) /* StackUnitEncumbrance */
     , (69005,  15,         10) /* StackUnitValue */
     , (69005,  16,          8) /* ItemUseable - Contained */
     , (69005,  18,          1) /* UiEffects - Magical */
     , (69005,  19,         10) /* Value */
     , (69005,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69005, 105,          6) /* ItemWorkmanship */
     , (69005, 106,        250) /* ItemSpellcraft */
     , (69005, 109,          0) /* ItemDifficulty */
	 , (69005, 366,         39) /* UseRequiresSkill - Cooking */
     , (69005, 367,        200) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69005,  11, True ) /* IgnoreCollisions */
     , (69005,  13, True ) /* Ethereal */
     , (69005,  14, True ) /* GravityStatus */
     , (69005,  19, True ) /* Attackable */
     , (69005,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69005,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69005,  14, 'Use this item to drink it.') /* Use */
     , (69005,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69005,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69005,   1, 0x02001258) /* Setup */
     , (69005,   3, 0x20000014) /* SoundTable */
     , (69005,   8, 0x06005A65) /* Icon */
     , (69005,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69005,  23,         65) /* UseSound - Drink1 */
     , (69005,  50, 0x06005EC2) /* IconOverlay */
     , (69005,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69005,  1332,      2) /* Strength Self VI */
     , (69005,  1354,      2) /* Endurance Self VI */
     , (69005,  1378,      2) /* Coordination Self VI */
     , (69005,  1402,      2) /* Quickness Self VI */
     , (69005,  1426,      2) /* Focus Self VI */
     , (69005,  1450,      2) /* Willpower Self VI */;

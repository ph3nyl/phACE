DELETE FROM `weenie` WHERE `class_Id` = 69008;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69008, 'phACE.phood-UlgrimsSpecialReserveT3', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69008,   1,         32) /* ItemType - Food */
     , (69008,   5,         50) /* EncumbranceVal */
     , (69008,  11,        100) /* MaxStackSize */
     , (69008,  12,          1) /* StackSize */
     , (69008,  13,         50) /* StackUnitEncumbrance */
     , (69008,  15,         10) /* StackUnitValue */
     , (69008,  16,          8) /* ItemUseable - Contained */
     , (69008,  18,          1) /* UiEffects - Magical */
     , (69008,  19,         10) /* Value */
     , (69008,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69008, 105,          3) /* ItemWorkmanship */
     , (69008, 106,        100) /* ItemSpellcraft */
     , (69008, 109,          0) /* ItemDifficulty */
	 , (69008, 366,         39) /* UseRequiresSkill - Cooking */
     , (69008, 367,         50) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69008,  11, True ) /* IgnoreCollisions */
     , (69008,  13, True ) /* Ethereal */
     , (69008,  14, True ) /* GravityStatus */
     , (69008,  19, True ) /* Attackable */
     , (69008,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69008,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69008,  14, 'Use this item to drink it.') /* Use */
     , (69008,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69008,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69008,   1, 0x02001258) /* Setup */
     , (69008,   3, 0x20000014) /* SoundTable */
     , (69008,   8, 0x06005A65) /* Icon */
     , (69008,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69008,  23,         65) /* UseSound - Drink1 */
     , (69008,  50, 0x06005EC2) /* IconOverlay */
     , (69008,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69008,  1329,      2) /* Strength Self III */
     , (69008,  1351,      2) /* Endurance Self III */
     , (69008,  1375,      2) /* Coordination Self III */
     , (69008,  1399,      2) /* Quickness Self III */
     , (69008,  1423,      2) /* Focus Self III */
     , (69008,  1447,      2) /* Willpower Self III */;

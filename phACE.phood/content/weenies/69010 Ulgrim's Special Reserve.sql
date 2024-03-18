DELETE FROM `weenie` WHERE `class_Id` = 69010;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69010, 'phACE.phood-UlgrimsSpecialReserveT1', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69010,   1,         32) /* ItemType - Food */
     , (69010,   5,         50) /* EncumbranceVal */
     , (69010,  11,        100) /* MaxStackSize */
     , (69010,  12,          1) /* StackSize */
     , (69010,  13,         50) /* StackUnitEncumbrance */
     , (69010,  15,         10) /* StackUnitValue */
     , (69010,  16,          8) /* ItemUseable - Contained */
     , (69010,  18,          1) /* UiEffects - Magical */
     , (69010,  19,         10) /* Value */
     , (69010,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69010, 105,          1) /* ItemWorkmanship */
     , (69010, 106,         50) /* ItemSpellcraft */
     , (69010, 109,          0) /* ItemDifficulty */
	 , (69010, 366,         39) /* UseRequiresSkill - Cooking */
     , (69010, 367,          0) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69010,  11, True ) /* IgnoreCollisions */
     , (69010,  13, True ) /* Ethereal */
     , (69010,  14, True ) /* GravityStatus */
     , (69010,  19, True ) /* Attackable */
     , (69010,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69010,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69010,  14, 'Use this item to drink it.') /* Use */
     , (69010,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69010,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69010,   1, 0x02001258) /* Setup */
     , (69010,   3, 0x20000014) /* SoundTable */
     , (69010,   8, 0x06005A65) /* Icon */
     , (69010,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69010,  23,         65) /* UseSound - Drink1 */
     , (69010,  50, 0x06005EC2) /* IconOverlay */
     , (69010,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69010,     2,      2) /* Strength Self I */
     , (69010,  1349,      2) /* Endurance Self I */
     , (69010,  1373,      2) /* Coordination Self I */
     , (69010,  1397,      2) /* Quickness Self I */
     , (69010,  1421,      2) /* Focus Self I */
     , (69010,  1445,      2) /* Willpower Self I */;

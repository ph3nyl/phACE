DELETE FROM `weenie` WHERE `class_Id` = 69002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69002, 'phACE.phood-UlgrimsSpecialReserve', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69002,   1,         32) /* ItemType - Food */
     , (69002,   5,         50) /* EncumbranceVal */
     , (69002,  11,        100) /* MaxStackSize */
     , (69002,  12,          1) /* StackSize */
     , (69002,  13,         50) /* StackUnitEncumbrance */
     , (69002,  15,         10) /* StackUnitValue */
     , (69002,  16,          8) /* ItemUseable - Contained */
     , (69002,  18,          1) /* UiEffects - Magical */
     , (69002,  19,         10) /* Value */
     , (69002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69002, 105,          0) /* ItemWorkmanship */
     , (69002, 106,        250) /* ItemSpellcraft */
     , (69002, 109,          0) /* ItemDifficulty */
	 , (69002, 366,         39) /* UseRequiresSkill - Cooking */
     , (69002, 367,          0) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69002,  11, True ) /* IgnoreCollisions */
     , (69002,  13, True ) /* Ethereal */
     , (69002,  14, True ) /* GravityStatus */
     , (69002,  19, True ) /* Attackable */
     , (69002,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69002,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69002,  14, 'Use this item to drink it.') /* Use */
     , (69002,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69002,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69002,   1, 0x02001258) /* Setup */
     , (69002,   3, 0x20000014) /* SoundTable */
     , (69002,   8, 0x06005A65) /* Icon */
     , (69002,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69002,  23,         65) /* UseSound - Drink1 */
     , (69002,  50, 0x06005EC2) /* IconOverlay */
     , (69002,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69002,  3864,      2)
     , (69002,  3863,      2)
     , (69002,  3533,      2)
     , (69002,  3531,      2)
     , (69002,  3530,      2)
     , (69002,  3862,      2);

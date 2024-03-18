DELETE FROM `weenie` WHERE `class_Id` = 69006;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69006, 'phACE.phood-UlgrimsSpecialReserveT5', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69006,   1,         32) /* ItemType - Food */
     , (69006,   5,         50) /* EncumbranceVal */
     , (69006,  11,        100) /* MaxStackSize */
     , (69006,  12,          1) /* StackSize */
     , (69006,  13,         50) /* StackUnitEncumbrance */
     , (69006,  15,         10) /* StackUnitValue */
     , (69006,  16,          8) /* ItemUseable - Contained */
     , (69006,  18,          1) /* UiEffects - Magical */
     , (69006,  19,         10) /* Value */
     , (69006,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69006, 105,          5) /* ItemWorkmanship */
     , (69006, 106,        200) /* ItemSpellcraft */
     , (69006, 109,          0) /* ItemDifficulty */
	 , (69006, 366,         39) /* UseRequiresSkill - Cooking */
     , (69006, 367,        150) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69006,  11, True ) /* IgnoreCollisions */
     , (69006,  13, True ) /* Ethereal */
     , (69006,  14, True ) /* GravityStatus */
     , (69006,  19, True ) /* Attackable */
     , (69006,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69006,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69006,  14, 'Use this item to drink it.') /* Use */
     , (69006,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69006,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69006,   1, 0x02001258) /* Setup */
     , (69006,   3, 0x20000014) /* SoundTable */
     , (69006,   8, 0x06005A65) /* Icon */
     , (69006,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69006,  23,         65) /* UseSound - Drink1 */
     , (69006,  50, 0x06005EC2) /* IconOverlay */
     , (69006,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69006,  1331,      2) /* Strength Self V */
     , (69006,  1353,      2) /* Endurance Self V */
     , (69006,  1377,      2) /* Coordination Self V */
     , (69006,  1401,      2) /* Quickness Self V */
     , (69006,  1425,      2) /* Focus Self V */
     , (69006,  1449,      2) /* Willpower Self V */;

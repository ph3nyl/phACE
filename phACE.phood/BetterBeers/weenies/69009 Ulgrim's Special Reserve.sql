DELETE FROM `weenie` WHERE `class_Id` = 69009;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69009, 'phACE.phood-UlgrimsSpecialReserveT2', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69009,   1,         32) /* ItemType - Food */
     , (69009,   5,         50) /* EncumbranceVal */
     , (69009,  11,        100) /* MaxStackSize */
     , (69009,  12,          1) /* StackSize */
     , (69009,  13,         50) /* StackUnitEncumbrance */
     , (69009,  15,         10) /* StackUnitValue */
     , (69009,  16,          8) /* ItemUseable - Contained */
     , (69009,  18,          1) /* UiEffects - Magical */
     , (69009,  19,         10) /* Value */
     , (69009,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69009, 105,          2) /* ItemWorkmanship */
     , (69009, 106,         50) /* ItemSpellcraft */
     , (69009, 109,          0) /* ItemDifficulty */
	 , (69009, 366,         39) /* UseRequiresSkill - Cooking */
     , (69009, 367,          0) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69009,  11, True ) /* IgnoreCollisions */
     , (69009,  13, True ) /* Ethereal */
     , (69009,  14, True ) /* GravityStatus */
     , (69009,  19, True ) /* Attackable */
     , (69009,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69009,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69009,  14, 'Use this item to drink it.') /* Use */
     , (69009,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69009,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69009,   1, 0x02001258) /* Setup */
     , (69009,   3, 0x20000014) /* SoundTable */
     , (69009,   8, 0x06005A65) /* Icon */
     , (69009,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69009,  23,         65) /* UseSound - Drink1 */
     , (69009,  50, 0x06005EC2) /* IconOverlay */
     , (69009,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69009,  1328,      2) /* Strength Self II */
     , (69009,  1350,      2) /* Endurance Self II */
     , (69009,  1374,      2) /* Coordination Self II */
     , (69009,  1398,      2) /* Quickness Self II */
     , (69009,  1422,      2) /* Focus Self II */
     , (69009,  1446,      2) /* Willpower Self II */;

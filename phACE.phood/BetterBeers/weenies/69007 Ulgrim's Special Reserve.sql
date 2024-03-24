DELETE FROM `weenie` WHERE `class_Id` = 69007;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69007, 'phACE.phood-UlgrimsSpecialReserveT4', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69007,   1,         32) /* ItemType - Food */
     , (69007,   5,         50) /* EncumbranceVal */
     , (69007,  11,        100) /* MaxStackSize */
     , (69007,  12,          1) /* StackSize */
     , (69007,  13,         50) /* StackUnitEncumbrance */
     , (69007,  15,         10) /* StackUnitValue */
     , (69007,  16,          8) /* ItemUseable - Contained */
     , (69007,  18,          1) /* UiEffects - Magical */
     , (69007,  19,         10) /* Value */
     , (69007,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69007, 105,          4) /* ItemWorkmanship */
     , (69007, 106,        150) /* ItemSpellcraft */
     , (69007, 109,          0) /* ItemDifficulty */
	 , (69007, 366,         39) /* UseRequiresSkill - Cooking */
     , (69007, 367,        100) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69007,  11, True ) /* IgnoreCollisions */
     , (69007,  13, True ) /* Ethereal */
     , (69007,  14, True ) /* GravityStatus */
     , (69007,  19, True ) /* Attackable */
     , (69007,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69007,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69007,  14, 'Use this item to drink it.') /* Use */
     , (69007,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69007,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69007,   1, 0x02001258) /* Setup */
     , (69007,   3, 0x20000014) /* SoundTable */
     , (69007,   8, 0x06005A65) /* Icon */
     , (69007,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69007,  23,         65) /* UseSound - Drink1 */
     , (69007,  50, 0x06005EC2) /* IconOverlay */
     , (69007,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69007,  1330,      2) /* Strength Self IV */
     , (69007,  1352,      2) /* Endurance Self IV */
     , (69007,  1376,      2) /* Coordination Self IV */
     , (69007,  1400,      2) /* Quickness Self IV */
     , (69007,  1424,      2) /* Focus Self IV */
     , (69007,  1448,      2) /* Willpower Self IV */;

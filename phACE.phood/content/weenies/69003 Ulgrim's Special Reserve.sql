DELETE FROM `weenie` WHERE `class_Id` = 69003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69003, 'phACE.phood-UlgrimsSpecialReserveT8', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69003,   1,         32) /* ItemType - Food */
     , (69003,   5,         50) /* EncumbranceVal */
     , (69003,  11,        100) /* MaxStackSize */
     , (69003,  12,          1) /* StackSize */
     , (69003,  13,         50) /* StackUnitEncumbrance */
     , (69003,  15,         10) /* StackUnitValue */
     , (69003,  16,          8) /* ItemUseable - Contained */
     , (69003,  18,          1) /* UiEffects - Magical */
     , (69003,  19,         10) /* Value */
     , (69003,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69003, 105,          8) /* ItemWorkmanship */
     , (69003, 106,        350) /* ItemSpellcraft */
     , (69003, 109,          0) /* ItemDifficulty */
	 , (69003, 366,         39) /* UseRequiresSkill - Cooking */
     , (69003, 367,        300) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69003,  11, True ) /* IgnoreCollisions */
     , (69003,  13, True ) /* Ethereal */
     , (69003,  14, True ) /* GravityStatus */
     , (69003,  19, True ) /* Attackable */
     , (69003,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69003,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69003,  14, 'Use this item to drink it.') /* Use */
     , (69003,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69003,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69003,   1, 0x02001258) /* Setup */
     , (69003,   3, 0x20000014) /* SoundTable */
     , (69003,   8, 0x06005A65) /* Icon */
     , (69003,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69003,  23,         65) /* UseSound - Drink1 */
     , (69003,  50, 0x06005EC2) /* IconOverlay */
     , (69003,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69003,  4325,      2) /* Incantation of Strength Self */
     , (69003,  4299,      2) /* Incantation of Endurance Self */
     , (69003,  4297,      2) /* Incantation of Coordination Self */
     , (69003,  4319,      2) /* Incantation of Quickness Self */
     , (69003,  4305,      2) /* Incantation of Focus Self */
     , (69003,  4329,      2) /* Incantation of Willpower Self */;

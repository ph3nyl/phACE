DELETE FROM `weenie` WHERE `class_Id` = 69004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69004, 'phACE.phood-UlgrimsSpecialReserveT7', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69004,   1,         32) /* ItemType - Food */
     , (69004,   5,         50) /* EncumbranceVal */
     , (69004,  11,        100) /* MaxStackSize */
     , (69004,  12,          1) /* StackSize */
     , (69004,  13,         50) /* StackUnitEncumbrance */
     , (69004,  15,         10) /* StackUnitValue */
     , (69004,  16,          8) /* ItemUseable - Contained */
     , (69004,  18,          1) /* UiEffects - Magical */
     , (69004,  19,         10) /* Value */
     , (69004,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (69004, 105,          7) /* ItemWorkmanship */
     , (69004, 106,        300) /* ItemSpellcraft */
     , (69004, 109,          0) /* ItemDifficulty */
	 , (69004, 366,         39) /* UseRequiresSkill - Cooking */
     , (69004, 367,        250) /* UseRequiresSkillLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69004,  11, True ) /* IgnoreCollisions */
     , (69004,  13, True ) /* Ethereal */
     , (69004,  14, True ) /* GravityStatus */
     , (69004,  19, True ) /* Attackable */
     , (69004,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69004,   1, 'Ulgrim''s Special Reserve') /* Name */
     , (69004,  14, 'Use this item to drink it.') /* Use */
     , (69004,  16, 'A bottle of Ulgrim''s Special Reserve.') /* LongDesc */
     , (69004,  20, 'Bottles of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69004,   1, 0x02001258) /* Setup */
     , (69004,   3, 0x20000014) /* SoundTable */
     , (69004,   8, 0x06005A65) /* Icon */
     , (69004,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69004,  23,         65) /* UseSound - Drink1 */
     , (69004,  50, 0x06005EC2) /* IconOverlay */
     , (69004,  52, 0x06005EBB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (69004,  2087,      2) /* Might of the Lugians */
     , (69004,  2059,      2) /* Perseverance */
     , (69004,  2061,      2) /* Honed Control */
     , (69004,  2081,      2) /* Hastening */
     , (69004,  2067,      2) /* Inner Calm */
     , (69004,  2091,      2) /* Mind Blossom */;

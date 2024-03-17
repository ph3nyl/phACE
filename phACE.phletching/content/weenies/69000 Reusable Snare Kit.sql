DELETE FROM `weenie` WHERE `class_Id` = 69000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69000, 'phace.phletching-ReusableSnareKit', 38, '2024-03-09 09:36:50') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69000,   1,       2048) /* ItemType */
     , (69000,   3,         17) /* PaletteTemplate */
     , (69000,   5,         10) /* EncumbranceVal */
     , (69000,   8,          5) /* Mass */
     , (69000,   9,          0) /* ValidLocations - None */
     , (69000,  11,          1) /* MaxStackSize */
     , (69000,  12,          1) /* StackSize */
     , (69000,  13,         10) /* StackUnitEncumbrance */
     , (69000,  14,          5) /* StackUnitMass */
     , (69000,  15,       2000) /* StackUnitValue */
     , (69000,  16,          8) /* ItemUseable */
     , (69000,  19,       2000) /* Value */
     , (69000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69000,  94,  134218752) /* TargetType - Useless, CraftFletchingIntermediate */
     , (69000, 150,        104) /* HookPlacement - XXXUnknown68 */
     , (69000, 151,          2) /* HookType - Wall */
     , (69000, 280,       1000) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69000,  22, True ) /* Inscribable */
     , (69000,  23, True ) /* DestroyOnSell */
     , (69000,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (69000,  39,     1.5) /* DefaultScale */
	 , (69000, 167,    10.0) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69000,   1, 'Reusable Snare Kit') /* Name */
     , (69000,  14, 'Use this kit before/during combat.') /* Use */
     , (69000,  16, 'A reusable fletching kit that can be used to add a snaring effect to your next missile.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69000,   1, 0x0200012E) /* Setup */
     , (69000,   3, 0x20000014) /* SoundTable */
     , (69000,   6, 0x04000BEF) /* PaletteBase */
     , (69000,   7, 0x10000146) /* ClothingBase */
     , (69000,   8, 0x06001EF8) /* Icon */
     , (69000,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69000,  23, 0x0A000188) /* UseSound */
     , (69000,  27, 0x800000e9) /* UseUserAnimation */
     , (69000,  50, 0x060028F4) /* IconOverlay */;


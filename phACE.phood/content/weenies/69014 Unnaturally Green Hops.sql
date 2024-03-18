DELETE FROM `weenie` WHERE `class_Id` = 69014;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69014, 'phACE.phood-UnnaturallyGreenHops', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69014,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69014,   5,         20) /* EncumbranceVal */
     , (69014,  11,        100) /* MaxStackSize */
     , (69014,  12,          1) /* StackSize */
     , (69014,  13,         20) /* StackUnitEncumbrance */
     , (69014,  15,         30) /* StackUnitValue */
     , (69014,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (69014,  19,         30) /* Value */
     , (69014,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69014,  94,    4194336) /* TargetType - Food, CraftCookingBase */
     , (69014, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69014,  11, True ) /* IgnoreCollisions */
     , (69014,  13, True ) /* Ethereal */
     , (69014,  14, True ) /* GravityStatus */
     , (69014,  19, True ) /* Attackable */
     , (69014,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69014,   1, 'Unnaturally Green Hops') /* Name */
     , (69014,  14, 'Add hops to wort to create finished wort.') /* Use */
     , (69014,  16, 'These hops are an unnatural, almost emissive, green. The color is so vivid that it seems to bleed out of the hops and into the air around it.') /* LongDesc */
     , (69014,  20, 'Bags of Unnaturally Green Hops') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69014,   1, 0x0200128E) /* Setup */
     , (69014,   3, 0x20000014) /* SoundTable */
     , (69014,   8, 0x06005A72) /* Icon */
     , (69014,  22, 0x3400002B) /* PhysicsEffectTable */;
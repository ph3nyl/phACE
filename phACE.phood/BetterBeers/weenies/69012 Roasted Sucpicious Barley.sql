DELETE FROM `weenie` WHERE `class_Id` = 69012;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69012, 'phACE.phood-RoastedSuspiciousBarley', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69012,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69012,   5,         20) /* EncumbranceVal */
     , (69012,  11,        100) /* MaxStackSize */
     , (69012,  12,          1) /* StackSize */
     , (69012,  13,         20) /* StackUnitEncumbrance */
     , (69012,  15,         20) /* StackUnitValue */
     , (69012,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (69012,  19,         20) /* Value */
     , (69012,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69012,  94,    4194336) /* TargetType - Food, CraftCookingBase */
     , (69012, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69012,  11, True ) /* IgnoreCollisions */
     , (69012,  13, True ) /* Ethereal */
     , (69012,  14, True ) /* GravityStatus */
     , (69012,  19, True ) /* Attackable */
     , (69012,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69012,   1, 'Roasted Suspicious Barley') /* Name */
     , (69012,  14, 'Add barley to a full brew kettle to create wort.') /* Use */
     , (69012,  16, 'This barley has been skillfully roasted. It can be used to produce a rich, creamy stout. It is unusually dark, as if the grains were little pieces of the void itself.') /* LongDesc */
     , (69012,  20, 'Bags of Roasted Suspicious Barley') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69012,   1, 0x0200128E) /* Setup */
     , (69012,   3, 0x20000014) /* SoundTable */
     , (69012,   8, 0x06005A69) /* Icon */
     , (69012,  22, 0x3400002B) /* PhysicsEffectTable */;

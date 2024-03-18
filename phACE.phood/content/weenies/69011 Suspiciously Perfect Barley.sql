DELETE FROM `weenie` WHERE `class_Id` = 69011;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69011, 'phACE.phood-SuspiciouslyPerfectBarley', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69011,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69011,   5,         20) /* EncumbranceVal */
     , (69011,  11,        100) /* MaxStackSize */
     , (69011,  12,          1) /* StackSize */
     , (69011,  13,         20) /* StackUnitEncumbrance */
     , (69011,  15,         20) /* StackUnitValue */
     , (69011,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (69011,  19,         20) /* Value */
     , (69011,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69011,  94,    4194336) /* TargetType - Food, CraftCookingBase */
     , (69011, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69011,  11, True ) /* IgnoreCollisions */
     , (69011,  13, True ) /* Ethereal */
     , (69011,  14, True ) /* GravityStatus */
     , (69011,  19, True ) /* Attackable */
     , (69011,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69011,   1, 'Suspiciously Perfect Barley') /* Name */
     , (69011,  14, 'Roast barley in a baking pan to reduce moisture content and deepen the resulting flavor.') /* Use */
     , (69011,  16, 'This barley is pure, plump, and has uniform undamaged kernels. It is entirely free of inert materials, insects, and ergot. It''s perfect... Almost too perfect.') /* LongDesc */
     , (69011,  20, 'Bags of Suspiciously Perfect Barley') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69011,   1, 0x0200128E) /* Setup */
     , (69011,   3, 0x20000014) /* SoundTable */
     , (69011,   8, 0x06005A69) /* Icon */
     , (69011,  22, 0x3400002B) /* PhysicsEffectTable */;

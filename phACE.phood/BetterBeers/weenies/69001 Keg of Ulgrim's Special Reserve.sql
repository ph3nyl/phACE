DELETE FROM `weenie` WHERE `class_Id` = 69001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69001, 'phACE.phood-KegOfUlgrimsSpecialReserve', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69001,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69001,   5,       1000) /* EncumbranceVal */
     , (69001,  11,        100) /* MaxStackSize */
     , (69001,  12,          1) /* StackSize */
     , (69001,  13,       1000) /* StackUnitEncumbrance */
     , (69001,  15,        100) /* StackUnitValue */
     , (69001,  16,          1) /* ItemUseable - No */
     , (69001,  19,        100) /* Value */
     , (69001,  33,          0) /* Bonded - Normal */
     , (69001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69001, 114,          0) /* Attuned - Normal */
     , (69001, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69001,  11, True ) /* IgnoreCollisions */
     , (69001,  13, True ) /* Ethereal */
     , (69001,  14, True ) /* GravityStatus */
     , (69001,  19, True ) /* Attackable */
     , (69001,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69001,   1, 'Keg of Ulgrim''s Special Reserve') /* Name */
     , (69001,  16, 'A keg of Ulgrim''s Special Reserve. Use a pack of Empty Bottles on this keg to serve it to guests.') /* LongDesc */
     , (69001,  20, 'Kegs of Ulgrim''s Special Reserve') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69001,   1, 0x02001270) /* Setup */
     , (69001,   3, 0x20000014) /* SoundTable */
     , (69001,   8, 0x06005A73) /* Icon */
     , (69001,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69001,  50, 0x06005EC2) /* IconOverlay */;

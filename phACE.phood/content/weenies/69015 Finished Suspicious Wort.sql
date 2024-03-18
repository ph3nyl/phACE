DELETE FROM `weenie` WHERE `class_Id` = 69013;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69013, 'phACE.phood-FinishedSuspiciousWort', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69013,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69013,   5,        150) /* EncumbranceVal */
     , (69013,  11,        100) /* MaxStackSize */
     , (69013,  12,          1) /* StackSize */
     , (69013,  16,          1) /* ItemUseable - No */
     , (69013,  19,         50) /* Value */
     , (69013,  33,          0) /* Bonded - Normal */
     , (69013,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69013, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69013,  11, True ) /* IgnoreCollisions */
     , (69013,  13, True ) /* Ethereal */
     , (69013,  14, True ) /* GravityStatus */
     , (69013,  19, True ) /* Attackable */
     , (69013,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69013,   1, 'Finished Suspicious Wort') /* Name */
     , (69013,  16, 'An aromatic, and impossibly dark wort of dubious origin. You get the feeling that this fragrant sugary liquid wants something. Like it''s calling out... but to what? Or who?') /* LongDesc */
     , (69013,  20, 'kettles of Finished Suspicious Wort') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69013,   1, 0x02001272) /* Setup */
     , (69013,   3, 0x20000014) /* SoundTable */
     , (69013,   8, 0x06005A7E) /* Icon */
     , (69013,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69013,  50, 0x06005EBD) /* IconOverlay */;

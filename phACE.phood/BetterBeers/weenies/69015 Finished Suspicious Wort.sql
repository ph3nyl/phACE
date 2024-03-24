DELETE FROM `weenie` WHERE `class_Id` = 69015;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (69015, 'phACE.phood-FinishedSuspiciousWort', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (69015,   1,    4194304) /* ItemType - CraftCookingBase */
     , (69015,   5,        150) /* EncumbranceVal */
     , (69015,  11,        100) /* MaxStackSize */
     , (69015,  12,          1) /* StackSize */
     , (69015,  16,          1) /* ItemUseable - No */
     , (69015,  19,         50) /* Value */
     , (69015,  33,          0) /* Bonded - Normal */
     , (69015,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (69015, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (69015,  11, True ) /* IgnoreCollisions */
     , (69015,  13, True ) /* Ethereal */
     , (69015,  14, True ) /* GravityStatus */
     , (69015,  19, True ) /* Attackable */
     , (69015,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (69015,   1, 'Finished Suspicious Wort') /* Name */
     , (69015,  16, 'An aromatic, and impossibly dark wort of dubious origin. You get the feeling that this fragrant sugary liquid wants something. Like it''s calling out... but to what? Or who?') /* LongDesc */
     , (69015,  20, 'kettles of Finished Suspicious Wort') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (69015,   1, 0x02001272) /* Setup */
     , (69015,   3, 0x20000014) /* SoundTable */
     , (69015,   8, 0x06005A7E) /* Icon */
     , (69015,  22, 0x3400002B) /* PhysicsEffectTable */
     , (69015,  50, 0x06005EBD) /* IconOverlay */;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Command;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;
using ACE.Shared;
using HarmonyLib;
using Spell = ACE.Server.Entity.Spell;

namespace phACE.phood
{
    [HarmonyPatchCategory("VitalSaturation")]
    public class Patch_VitalSaturation
    {
        #region Init/Fini
        private const bool DEBUG_PRINT = false;
        private readonly static string[] HungerLevelNames = { "Starving", "Hungry", "Satisfied", "Full", "Stuffed" };
        private static readonly Spell wellFed = new(06902);

        public static void Init()
        {
            //
        }
        public static void Fini()
        {
            //
        }
        #endregion

        #region Patches
        
        #region Creature_Vitals.cs
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Creature), nameof(Creature.VitalHeartBeat), new Type[] { typeof(CreatureVital) })]
        public static bool PreVitalHeartBeat(CreatureVital vital, ref Creature __instance, ref bool __result)
        {

            var vitalCurrent = vital.Current;
            var vitalMax = vital.MaxValue;

            if (vitalCurrent == vitalMax && vital.RegenRate > 0)
            {
                __result = false;
                return false;
            }


            if (vitalCurrent > vitalMax)
            {
                __instance.UpdateVital(vital, vitalMax);
                __result = true;
                return false;
            }

            if (vital.RegenRate == 0.0) return false;

            // take attributes into consideration (strength, endurance)
            var attributeMod = __instance.GetAttributeMod(vital);

            // take stance into consideration (combat, crouch, sitting, sleeping)
            var stanceMod = __instance.GetStanceMod(vital);

            // take enchantments into consideration:
            // (regeneration / rejuvenation / mana renewal / etc.)
            var enchantmentMod = __instance.EnchantmentManager.GetRegenerationMod(vital);

            var augMod = 1.0f;
            if (__instance is Player player && player.AugmentationFasterRegen > 0)
                augMod += player.AugmentationFasterRegen;

            // cap rate?
            var currentTick = (vital.RegenRate + GetExtraRegen(vital, __instance)) * attributeMod * stanceMod * enchantmentMod * augMod;

            // add in partially accumulated / rounded vitals from previous tick(s)
            var totalTick = currentTick + vital.PartialRegen;

            // accumulate partial vital rates between ticks
            var intTick = (int)totalTick;
            vital.PartialRegen = totalTick - intTick;

            if (intTick != 0)
            {
                //if (this is Player)
                //Console.WriteLine($"VitalTick({vital.Vital.ToSentence()}): attributeMod={attributeMod}, stanceMod={stanceMod}, enchantmentMod={enchantmentMod}, regenRate={vital.RegenRate}, currentTick={currentTick}, totalTick={totalTick}, accumulated={vital.PartialRegen}");

                __instance.UpdateVitalDelta(vital, intTick);
                if (vital.Vital == PropertyAttribute2nd.MaxHealth)
                {
                    if (intTick > 0)
                        __instance.DamageHistory.OnHeal((uint)intTick);
                    else
                    {
                        __instance.DamageHistory.Add(__instance, DamageType.Health, (uint)Math.Abs(intTick));

                        if (__instance.Health.Current <= 0)
                        {
                            __instance.OnDeath(__instance.DamageHistory.LastDamager, DamageType.Health);
                            __instance.Die();
                        }
                    }

                    return false;
                }
            }
            return false;
        }

        private static double GetExtraRegen(CreatureVital vital, Creature __instance)
        {
            var regenPool = 0d;
            var extraRegenAmount = 0d;
            switch (vital.Vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                    regenPool = __instance.GetProperty(FakeFloat.SaturationHealth) ?? 0;
                    extraRegenAmount = Settings.BonusRegenHealth;
                break;
                case PropertyAttribute2nd.MaxStamina:
                    regenPool = __instance.GetProperty(FakeFloat.SaturationStamina) ?? 0;
                    extraRegenAmount = Settings.BonusRegenStamina;
                break;
                case PropertyAttribute2nd.MaxMana:
                    regenPool = __instance.GetProperty(FakeFloat.SaturationMana) ?? 0;
                    extraRegenAmount = Settings.BonusRegenMana;
                break;
            }

            if (regenPool <= 0 || vital.Missing == 0)
                return 0d;

            var previousRegenPool = regenPool;

            var regenValue = Math.Min(extraRegenAmount, regenPool);
            regenValue = Math.Min(regenValue, (int)vital.Missing);
            regenPool -= regenValue;

            switch (vital.Vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                    __instance.SetProperty(FakeFloat.SaturationHealth, regenPool);
                break;
                case PropertyAttribute2nd.MaxStamina:
                    __instance.SetProperty(FakeFloat.SaturationStamina, regenPool);
                break;
                case PropertyAttribute2nd.MaxMana:
                    __instance.SetProperty(FakeFloat.SaturationMana, regenPool);
                break;
            }

            if (previousRegenPool != regenPool && __instance is Player player && player != null)
            {
                var vitalString = "";
                switch (vital.Vital)
                {
                    case PropertyAttribute2nd.MaxHealth:
                    vitalString = "Health";
                    break;
                    case PropertyAttribute2nd.MaxStamina:
                    vitalString = "Stamina";
                    break;
                    case PropertyAttribute2nd.MaxMana:
                    vitalString = "Mana";
                    break;
                }

                string fullness = "";
                if (regenPool == 0 && !(previousRegenPool == 0))
                { fullness = $"You are now {HungerLevelNames[0]} for {vitalString} food! ({regenPool}/{Settings.MaxRegenPoolValue})"; }
                else if (regenPool <= Settings.MaxRegenPoolValue * 0.25f && !(previousRegenPool <= Settings.MaxRegenPoolValue * 0.25f))
                {
                    fullness = $"You are now {HungerLevelNames[1]} for {vitalString} food. ({regenPool}/{Settings.MaxRegenPoolValue})";
                    //player.EnchantmentManager.Remove(player.EnchantmentManager.GetEnchantment(3760), true);
                    player.EnchantmentManager.Remove(player.EnchantmentManager.GetEnchantment(wellFed.ImbuedEffect, player.Guid.Full), true);
                }
                else if (regenPool <= Settings.MaxRegenPoolValue * 0.5f && !(previousRegenPool <= Settings.MaxRegenPoolValue * 0.5f))
                { fullness = $"You are still {HungerLevelNames[2]} of {vitalString} food. ({regenPool}/{Settings.MaxRegenPoolValue})"; }
                else if (regenPool <= Settings.MaxRegenPoolValue * 0.75f && !(previousRegenPool <= Settings.MaxRegenPoolValue * 0.75f))
                { fullness = $"You are still {HungerLevelNames[3]} of {vitalString} food. ({regenPool}/{Settings.MaxRegenPoolValue})"; }

                if (fullness != "")
                {
                    player.SendMessage(fullness);
                }

            }
            return regenValue;
        }
        #endregion

        #region Food.cs
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Food), nameof(Food.ApplyConsumable), new Type[] { typeof(Player) })]
        public static bool PreApplyConsumable(Player player, ref Food __instance)
        {
            if (player.IsDead) return false;

            // verify item is still valid
            if (player.FindObject(__instance.Guid.Full, Player.SearchLocations.MyInventory) == null)
            {
                //player.SendWeenieError(WeenieError.ObjectGone);   // results in 'Unable to move object!' transient error
                player.SendTransientError($"Cannot find the {__instance.Name}");   // custom message
                return false;
            }

            if (__instance.BoostValue == 0 || __instance.GetUseSound() != Sound.Eat1) return true;

            var ratingMod = __instance.BoostValue > 0 ? player.GetHealingRatingMod() : 1.0f;

            var boostValue = (int)Math.Round(__instance.BoostValue * ratingMod);

            bool skipConsume = false;

            if (__instance.BoosterEnum == PropertyAttribute2nd.Health && __instance.BoostValue != 0 && __instance.GetUseSound() == Sound.Eat1)
            {
                if ((boostValue * Settings.BoostToSatConversionFactor) + player.GetProperty(FakeFloat.SaturationHealth) > (boostValue * Settings.BoostToSatConversionFactor) * Settings.SpiceOfLife)
                {
                    player.SendMessage($"You just can't bring yourself to eat the {__instance.Name}. Maybe better food would be more appetizing.");
                    skipConsume = true;
                }
                else
                    AddExtraRegen(player, player.GetCreatureVital(PropertyAttribute2nd.Health), __instance);
            }
            else if (__instance.BoosterEnum == PropertyAttribute2nd.Stamina && __instance.BoostValue != 0 && __instance.GetUseSound() == Sound.Eat1)
            {
                if ((boostValue * Settings.BoostToSatConversionFactor) + player.GetProperty(FakeFloat.SaturationStamina) > (boostValue * Settings.BoostToSatConversionFactor) * Settings.SpiceOfLife)
                {
                    player.SendMessage($"You just can't bring yourself to eat the {__instance.Name}. Maybe better food would be more appetizing.");
                    skipConsume = true;
                }
                else
                    AddExtraRegen(player, player.GetCreatureVital(PropertyAttribute2nd.Stamina), __instance);
            }
            else if (__instance.BoosterEnum == PropertyAttribute2nd.Mana && __instance.BoostValue != 0 && __instance.GetUseSound() == Sound.Eat1)
            {
                if ((boostValue * Settings.BoostToSatConversionFactor) + player.GetProperty(FakeFloat.SaturationMana) > (boostValue * Settings.BoostToSatConversionFactor) * Settings.SpiceOfLife)
                {
                    player.SendMessage($"You just can't bring yourself to eat the {__instance.Name}. Maybe better food would be more appetizing.");
                    skipConsume = true;
                }
                else
                    AddExtraRegen(player, player.GetCreatureVital(PropertyAttribute2nd.Mana), __instance);
            }

            if (skipConsume) return false;

            var soundEvent = new GameMessageSound(player.Guid, __instance.GetUseSound(), 1.0f);
            player.EnqueueBroadcast(soundEvent);

            if (!__instance.UnlimitedUse)
                player.TryConsumeFromInventoryWithNetworking(__instance, 1);

            return false;
        }

        public static void AddExtraRegen(Player player, CreatureVital vital, Food __instance)
        {
            var previousPoolValue = 0d;
            var newPoolvalue = 0d;
            var vitalString = "";
            switch (vital.Vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                previousPoolValue = player.GetProperty(FakeFloat.SaturationHealth) ?? 0;
                newPoolvalue = Math.Clamp(previousPoolValue + (__instance.BoostValue * Settings.BoostToSatConversionFactor), 0, Settings.MaxRegenPoolValue);
                player.SetProperty(FakeFloat.SaturationHealth, newPoolvalue);
                vitalString = "Health";
                break;
                case PropertyAttribute2nd.MaxStamina:
                previousPoolValue = player.GetProperty(FakeFloat.SaturationStamina) ?? 0;
                newPoolvalue = Math.Clamp(previousPoolValue + (__instance.BoostValue * Settings.BoostToSatConversionFactor), 0d, Settings.MaxRegenPoolValue);
                player.SetProperty(FakeFloat.SaturationStamina, newPoolvalue);
                vitalString = "Stamina";
                break;
                case PropertyAttribute2nd.MaxMana:
                previousPoolValue = player.GetProperty(FakeFloat.SaturationMana) ?? 0;
                newPoolvalue = Math.Clamp(previousPoolValue + (__instance.BoostValue * Settings.BoostToSatConversionFactor), 0d, Settings.MaxRegenPoolValue);
                player.SetProperty(FakeFloat.SaturationMana, newPoolvalue);
                vitalString = "Mana";
                break;
            }

            var amount = newPoolvalue - previousPoolValue;
            var verb = "adds";
            var complement = "to";
            if (amount < 0)
            {
                verb = "removes";
                complement = "from";
                amount = Math.Abs(amount);
            }

            var fullness = "";
            if (newPoolvalue >= Settings.MaxRegenPoolValue)
            { fullness = $" You are completely {HungerLevelNames[4]} with {vitalString} food!"; }
            else if (newPoolvalue >= Settings.MaxRegenPoolValue * 0.75f)
            { fullness = $" You are {HungerLevelNames[4]} of {vitalString} food."; }
            else if (newPoolvalue >= Settings.MaxRegenPoolValue * 0.5f)
            { fullness = $" You are {HungerLevelNames[3]} with {vitalString} food."; }
            else if (newPoolvalue >= Settings.MaxRegenPoolValue * 0.25f)
            {
                fullness = $" You are {HungerLevelNames[2]} for {vitalString} food.";
                GiveWellFed(player);
            }
            else if (newPoolvalue >= 0)
            { fullness = $" You are still {HungerLevelNames[1]} for {vitalString} food!"; }

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {__instance.Name} {verb} {amount:N0} points {complement} your {vitalString} saturation pool.{fullness} ({newPoolvalue}/{Settings.MaxRegenPoolValue})", ChatMessageType.Broadcast));


        }

        private static void GiveWellFed(Player player)
        {
            if (player.GetProperty(FakeFloat.SaturationHealth) >= Settings.MaxRegenPoolValue * 0.25
             && player.GetProperty(FakeFloat.SaturationStamina) >= Settings.MaxRegenPoolValue * 0.25
             && player.GetProperty(FakeFloat.SaturationMana) >= Settings.MaxRegenPoolValue * 0.25)
            {
                if (ContentManager.SIDs.ContainsKey(06902))
                    player.TryCastSpell(ContentManager.SIDs[06902], player, null, null, false, false, false);
                //player.TryCastSpell(wellFed, player, null, null, false, false, false);
                //player.TryCastSpell(new Spell(06902), player, null, null, false, false, false);
            }
        }
        #endregion

        #region AppraiseInfo.cs
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ACE.Server.Network.Structure.AppraiseInfo), nameof(ACE.Server.Network.Structure.AppraiseInfo.BuildProfile), new Type[] { typeof(WorldObject), typeof(Player), typeof(bool) })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void PostBuildProfile(WorldObject wo, Player examiner, bool success, ref ACE.Server.Network.Structure.AppraiseInfo __instance)
        {
            if (wo is Food && wo is Food food && food.GetUseSound() == Sound.Eat1)
            {
                var amount = wo.GetProperty(PropertyInt.BoostValue) * Settings.BoostToSatConversionFactor;
                var verb = "adds";
                var verb2 = "raise";
                var complement = "to";
                var vitalString = "";
                var maxOrMin = "maximum";
                if (amount < 0)
                {
                    verb = "removes";
                    complement = "from";
                    verb2 = "lower";
                    maxOrMin = "minimum";
                    //amount = Math.Abs(amount);
                }
                switch (wo.GetProperty(PropertyInt.BoosterEnum))
                {
                    case 2: //health
                            //__instance.SetProperty(FakeFloat.SaturationHealth, __instance.GetProperty(PropertyInt.BoostValue) ?? 0.0f);
                    vitalString = "Health";
                    //__instance.RemoveProperty(PropertyInt.BoostValue);
                    //__instance.RemoveProperty(PropertyInt.BoosterEnum);
                    break;
                    case 4: //stamina
                            //__instance.SetProperty(FakeFloat.SaturationStamina, __instance.GetProperty(PropertyInt.BoostValue) ?? 0.0f);
                    vitalString = "Stamina";
                    break;
                    case 6: //mana
                            //__instance.SetProperty(FakeFloat.SaturationMana, __instance.GetProperty(PropertyInt.BoostValue) ?? 0.0f);
                    vitalString = "Mana";
                    break;
                }
                string useMessage = wo.GetProperty(PropertyString.Use);
                //ModManager.Log(useMessage);
                useMessage += $"\n\nThis {verb} up to {amount} points {complement} your {vitalString} saturation pool when eaten, ";
                if (amount * Settings.SpiceOfLife >= 3600)
                    useMessage += $"and can {verb2} your {vitalString} saturation up to the {maxOrMin} of {Settings.MaxRegenPoolValue}.\n";
                else
                    useMessage += $"but can only {verb2} your {vitalString} saturation up to a {maxOrMin} of {amount * Settings.SpiceOfLife}.\n";
                //ModManager.Log(useMessage);
                __instance.PropertiesString[PropertyString.Use] = useMessage;
                __instance.PropertiesInt.Remove(PropertyInt.BoostValue);
            }
        }

        #endregion

        #endregion

        #region Commands

        [CommandHandler("sat", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Show the character's current hunger status.")]
        [CommandHandler("saturation", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Show the character's current hunger status.")]
        [CommandHandler("food", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Show the character's current hunger status.")]
        [CommandHandler("hunger", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Show the character's current hunger status.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void HandleHunger(Session session, params string[] parameters)
        {
            var player = session.Player;

            var health = player.GetProperty(FakeFloat.SaturationHealth) ?? 0;
            var stamina = player.GetProperty(FakeFloat.SaturationStamina) ?? 0;
            var mana = player.GetProperty(FakeFloat.SaturationMana) ?? 0;
            var max = Settings.MaxRegenPoolValue;

            string? healthFullness;
            if (health >= max * 0.75f)
                healthFullness = HungerLevelNames[4];
            else if (health >= max * 0.5f)
                healthFullness = HungerLevelNames[3];
            else if (health >= max * 0.25f)
                healthFullness = HungerLevelNames[2];
            else if (health > 0)
                healthFullness = HungerLevelNames[1];
            else
                healthFullness = HungerLevelNames[0];

            string? staminaFullness;
            if (stamina >= max * 0.75f)
                staminaFullness = HungerLevelNames[4];
            else if (stamina >= max * 0.5f)
                staminaFullness = HungerLevelNames[3];
            else if (stamina >= max * 0.25f)
                staminaFullness = HungerLevelNames[2];
            else if (stamina > 0)
                staminaFullness = HungerLevelNames[1];
            else
                staminaFullness = HungerLevelNames[0];

            string? manaFullness;
            if (mana >= max * 0.75f)
                manaFullness = HungerLevelNames[4];
            else if (mana >= max * 0.5f)
                manaFullness = HungerLevelNames[3];
            else if (mana >= max * 0.25f)
                manaFullness = HungerLevelNames[2];
            else if (mana > 0)
                manaFullness = HungerLevelNames[1];
            else
                manaFullness = HungerLevelNames[0];

            session.Player.SendMessage($"Health Saturation : {healthFullness} ({health}/{max})");
            session.Player.SendMessage($"Stamina Saturation : {staminaFullness} ({stamina}/{max})");
            session.Player.SendMessage($"Mana Saturation : {manaFullness} ({mana}/{max})");
        }

        //[CommandHandler("unbuff", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        //public static void Unbuff(Session session, params string[] parameters)
        //=> session.Player.EnchantmentManager.DispelAllEnchantments();

        #endregion


    }
}
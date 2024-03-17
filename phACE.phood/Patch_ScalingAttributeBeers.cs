using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using HarmonyLib;

namespace phACE.phood
{
    [HarmonyPatchCategory("ScalingAttributeBeers")]
    public class Patch_ScalingAttributeBeers
    { 
        #region Init/Fini
        private const bool DEBUG_PRINT = false;

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

        #region RecipeManager.cs
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RecipeManager), nameof(RecipeManager.UseObjectOnTarget), new Type[] { typeof(Player), typeof(WorldObject), typeof(WorldObject), typeof(bool) })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static bool PreUseObjectOnTarget(Player player, WorldObject source, WorldObject target, bool confirmed, ref RecipeManager __instance)
        {
            if (source.WeenieClassId != 29180 /*Empty Bottles*/ || target.WeenieClassId != 69001 /*Keg of Ulgrim's Special Reserve*/) return true;

            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return false;
            }

            bool item = PropertyManager.GetBool("allow_combat_mode_crafting").Item;
            if (!item && player.CombatMode != CombatMode.NonCombat)
            {
                player.SendUseDoneEvent(WeenieError.YouMustBeInPeaceModeToTrade);
                return false;
            }

            player.Skills.TryGetValue(Skill.Fletching, out var skill);
            if (skill != null && Mod.Settings != null)
            {
                if (skill.AdvancementClass == SkillAdvancementClass.Untrained)
                {
                    //error - untrained cooking
                }
                else
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(69002);

                    player.SendMessage("RecipeManager.UseObjectOnTarget");
                    player.SendMessage($"source = {source.Name}, stack = {source.StackSize}/{source.MaxStackSize}");
                    player.SendMessage($"target = {target.Name}, stack = {target.StackSize}/{target.MaxStackSize}");
                    player.SendMessage($"result = {wo.Name}, stack = {wo.StackSize}/{wo.MaxStackSize}");

                    /* todo: this doesnt work. make weenies
                    switch (skill.Current)
                    {
                        //level I +10 stat "Poorly crafted"
                        case var _ when skill.Current < 50:
                        wo.Workmanship = 1;
                        wo.UseRequiresSkillLevel = 0;
                        wo.Biota.PropertiesSpellBook.Add(2, 1.0f); //Str I
                        wo.Biota.PropertiesSpellBook.Add(1349, 1.0f); //End I
                        wo.Biota.PropertiesSpellBook.Add(1373, 1.0f); //Crd I
                        wo.Biota.PropertiesSpellBook.Add(1397, 1.0f); //Qik I
                        wo.Biota.PropertiesSpellBook.Add(1421, 1.0f); //Fcs I
                        wo.Biota.PropertiesSpellBook.Add(1445, 1.0f); //Wil I
                        break;

                        //level II +15 stat "Well-crafted"
                        case var _ when (skill.Current >= 50 && skill.Current < 100):
                        wo.Workmanship = 2;
                        wo.UseRequiresSkillLevel = 0;
                        wo.Biota.PropertiesSpellBook.Add(1328, 1.0f); //Str II
                        wo.Biota.PropertiesSpellBook.Add(1350, 1.0f); //End II
                        wo.Biota.PropertiesSpellBook.Add(1374, 1.0f); //Crd II
                        wo.Biota.PropertiesSpellBook.Add(1398, 1.0f); //Qik II
                        wo.Biota.PropertiesSpellBook.Add(1422, 1.0f); //Fcs II
                        wo.Biota.PropertiesSpellBook.Add(1446, 1.0f); //Wil II
                        break;

                        //level III +20 stat "Finely crafted"
                        case var _ when (skill.Current >= 100 && skill.Current < 150):
                        wo.Workmanship = 3;
                        wo.UseRequiresSkillLevel = 50;
                        wo.Biota.PropertiesSpellBook.Add(1329, 1.0f); //Str III
                        wo.Biota.PropertiesSpellBook.Add(1351, 1.0f); //End III
                        wo.Biota.PropertiesSpellBook.Add(1375, 1.0f); //Crd III
                        wo.Biota.PropertiesSpellBook.Add(1399, 1.0f); //Qik III
                        wo.Biota.PropertiesSpellBook.Add(1423, 1.0f); //Fcs III
                        wo.Biota.PropertiesSpellBook.Add(1447, 1.0f); //Wil III
                        break;

                        //level IV +20 stat "Exquisitely crafted"
                        case var _ when (skill.Current >= 150 && skill.Current < 200):
                        wo.Workmanship = 4;
                        wo.UseRequiresSkillLevel = 100;
                        wo.Biota.PropertiesSpellBook.Add(1330, 1.0f); //Str IV
                        wo.Biota.PropertiesSpellBook.Add(1352, 1.0f); //End IV
                        wo.Biota.PropertiesSpellBook.Add(1376, 1.0f); //Crd IV
                        wo.Biota.PropertiesSpellBook.Add(1400, 1.0f); //Qik IV
                        wo.Biota.PropertiesSpellBook.Add(1424, 1.0f); //Fcs IV
                        wo.Biota.PropertiesSpellBook.Add(1448, 1.0f); //Wil IV
                        break;

                        //level V +25 stat "Magnificent"
                        case var _ when (skill.Current >= 200 && skill.Current < 250):
                        wo.Workmanship = 5;
                        wo.UseRequiresSkillLevel = 150;
                        wo.Biota.PropertiesSpellBook.Add(1331, 1.0f); //Str V
                        wo.Biota.PropertiesSpellBook.Add(1353, 1.0f); //End V
                        wo.Biota.PropertiesSpellBook.Add(1377, 1.0f); //Crd V
                        wo.Biota.PropertiesSpellBook.Add(1401, 1.0f); //Qik V
                        wo.Biota.PropertiesSpellBook.Add(1425, 1.0f); //Fcs V
                        wo.Biota.PropertiesSpellBook.Add(1449, 1.0f); //Wil V
                        break;

                        //level VI +30 stat "Nearly flawless"
                        case var _ when (skill.Current >= 250 && skill.Current < 300):
                        wo.Workmanship = 6;
                        wo.UseRequiresSkillLevel = 200;
                        wo.Biota.PropertiesSpellBook.Add(1332, 1.0f); //Str VI
                        wo.Biota.PropertiesSpellBook.Add(1354, 1.0f); //End VI
                        wo.Biota.PropertiesSpellBook.Add(1378, 1.0f); //Crd VI
                        wo.Biota.PropertiesSpellBook.Add(1402, 1.0f); //Qik VI
                        wo.Biota.PropertiesSpellBook.Add(1426, 1.0f); //Fcs VI
                        wo.Biota.PropertiesSpellBook.Add(1450, 1.0f); //Wil VI
                        break;

                        //level VII +35 stat "Flawless"
                        case var _ when (skill.Current >= 300 && skill.Current < 350):
                        wo.Workmanship = 7;
                        wo.UseRequiresSkillLevel = 250;
                        wo.Biota.PropertiesSpellBook.Add(2087, 1.0f); //Str VII
                        wo.Biota.PropertiesSpellBook.Add(2059, 1.0f); //End VII
                        wo.Biota.PropertiesSpellBook.Add(2061, 1.0f); //Crd VII
                        wo.Biota.PropertiesSpellBook.Add(2081, 1.0f); //Qik VII
                        wo.Biota.PropertiesSpellBook.Add(2067, 1.0f); //Fcs VII
                        wo.Biota.PropertiesSpellBook.Add(2091, 1.0f); //Wil VII
                        break;

                        //level VIII +40 stat "Utterly flawless"
                        case var _ when (skill.Current >= 350 && skill.Current < 400):
                        wo.Workmanship = 8;
                        wo.UseRequiresSkillLevel = 300;
                        wo.Biota.PropertiesSpellBook.Add(4325, 1.0f); //Str VIII
                        wo.Biota.PropertiesSpellBook.Add(4299, 1.0f); //End VIII
                        wo.Biota.PropertiesSpellBook.Add(4297, 1.0f); //Crd VIII
                        wo.Biota.PropertiesSpellBook.Add(4319, 1.0f); //Qik VIII
                        wo.Biota.PropertiesSpellBook.Add(4305, 1.0f); //Fcs VIII
                        wo.Biota.PropertiesSpellBook.Add(4329, 1.0f); //Wil VIII
                        break;

                        //level 'IX' +45 stat "Incomparable"
                        case var _ when skill.Current >= 400:
                        wo.Workmanship = 9;
                        wo.UseRequiresSkillLevel = 350;
                        wo.Biota.PropertiesSpellBook.Add(3864, 1.0f); //Str IX
                        wo.Biota.PropertiesSpellBook.Add(3863, 1.0f); //End IX
                        wo.Biota.PropertiesSpellBook.Add(3533, 1.0f); //Crd IX
                        wo.Biota.PropertiesSpellBook.Add(3531, 1.0f); //Qik IX
                        wo.Biota.PropertiesSpellBook.Add(3530, 1.0f); //Fcs IX
                        wo.Biota.PropertiesSpellBook.Add(3862, 1.0f); //Wil IX
                        break;
                    }*/

                    player.TryConsumeFromInventoryWithNetworking(29180, 1);
                    player.TryConsumeFromInventoryWithNetworking(69001, 1);

                    player.TryCreateInInventoryWithNetworking(wo);

                    player.SendUseDoneEvent();

                    return false;
                }
            }
            return true;
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

            if (!ContentManager.WCIDs.Contains(__instance.WeenieClassId)) return true;

            //player.SendMessage("food.cs");
            foreach(var spellID in __instance.Biota.GetKnownSpellsIds(player.CharacterDatabaseLock))
            {
                var spell = new Spell(spellID);
                player.TryCastSpell(spell, player, __instance, tryResist: false);
            }

            return false;
        }
        #endregion

        #endregion
    }
}
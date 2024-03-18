using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
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
            if (source.WeenieClassId != 29180 /*Empty Bottles*/ || target.WeenieClassName != "phACE.phood-KegOfUlgrimsSpecialReserve") return true;

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

            player.Skills.TryGetValue(Skill.Cooking, out var skill);
            if (skill != null && Mod.Settings != null)
            {
                if (skill.AdvancementClass == SkillAdvancementClass.Untrained)
                {
                    //error - untrained cooking
                    //player.SendUseDoneEvent(WeenieError.SkillTooLow);
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    return false;
                }
                else
                {
                    WorldObject wo = skill.Current switch
                    {
                        //level I +10 stat "Poorly crafted"
                        var _ when skill.Current < 50 => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT1"),
                        //level II +15 stat "Well-crafted"
                        var _ when (skill.Current >= 50 && skill.Current < 100) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT2"),
                        //level III +20 stat "Finely crafted"
                        var _ when (skill.Current >= 100 && skill.Current < 150) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT3"),
                        //level IV +20 stat "Exquisitely crafted"
                        var _ when (skill.Current >= 150 && skill.Current < 200) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT4"),
                        //level V +25 stat "Magnificent"
                        var _ when (skill.Current >= 200 && skill.Current < 250) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT5"),
                        //level VI +30 stat "Nearly flawless"
                        var _ when (skill.Current >= 250 && skill.Current < 300) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT6"),
                        //level VII +35 stat "Flawless"
                        var _ when (skill.Current >= 300 && skill.Current < 350) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT7"),
                        //level VIII +40 stat "Utterly flawless"
                        var _ when (skill.Current >= 350 && skill.Current < 400) => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT8"),
                        //level 'IX' +45 stat "Incomparable"
                        _ => WorldObjectFactory.CreateNewWorldObject("phACE.phood-UlgrimsSpecialReserveT9"),
                    };
                    wo.SetStackSize(50);

                    var actionChain = new ActionChain();

                    // if something other that NonCombat.Ready,
                    // manually send this swap
                    var prevStance = player.CurrentMotionState.Stance;

                    var animTime = 0.0f;

                    if (prevStance != MotionStance.NonCombat)
                        animTime = player.EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.Ready, (MotionCommand)prevStance);

                    // start the eat/drink motion
                    var useAnimTime = player.EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.ClapHands);
                    animTime += useAnimTime;

                    // remove source/target
                    actionChain.AddAction(player, () => { player.TryConsumeFromInventoryWithNetworking(source, 1); });
                    actionChain.AddAction(player, () => { player.TryConsumeFromInventoryWithNetworking(target, 1); });

                    // create beers in player inv
                    actionChain.AddAction(player, () => { player.TryCreateInInventoryWithNetworking(wo); });

                    if (prevStance != MotionStance.NonCombat)
                        animTime += player.EnqueueMotion_Force(actionChain, prevStance, MotionCommand.Ready, MotionCommand.NonCombat);

                    actionChain.AddAction(player, () => { player.SendUseDoneEvent(); });

                    actionChain.EnqueueChain();

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
            player.TryConsumeFromInventoryWithNetworking(__instance, 1);

            return false;
        }
        #endregion

        #endregion
    }
}
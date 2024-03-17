using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.WorldObjects;
using HarmonyLib;

namespace phACE.phletching
{
    [HarmonyPatchCategory("MissileRecovery")]
    public class Patch_MissileRecovery
    {
        #region Init/Fini
        private const bool DEBUG_PRINT = false;
        static readonly Dictionary<uint, List<uint>> ammoHistory = new();
        
        public static void Init()
        {
            ammoHistory.Clear();
        }
        public static void Fini()
        {
            ammoHistory.Clear();
        }
        #endregion

        #region Patches

        #region ProjectileCollisionHelper.cs
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ProjectileCollisionHelper), nameof(ProjectileCollisionHelper.OnCollideObject), new Type[] { typeof(WorldObject), typeof(WorldObject) })]
        public static bool PreOnCollideObject(WorldObject worldObject, WorldObject target)
        {
            if (!worldObject.PhysicsObj.is_active()) return false;

            if (worldObject.ProjectileTarget == null || worldObject.ProjectileTarget != target)
            {
                ProjectileCollisionHelper.OnCollideEnvironment(worldObject);
                return false;
            }

            if (target is Creature targetCreature && targetCreature.IsAlive)
            {
                if (worldObject.ProjectileSource is Player sourcePlayer)
                {
                    bool projectileBroken = true;
                    sourcePlayer.Skills.TryGetValue(Skill.Fletching, out var skill);
                    if (skill != null && Mod.Settings != null)
                    {
                        var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                        if (skill.AdvancementClass == SkillAdvancementClass.Untrained)
                        {
                            if (rng < Mod.Settings.UntrainedRecoveryChance) //default 10% untrained retrieval chance regardless of skill level
                            {
                                projectileBroken = false;
                            }
                        }
                        else if (skill.AdvancementClass == SkillAdvancementClass.Trained)
                        {
                            if (skill.Current < Mod.Settings.TrainedRecoveryBreakpoint)
                            {
                                if (rng < (Mod.Settings.TrainedRecoveryChance * skill.Current / Mod.Settings.TrainedRecoveryBreakpoint)) //proportion of trained retrieval chance based on breakpoint
                                {
                                    projectileBroken = false;
                                }
                            }
                            else
                            {
                                if (rng < Mod.Settings.TrainedRecoveryChance) //default 33% trained retrieval chance
                                {
                                    projectileBroken = false;
                                }
                            }
                        }
                        else if (skill.AdvancementClass == SkillAdvancementClass.Specialized)
                        {
                            if (skill.Current < Mod.Settings.SpecializedRecoveryBreakpoint)
                            {
                                if (rng < (Mod.Settings.SpecializedRecoveryChance * skill.Current / Mod.Settings.SpecializedRecoveryBreakpoint)) //proportion of specialized retrieval chance based on breakpoint
                                {
                                    projectileBroken = false;
                                }
                            }
                            else
                            {
                                if (rng < Mod.Settings.SpecializedRecoveryChance) //default 50% specialized retrieval chance
                                {
                                    projectileBroken = false;
                                }
                            }
                        }

                        if (!projectileBroken)
                        {
                            //if (DEBUG_PRINT) sourcePlayer.SendMessage("[phace.phletching]projectile survived");
                            if (!ammoHistory.TryGetValue(target.Guid.Full, out var hitList))
                            {
                                //Make/add the missing list
                                hitList = new();
                                ammoHistory.Add(target.Guid.Full, hitList);
                            }

                            //Add the successful ammo to the list
                            hitList.Add(worldObject.WeenieClassId);
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region Creature_Death.cs
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Creature), "GenerateTreasure", new Type[] { typeof(DamageHistoryInfo), typeof(Corpse) })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void PostGenerateTreasure(DamageHistoryInfo killer, Corpse corpse, ref Creature __instance, ref List<WorldObject> __result)
        {
            List<WorldObject> dropAmmo = new();
            if (ammoHistory.TryGetValue(__instance.Guid.Full, out var hitList))
            {
                while (hitList.Count > 0)
                {
                    uint wcid = hitList.First();
                    int amount = 0;
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(wcid);

                    foreach (uint projectile in hitList)
                    {
                        if (projectile == wcid) amount++;
                    }
                    var removed = hitList.RemoveAll(projectile =>  projectile == wcid);

                    while (amount > 0)
                    {
                        if (wo.MaxStackSize.HasValue)
                        {
                            if ((wo.MaxStackSize.Value != 0) & (amount > wo.MaxStackSize.Value))
                            {
                                // fit what we can in this stack and move on to a new stack
                                wo.SetStackSize(wo.MaxStackSize.Value);
                                dropAmmo.Add(wo);
                                amount -= wo.MaxStackSize.Value;
                            }
                            else
                            {
                                // we can fit all the remaining amount in this stack
                                wo.SetStackSize(amount);
                                dropAmmo.Add(wo);
                                amount = 0;
                            }
                        }
                        else
                        {
                            if (amount > 0)
                            {
                                amount--;
                                dropAmmo.Add(wo);
                            }
                        }
                    }
                }

                hitList.Clear();

                foreach (WorldObject wo in dropAmmo)
                {
                    if (corpse != null)
                        corpse.TryAddToInventory(wo);
                    else
                        __result.Add(wo);
                }
                ammoHistory.Remove(__instance.Guid.Full);
            }
            return;
        }
        #endregion

        #endregion

    }
}
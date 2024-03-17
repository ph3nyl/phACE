using ACE.Server.WorldObjects;
using ACE.Shared;
using HarmonyLib;
using System.Numerics;

namespace phACE.phletching
{
    [HarmonyPatchCategory("SnareOnHitKits")]
    public class Patch_SnareOnHitKits
    {
        #region Init/Fini
        private const bool DEBUG_PRINT = true;
        public static void Init()
        {

        }
        public static void Fini()
        {

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
                    if (worldObject.GetProperty(FakeBool.SnareOnNextMissile) == true) //snare arrow collision
                    {
                        if (DEBUG_PRINT) sourcePlayer.SendMessage("Snare Missile Collision");
                        worldObject.SetProperty(FakeBool.SnareOnNextMissile, false);
                        sourcePlayer.TryCastSpell(ContentManager.SIDs[6901], targetCreature, null, null, false, false, false);
                    }
                }
            }
            return true;
        }
        #endregion

        #region Gem.cs
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Gem), nameof(Gem.UseGem), new Type[] { typeof(Player) })]
        public static void PostUseGem(Player player, ref Gem __instance)
        {
            if (!ContentManager.WCIDs.Contains(__instance.WeenieClassId)) return;

            switch (__instance.WeenieClassId)
            {
                case 69000:
                player.SendMessage("You use the Snare Kit, causing your next missile attack to Snare on Hit");
                player.SetProperty(FakeBool.SnareOnNextMissile, true);
                break;
            }
        }
        #endregion

        #region Creature_Missile.cs
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Creature), nameof(Creature.LaunchProjectile), new Type[] { typeof(WorldObject), typeof(WorldObject), typeof(WorldObject), typeof(Vector3), typeof(Quaternion), typeof(Vector3) })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void PostLaunchProjectile(WorldObject weapon, WorldObject ammo, WorldObject target, Vector3 origin, Quaternion orientation, Vector3 velocity, ref Creature __instance, ref WorldObject __result)
        {
            if (__instance is Player player && __instance.GetProperty(FakeBool.SnareOnNextMissile) == true)
            {
                if (DEBUG_PRINT) player.SendMessage("SnareOnNextMissile => false");
                player.SetProperty(FakeBool.SnareOnNextMissile, false);
                __result.SetProperty(FakeBool.SnareOnNextMissile, true);
            }
        }
        #endregion

        #endregion

    }
}
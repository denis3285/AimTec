namespace Support_AIO.Bases
{
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;

    using Spell = Aimtec.SDK.Spell;
    using Aimtec.SDK.TargetSelector;

    /// <summary>
    ///     The UtilityData class.
    /// </summary>
    internal static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the valid generic (lane or jungle) minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllGenericMinionsTargets()
        {
            return GetAllGenericMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (lane or jungle) minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllGenericMinionsTargetsInRange(float range)
        {
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game.
        /// </summary>
        public static List<Obj_AI_Base> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
        }
        public static bool IsWall(this Vector3 pos, bool includeBuildings = false)
        {
            var point = NavMesh.WorldToCell(pos).Flags;
            return point.HasFlag(NavCellFlags.Wall) || includeBuildings && point.HasFlag(NavCellFlags.Building);
        }
        public static bool AnyWallInBetween(Vector3 startPos, Vector3 endPos)
        {
            for (var i = 0; i < startPos.Distance(endPos); i++)
            {
                var point = NavMesh.WorldToCell(startPos.Extend(endPos, i));
                if (point.Flags.HasFlag(NavCellFlags.Wall) || point.Flags.HasFlag(NavCellFlags.Building))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        ///     Gets the valid ally heroes targets in the game.
        /// </summary>
        public static List<Obj_AI_Hero> GetAllyHeroesTargets()
        {
            return GetAllyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetAllyHeroesTargetsInRange(float range)
        {
            return GameObjects.AllyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyLaneMinionsTargets()
        {
            return GetAllyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.AllyMinions.Where(m => m.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetBestEnemyHeroesTargets()
        {
            return GetBestEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetBestEnemyHeroesTargetsInRange(float range)
        {
            return TargetSelector.Implementation.GetOrderedTargets(range);
        }


        public static Obj_AI_Hero GetBestEnemyHeroTarget()
        {
            return GetBestEnemyHeroTargetInRange(float.MaxValue);
        }

        public static Obj_AI_Hero GetBestEnemyHeroTargetInRange(float range)
        {
            var ts = TargetSelector.Implementation;
            var target = ts.GetTarget(range);
            if (target != null && target.IsValidTarget())
            {
                return target;
            }

            var firstTarget = ts.GetOrderedTargets(range)
                .FirstOrDefault(t => t.IsValidTarget());
            if (firstTarget != null)
            {
                return firstTarget;
            }

            return null;
        }

        public static Obj_AI_Hero GetBestKillableHero(this Spell spell, DamageType damageType = DamageType.True, bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(spell.Range-100f).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game.
        /// </summary>
        public static List<Obj_AI_Hero> GetEnemyHeroesTargets()
        {
            return GetEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetEnemyHeroesTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidTarget(range) && m.UnitSkinName.Contains("Minion") && !m.UnitSkinName.Contains("Odin")).ToList();
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.Jungle.Where(m => !GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetLargeJungleMinionsTargets()
        {
            return GetLargeJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetLargeJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLarge.Where(m => m.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetLegendaryJungleMinionsTargets()
        {
            return GetLegendaryJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetLegendaryJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLegendary.Where(m => m.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetSmallJungleMinionsTargets()
        {
            return GetSmallJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetSmallJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleSmall.Where(m => m.IsValidTarget(range)).ToList();
        }

        #endregion
    }
}
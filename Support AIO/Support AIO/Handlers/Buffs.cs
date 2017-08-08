#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Handlers/Buffs.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Support_AIO.Handlers
{
    #region

    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Base;
    using Data;

    #endregion

    internal static class Buffs
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the custom damage.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="auraname">The auraname.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static int GetCustomDamage(this Obj_AI_Hero source, string auraname, Obj_AI_Hero target)
        {
            // todo: needs updating badly! ;_;

            if (auraname == "sheen")
            {
                return
                    (int)
                    source.CalculateDamage(target, DamageType.Physical,
                        1.0 * source.FlatPhysicalDamageMod + source.BaseAttackDamage);
            }

            if (auraname == "lichbane")
            {
                return
                    (int)
                    source.CalculateDamage(target, DamageType.Magical,
                        (0.75 * source.FlatPhysicalDamageMod + source.BaseAttackDamage) +
                        (0.50 * source.FlatMagicDamageMod));
            }

            return 0;
        }

        #endregion

        internal static void StartOnUpdate()
        {
            Game.OnUpdate += OnAllyBuffUpdate;
            Game.OnUpdate += OnEnemyBuffUpdate;
            BuffManager.OnAddBuff += Obj_AI_Base_OnBuffAdd;
        }

        #region Private Methods

        private static void OnEnemyBuffUpdate()
        {
            foreach (var enemy in ZLib.Heroes)
            {
                if (!enemy.Player.IsEnemy
                    || enemy.Player == null
                    || !enemy.Player.IsValidTarget())
                {
                    continue;
                }

                var aura = ZLib.CachedAuras.Find(au => enemy.Player.HasBuff(au.Name));
                if (aura == null)
                {
                    continue;
                }

                Gamedata data = null;

                if (aura.Champion == null
                    && aura.Slot == SpellSlot.Unknown)
                    data = new Gamedata { SpellName = aura.Name };

                if (aura.Champion != null
                    && aura.Slot != SpellSlot.Unknown)
                {
                    data = ZLib.CachedSpells.Where(x => x.Slot == aura.Slot)
                        .FirstOrDefault(x => x.HeroNameMatch(aura.Champion));
                }

                if (aura.Reverse
                    && aura.DoT)
                {
                    if ((int) (Game.ClockTime * 1000) - aura.TickLimiter >= aura.Interval * 1000)
                    {
                        foreach (var ally in ZLib.Allies())
                        {
                            if (ally.Player.Distance(enemy.Player) <= aura.Radius + 35)
                            {
                                Projections.EmulateDamage(enemy.Player, ally, data, EventType.Buff, "aura.DoT");
                            }
                        }

                        aura.TickLimiter = (int) (Game.ClockTime * 1000);
                    }
                }

                if (aura.Reverse
                    && aura.Evade)
                {
                    if ((int) (Game.ClockTime * 1000) - aura.TickLimiter >= 100)
                    {
                        foreach (var ally in ZLib.Allies())
                        {
                            if (ally.Player.Distance(enemy.Player) <= aura.Radius + 35)
                            {
                                Projections.EmulateDamage(enemy.Player, ally, data, EventType.Buff, "aura.Evade");
                            }
                        }

                        aura.TickLimiter = (int) (Game.ClockTime * 1000);
                    }
                }
            }
        }

        private static void Obj_AI_Base_OnBuffAdd(Obj_AI_Base sender, Buff buff)
        {
            #region Buffs

            foreach (var ally in ZLib.Allies())
            {
                if (sender.NetworkId == ally.Player.NetworkId)
                {
                    if (buff.Name == "rengarralertsound")
                    {
                        Projections.EmulateDamage(sender, ally, new Gamedata { SpellName = "Stealth" },
                            EventType.Stealth,
                            "handlers.OnBuffAdd");
                    }
                }
            }

            #endregion
        }

        private static void OnAllyBuffUpdate()
        {
            foreach (var hero in ZLib.Allies())
            {
                if (hero.Player == null
                    || !hero.Player.IsValidTarget())
                {
                    continue;
                }

                var aura = ZLib.CachedAuras.Find(au => hero.Player.HasBuff(au.Name));
                if (aura == null)
                {
                    continue;
                }

                if (aura.Reverse)
                {
                    continue;
                }

                var owner = hero.Player.GetBuff(aura.Name).Caster as Obj_AI_Hero;
                if (owner == null
                    || !owner.IsEnemy)
                {
                    continue;
                }

                Gamedata data = null;

                if (aura.Champion == null
                    && aura.Slot == SpellSlot.Unknown)
                    data = new Gamedata { SpellName = aura.Name };

                if (aura.Champion != null
                    && aura.Slot != SpellSlot.Unknown)
                {
                    data = ZLib.CachedSpells.Where(x => x.Slot == aura.Slot)
                        .FirstOrDefault(x => x.HeroNameMatch(aura.Champion));
                }

                if (aura.Evade)
                {
                    DelayAction.Queue(aura.EvadeTimer,
                        () =>
                        {
                            // double check after delay incase we no longer have the buff
                            if (hero.Player.HasBuff(aura.Name))
                            {
                                if ((int) (Game.ClockTime * 1000) - aura.TickLimiter >= 250)
                                {
                                    Projections.EmulateDamage(owner, hero, data, EventType.Buff, "aura.Evade");
                                    aura.TickLimiter = (int) (Game.ClockTime * 1000);
                                }
                            }
                        });
                }

                if (aura.DoT)
                {
                    if ((int) (Game.ClockTime * 1000) - aura.TickLimiter >= aura.Interval * 1000)
                    {
                        if (aura.Name == "velkozresearchstack"
                            && !hero.Player.HasBuffOfType(BuffType.Slow))
                        {
                            continue;
                        }

                        // ReSharper disable once PossibleNullReferenceException
                        Projections.EmulateDamage(owner, hero, data, EventType.Buff, "aura.DoT");
                        aura.TickLimiter = (int) (Game.ClockTime * 1000);
                    }
                }
            }
        }

        #endregion
    }
}
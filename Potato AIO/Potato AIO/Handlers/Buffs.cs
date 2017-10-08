#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Handlers/Buffs.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Potato_AIO.Handlers
{
    #region

    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
    using Base;
    using Data;

    #endregion

    internal static class Buffs
    {
        #region Private Methods and Operators

        private static void OnBuffProximity()
        {
            try
            {
                foreach (var unit in ZLib.GetUnits())
                {
                    if (unit.Instance == null || !unit.Instance.IsValidTarget(float.MaxValue, true))
                    {
                        continue;
                    }

                    var aura = ZLib.CachedAuras.Find(au => unit.Instance.HasBuff(au.Name));
                    if (aura == null)
                    {
                        continue;
                    }

                    var owner = GetBuffCaster(unit, aura.Name);
                    if (owner == null || unit.Instance.NetworkId == owner.NetworkId)
                    {
                        continue;
                    }

                    Gamedata data = null;

                    if (aura.Champion == null && aura.Slot == SpellSlot.Unknown)
                        data = new Gamedata { SpellName = aura.Name };

                    if (aura.Champion != null && aura.Slot != SpellSlot.Unknown)
                    {
                        data = ZLib.CachedSpells
                            .Where(x => x.Slot == aura.Slot).FirstOrDefault(x => x.HeroNameMatch(aura.Champion));
                    }

                    if (aura.Reverse && aura.DoT)
                    {
                        if ((int)(Game.ClockTime * 1000) - aura.TickLimiter >= aura.Interval * 1000)
                        {
                            foreach (var ally in ZLib.GetUnits())
                            {
                                if (ally.Instance.Distance(unit.Instance) <= aura.Radius + 35)
                                {
                                    Projections.EmulateDamage(owner, ally, data, EventType.Buff, "aura.dot");
                                }
                            }

                            aura.TickLimiter = (int)(Game.ClockTime * 1000);
                        }
                    }

                    if (aura.Reverse && aura.Evade)
                    {
                        if ((int)(Game.ClockTime * 1000) - aura.TickLimiter >= 100)
                        {
                            foreach (var ally in ZLib.GetUnits())
                            {
                                if (ally.Instance.Distance(unit.Instance) <= aura.Radius + 35)
                                {
                                    Projections.EmulateDamage(owner, ally, data, EventType.Buff, "aura.evade");
                                }
                            }

                            aura.TickLimiter = (int)(Game.ClockTime * 1000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.OnBuffSurrounding");
                    Console.WriteLine(e);
                }
            }
        }

        private static void OnBuffGain(Obj_AI_Base sender, Buff buff)
        {
            #region Buffs

            foreach (var ally in ZLib.GetUnits())
            {
                if (sender.NetworkId == ally.Instance.NetworkId)
                {
                    if (buff.Name == "rengarralertsound")
                    {
                        Projections.EmulateDamage(sender, ally, new Gamedata { SpellName = "Stealth" }, EventType.Stealth,
                            "handlers.onbuffadd");
                    }
                }
            }

            #endregion
        }

        private static void OnBuffUpdate()
        {
            try
            {
                foreach (var hero in ZLib.GetUnits())
                {
                    if (hero.Instance == null || !hero.Instance.IsValidTarget(float.MaxValue, true))
                    {
                        continue;
                    }

                    var aura = ZLib.CachedAuras.Find(au => hero.Instance.HasBuff(au.Name));
                    if (aura == null)
                    {
                        continue;
                    }

                    var owner = GetBuffCaster(hero, aura.Name);
                    if (owner == null || owner.NetworkId == hero.Instance.NetworkId)
                    {
                        continue;
                    }

                    if (aura.Reverse)
                    {
                        continue;
                    }

                    var data = new Gamedata();

                    if (aura.Champion == null && aura.Slot == SpellSlot.Unknown)
                        data = new Gamedata { SpellName = aura.Name };

                    if (aura.Champion != null && aura.Slot != SpellSlot.Unknown)
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
                                if (hero.Instance.HasBuff(aura.Name))
                                {
                                    if ((int)(Game.ClockTime * 1000) - aura.TickLimiter >= 250)
                                    {
                                        Projections.EmulateDamage(owner, hero, data, EventType.Buff, "aura.evade");
                                        aura.TickLimiter = (int)(Game.ClockTime * 1000);
                                    }
                                }
                            });
                    }

                    if (aura.DoT)
                    {
                        if ((int)(Game.ClockTime * 1000) - aura.TickLimiter >= aura.Interval * 1000)
                        {
                            if (aura.Name == "velkozresearchstack" && !hero.Instance.HasBuffOfType(BuffType.Slow))
                            {
                                continue;
                            }

                            // ReSharper disable once PossibleNullReferenceException
                            Projections.EmulateDamage(owner, hero, data, EventType.Buff, "aura.dot");
                            aura.TickLimiter = (int)(Game.ClockTime * 1000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.OnBuffUpdate");
                    Console.WriteLine(e);
                }
            }
        }

        private static Obj_AI_Base GetBuffCaster(Unit unit, string auraName)
        {
            var buffInst = unit.Instance.BuffManager.GetBuff(auraName, true);
            if (buffInst != null)
            {
                if (buffInst.IsActive && buffInst.IsValid)
                {
                    return buffInst.Caster as Obj_AI_Base;
                }
            }

            return null;
        }

        #endregion

        #region Internal Methods and Operators

        /// <summary>
        ///     Amplifies the basic attack.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>System.Double.</returns>
        internal static double AmplifyAuto(this Obj_AI_Base source, Obj_AI_Base target)
        {
            double dmg = 0;

            var aiHero = source as Obj_AI_Hero;
            if (aiHero != null)
            {
                if (source.HasBuff("sheen"))
                    dmg +=
                        source.CalculateDamage(target, DamageType.Physical,
                            1.0 * source.FlatPhysicalDamageMod + source.BaseAttackDamage);

                if (source.HasBuff("lichbane"))
                {
                    dmg +=
                        source.CalculateDamage(target, DamageType.Magical,
                            (0.75 * source.FlatPhysicalDamageMod + source.BaseAttackDamage) +
                            (0.50 * source.FlatMagicDamageMod));
                }

                if (source.HasBuff("itemdisknighstalkerdamageproc"))
                {
                    dmg += source.CalculateDamage(target, DamageType.Physical, 50 + (15 * source.Level));
                }

                if (source.HasBuff("itemstatikshankcharge") && source.GetBuff("itemstatikshankcharge").Count == 100)
                {
                    return (int)source.CalculateDamage(target, DamageType.Physical,
                        new[] { 62, 120, 200, 200 }[Math.Min(18, source.Level) / 6]);
                }

                foreach (var data in ZLib.CachedSpells.Where(x => x.ChampionName.ToLower() == aiHero.ChampionName.ToLower()))
                {
                    if (aiHero.HasBuff(data.SpellName.ToLower()))
                    {
                        if (data.BasicAttackAmplifier)
                        {
                            dmg += aiHero.GetSpellDamage(target, data.Slot);
                        }
                    }
                    else if (aiHero.Buffs.Any(x => x.IsActive && x.IsValid && x.Name.ToLower().Contains(data.SpellName.ToLower())))
                    {
                        if (data.BasicAttackAmplifier)
                        {
                            dmg += aiHero.GetSpellDamage(target, data.Slot);
                        }
                    }
                }

                return dmg;
            }

            return dmg;
        }

        internal static void Initialize()
        {
            Game.OnUpdate += OnBuffUpdate;
            Game.OnUpdate += OnBuffProximity;
            BuffManager.OnAddBuff += OnBuffGain;
        }

        #endregion
    }
}
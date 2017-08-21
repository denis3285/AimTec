#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    ZLib.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

using Aimtec.SDK.Util.Cache;
using Support_AIO.Base;
using Support_AIO.Data;
using Support_AIO.Handlers;

namespace Support_AIO
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
 

    #endregion

    /// <summary>
    ///     Class ZLib.
    /// </summary>
    public static class ZLib
    {
        #region Delegates and Events

        /// <summary>
        ///     The OnPredictDamage event.
        /// </summary>
        public static event OnPredictDamageHandlder OnPredictDamage;

        /// <summary>
        ///     The OnPredictDamage handlder.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="args">The <see cref="PredictDamageEventArgs" /> instance containing the event data.</param>
        public delegate void OnPredictDamageHandlder(Unit unit, PredictDamageEventArgs args);

        #endregion

        #region Static Fields and Constants

        /// <summary>
        ///     The list of all the units the library is tracking
        /// </summary>
        public static List<Unit> AllUnits = new List<Unit>();

        /// <summary>
        ///     The active predicted damge instances [HPInstance].
        /// </summary>
        public static Dictionary<int, HPInstance> DamageCollection = new Dictionary<int, HPInstance>();

        /// <summary>
        ///     A generated entry lists of all spells.
        /// </summary>
        public static List<Gamedata> Spells = new List<Gamedata>();

        /// <summary>
        ///     A generated entry lists of spells only in the current game.
        /// </summary>
        public static List<Gamedata> CachedSpells = new List<Gamedata>();

        /// <summary>
        ///     A generated entry lists of enemy spells only in the current game.
        /// </summary>
        public static List<Gamedata> EnemyCachedSpells = new List<Gamedata>();

        /// <summary>
        ///     A generated entry lists of ally spells only in the current game.
        /// </summary>
        public static List<Gamedata> AllyCachedSpells = new List<Gamedata>();

        /// <summary>
        ///     A generated entry list of all auras.
        /// </summary>
        public static List<Auradata> AuraList = new List<Auradata>();

        /// <summary>
        ///     A generated entry lists of auras only in the current game.
        /// </summary>
        public static List<Auradata> CachedAuras = new List<Auradata>();

        /// <summary>
        ///     A generated entry list of all troys.
        /// </summary>
        public static List<Troydata> TroyList = new List<Troydata>();

        /// <summary>
        ///     A generated entry list of all turrets.
        /// </summary>
        public static List<Unit> Turrets = new List<Unit>();

        internal static bool Init;
        internal static Menu Menu;

        #endregion

        #region Private Methods and Operators

        private static void GetHeroesInGame()
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>())
            {
                AllUnits.Add(new Unit { Instance = i });
            }

            Console.WriteLine("ZLib: Heroes List Generated!");
        }

        private static void GetTurretsInGame()
        {
            foreach (var t in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (!t.IsDead)
                {
                    Turrets.Add(new Unit { Instances = t });
                }
            }

            Console.WriteLine("ZLib: Turret List Generated!");
        }

        private static void GetGameTroysInGame(Troydata data)
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>())
            {
                foreach (var item in TroyList.Where(x => x.ChampionName.ToLower() == i.ChampionName.ToLower()))
                {
                    Gametroy.Troys.Add(new Gametroy(i.ChampionName, item.Slot, item.Name, 0, false));
                }
            }

            Console.WriteLine("ZLib: Gametroy List Generated!");
        }



        private static void GetSpellsInGame(Gamedata data)
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.Team != ObjectManager.GetLocalPlayer().Team))
            {
                foreach (var item in Spells.Where(x => x.ChampionName.ToLower() == i.ChampionName.ToLower()))
                {
                    CachedSpells.Add(item);
                    EnemyCachedSpells.Add(item);
                }
            }

            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.Team == ObjectManager.GetLocalPlayer().Team))
            {
                foreach (var item in Spells.Where(x => x.ChampionName.ToLower() == i.ChampionName.ToLower()))
                {
                    CachedSpells.Add(item);
                    AllyCachedSpells.Add(item);
                }
            }

            Console.WriteLine("ZLib: Spell List Generated!");
        }

        private static void GetAurasInGame(Auradata data)
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>())
            {
                foreach (var aura in AuraList.Where(x => x.Champion != null && x.Champion.ToLower() == i.ChampionName.ToLower()))
                {
                    CachedAuras.Add(aura);
                }
            }

            foreach (var generalaura in AuraList.Where(x => string.IsNullOrEmpty(x.Champion)))
            {
                CachedAuras.Add(generalaura);
            }

            Console.WriteLine("ZLib: Aura List Generated!");
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Attaches the specified the menu initializing the library.
        /// </summary>
        /// <param name="parent">The root menu.</param>
        /// <param name="menuDisplayName">Display name of the attached menu.</param>
        public static void Attach(Menu parent, string menuDisplayName = "ZLib")
        {
            GetHeroesInGame();
            GetTurretsInGame();
            GetSpellsInGame(new Gamedata());
            GetAurasInGame(new Auradata());
            GetGameTroysInGame(new Troydata());

            Helpers.CreateLogPath();

            Menu = new Menu("zlib", menuDisplayName);
            var hpmenu = new Menu("dhp", "Debug Health Prediction");

            var itest = new MenuBool("testdamage", "Trigger Damage Event", false);
            hpmenu.Add(itest).OnValueChanged += (sender, eventArgs) =>
            {
                if (eventArgs.GetNewValue<MenuBool>().Value)
                {
                    var caster = ObjectManager.GetLocalPlayer();

                    var target = AllUnits.First(x =>
                        ((Obj_AI_Hero)x.Instance).ChampionName.ToLower() ==
                        Menu["dhp"]["testdamagetarget"]
                            .As<MenuList>().SelectedItem.ToLower());

                    var type = (EventType)Enum.Parse(typeof(EventType),
                        Menu["dhp"]["testdamagetype"].As<MenuList>().SelectedItem);

                    Projections.EmulateDamage(caster, target, new Gamedata { SpellName = "test" + Environment.TickCount }, type,
                        "debug.Test");
                    DelayAction.Queue(100, () => itest.Value = false);
                }
            };

            hpmenu.Add(new MenuList("testdamagetype", "EventType",
                Enum.GetValues(typeof(EventType)).Cast<EventType>().Select(v => v.ToString()).ToArray(), 0));
            hpmenu.Add(new MenuList("testdamagetarget", "Target",
                AllUnits.Select(x => ((Obj_AI_Hero)x.Instance).ChampionName).ToArray(), AllUnits.Count - 1));

            Menu.Add(hpmenu);
            Menu.Add(new MenuBool("acdebug", "Debug Income Damage", false));
            Menu.Add(new MenuBool("dumpdata", "Debug & Dump Spell Data", false));
            Menu.Add(new MenuBool("logerror", "Debug Errors in Console"));

            Menu.Add(new MenuList("hprior", "Hero Priority:", new[] { "Low HP", "Most AD/AP", "Most HP" }, 1));
            Menu.Add(
                new MenuSlider("weightdmg", "Weight Income Damage (%)", 115, 100, 150).SetToolTip(
                    "Make it think you are taking more damage than calulated."));
            Menu.Add(
                new MenuSlider("lagtolerance", "Lag Tolerance (%)", 25).SetToolTip(
                    "Make it think you are taking damage longer than intended"));
            var WhiteList = new Menu("whitelist", "Whitelist");
            {
                foreach (var target in GameObjects.AllyHeroes)
                {
                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                }
            }
            Menu.Add(WhiteList);
            parent.Add(Menu);

            Projections.Initizialize();
            Drawings.Initialize();
            Buffs.Initialize();
            Gametroys.Initialize();

            Console.WriteLine("ZLib: Loaded!");
        }

        /// <summary>
        ///     Gets the units in the priority order.
        /// </summary>
        /// <returns>IEnumerable&lt;Unit&gt;.</returns>
        public static IEnumerable<Unit> GetUnits()
        {
            switch (Menu["hprior"].As<MenuList>().Value)
            {
                case 0:
                    return AllUnits.Where(h => !h.Instance.IsDead)
                        .OrderBy(h => h.Instance.Health / h.Instance.MaxHealth * 100);

                case 1:
                    return AllUnits.Where(h => !h.Instance.IsDead)
                        .OrderByDescending(h => h.Instance.FlatPhysicalDamageMod + h.Instance.FlatMagicDamageMod);

                case 2:
                    return AllUnits.Where(h => !h.Instance.IsDead)
                        .OrderByDescending(h => h.Instance.Health);
            }

            return null;
        }

        /// <summary>
        ///     Gets the game data.
        /// </summary>
        /// <param name="hero">The hero.</param>
        /// <param name="slot">The slot.</param>
        /// <returns>Gamedata.</returns>
        public static Gamedata GetGameData(Obj_AI_Hero hero, SpellSlot slot)
        {
            foreach (var entry in Spells)
            {
                if (entry.ChampionName.ToLower() == hero.ChampionName.ToLower())
                {
                    if (entry.Slot == slot)
                    {
                        return entry;
                    }
                }
            }

            return new Gamedata();
        }

        /// <summary>
        ///     Gets the game data.
        /// </summary>
        /// <param name="hero">The hero.</param>
        /// <param name="spellName">Name of the spell.</param>
        /// <returns>Gamedata.</returns>
        public static Gamedata GetGameData(Obj_AI_Hero hero, string spellName)
        {
            foreach (var entry in Spells)
            {
                if (entry.ChampionName.ToLower() == hero.ChampionName.ToLower())
                {
                    if (entry.SpellName.ToLower() == spellName.ToLower())
                    {
                        return entry;
                    }
                }
            }

            return new Gamedata();
        }

        /// <summary>
        ///     Emulates the damage.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="hero">The hero.</param>
        /// <param name="data">The data.</param>
        /// <param name="dmgType">Type of the damage.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="dmgEntry">The damage ammount.</param>
        /// <param name="expiry">The time of expiry.</param>
        public static void EmulateDamage(Obj_AI_Base sender, Unit hero, Gamedata data, EventType dmgType, string notes = null,
            float dmgEntry = 0f, int expiry = 500)
        {
            Projections.EmulateDamage(sender, hero, data, dmgType, notes, dmgEntry, expiry);
        }

        #endregion

        #region Internal Methods and Operators

        internal static void TriggerOnPredictDamage(Unit unit, PredictDamageEventArgs args)
        {
            try
            {
                OnPredictDamage?.Invoke(unit, args);
            }
            catch (Exception e)
            {
                if (Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at ZLib.TriggerOnPredictDamage");
                    Console.WriteLine(e);
                }
            }
        }

        #endregion
    }
}
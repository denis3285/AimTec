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
    using Base;
    using Data;
    using Handlers;

    #endregion

    public static class ZLib
    {
        #region Delegates and Events

        public static event OnPredictDamageHanlder OnPredictDamage;

        public delegate void OnPredictDamageHanlder(Champion hero, PredictDamageEventArgs args);

        #endregion

        #region Static Fields and Constants

        internal static bool Init;
        internal static Menu Menu;

        /// <summary>
        ///     The heroes
        /// </summary>
        public static List<Champion> Heroes = new List<Champion>();

        /// <summary>
        ///     The active predicted damge instances
        /// </summary>
        public static Dictionary<int, HPInstance> DamageCollection = new Dictionary<int, HPInstance>();

        /// <summary>
        ///     A generated spell lists of everything
        /// </summary>
        public static List<Gamedata> Spells = new List<Gamedata>();

        /// <summary>
        ///     A generated spell lists of spells only in the current game.
        /// </summary>
        public static List<Gamedata> CachedSpells = new List<Gamedata>();

        /// <summary>
        ///     A generated aura lists of everything
        /// </summary>
        public static List<Auradata> AuraList = new List<Auradata>();

        /// <summary>
        ///     A generated aura lists of spells only in the current game.
        /// </summary>
        public static List<Auradata> CachedAuras = new List<Auradata>();

        /// <summary>
        ///     A generated troy lists of everything
        /// </summary>
        public static List<Troydata> TroyList = new List<Troydata>();

        #endregion

        #region Private Methods and Operators

        private static void GetHeroesInGame()
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.Team != Player.Team))
            {
                Heroes.Add(new Champion { Player = i });
            }

            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.Team == Player.Team))
            {
                Heroes.Add(new Champion { Player = i });
            }
        }

        private static void GetGameTroysInGame()
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.Team != Player.Team))
            {
                foreach (var item in TroyList.Where(x => x.ChampionName.ToLower() == i.ChampionName.ToLower()))
                {
                    Gametroy.Troys.Add(new Gametroy(i.ChampionName, item.Slot, item.Name, 0, false));
                }
            }
        }

        private static void GetSpellsInGame()
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.Team != Player.Team))
            {
                foreach (var item in Spells.Where(x => x.ChampionName.ToLower() == i.ChampionName.ToLower()))
                {
                    CachedSpells.Add(item);
                }
            }
        }

        private static void GetAurasInGame()
        {
            foreach (var i in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.Team != Player.Team))
            {
                foreach (var aura in AuraList.Where(x => x.Champion != null
                    && x.Champion.ToLower() == i.ChampionName.ToLower()))
                {
                    CachedAuras.Add(aura);
                }
            }

            foreach (var generalaura in AuraList.Where(x => string.IsNullOrEmpty(x.Champion)))
            {
                CachedAuras.Add(generalaura);
            }
        }

        private static void LoadSpellMenu(Menu parent)
        {
            foreach (var unit in Heroes.Where(h => h.Player.Team != Player.Team))
            {
                var menu = new Menu(unit.Player.ChampionName.ToLower() + "menu", unit.Player.ChampionName);

                // new menu per spell
                foreach (var entry in CachedSpells)
                {
                    if (entry.ChampionName.ToLower() == unit.Player.ChampionName.ToLower())
                    {
                        var newmenu = new Menu(entry.SpellName, entry.SpellName);

                        // activation parameters
                        newmenu.Add(new MenuBool(entry.SpellName + "predict", "Enabled"));
                        newmenu.Add(new MenuBool(entry.SpellName + "danger", "Danger",
                            entry.EventTypes.Contains(EventType.Danger)));
                        newmenu.Add(new MenuBool(entry.SpellName + "crowdcontrol", "CC",
                            entry.EventTypes.Contains(EventType.CrowdControl)));
                        newmenu.Add(new MenuBool(entry.SpellName + "ultimate", "Danger Ultimate",
                            entry.EventTypes.Contains(EventType.Ultimate)));
                        newmenu.Add(new MenuBool(entry.SpellName + "forceexhaust", "Force Exhaust",
                            entry.EventTypes.Contains(EventType.ForceExhaust)));
                        menu.Add(newmenu);

                        DelayAction.Queue(5000,
                            () => newmenu[entry.SpellName + "predict"].As<MenuBool>().Value = entry.CastRange != 0);
                    }
                }

                parent.Add(menu);
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void Attach(Menu parent)
        {
            if (Init)
            {
                return;
            }

            new Auradata();
            new Gamedata();
            new Troydata();

            GetHeroesInGame();
            GetSpellsInGame();
            GetGameTroysInGame();
            GetAurasInGame();

            Helpers.CreateLogPath();

            if (Player.ChampionName == "Zilean")

            {
                Menu = new Menu("zlib", "ZLib");

                var dmenu = new Menu("debugmenu", "Debug Tools");
                var hpmenu = new Menu("dhp", "Debug Health Prediction");

                var itest = new MenuBool("testdamage", "Trigger Damage Event", false);
                hpmenu.Add(itest).OnValueChanged += (sender, eventArgs) =>
                {
                    if (eventArgs.GetNewValue<MenuBool>().Value)
                    {
                        var caster = ObjectManager.GetLocalPlayer();

                        var target = Heroes.First(x =>
                            x.Player.ChampionName.ToLower() ==
                            Menu["debugmenu"]["dhp"]["testdamagetarget"]
                                .As<MenuList>().SelectedItem.ToLower());

                        var type = (EventType) Enum.Parse(typeof(EventType),
                            Menu["debugmenu"]["dhp"]["testdamagetype"].As<MenuList>().SelectedItem);

                        Projections.EmulateDamage(caster, target,
                            new Gamedata {SpellName = "test" + Environment.TickCount}, type,
                            "debug.Test");
                        DelayAction.Queue(100, () => itest.Value = false);
                    }
                };

                hpmenu.Add(new MenuList("testdamagetype", "EventType",
                    Enum.GetValues(typeof(EventType)).Cast<EventType>().Select(v => v.ToString()).ToArray(), 0));
                hpmenu.Add(new MenuList("testdamagetarget", "Target",
                    Heroes.Select(x => x.Player.ChampionName).ToArray(), 0));

                dmenu.Add(hpmenu);
                dmenu.Add(new MenuBool("acdebug", "Debug Income Damage", false));
                dmenu.Add(new MenuBool("dumpdata", "Debug & Dump Spell Data", false));
                Menu.Add(dmenu);

                Menu.Add(new MenuList("healthp", "Ally Priority:", new[] {"Low HP", "Most AD/AP", "Most HP"}, 1));
                Menu.Add(
                    new MenuSlider("weightdmg", "Weight Income Damage (%)", 115, 100, 150).SetToolTip(
                        "Make it think you are taking more damage than calulated."));
                Menu.Add(
                    new MenuSlider("lagtolerance", "Lag Tolerance (%)", 25).SetToolTip(
                        "Make it think you are taking damage longer than intended"));

                var uumenu = new Menu("evadem", "Spell Database");
                var WhiteList = new Menu("whitelist", "R Whitelist");
                {
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                    }
                }
                Menu.Add(WhiteList);
                LoadSpellMenu(uumenu);
                Menu.Add(uumenu);

                parent.Add(Menu);

                Projections.Init();
                Drawings.Init();
                Buffs.StartOnUpdate();
                Gametroys.StartOnUpdate();

                Init = true;
            }
            if (Player.ChampionName != "Zilean")

            {
                Menu = new Menu("zlib", "ZLib");

                var dmenu = new Menu("debugmenu", "Debug Tools");
                var hpmenu = new Menu("dhp", "Debug Health Prediction");

                var itest = new MenuBool("testdamage", "Trigger Damage Event", false);
                hpmenu.Add(itest).OnValueChanged += (sender, eventArgs) =>
                {
                    if (eventArgs.GetNewValue<MenuBool>().Value)
                    {
                        var caster = ObjectManager.GetLocalPlayer();

                        var target = Heroes.First(x =>
                            x.Player.ChampionName.ToLower() ==
                            Menu["debugmenu"]["dhp"]["testdamagetarget"]
                                .As<MenuList>().SelectedItem.ToLower());

                        var type = (EventType)Enum.Parse(typeof(EventType),
                            Menu["debugmenu"]["dhp"]["testdamagetype"].As<MenuList>().SelectedItem);

                        Projections.EmulateDamage(caster, target,
                            new Gamedata { SpellName = "test" + Environment.TickCount }, type,
                            "debug.Test");
                        DelayAction.Queue(100, () => itest.Value = false);
                    }
                };

                hpmenu.Add(new MenuList("testdamagetype", "EventType",
                    Enum.GetValues(typeof(EventType)).Cast<EventType>().Select(v => v.ToString()).ToArray(), 0));
                hpmenu.Add(new MenuList("testdamagetarget", "Target",
                    Heroes.Select(x => x.Player.ChampionName).ToArray(), 0));

                dmenu.Add(hpmenu);
                dmenu.Add(new MenuBool("acdebug", "Debug Income Damage", false));
                dmenu.Add(new MenuBool("dumpdata", "Debug & Dump Spell Data", false));
                Menu.Add(dmenu);

                Menu.Add(new MenuList("healthp", "Ally Priority:", new[] { "Low HP", "Most AD/AP", "Most HP" }, 1));
                Menu.Add(
                    new MenuSlider("weightdmg", "Weight Income Damage (%)", 115, 100, 150).SetToolTip(
                        "Make it think you are taking more damage than calulated."));
                Menu.Add(
                    new MenuSlider("lagtolerance", "Lag Tolerance (%)", 25).SetToolTip(
                        "Make it think you are taking damage longer than intended"));

                var uumenu = new Menu("evadem", "Spell Database");
                var WhiteList = new Menu("whitelist", "Shielding Whitelist");
                {
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                    }
                }
                Menu.Add(WhiteList);
                LoadSpellMenu(uumenu);
                Menu.Add(uumenu);

                parent.Add(Menu);

                Projections.Init();
                Drawings.Init();
                Buffs.StartOnUpdate();
                Gametroys.StartOnUpdate();

                Init = true;
            }
        }

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

        internal static bool IsSpellEnabled(Gamedata data)
        {
            return Menu["evadem"][data.ChampionName.ToLower() + "menu"][data.SpellName]
                [data.SpellName + "predict"].As<MenuBool>().Enabled;
        }

        /// <summary>
        ///     Checks if the spell is enabled through the menu
        /// </summary>
        /// <param name="data">The spell.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsSpellEnabled(Gamedata data, EventType type)
        {
            if (type != EventType.CrowdControl
                && type != EventType.Danger
                && type != EventType.Ultimate
                && type != EventType.ForceExhaust)
            {
                return false;
            }

            return Menu["evadem"][data.ChampionName.ToLower() + "menu"][data.SpellName]
                [data.SpellName + type.ToString().ToLower()].As<MenuBool>().Enabled;
        }

        public static void EmulateDamage(Obj_AI_Base sender, Champion hero, Gamedata data, EventType dmgType, string notes = null,
            float dmgEntry = 0f, int expiry = 500)
        {
            Projections.EmulateDamage(sender, hero, data, dmgType, notes, dmgEntry, expiry);
        }

        public static IEnumerable<Champion> Allies()
        {
            switch (Menu["healthp"].As<MenuList>().Value)
            {
                case 0:
                    return Heroes.Where(h => h.Player.IsAlly && !h.Player.IsDead)
                        .OrderBy(h => h.Player.Health / h.Player.MaxHealth * 100);

                case 1:
                    return Heroes.Where(h => h.Player.IsAlly && !h.Player.IsDead)
                        .OrderByDescending(h => h.Player.FlatPhysicalDamageMod + h.Player.FlatMagicDamageMod);

                case 2:
                    return Heroes.Where(h => h.Player.IsAlly && !h.Player.IsDead)
                        .OrderByDescending(h => h.Player.Health);
            }

            return null;
        }

        #endregion

        internal static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        internal static void TriggerOnPredictDamage(Champion hero, PredictDamageEventArgs args)
        {
            OnPredictDamage?.Invoke(hero, args);
        }
    }
}
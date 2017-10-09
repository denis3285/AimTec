using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Orbwalking;
using Aimtec;
using Aimtec.SDK;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Menu.Config;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Nunu : Champion
    {
        protected override void Combo()
        {
            if (RootMenu["savestacks"].Enabled)
            {
                if (!Player.HasBuff("visionary"))
                {
                    bool useE = RootMenu["combo"]["usee"].Enabled;


                    bool useW = RootMenu["combo"]["usew"].Enabled;
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target.IsValidTarget(E.Range))
                            {

                                if (target != null)
                                {

                                    E.CastOnUnit(target);

                                }
                            }

                        }
                    }
                    if (useW)
                    {

                        var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                        if (target.IsValidTarget(W.Range) && target != null)
                        {


                            foreach (var ally in GameObjects.AllyHeroes.Where(
                                    x => x.Distance(Player) <= W.Range &&
                                         RootMenu["priority"][x.ChampionName.ToLower() + "priority"]
                                             .As<MenuSlider>()
                                             .Value != 0)
                                .OrderByDescending(
                                    x => RootMenu["priority"][x.ChampionName.ToLower() + "priority"]
                                        .As<MenuSlider>()
                                        .Value))
                            {
                                if (ally != null)
                                {
                                    W.CastOnUnit(ally);
                                }
                            }



                        }





                    }

                }
            }
            if (!RootMenu["savestacks"].Enabled)
            {

                bool useE = RootMenu["combo"]["usee"].Enabled;


                bool useW = RootMenu["combo"]["usew"].Enabled;
                if (useE)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range))
                        {

                            if (target != null)
                            {

                                E.CastOnUnit(target);

                            }
                        }

                    }
                }
                if (useW)
                {

                    var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                    if (target.IsValidTarget(W.Range) && target != null)
                    {


                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range &&
                                     RootMenu["priority"][x.ChampionName.ToLower() + "priority"]
                                         .As<MenuSlider>()
                                         .Value != 0)
                            .OrderByDescending(
                                x => RootMenu["priority"][x.ChampionName.ToLower() + "priority"]
                                    .As<MenuSlider>()
                                    .Value))
                        {
                            if (ally != null)
                            {
                                W.CastOnUnit(ally);
                            }
                        }



                    }





                }

            }
        }



        protected override void Farming()
        {

            if (RootMenu["savestacks"].Enabled)
            {
                if (!Player.HasBuff("visionary"))
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (!minion.IsValidTarget())
                            {
                                return;
                            }

                            if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["qaa"].Enabled)
                                {
                                    Q.Cast(minion);
                                }
                                if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                                {
                                    if (!Player.HasBuff("visionary"))
                                    {
                                        if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                                        {
                                            Q.Cast(minion);
                                        }
                                    }
                                    if (Player.HasBuff("visionary"))
                                    {
                                        if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q,
                                                DamageStage.Empowered))
                                        {
                                            Q.Cast(minion);
                                        }
                                    }
                                }
                            }
                            if (RootMenu["farming"]["lane"]["usew"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(W.Range))
                                {
                                    W.Cast();
                                }


                            }
                        }
                    }
                    if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                            .Where(m => m.IsValidTarget(E.Range)).ToList())
                        {
                            if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                return;
                            }
                            bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                            bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;
                            bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;

                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.Cast(jungleTarget);
                                }
                            }
                            if (useW)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(W.Range))
                                {
                                    W.Cast();
                                }
                            }
                            if (useE)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                                {
                                    E.Cast(jungleTarget);
                                }
                            }

                        }
                    }

                }
            }
            if (!RootMenu["savestacks"].Enabled)
            {
                if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                {
                    foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {
                        if (!minion.IsValidTarget())
                        {
                            return;
                        }

                        if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                        {


                            if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["qaa"].Enabled)
                            {
                                Q.Cast(minion);
                            }
                            if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                            {
                                if (!Player.HasBuff("visionary"))
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.Cast(minion);
                                    }
                                }
                                if (Player.HasBuff("visionary"))
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q,
                                            DamageStage.Empowered))
                                    {
                                        Q.Cast(minion);
                                    }
                                }
                            }
                        }
                        if (RootMenu["farming"]["lane"]["usew"].Enabled && minion != null)
                        {


                            if (minion.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }


                        }
                    }
                }
                if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                {
                    foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                        .Where(m => m.IsValidTarget(E.Range)).ToList())
                    {
                        if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                        {
                            return;
                        }
                        bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                        bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;
                        bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;

                        if (useQ)
                        {
                            if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast(jungleTarget);
                            }
                        }
                        if (useW)
                        {
                            if (jungleTarget != null && jungleTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                        }
                        if (useE)
                        {
                            if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                            {
                                E.Cast(jungleTarget);
                            }
                        }

                    }
                }

            }
        }


        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private int rdelay;

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }

        protected override void Drawings()
        {
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;
            if (RootMenu["drawings"]["drawtoggle"].Enabled)
            {
                if (RootMenu["savestacks"].Enabled)
                {
                    Render.Text("Save Stacks: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.GreenYellow);

                }
                if (!RootMenu["savestacks"].Enabled)
                {
                    Render.Text("Save Stacks:  OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.Red);

                }
            }

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }

        }




        protected override void Killsteal()
        {

            if (E.Ready &&
                RootMenu["ks"]["kse"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    E.Cast(bestTarget);
                }
            }


        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {


        }

        protected override void Harass()
        {

            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["usee"].Enabled;
                if (useQ)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range))
                        {

                            if (target != null)
                            {
                                E.Cast(target);
                            }
                        }

                    }
                }

            }
        }



        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);
            RootMenu.Add(new MenuKeyBind("savestacks", "Save Stacks for Q", KeyCode.G, KeybindType.Toggle));
            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Auto Q nearest Minion"));
                ComboMenu.Add(new MenuSlider("qhp", " ^- if Health Percent lower than", 80, 0, 100));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));

            }
            RootMenu.Add(ComboMenu);
            var wpriorityMenu = new Menu("priority", "W Priority");
            wpriorityMenu.Add(new MenuSeperator("meow", "0 - Disabled"));
            wpriorityMenu.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
            foreach (var target in GameObjects.AllyHeroes)
            {

                wpriorityMenu.Add(new MenuSlider(target.ChampionName.ToLower() + "priority",
                    target.ChampionName + " Priority: ", 1, 0, 5));


            }
            RootMenu.Add(wpriorityMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qaa", "^- Only for Last Hit"));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("autoq", "Auto Steal Q Baron / Dragon / Buffs"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("kse", "Killseal with E"));

            }
            RootMenu.Add(KillstealMenu);


            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Toggle"));
            }
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                if (RootMenu["savestacks"].Enabled)
                {
                    if (!Player.HasBuff("visionary"))
                    {
                        E.Cast(target);
                    }
                }
                if (!RootMenu["savestacks"].Enabled)
                {

                    E.Cast(target);

                }
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 250);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 700);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 550);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 650);
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["useq"].Enabled)
            {
                if (Player.HealthPercent() <= RootMenu["combo"]["qhp"].As<MenuSlider>().Value)
                {
                    foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range))
                        {
                            Q.Cast(minion);
                        }
                    }
                    foreach (var minion in Potato_AIO.Bases.Extensions.GetGenericJungleMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range))
                        {
                            Q.Cast(minion);
                        }
                    }
                }

            }
            if (RootMenu["farming"]["jungle"]["autoq"].Enabled)
            {
                foreach (var monsters in Bases.GameObjects.Jungle)
                {
                    if (monsters != null)
                    {
                        if (monsters.Name.Contains("Dragon") || monsters.Name.Contains("Baron") ||
                            monsters.Name.Contains("Red") || monsters.Name.Contains("Blue"))
                        {
                            if (monsters.IsValidTarget() && monsters.Distance(Player) < 300)
                            {
                                if (!Player.HasBuff("visionary"))
                                {
                                    if (monsters.Health < Player.GetSpellDamage(monsters, SpellSlot.Q))
                                    {
                                        Q.Cast(monsters);
                                    }
                                }
                                if (Player.HasBuff("visionary"))
                                {
                                    if (monsters.Health <
                                        Player.GetSpellDamage(monsters, SpellSlot.Q, DamageStage.Empowered))
                                    {
                                        Q.Cast(monsters);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void LastHit()
        {
            foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                if (RootMenu["savestacks"].Enabled)
                {
                    if (!Player.HasBuff("visionary"))
                    {

                        if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                        {
                            if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                            {
                                if (!Player.HasBuff("visionary"))
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.Cast(minion);
                                    }
                                }
                                if (Player.HasBuff("visionary"))
                                {
                                    if (minion.Health <
                                        Player.GetSpellDamage(minion, SpellSlot.Q, DamageStage.Empowered))
                                    {
                                        Q.Cast(minion);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!RootMenu["savestacks"].Enabled)
                {


                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {
                        if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                        {
                            if (!Player.HasBuff("visionary"))
                            {
                                if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                                {
                                    Q.Cast(minion);
                                }
                            }
                            if (Player.HasBuff("visionary"))
                            {
                                if (minion.Health <
                                    Player.GetSpellDamage(minion, SpellSlot.Q, DamageStage.Empowered))
                                {
                                    Q.Cast(minion);
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
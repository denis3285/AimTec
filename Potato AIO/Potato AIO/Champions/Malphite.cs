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
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Malphite : Champion
    {


        protected override void Combo()
        {
            if (E.Ready && RootMenu["combo"]["useE"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast();
                        }
                    }
                }


            }
            if (Q.Ready && RootMenu["combo"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["combo"]["useW"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            W.Cast();
                        }
                    }
                }


            }

            if (R.Ready && RootMenu["combo"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)

                        {
                            if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                            {
                                R.CastIfWillHit(target, RootMenu["combo"]["hitr"].As<MenuSlider>().Value-1);
                            }
                            if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                            {
                               R.Cast(target);
                            }
                        }
                    }
                }


            }



        }


        protected override void Farming()
        {

            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                bool useW = RootMenu["farming"]["lane"]["useW"].Enabled;
                bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
                if (Q.Ready && useQ)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {


                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                if (!RootMenu["farming"]["lane"]["useQ"].Enabled)
                                {
                                    Q.Cast(minion);
                                }
                                if (RootMenu["farming"]["lane"]["useQ"].Enabled)
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.Cast(minion);
                                    }
                                }
                            }


                        }
                    }
                }


                if (useW)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {

                        if (minion.IsValidTarget(W.Range) && minion != null)
                        {
                            W.Cast();
                        }

                    }
                }
                if (useE)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(E.Range, false, false,
                                Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                        {

                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                E.Cast();
                            }
                        }
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;
                    bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;


                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                    if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast();
                    }
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }
                }
            }
        }


        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;

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

            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
                    .ToList()
                    .ForEach(
                        unit =>
                        {


                            var heroUnit = unit as Obj_AI_Hero;
                            int width = 103;
                            int height = 8;
                            int xOffset = SxOffset(heroUnit);
                            int yOffset = SyOffset(heroUnit);
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;

                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.R) +
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.R) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.E))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.R) +
                                Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.CornflowerBlue);
            }
        }



        protected override void Killsteal()
        {


            if (Q.Ready &&
                RootMenu["ks"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }

        }



        protected override void Harass()
        {
            if (E.Ready && RootMenu["harass"]["useE"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast();
                        }
                    }
                }


            }
            if (Q.Ready && RootMenu["harass"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["harass"]["useW"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            W.Cast();
                        }
                    }
                }


            }

        }


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 3, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R", KeyCode.T, KeybindType.Press));

            }
            RootMenu.Add(ComboMenu);
            ComboMenu = new Menu("harass", "Harass");
            {
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
            }
            RootMenu.Add(ComboMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("lastq", "^- Only for Last Hit"));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "^- Min. Minion for E", 3, 1, 6));


            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));

            }
            RootMenu.Add(KillstealMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready)
            {
                Q.Cast(target);
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 625);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 450);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 400);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1000);
            R.SetSkillshot(0.00f, 250, 1400, false, SkillshotType.Circle, false, HitChance.None);

        }

        protected override void SemiR()
        {

            if (RootMenu["combo"]["semir"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                if (R.Ready && RootMenu["combo"]["useR"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                    if (target.IsValidTarget(R.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                R.Cast(target);
                            }
                        }
                    }


                }
            }

        }

        protected override void LastHit()
        {
            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {

                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
                if (Q.Ready && useQ)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {


                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {

                                if (RootMenu["farming"]["lane"]["useQ"].Enabled)
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
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
}

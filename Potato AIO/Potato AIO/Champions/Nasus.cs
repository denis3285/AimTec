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
    class Nasus : Champion
    {
        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useQAA = RootMenu["combo"]["useQAA"].Enabled;
            bool useW = RootMenu["combo"]["useW"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;
            bool useR = RootMenu["combo"]["useR"].Enabled;


            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (Q.Ready && useQA && !useQAA && target.IsValidTarget(Q.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        Q.Cast();
                    }
                }
            }
            if (useR)
            {
                if (R.Ready && Player.HealthPercent() <= RootMenu["combo"]["hpR"].As<MenuSlider>().Value)
                {
                    if (target.IsValidTarget(400) && Player.CountEnemyHeroesInRange(1000) <= RootMenu["combo"]["rRange"].As<MenuSlider>().Value)
                    {
                        R.Cast();
                    }
                }
            }
            if (W.Ready && useW && target.IsValidTarget(W.Range))
            {
                if (target != null)
                {
                    if (RootMenu["combo"]["WAA"].Enabled)
                    {
                        if (target.Distance(Player) > 300)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    if (!RootMenu["combo"]["WAA"].Enabled)
                    {

                        W.CastOnUnit(target);

                    }
                }
            }

            if (E.Ready && useE && target.IsValidTarget(E.Range))
            {
                if (target != null)
                {
                    E.Cast(target);
                }
            }


        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;
            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {

                switch (RootMenu["farming"]["lane"]["lanemode"].As<MenuList>().Value)
                {
                    case 0:

                        if (Q.Ready)
                        {
                            foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                            {
                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.Cast();

                                    }
                                }
                            }
                        }

                        break;

                    case 1:

                        if (Q.Ready)
                        {
                            foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                            {
                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    Q.Cast();
                                }
                            }
                        }
                        break;
                    case 2:

                        break;
                }

                if (useE)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                                minion.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                        {

                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                E.Cast(minion);
                            }
                        }
                    }
                }
            }
            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;
                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }
                    if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
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


        static double GetW(Obj_AI_Base target)
        {

            double meow = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
            {
                meow = 60;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 2)
            {
                meow = 110;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 3)
            {
                meow = 160;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 4)
            {
                meow = 210;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 5)
            {
                meow = 260;
            }

            double calc = ((target.MaxHealth - target.Health)/target.MaxHealth+1);
            double full = calc * meow;
            double damage = Player.CalculateDamage(target, DamageType.Physical, full);
            return damage;

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
                            var drawStartXPos = (float)(barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(unit, SpellSlot.Q)
                                                            ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(unit, SpellSlot.Q))) / unit.MaxHealth * 100 / 100)
                                                            : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(unit, SpellSlot.Q) ? Color.GreenYellow : Color.Orange);

                        });
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                var minion = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(E.Range));
                foreach (var m in minion)
                {
                    if (m.IsValidTarget() && !m.IsDead)
                    {


                        if (Q.Ready)
                        {
                            if (Player.GetSpellDamage(m, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(m, SpellSlot.Q) >= m.Health)
                            {
                                Render.Circle(m.Position, 100, 40, Color.GreenYellow);
                            }
                        }
                    }

                }
            }
        }



        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useE"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.Cast(bestTarget);
                }
            }

        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!RootMenu["combo"]["useQAA"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                Q.Cast();
            }

            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
            {
                if (!RootMenu["harass"]["useQAA"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                Q.Cast();
            }
        }

        protected override void Harass()
        {


            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useQAA = RootMenu["harass"]["useQAA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (manapercent < Player.ManaPercent())
            {
                if (Q.Ready && useQA && !useQAA && target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast();
                        }
                    }
                }
                if (W.Ready && RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (RootMenu["harass"]["WAA"].Enabled)
                        {
                            if (target.Distance(Player) > 300)
                            {
                                W.CastOnUnit(target);
                            }
                        }
                        if (!RootMenu["harass"]["WAA"].Enabled)
                        {

                            W.CastOnUnit(target);

                        }
                    }
                }
                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        E.Cast(target);
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
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo", true));
                ComboMenu.Add(new MenuBool("useQAA", "^- Only for AA Reset", false));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("WAA", "^- Only if out of AA Range"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hpR", "^- if my Health Below", 30, 10, 100));
                ComboMenu.Add(new MenuSlider("rRange", "If X Enemies Near Me", 3, 1, 5));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass", true));
                HarassMenu.Add(new MenuBool("useQAA", "^- Only for AA Reset", false));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("WAA", "^- Only if out of AA Range"));
                HarassMenu.Add(new MenuBool("useE", "Use E"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuList("lanemode", "LaneClear Mode:", new[] { "Q Last Hit Only", "Use Q Always", "Never" }, 0));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "Min. minion for E", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            var LastHitMenu = new Menu("lasthit", "Last Hit");
            {
                LastHitMenu.Add(new MenuBool("useQ", "Use Q LastHit"));

            }
            RootMenu.Add(LastHitMenu);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useE", "Use E to Killsteal"));

            }
            RootMenu.Add(KillstealMenu);

           
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Killable Minions for Q"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Lasthit))
            {

                if (e.Target.IsMinion)
                {
                    Q.Cast();
                }


            }
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                W.CastOnUnit(target);
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 300f);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 600f);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 650f);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 175f);
            E.SetSkillshot(0.8f, 300, 1000, false, SkillshotType.Circle, false, HitChance.None);


        }

        protected override void SemiR()
        {
          
        }

        protected override void LastHit()
        {
            bool useQ = RootMenu["lasthit"]["useQ"].Enabled;

            if (useQ && Q.Ready)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (minion.IsValidTarget(Q.Range) && minion != null)
                    {
                        Console.WriteLine(Player.GetSpellDamage(minion, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(minion, SpellSlot.Q));
                        if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q, DamageStage.Buff) + Player.GetSpellDamage(minion, SpellSlot.Q))
                        {
                            if (Q.Cast())
                            {
                                Orbwalker.Implementation.ForceTarget(minion);
                            }
                        }
                    }
                }
            }
        }
    }
}

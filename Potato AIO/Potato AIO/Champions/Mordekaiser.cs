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
    class Mordekaiser : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"].Enabled;


            bool useR = RootMenu["combo"]["user"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;


            if (useE)
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
            if (useQ && !RootMenu["combo"]["qaa"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(300);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(300))
                    {

                        if (target != null)
                        {
                            Q.Cast();
                        }
                    }

                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target != null)
                {
                    if (target.IsValidTarget() && !Orbwalker.Implementation.IsWindingUp)
                    {

                        if (target.IsValidTarget(W.Range))
                        {
                            if (Player.HasBuff("mordekaiserwactive") || Player.HasBuff("mordekaiserwinactive"))
                            {
                                if (target.IsValidTarget(300) && wtime < Game.TickCount)
                                {

                                    W.Cast();
                                }
                            }
                            foreach (var allies in GameObjects.AllyHeroes)
                            {
                                if (!Player.HasBuff("mordekaiserwactive") && !Player.HasBuff("mordekaiserwinactive"))
                                {
                                    if (allies != null && !allies.IsMe)
                                    {

                                        if (allies.Distance(Player) < W.Range)
                                        {
                                            if (Player.Distance(target) < allies.Distance(target))
                                            {
                                                if (target.IsValidTarget(300))
                                                {
                                                    if (W.Cast())
                                                    {
                                                        wtime = Game.TickCount + 2500;
                                                    }
                                                }
                                            }
                                            if (Player.Distance(target) > allies.Distance(target))
                                            {
                                                if (allies.Distance(target) < 300)
                                                {
                                                    if (W.CastOnUnit(allies))
                                                    {
                                                        wtime = Game.TickCount + 2500;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else if (target.IsValidTarget(300))
                                    {
                                        if (W.Cast())
                                        {
                                            wtime = Game.TickCount + 2500;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (useR)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget() && target != null)
                {

                    if (target.IsValidTarget(R.Range))
                    {


                        if (target.HealthPercent() <= RootMenu["combo"]["rhp"].As<MenuSlider>().Value)
                        {
                            if (!RootMenu["blacklist"][target.ChampionName.ToLower()].Enabled)
                            {
                                R.CastOnUnit(target);
                            }
                        }
                    }

                }
            }

        }


        protected override void Farming()
        {


            foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }

                if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                {


                    if (minion.IsValidTarget(W.Range) && !RootMenu["farming"]["lane"]["lastq"].Enabled)
                    {
                        Q.Cast();
                    }
                    if (minion.IsValidTarget(W.Range) && RootMenu["farming"]["lane"]["lastq"].Enabled)
                    {
                        if (!Player.HasBuff("MordekaiserMaceOfSpades15") && !Player.HasBuff("MordekaiserMaceOfSpades2"))

                        {
                            if (Player.GetSpellDamage(minion, SpellSlot.Q) + Player.GetAutoAttackDamage(minion) >
                                minion.Health)
                            {
                                if (Q.Cast())
                                {
                                    Player.IssueOrder(OrderType.AttackUnit, minion);
                                }
                            }
                        }

                    }
                }
                if (RootMenu["farming"]["lane"]["usee"].Enabled)
                {
                    if (minion.IsValidTarget(E.Range) && minion != null)
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                minion.ServerPosition)) >=
                            RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                        {
                            E.Cast(minion);
                        }
                    }
                }
            }


            foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                .Where(m => m.IsValidTarget(E.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }
                bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                bool useW = RootMenu["farming"]["jungle"]["usee"].Enabled;


                if (useQ)
                {
                    if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }
                }
                if (useW)
                {
                    if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
                    }
                }
            }


        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private int wtime;
        private int rdelayyyyyy;

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

            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.CornflowerBlue);
            }

            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }

            if (RootMenu["drawings"]["drawrdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
                    .ToList()
                    .ForEach(
                        unit =>
                        {

                            double Rdamage;
                            Console.WriteLine(((Player.GetSpellDamage(unit, SpellSlot.R,
                                                    DamageStage.DamagePerSecond) * 10) - (unit.HPRegenRate * 10) +
                                               (Player.GetSpellDamage(unit, SpellSlot.R))));
                            var heroUnit = unit as Obj_AI_Hero;
                            int width = 103;

                            int xOffset = SxOffset(heroUnit);
                            int yOffset = SyOffset(heroUnit);
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;
                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health >
                                                     (Player.GetSpellDamage(unit, SpellSlot.R,
                                                          DamageStage.DamagePerSecond) * 10) - (unit.HPRegenRate * 10) +
                                                     (Player.GetSpellDamage(unit, SpellSlot.R))
                                             ? width * ((unit.Health - ((Player.GetSpellDamage(unit, SpellSlot.R,
                                                                             DamageStage.DamagePerSecond) * 10) - (unit.HPRegenRate * 10) +
                                                                        (Player.GetSpellDamage(unit, SpellSlot.R)))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < ((Player.GetSpellDamage(unit, SpellSlot.R,
                                                   DamageStage.DamagePerSecond) * 10) - (unit.HPRegenRate * 10) +
                                (Player.GetSpellDamage(unit, SpellSlot.R)))
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }



        protected override void Killsteal()
        {


        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }

            }


            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
            {

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["harass"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }

            }


        }

        protected override void Harass()
        {



            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useW = RootMenu["harass"]["usew"].Enabled;
            bool useE = RootMenu["harass"]["usee"].Enabled;


            if (useE)
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
            if (useQ && !RootMenu["harass"]["qaa"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(300);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(300))
                    {

                        if (target != null)
                        {
                            Q.Cast();
                        }
                    }

                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target != null)
                {
                    if (target.IsValidTarget() && !Orbwalker.Implementation.IsWindingUp)
                    {

                        if (target.IsValidTarget(W.Range))
                        {
                            if (Player.HasBuff("mordekaiserwactive") || Player.HasBuff("mordekaiserwinactive"))
                            {
                                if (target.IsValidTarget(300) && wtime < Game.TickCount)
                                {

                                    W.Cast();
                                }
                            }
                            foreach (var allies in GameObjects.AllyHeroes)
                            {
                                if (!Player.HasBuff("mordekaiserwactive") && !Player.HasBuff("mordekaiserwinactive"))
                                {
                                    if (allies != null && !allies.IsMe)
                                    {

                                        if (allies.Distance(Player) < W.Range)
                                        {
                                            if (Player.Distance(target) < allies.Distance(target))
                                            {
                                                if (target.IsValidTarget(300))
                                                {
                                                    if (W.Cast())
                                                    {
                                                        wtime = Game.TickCount + 2500;
                                                    }
                                                }
                                            }
                                            if (Player.Distance(target) > allies.Distance(target))
                                            {
                                                if (allies.Distance(target) < 300)
                                                {
                                                    if (W.CastOnUnit(allies))
                                                    {
                                                        wtime = Game.TickCount + 2500;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else if (target.IsValidTarget(300))
                                    {
                                        if (W.Cast())
                                        {
                                            wtime = Game.TickCount + 2500;
                                        }

                                    }
                                }
                            }
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("rhp", "^- If Enemy has X Health", 25, 1, 100));
                ComboMenu.Add(new MenuBool("autor", "Auto R if Can Kill"));
            }
            RootMenu.Add(ComboMenu);
            var BlackList = new Menu("blacklist", "R Blacklist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                }
            }
            RootMenu.Add(BlackList);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {

                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("lastq", "^- Only to Last Hit"));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if Hits X", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {

                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);

            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawrdamage", "Draw R Damage"));
            }

            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 300);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 1000);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 675);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 650);

            E.SetSkillshot(0.25f, 12f * 2 * (float) Math.PI / 180, 2000f, false, SkillshotType.Cone);
        }

        protected override void SemiR()
        {



            if (Player.HasBuff("mordekaisercotgself"))
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(2000);

                if (target.IsValidTarget() && target != null && !target.IsDead)
                {

                    if (target.IsValidTarget(2000) && rdelayyyyyy < Game.TickCount)
                    {
                        if (R.Cast(target.ServerPosition))
                        {
                            rdelayyyyyy = 1000 + Game.TickCount;
                        }
                    }
                }
            }
            if (Player.HasBuff("mordekaisercotgself"))
            {
                if (Player.CountEnemyHeroesInRange(2000) == 0)
                {
                    if ( rdelayyyyyy < Game.TickCount)
                    {
                        if (R.Cast(Player.ServerPosition))
                        {
                            rdelayyyyyy = 1000 + Game.TickCount;
                        }
                    }
                }
            }
            if (RootMenu["combo"]["autor"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(R, DamageType.Magical, false);


                if (bestTarget.IsValidTarget())
                {
                    if ((Player.GetSpellDamage(bestTarget, SpellSlot.R, DamageStage.DamagePerSecond) * 10) -
                        (bestTarget.HPRegenRate * 10) + (Player.GetSpellDamage(bestTarget, SpellSlot.R)) >
                        bestTarget.Health)
                    {
                        R.CastOnUnit(bestTarget);
                    }
                }
            }
        }


        protected override void LastHit()
        {

        }
    }
}

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
    class Swain : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
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
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range))
                    {

                        if (target != null)
                        {
                            if (Q.Cast(target))
                            {
                                rdelay = 500 + Game.TickCount;
                            }
                        }
                    }

                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(W.Range))
                    {

                        if (target != null)
                        {
                            if (!RootMenu["combo"]["wq"].Enabled)
                            {
                                W.Cast(target);
                            }
                            if (rdelay < Game.TickCount)
                            {
                                if (RootMenu["combo"]["wq"].Enabled && target.HasBuff("swainqslow") || !Q.Ready)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }

                }
            }
            if (RootMenu["combo"]["user"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(R.Range) && RootMenu["combo"]["hitr"].As<MenuSlider>().Value <=
                        Player.CountEnemyHeroesInRange(620))
                    {

                        if (target != null)
                        {

                            if (!Player.HasBuff("SwainMetamorphism"))
                            {
                                R.Cast();
                            }
                        }
                    }
                    if (target != null)
                    {
                        if (Player.GetSpellDamage(target, SpellSlot.Q) +
                            Player.GetSpellDamage(target, SpellSlot.E) +
                            Player.GetSpellDamage(target, SpellSlot.W) +
                            Player.GetSpellDamage(target, SpellSlot.R) >= target.Health)
                        {
                            if (!Player.HasBuff("SwainMetamorphism"))
                            {
                                R.Cast();
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
                foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                {
                    if (!minion.IsValidTarget())
                    {
                        return;
                    }

                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(325, false, false,
                                    minion.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hitq"].As<MenuSlider>().Value)
                            {
                                Q.Cast(minion);
                            }

                        }

                    }
                    if (RootMenu["farming"]["lane"]["usew"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                                    minion.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                            {
                                W.Cast(minion);
                            }

                        }

                    }
                    if (RootMenu["farming"]["lane"]["user"].Enabled)
                    {
                        if (minion.IsValidTarget(R.Range) && minion != null)
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(620, false, false,
                                    Player.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hitr"].As<MenuSlider>().Value)
                            {
                                if (!Player.HasBuff("SwainMetamorphism"))
                                {
                                    R.Cast();
                                }
                            }
                        }
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(W.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;
                    bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;
                    bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;
                    bool useR = RootMenu["farming"]["jungle"]["user"].Enabled;


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
                            W.Cast(jungleTarget);
                        }
                    }
                    if (useE)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
                        }
                    }
                    if (useR)
                    {

                        if (jungleTarget != null && jungleTarget.IsValidTarget(R.Range))
                        {
                            if (!Player.HasBuff("SwainMetamorphism"))
                            {
                                R.Cast();
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

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range * 2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {


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
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) +
                                                          Player.GetSpellDamage(unit, SpellSlot.R)

                                                         )) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
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
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.Cast(bestTarget);
                }
            }
            if (W.Ready &&
                RootMenu["ks"]["ksw"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {
                    W.Cast(bestTarget);
                }
            }


        }





        protected override void Harass()
        {

            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
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
                if (useQ)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(Q.Range))
                        {

                            if (target != null)
                            {
                                if (Q.Cast(target))
                                {
                                    rdelay = 300 + Game.TickCount;
                                }
                            }
                        }

                    }
                }
                if (useW)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(W.Range))
                        {

                            if (target != null && rdelay < Game.TickCount)
                            {
                                if (!RootMenu["harass"]["wq"].Enabled)
                                {
                                    W.Cast(target);
                                }
                                if (rdelay < Game.TickCount)
                                {
                                    if (RootMenu["harass"]["wq"].Enabled && target.HasBuff("swainqslow") || !Q.Ready)
                                    {
                                        W.Cast(target);
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
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("wq", " ^- Wait for Q Slow"));
                ComboMenu.Add(new MenuBool("autow", " ^- Auto W on CC"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- If Hits X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuSlider("hitr", " ^- OR My Health Lower than", 50, 1, 100));
                ComboMenu.Add(new MenuBool("killr", "Force R if Killable with Combo"));
                ComboMenu.Add(new MenuBool("autor", "Auto R Turn-Off"));
                ComboMenu.Add(new MenuSlider("rmana", "Auto R Turn-Off if Below X Mana", 10, 0, 100));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                HarassMenu.Add(new MenuBool("wq", " ^- Wait for Q Slow"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hitq", "^- if Hits X", 3, 0, 6));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitw", "^- if Hits X", 3, 0, 6));

                LaneClear.Add(new MenuBool("user", "Use R to Farm"));
                LaneClear.Add(new MenuSlider("hitr", "^- if X Minions in Range", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
                JungleClear.Add(new MenuBool("user", "Use R to Farm"));

            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("kse", "Killseal with E"));
                KillstealMenu.Add(new MenuBool("ksw", "Killseal with W"));
            }
            RootMenu.Add(KillstealMenu);


            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));

                DrawMenu.Add(new MenuBool("drawr", "Draw R Range", false));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));

            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                W.Cast(Args.EndPosition);
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 700);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 900);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 625);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 620);
            W.SetSkillshot(1.2f, 125f, float.MaxValue, false, SkillshotType.Circle);
            Q.SetSkillshot(0.5f, 50, 2000, false, SkillshotType.Circle, false, HitChance.None);
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["autow"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(W.Range)))
                {

                    W.Cast(target);
                }

            }
            if (RootMenu["combo"]["autor"].Enabled)
            {
                if (Player.HasBuff("SwainMetamorphism"))
                {
                    if (Potato_AIO.Bases.Extensions.GetGenericJungleMinionsTargetsInRange(700).Count == 0 &&
                        Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(700).Count == 0 &&
                        Player.CountEnemyHeroesInRange(700) == 0 || Player.ManaPercent() <=
                        RootMenu["combo"]["rmana"].As<MenuSlider>().Value)
                    {
                        R.Cast();
                    }
                }

            }

        }

        protected override void LastHit()
        {


        }
    }
}

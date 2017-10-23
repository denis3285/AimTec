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
using Aimtec.SDK.Prediction.Collision;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;
using Potato_AIO.WDodge;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Sejuani : Champion
    {
        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);



            if (Q.Ready && useQA && target.IsValidTarget(Q.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        Q.Cast(target);
                    }
                }
            }
            if (RootMenu["combo"]["useW"].Enabled && target.IsValidTarget(W.Range))
            {
                if (target != null)
                {

                    W.Cast(target);

                }

            }

            if (E.Ready && useE && target.IsValidTarget(E.Range))
            {
                if (target != null)
                {
                    E.Cast(target);
                }
            }

            if (R.Ready && RootMenu["combo"]["useR"].Enabled)
            {
                var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (targets.IsValidTarget(R.Range))
                {
                    if (targets != null)
                    {
                        if (!targets.IsDead)

                        {
                            var meow = R.GetPrediction(targets);
                            if (meow.CastPosition.CountEnemyHeroesInRange(500) >=
                                RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                            {
                                R.Cast(meow.CastPosition);
                            }
                        }
                    }
                }


            }


        }




        protected override void Farming()
        {

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;

                bool useE = RootMenu["farming"]["lane"]["useW"].Enabled;


                if (useQ)
                {
                    if (Q.Ready)
                    {

                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {


                                Q.Cast(minion);

                            }
                        }


                    }
                }
                if (useE)
                {
                    if (W.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {
                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {
                                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                                        minion.ServerPosition)) >= RootMenu["farming"]["lane"]["ehit"]
                                        .As<MenuSlider>().Value)
                                {

                                    W.Cast(minion);
                                }
                            }
                        }


                    }
                }
               
            }

            foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(Q.Range))
                .ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;

                            if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready &&
                                jungleTarget.IsValidTarget(W.Range))
                            {
                                W.Cast(jungleTarget);
                            }
                            if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast(jungleTarget);
                            }
                            if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready &&
                                jungleTarget.IsValidTarget(E.Range))
                            {
                                E.Cast(jungleTarget);
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
                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {

                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                    if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready &&
                        jungleTarget.IsValidTarget(W.Range))
                    {
                        W.Cast(jungleTarget);
                    }
                    if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready &&
                        jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
                    }


                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimes;

        public int Hmmmmm { get; private set; }

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
                            double Rdamage = 0;
                            if (R.Ready)
                            {
                                Rdamage = Player.GetSpellDamage(unit, SpellSlot.R);
                            }
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
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) + Rdamage +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) + Rdamage +
                                                          Player.GetSpellDamage(unit, SpellSlot.E))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) + Rdamage +
                                Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

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
                Render.Circle(Player.Position, R.Range, 50, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.LightGreen);
            }
        }


        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useQ"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
            if (RootMenu["killsteal"]["useW"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {
                   W.Cast(bestTarget);
                }
            }

        }



        protected override void Harass()
        {


            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (manapercent < Player.ManaPercent())
            {

                if (Q.Ready && useQA && target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast(target);
                        }
                    }
                }
                if (RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {

                        W.Cast(target);

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

        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "SejuaniW")
                {
                    Hmmmmm = 900 + Game.TickCount;

                    Orbwalker.Implementation.AttackingEnabled = false;
                   
                }
            }
        }

        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("ehit", "^- If Hits X Minions", 3, 1, 6));
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
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("useW", "Use W to Killsteal"));
            }
            RootMenu.Add(KillstealMenu);

           
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }



        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 650);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 600);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 750);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1350);
            Q.SetSkillshot(0.25f, 100f, 1800, false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(0.25f, 110f, 1600f, false, SkillshotType.Line);
       


        }

        protected override void SemiR()
        {
            if (Hmmmmm < Game.TickCount)
            {
                Orbwalker.Implementation.AttackingEnabled = true;
            }
            if (RootMenu["combo"]["semir"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                if (R.Ready)
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

        }
    }
}

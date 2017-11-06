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

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Galio : Champion
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

                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        var meow = E.GetPrediction(target);
                        if (!Extensions.AnyWallInBetween(Player.ServerPosition, meow.CastPosition))
                        {
                            E.Cast(target);
                        }
                    }
                }
                if (RootMenu["combo"]["useW"].Enabled && target.IsValidTarget(W.Range - 80))
                {
                    if (target != null)
                    {
                        if (!Orbwalker.Implementation.IsWindingUp)
                        {
                            if (!W.IsCharging)
                            {
                                if (W.StartCharging(Game.CursorPos))
                                {
                                    meowmeowtimes = RootMenu["combo"]["timew"].As<MenuSlider>().Value + Game.TickCount;
                                }
                            }
                            if (meowmeowtimes < Game.TickCount)
                            {
                                if (W.IsCharging)
                                {

                                    W.ShootChargedSpell(Game.CursorPos, true);
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

                if (useQ)
                {

                    if (Q.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(220, false, false,
                                        minion.ServerPosition)) >= RootMenu["farming"]["lane"]["qhit"]
                                        .As<MenuSlider>().Value)
                                {

                                    Q.Cast(minion);
                                }
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
                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                   
                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimes;

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
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.Q,
                                                         DamageStage.DamagePerSecond) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.Q,
                                                              DamageStage.DamagePerSecond) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.DamagePerSecond) +
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
                Render.Circle(Player.Position, 650, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                DrawCircleOnMinimap(Player.Position, R.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, 800, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["flashw"].Enabled)
            {
                Render.Circle(Player.Position, W.Range + 340, 50, Color.LightGreen);
            }
        }
        public static void DrawCircleOnMinimap(
            Vector3 center,
            float radius,
            Color color,
            int thickness = 1,
            int quality = 100)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float)Math.Sin(angle))
                );
            }
            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                Vector2 aonScreen;
                Vector2 bonScreen;

                Render.WorldToMinimap(a, out aonScreen);
                Render.WorldToMinimap(b, out bonScreen);

                Render.Line(aonScreen, bonScreen, color);
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

                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        var meow = E.GetPrediction(target);
                        if (!Extensions.AnyWallInBetween(Player.ServerPosition, meow.CastPosition))
                        {
                            E.Cast(target);
                        }
                    }
                }
                if (RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range-80))
                {
                    if (target != null)
                    {
                        if (!Orbwalker.Implementation.IsWindingUp)
                        {
                            if (!W.IsCharging)
                            {
                                if (W.StartCharging(Game.CursorPos))
                                {
                                    meowmeowtimes = RootMenu["harass"]["timew"].As<MenuSlider>().Value + Game.TickCount;
                                }
                            }
                            if (meowmeowtimes < Game.TickCount)
                            {
                                if (W.IsCharging)
                                {

                                    W.ShootChargedSpell(Game.CursorPos, true);
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
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo", true));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuSlider("timew", "^- Release after X ms.", 1000, 0, 2500));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass", true));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuSlider("timew", "^- Release after X ms.", 1000, 0, 2500));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("qhit", "^- If Hits X Minions", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Q to Killsteal"));

            }
            RootMenu.Add(KillstealMenu);

           
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("flashw", "Draw Flash > W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Minimap"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Add(new MenuKeyBind("flashw", "Flash > W", KeyCode.T, KeybindType.Press));
            RootMenu.Add(new MenuSlider("timeflash", "^-  Flash After X ms.", 1000, 0, 2500));
            RootMenu.Attach();

        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < 200 && W.Ready)
            {
                if (!W.IsCharging)
                {
                    W.StartCharging(Game.CursorPos);

                }

                if (W.IsCharging)
                {

                    W.ShootChargedSpell(Game.CursorPos, true);
                }

           
               
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 760);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 450);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 550);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 4000f);
            Q.SetSkillshot(0.5f, 180, 1400f, false, SkillshotType.Circle, false, HitChance.None);
            E.SetSkillshot(0.5f, 70, 1400, false, SkillshotType.Line, false, HitChance.None);
            W.SetCharged("GalioW", "GalioW", 420, 450, 0.2f);

            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);

        }

        protected override void SemiR()
        {
            if (RootMenu["flashw"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range + 340);
                if (W.Ready)
                {
                    if (Flash.Ready && Flash != null && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(W.Range + 340))
                        {
                            if (!W.IsCharging)
                            {
                                if (W.StartCharging(Game.CursorPos))
                                {
                                    meowmeowtimes = RootMenu["timeflash"].As<MenuSlider>().Value + Game.TickCount;
                                }
                            }
                            if (meowmeowtimes < Game.TickCount)
                            {
                                if (W.IsCharging)
                                {



                                    if (Flash.Cast(target.ServerPosition))
                                    {
                                        W.ShootChargedSpell(Game.CursorPos, true);

                                    }
                                }
                            }




                        }
                    }
                }
            }

            if (Player.HasBuff("GalioW"))
            {
                Orbwalker.Implementation.AttackingEnabled = false;
            }
            else Orbwalker.Implementation.AttackingEnabled = true;
            if (Player.GetSpell(SpellSlot.R).Level == 1)
            {
                R.Range = 4000f;
            }
            if (Player.GetSpell(SpellSlot.R).Level == 2)
            {
                R.Range = 4750;
            }
            if (Player.GetSpell(SpellSlot.R).Level ==3)
            {
                R.Range = 5500;
            }
        }

        protected override void LastHit()
        { 
        }
    }
}

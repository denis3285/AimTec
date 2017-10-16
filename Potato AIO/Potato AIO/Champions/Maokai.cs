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
    class Maokai : Champion
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
            if (RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range))
            {
                if (target != null)
                {

                    W.Cast(target);

                }

            }

            if (E.Ready && useE)
            {
                var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (targets != null && targets.IsValidTarget(E.Range))
                {
                    E.Cast(targets);
                }
            }



        }




        protected override void Farming()
        {

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;

                bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;


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
                            if (E.Ready)
                            {
                                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                                {
                                    if (minion.IsValidTarget(Q.Range) && minion != null)
                                    {
                                        

                                            E.Cast(minion);
                                        
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
                 
                            if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready && jungleTarget.IsValidTarget(W.Range))
                            {
                                W.Cast(jungleTarget);
                            }
                            if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast(jungleTarget);
                            }
                            if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready && jungleTarget.IsValidTarget(E.Range))
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
                            if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready && jungleTarget.IsValidTarget(W.Range))
                            {
                                W.Cast(jungleTarget);
                            }
                            if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready && jungleTarget.IsValidTarget(E.Range))
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
        private Vector3 position;
        private int zzzzzzzzzz;

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
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
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
                DrawCircleOnMinimap(Player.Position, R.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position,600, 50, Color.LightGreen);
            }
            Render.Circle(position, 150, 40, Color.Wheat);
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
                if (RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {

                        W.Cast(target);

                    }

                }

                if (E.Ready && useE)
                {
                    var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (targets != null && targets.IsValidTarget(E.Range))
                    {
                        E.Cast(targets);
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
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass", true));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
               
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

            }
            RootMenu.Add(KillstealMenu);     
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Minimap"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            var zzzzzz = new Menu("wset", "W Settings");
            WDodge.EvadeManager.Attach(zzzzzz);
            WDodge.EvadeOthers.Attach(zzzzzz);
            WDodge.EvadeTargetManager.Attach(zzzzzz);
            RootMenu.Add(zzzzzz);
            RootMenu.Add(DrawMenu);
            RootMenu.Add(new MenuKeyBind("insec", "Insec with W > Q", KeyCode.T, KeybindType.Press));
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                W.Cast(target);


            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 650);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 525);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1100);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 3000);
            Q.SetSkillshot(0.4f, 100, 1800f, false, SkillshotType.Line);
            E.SetSkillshot(1.2f, 50, 1400f, false, SkillshotType.Circle, false, HitChance.None);

        }

        protected override void SemiR()
        {

            if (RootMenu["insec"].Enabled)
            {
                if (position == Vector3.Zero)
                {
                   
                    Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                }
                var target = TargetSelector.Implementation.GetTarget(W.Range);
                if (target != null && target.IsValidTarget(W.Range))
                {
                    if (Q.Ready && Player.Mana > Player.GetSpell(SpellSlot.Q).Cost + Player.GetSpell(SpellSlot.W).Cost)
                    {
                        W.Cast(target);
                       
                        if (!position.IsZero)
                        {
                            Player.IssueOrder(OrderType.MoveTo,
                                position);
                        }
                        if (Player.Distance(position) < 150)
                        {
                            Q.Cast(target);
                        }
                        if (position.Distance(target) < 150)
                        {
                            position = target.ServerPosition.Extend(Player.ServerPosition, -200);
                        }
                       

                    }
                }
            }
            
            if (!position.IsZero && zzzzzzzzzz+100 < Game.TickCount)
            {
                zzzzzzzzzz = 2000 + Game.TickCount;
            }
            if (zzzzzzzzzz < Game.TickCount)
            {
                position = new Vector3(0, 0, 0);
            }
        }
        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "MaokaiQ")
                {

                    position = new Vector3(0, 0, 0);
                }
                if (args.SpellData.Name == "MaokaiW")
                {
                    if (RootMenu["insec"].Enabled)
                    { var target = TargetSelector.Implementation.GetTarget(W.Range);
                        if (target != null && target.IsValidTarget(W.Range))
                        {
                           
                            position = target.ServerPosition.Extend(Player.ServerPosition, -300-(target.MoveSpeed/10));
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

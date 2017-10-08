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
    class Garen : Champion
    {
        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }
           
    
            var ally = args.Target as Obj_AI_Hero;
            var target = args.Sender as Obj_AI_Hero;
            if (ally != null && target != null)
            {

                if (RootMenu["combo"]["WAA"].Enabled&& RootMenu["combo"]["useW"].Enabled && args.SpellData.Name.Contains("BasicAttack"))
                {
                    if (ally.IsHero && ally.IsMe && target.IsHero)
                    {
                        W.Cast();
                    }
                }
            }
        }
        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useQAA = RootMenu["combo"]["useQAA"].Enabled;
            bool useW = RootMenu["combo"]["useW"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;
            bool useR = RootMenu["combo"]["useR"].Enabled;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
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
            if (E.Ready && useE && target.IsValidTarget(E.Range) && !Player.HasBuff("GarenE") && !Player.HasBuff("GarenQ"))
            {
                if (target != null)
                {
                    E.Cast(target);
                }
            }

            if (W.Ready && useW && target.IsValidTarget(E.Range))
            {
                if (target != null)
                {
                    if (!RootMenu["combo"]["WAA"].Enabled)
                    {

                        W.Cast();

                    }
                }
            }
     



        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;
           


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



            if (useE)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(E.Range, false, false,
                            Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                    {

                        if (minion.IsValidTarget(E.Range) && minion != null && !Player.HasBuff("GarenE"))
                        {
                            E.Cast();
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

                if (useQs && Q.Ready && jungleTarget.IsValidTarget(300))
                {
                    Q.Cast();
                }
                if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range) && !Player.HasBuff("GarenE"))
                {
                    E.Cast();
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

        static double GetR(Obj_AI_Base target)
        {

            double meow = 0;
            double meoww = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 175;
                meoww = 0.278;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 350;
                meoww = 0.333;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 525;
                meoww = 0.4;
            }
            double calc = target.MaxHealth - target.Health;
            double mmmmm = calc * (meoww);
            double full = meow + mmmmm;
            double damage = Player.CalculateDamage(target, DamageType.Magical, full);
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
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
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
                Render.Circle(Player.Position, 600, 50, Color.CornflowerBlue);
            }
        }



        protected override void Killsteal()
        {

   
                var bestTarget = Bases.Extensions.GetBestKillableHero(R, DamageType.Physical, false);
           
                if (bestTarget != null &&
                   GetR(bestTarget) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    R.Cast(bestTarget);
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
                ComboMenu.Add(new MenuBool("WAA", "^- Only on Enemy Auto Attacks"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Auto R if Killable"));
            }
            RootMenu.Add(ComboMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "Min. minion for E", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);              
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Engage Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw R Damage"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 600f);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 325);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 400);

        }

        protected override void SemiR()
        {
            if (Player.HasBuff("GarenE"))
            {
                Orbwalker.Implementation.AttackingEnabled = false;
            }
            else 
                Orbwalker.Implementation.AttackingEnabled = true;
            
        
        }

        protected override void LastHit()
        {

        }
    }
}

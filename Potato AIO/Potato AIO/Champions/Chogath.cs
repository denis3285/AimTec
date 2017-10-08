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
    class Chogath : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;

            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range))
                    {

                        if (target != null)
                        {
                            Q.Cast(target);
                        }
                    }

                }
            }
            if (useE && !RootMenu["combo"]["eaa"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range))
                    {

                        if (target != null)
                        {
                            E.Cast();
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
                            W.Cast(target);

                        }
                    }

                }
            }
        }

        

        protected override void Farming()
        {

            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (!minion.IsValidTarget())
                    {
                        return;
                    }

                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(220, false, false,
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
                    if (RootMenu["farming"]["lane"]["usee"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(E.Range))
                        {

                            E.Cast();


                        }

                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(Q.Range)).ToList())
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
                            W.Cast(jungleTarget);
                        }
                    }
                    if (useE)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast();
                        }
                    }
                    
                }
            }

        }

        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };
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
                                (float)(barPos.X + (unit.Health >
                                                     
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health -
                                                         (
                                                          Player.GetSpellDamage(unit, SpellSlot.R)

                                                         )) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < 
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }



        static double GetR(Obj_AI_Base target)
        {

            double meow = 0;
            double meoww = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 300;
                meoww = 80;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 475;
                meoww = 120;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 650;
                meoww = 160;
            }
           
            double mmmmm = Player.TotalAbilityDamage * 0.5;
            double full = (meoww * Player.GetRealBuffCount("Feast")) * 0.10;
            double uhh = mmmmm + full + meow;
          
            return uhh;

        }
        static double GetRjungle(Obj_AI_Base target)
        {

            double meow = 1000;
            double meoww = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
              
                meoww = 80;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
            
                meoww = 120;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                
                meoww = 160;
            }

            double mmmmm = Player.TotalAbilityDamage * 0.5;
            double full = (meoww * Player.GetRealBuffCount("Feast")) * 0.10;
            double uhh = mmmmm + full + meow;

            return uhh;

        }
        protected override void Killsteal()
        {
            if (R.Ready &&
                RootMenu["combo"]["user"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(R, DamageType.Magical, false);
               
                if (bestTarget != null &&
                   GetR(bestTarget) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    R.Cast(bestTarget);
                }
            }
            if (E.Ready &&
                RootMenu["ks"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                   Q.Cast(bestTarget);
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
                if (useQ)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(Q.Range))
                        {

                            if (target != null)
                            {
                                Q.Cast(target);
                            }
                        }

                    }
                }
                if (useE && !RootMenu["harass"]["eaa"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range))
                        {

                            if (target != null)
                            {
                                E.Cast();
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
                                W.Cast(target);
                            
                            }
                        }

                    }
                }



            }
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
                if (RootMenu["combo"]["eaa"].Enabled)
                {
                    if (E.Ready)
                    {
                        E.Cast();

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
                if (RootMenu["harass"]["eaa"].Enabled)
                {
                    if (E.Ready)
                    {
                        E.Cast();

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
                ComboMenu.Add(new MenuBool("autoq", " ^- Auto Q on CC"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("eaa", " ^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("user", "Use Auto R if Killable"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("autoq", " ^- Auto Q on CC"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("eaa", " ^- Only for AA Reset"));
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
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
                JungleClear.Add(new MenuBool("autor", "Auto Steal Q Baron / Dragon"));

            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killseal with Q"));
                KillstealMenu.Add(new MenuBool("ksw", "Killseal with W"));
            }
            RootMenu.Add(KillstealMenu);


            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range", false));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw R Damage"));

            }
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready)
            {
                Q.Cast(Args.EndPosition);
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 950);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 650);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 280);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 400);
            Q.SetSkillshot(1.6f, 250f, int.MaxValue, false, SkillshotType.Circle, false, HitChance.None);
            W.SetSkillshot(0.25f, 175, 1750, false, SkillshotType.Cone, false, HitChance.None);
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["autoq"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(Q.Range)))
                {

                    Q.Cast(target);
                }

            }
            if (RootMenu["farming"]["jungle"]["autor"].Enabled)
            {
                foreach (var monsters in Bases.GameObjects.Jungle)
                {
                    if (monsters.Name.Contains("Dragon") || monsters.Name.Contains("Baron") || monsters.Name.Contains("Herald"))
                    {
                        if (monsters.IsValidTarget() && monsters.Distance(Player) < 400)
                        {
                                if (monsters.Health < GetRjungle(monsters))
                                {
                                    R.Cast(monsters);
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

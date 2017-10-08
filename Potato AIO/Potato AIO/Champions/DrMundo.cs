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
    class DrMundo : Champion
    {


        protected override void Combo()
        {

            if (Q.Ready && RootMenu["combo"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && Player.HealthPercent() >= RootMenu["combo"]["qhp"].As<MenuSlider>().Value)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["combo"]["useW"].Enabled && !Player.HasBuff("BurningAgony"))
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && Player.HealthPercent() >= RootMenu["combo"]["qhp"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                    }
                }


            }
            if (E.Ready && RootMenu["combo"]["useE"].Enabled && !RootMenu["combo"]["eaa"].Enabled)
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
            if (R.Ready && RootMenu["combo"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && Player.HealthPercent() <= RootMenu["combo"]["rhp"].As<MenuSlider>().Value)
                        {
                            R.Cast();
                        }
                    }
                }


            }



        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

            bool useW = RootMenu["farming"]["lane"]["useW"].Enabled;


            if (Q.Ready)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (minion.IsValidTarget(Q.Range) && minion != null)
                    {
                        if (!RootMenu["farming"]["lane"]["qlast"].Enabled)
                        {
                            Q.Cast(minion);
                        }
                        if (RootMenu["farming"]["lane"]["qlast"].Enabled)
                        {
                            if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                            {
                                Q.Cast(minion);
                            }
                        }

                    }
                }
            }


            if (useW)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range, false, false,
                            Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitW"].As<MenuSlider>().Value)
                    {

                        if (minion.IsValidTarget(W.Range) && minion != null && !Player.HasBuff("BurningAgony"))
                        {
                            W.Cast();
                        }
                    }
                }
            }

            if (useE)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    

                        if (minion.IsValidTarget(E.Range) && minion != null && !Player.HasBuff("GarenE"))
                        {
                            E.Cast();
                        }
                    
                }
            }

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
                if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) && !Player.HasBuff("BurningAgony"))
                {
                    W.Cast();
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
          
    
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.CornflowerBlue);
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
                if (!RootMenu["combo"]["eaa"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (E.Cast())
                {
                    Orbwalker.Implementation.ResetAutoAttackTimer();
                }
            }

            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
            {
                if (!RootMenu["harass"]["eaa"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (E.Cast())
                {
                    Orbwalker.Implementation.ResetAutoAttackTimer();
                }
            }
        }

        protected override void Harass()
        {

            if (Q.Ready && RootMenu["harass"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && Player.HealthPercent() >= RootMenu["harass"]["qhp"].As<MenuSlider>().Value)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["harass"]["useW"].Enabled && !Player.HasBuff("BurningAgony"))
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && Player.HealthPercent() >= RootMenu["harass"]["qhp"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                    }
                }


            }
            if (E.Ready && RootMenu["harass"]["useE"].Enabled && !RootMenu["harass"]["eaa"].Enabled)
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

        }


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuSlider("qhp", "^- Don't if HP Lower Than", 10, 0, 40));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuSlider("whp", "^- Don't if HP Lower Than", 10, 0, 40));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("eaa", "^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("rhp", "^- Use R if X Health", 50, 0, 100));
                ComboMenu.Add(new MenuBool("smartw", "Enable Smart W Turn Off"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useQ", "Use Q in Harass"));
                HarassMenu.Add(new MenuSlider("qhp", "^- Don't if HP Lower Than", 10, 0, 40));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuSlider("whp", "^- Don't if HP Lower Than", 10, 0, 40));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("eaa", "^- Only for AA Reset"));

            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qlast", "^- Only for Last Hit"));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitW", "Min. minion for W", 3, 1, 6));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
               
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
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
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 975);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 320);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 400);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 60, 2000f, true, SkillshotType.Line);

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["smartw"].Enabled)
            {
                if (Player.HasBuff("BurningAgony"))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                            Player.ServerPosition)) == 0 && Player.CountEnemyHeroesInRange(300) == 0 &&
                        Bases.GameObjects.Jungle.Count(h => h.IsValidTarget(00, false, false,
                            Player.ServerPosition)) == 0 || Player.HealthPercent() < RootMenu["combo"]["whp"].As<MenuSlider>().Value)
                    {
                        W.Cast();
                    }



                }
            }


        }

        protected override void LastHit()
        {

        }
    }
}

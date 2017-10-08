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
    class Amumu : Champion
    {
        protected override void Combo()
        {


            if (Q.Ready && RootMenu["combo"]["useq"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && target.Distance(Player) >= RootMenu["combo"]["qmin"].As<MenuSlider>().Value)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["combo"]["usew"].Enabled && !Player.HasBuff("AuraofDespair"))
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
            if (E.Ready && RootMenu["combo"]["usee"].Enabled)
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
            if (R.Ready && RootMenu["combo"]["user"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (Player.CountEnemyHeroesInRange(R.Range) >=
                                RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
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

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

            bool useW = RootMenu["farming"]["lane"]["useW"].Enabled;
            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;

            if (manapercent < Player.ManaPercent())
            {
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
                if (useW)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range, false, false,
                                Player.ServerPosition))>=RootMenu["farming"]["lane"]["hitW"].As<MenuSlider>().Value)
                        {

                       
                            if (W.Ready && minion.IsValidTarget(W.Range) && !Player.HasBuff("AuraofDespair"))
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

                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(E.Range, false, false,
                                    Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                            {


                                if (E.Ready && minion.IsValidTarget(E.Range))
                                {
                                    E.Cast();
                                }
                            }
                        }

                    }
                }
            }
            float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

            if (manapercents < Player.ManaPercent())
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
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) && !Player.HasBuff("AuraofDespair"))
                    {
                        W.Cast();
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
        protected override void Drawings()
        {

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
                Render.Circle(Player.Position, Q.Range, 50, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.CornflowerBlue);
            }
        }

        protected override void Harass()
        {
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;


            if (manapercent < Player.ManaPercent())
            {
                if (Q.Ready && RootMenu["harass"]["useq"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                    if (target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead && target.Distance(Player) >= RootMenu["harass"]["qmin"].As<MenuSlider>().Value)
                            {
                                Q.Cast(target);
                            }
                        }
                    }


                }
                if (W.Ready && RootMenu["harass"]["usew"].Enabled && !Player.HasBuff("AuraofDespair"))
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
                if (E.Ready && RootMenu["harass"]["usee"].Enabled)
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
        }


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuSlider("qmin", "^- Min. Q Range", 300, 0, 500));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- If Hits X Enemies", 2, 0, 5));
                ComboMenu.Add(new MenuBool("smartw", "Enable Smart W Turn Off"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuSlider("qmin", "^- Min. Q Range", 300, 0, 500));
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitW", "Min. minion for W", 3, 1, 6));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "Min. minion for E", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
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
                DrawMenu.Add(new MenuBool("draww", "Draw W Range", false));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
              
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1100);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 300);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 350);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 550);
            Q.SetSkillshot(0.25f, 85, 2000, true, SkillshotType.Line);

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["smartw"].Enabled)
            {
                if (Player.HasBuff("AuraofDespair"))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range + 50, false, false,
                            Player.ServerPosition)) == 0 && Player.CountEnemyHeroesInRange(W.Range + 50) == 0 &&
                        Bases.GameObjects.Jungle.Count(h => h.IsValidTarget(W.Range + 50, false, false,
                            Player.ServerPosition)) == 0)
                    {
                        W.Cast();
                    }



                }
            }
        }

        protected override void LastHit()
        {

        }

        protected override void Killsteal()
        {
        }
    }
}

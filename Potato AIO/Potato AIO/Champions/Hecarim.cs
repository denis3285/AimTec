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
    class Hecarim : Champion
    {


        protected override void Combo()
        {
            if (E.Ready && RootMenu["combo"]["useE"].Enabled)
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
            if (Q.Ready && RootMenu["combo"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["combo"]["useW"].Enabled)
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
       
            if (R.Ready && RootMenu["combo"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && target.CountEnemyHeroesInRange(300) >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                        {
                            R.Cast(target);
                        }
                    }
                }


            }



        }


        protected override void Farming()
        {


            bool useW = RootMenu["farming"]["lane"]["useW"].Enabled;

            bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
            if (Q.Ready && useQ)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (minion.IsValidTarget(Q.Range) && minion != null)
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(Q.Range, false, false,
                                Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitQ"].As<MenuSlider>().Value)
                        {

                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {
                                Q.Cast();
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

                        if (minion.IsValidTarget(W.Range) && minion != null)
                        {
                            W.Cast();
                        }
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
                    Q.Cast();
                }
                if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
                if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range))
                {
                    W.Cast();
                }
            }
        }


        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };
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
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.CornflowerBlue);
            }
        }



        protected override void Killsteal()
        {




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
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 3, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R", KeyCode.T, KeybindType.Press));

            }
            RootMenu.Add(ComboMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hitQ", "^- Min. Minion for Q", 3, 1, 6));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitW", "^- Min. Minion for W", 3, 1, 6));
                

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
                DrawMenu.Add(new MenuBool("drawe", "Draw E Engage Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 350);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 575);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1400);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1000);
            R.SetSkillshot(0.4f, 150, 3000, false, SkillshotType.Circle);

        }

        protected override void SemiR()
        {

            if (RootMenu["combo"]["semir"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                if (R.Ready && RootMenu["combo"]["useR"].Enabled)
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

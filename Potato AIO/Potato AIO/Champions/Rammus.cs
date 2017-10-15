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
    class Rammus : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useQA"].Enabled;
            bool useW = RootMenu["combo"]["useW"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;

            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range))
                    {

                        if (target != null)
                        {
                            if (!Player.HasBuff("PowerBall"))
                            {
                                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                                        Player.ServerPosition)) == 0 && Bases.GameObjects.Jungle.Count(h =>
                                        h.IsValidTarget(250, false, false,
                                            Player.ServerPosition)) == 0)
                                {
                                    Q.Cast();
                                }
                            }
                        }
                    }

                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(300);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(300))
                    {

                        if (target != null)
                        {
                            if (!Player.HasBuff("DefensiveBallCurl"))
                            {
                                if (!Player.HasBuff("PowerBall"))
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                                            Player.ServerPosition)) == 0)
                                    {
                                        W.Cast();
                                    }
                                }
                            }
                        }

                    }

                }
            }
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range))
                    {

                        if (target != null)
                        {
                            if (!RootMenu["combo"]["WE"].Enabled)
                            {
                                if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                {
                                    E.Cast(target);
                                }
                            }
                            if (RootMenu["combo"]["WE"].Enabled)
                            {
                                if (Player.HasBuff("DefensiveBallCurl"))
                                {
                                    if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                    {
                                        E.Cast(target);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(R.Range))
                    {

                        if (target != null)
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

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {


                bool useE = RootMenu["farming"]["lane"]["useW"].Enabled;


                if (useE)
                {
                    if (W.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(300))
                        {
                            if (minion.IsValidTarget(300) && minion != null)
                            {

                                if (!Player.HasBuff("DefensiveBallCurl"))
                                {
                                    if (!Player.HasBuff("PowerBall"))
                                    {
                                        W.Cast();
                                    }
                                }
                            }
                        }


                    }
                }
            }

            foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(600))
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
                        if (!Player.HasBuff("DefensiveBallCurl"))
                        {
                            if (!Player.HasBuff("PowerBall"))
                            {
                                W.Cast(jungleTarget);
                            }
                        }
                    }
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        if (!Player.HasBuff("PowerBall"))
                        {
                            Q.Cast(jungleTarget);
                        }
                    }
                    if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
                    }


                }

            }

            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(600)).ToList())
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
                        if (!Player.HasBuff("PowerBall"))
                        {
                            Q.Cast(jungleTarget);
                        }
                    }
                    if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready &&
                        jungleTarget.IsValidTarget(W.Range))
                    {
                        if (!Player.HasBuff("DefensiveBallCurl"))
                        {
                            if (!Player.HasBuff("PowerBall"))
                            {

                                W.Cast(jungleTarget);
                            }
                        }
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
                Render.Circle(Player.Position,Q.Range, 50, Color.LightGreen);
            }
        }
    



        protected override void Killsteal()
        {


        }



        protected override void Harass()
        {


            bool useQ = RootMenu["harass"]["useQA"].Enabled;
            bool useW = RootMenu["harass"]["useW"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;



            if (manapercent < Player.ManaPercent())
            {

                if (useQ)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(Q.Range))
                        {

                            if (target != null)
                            {
                                if (!Player.HasBuff("PowerBall"))
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                                            Player.ServerPosition)) == 0 && Bases.GameObjects.Jungle.Count(h =>
                                            h.IsValidTarget(250, false, false,
                                                Player.ServerPosition)) == 0)
                                    {
                                        Q.Cast();
                                    }
                                }
                            }
                        }

                    }
                }
                if (useW)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(300);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(300))
                        {

                            if (target != null)
                            {
                                if (!Player.HasBuff("DefensiveBallCurl"))
                                {
                                    if (!Player.HasBuff("PowerBall"))
                                    {
                                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                                                Player.ServerPosition)) == 0)
                                        {
                                            W.Cast();
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                if (useE)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range))
                        {

                            if (target != null)
                            {
                                if (!RootMenu["harass"]["WE"].Enabled)
                                {
                                    if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                    {
                                        E.Cast(target);
                                    }
                                }
                                if (RootMenu["harass"]["WE"].Enabled)
                                {
                                    if (Player.HasBuff("DefensiveBallCurl"))
                                    {
                                        if(RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                        {
                                            E.Cast(target);
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
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("WE", "^- Only if W Active", false));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuBool("autow", "Auto W Turn-Off"));
            }
            RootMenu.Add(ComboMenu);
            var BlackList = new Menu("whitelist", "E Whitelist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Use E on: " + target.ChampionName,
                        true));
                }
            }
            RootMenu.Add(BlackList);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("WE", "^- Only if W Active", false));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
               
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
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Engage Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
            }
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

            
            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                E.Cast(target);


            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1200);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 300);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 325);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 400);
           

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["autow"].Enabled)
            {
                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(400, false, false,
                        Player.ServerPosition)) == 0 && Bases.GameObjects.Jungle.Count(h => h.IsValidTarget(400, false,
                        false,
                        Player.ServerPosition)) == 0 && Player.CountEnemyHeroesInRange(400) == 0)
                {
                    if (Player.HasBuff("DefensiveBallCurl"))
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

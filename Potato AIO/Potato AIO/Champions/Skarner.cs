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
    class Skarner : Champion
    {
        protected override void Combo()
        {

            if (E.Ready && RootMenu["combo"]["usee"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast(target);
                        }
                    }
                }


            }
            if (Q.Ready && RootMenu["combo"]["useq"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast();
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
                            if (!RootMenu["combo"]["blacklist"][target.ChampionName.ToLower()].Enabled)
                            {
                                R.CastOnUnit(target);
                            }
                        }
                    }
                }


            }



        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;


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


                if (useE)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {

                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            E.Cast(minion);
                        }

                    }
                }
            }
            float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

            if (manapercents < Player.ManaPercent())
            {
                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;

                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
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
        }

        protected override void Harass()
        {
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;


            if (manapercent < Player.ManaPercent())
            {
                if (E.Ready && RootMenu["combo"]["usee"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (target.IsValidTarget(E.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                E.Cast(target);
                            }
                        }
                    }


                }
                if (Q.Ready && RootMenu["combo"]["useq"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                    if (target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q.Cast();
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
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                var BlackList = new Menu("blacklist", "R Blacklist");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                    }
                }
                ComboMenu.Add(BlackList);
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);              
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
              
            }
            KillstealMenu = new Menu("wset", "W Settings");
            WShield.EvadeManager.Attach(KillstealMenu);
            WShield.EvadeOthers.Attach(KillstealMenu);
            WShield.EvadeTargetManager.Attach(KillstealMenu);
            RootMenu.Add(KillstealMenu);
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                E.Cast(Args.EndPosition);
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 350);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1000);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 350);
            E.SetSkillshot(0.25f, 50, 1430, false, SkillshotType.Line);

        }

        protected override void SemiR()
        {
            if (Player.HasBuff("skarnerimpalebuff"))
            {
                Orbwalker.Implementation.AttackingEnabled = false;
            }
            else 
                Orbwalker.Implementation.AttackingEnabled = true;
            
        
        }

        protected override void LastHit()
        {

        }

        protected override void Killsteal()
        {
        }
    }
}

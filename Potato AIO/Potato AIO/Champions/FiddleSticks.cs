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
    class FiddleSticks : Champion
    {

        protected override void Combo()
        {
            if (!Player.HasBuff("fearmonger_marker"))
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
                                E.Cast(target);
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
                if (W.Ready && RootMenu["combo"]["useW"].Enabled && !Q.Ready && !E.Ready)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
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

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

            if (!Player.HasBuff("fearmonger_marker"))
            {

                if (RootMenu["farming"]["lane"]["useW"].Enabled)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range)
                        .OrderByDescending(x => x.MaxHealth).ThenByDescending(x => x.Health))
                    {

                        if (minion.IsValidTarget(W.Range) && minion != null && !E.Ready)
                        {
                            W.Cast(minion);
                        }


                    }
                }
                if (useE)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {

                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(450, false, false,
                                minion.ServerPosition)) >=
                            RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                        {
                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                E.Cast(minion);
                            }
                        }
                    }
                }

                foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(W.Range))
                    .ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                    if (!Player.HasBuff("fearmonger_marker"))
                    {


                        if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) && !E.Ready)
                        {
                            W.Cast(jungleTarget);
                        }

                    }
                }
                foreach (var jungleTarget in Bases.GameObjects.JungleLegendary.Where(m => m.IsValidTarget(Q.Range))
                    .ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useWs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    if (!Player.HasBuff("fearmonger_marker"))
                    {


                        if (useWs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                        {
                           Q.Cast(jungleTarget);
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

                    bool useWs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    if (!Player.HasBuff("fearmonger_marker"))
                    {


                        if (useWs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }

                    }
                }
                foreach (var jungleTarget in Bases.GameObjects.JungleLegendary.Where(m => m.IsValidTarget(W.Range))
                    .ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                    if (!Player.HasBuff("fearmonger_marker"))
                    {


                        if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) && !E.Ready)
                        {
                            W.Cast(jungleTarget);
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
                    bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                    if (!Player.HasBuff("fearmonger_marker"))
                    {



                        if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
                        }
                    }
                }
            }

        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;

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



        protected override void Killsteal()
        {
            if (RootMenu["killsteal"]["kse"].Enabled)
            {
                
var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
             
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(bestTarget);
                }
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
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo", true));
                ComboMenu.Add(new MenuBool("autoq", "^- Auto Q on CC"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("autoe", "^- Auto E on CC"));
              
            }
            RootMenu.Add(ComboMenu);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
               
                KillstealMenu.Add(new MenuBool("kse", "Use E to Killsteal"));
            }
            RootMenu.Add(KillstealMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                       
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "^- if Hits X Minions", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W in Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);              
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready)
            {
                Q.Cast(target);
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 575);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 575);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 750);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 800);

        }

        protected override void SemiR()
        {
            if (!Player.HasBuff("fearmonger_marker"))
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
                if (RootMenu["combo"]["autoe"].Enabled)
                {
                    foreach (var target in GameObjects.EnemyHeroes.Where(
                        t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                              t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                              t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                              t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(E.Range)))
                    {

                        E.Cast(target);
                    }
                }
            }
            if (Player.HasBuff("fearmonger_marker"))
            {

                Orbwalker.Implementation.AttackingEnabled = false;
                Orbwalker.Implementation.MovingEnabled = false;
            }
            if (Hmmmmm < Game.TickCount)
            {
                if (!Player.HasBuff("fearmonger_marker"))
                {
                    Orbwalker.Implementation.AttackingEnabled = true;
                    Orbwalker.Implementation.MovingEnabled = true;
                }
            }

        }
        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "Drain")
                {

                    Orbwalker.Implementation.AttackingEnabled = false;
                    Orbwalker.Implementation.MovingEnabled = false;
                    Hmmmmm = 500 + Game.TickCount;
                }
            }
        }
        protected override void LastHit()
        {
        }
    }
}

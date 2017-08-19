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
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Support_AIO;
using Support_AIO.Bases;
using Support_AIO.Handlers;
using Support_AIO.SpellBlocking;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Karma : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;



            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            switch (RootMenu["combo"]["combomode"].As<MenuList>().Value)
            {
                case 0:
                    if (R.Ready && Q.Ready)
                    {
                        var collisions =
                            (IList<Obj_AI_Base>)Q.GetPrediction(target).CollisionObjects;
                        if (collisions.Any())
                        {
                            if (collisions.All(c => Support_AIO.Bases.Extensions.GetAllGenericUnitTargets().Contains(c)))
                            {
                                return;
                            }
                        }
                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                R.Cast();
                            }
                        }
                    }
                    if (useQ && target.IsValidTarget(Q.Range))
                    {
                        Q.Cast(target);
                    }
                    if (target.IsValidTarget(W.Range) && useW && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useE && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }
                    break;
                case 1:
                    if (R.Ready && W.Ready)
                    {
                        if (target.IsValidTarget(W.Range) && useQ)
                        {

                            if (target != null)
                            {
                                R.Cast();
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    if ((!R.Ready || !W.Ready) && useQ && Q.Ready && !Player.HasBuff("KarmaMantra"))
                    {
                        if (target.IsValidTarget(Q.Range))
                        {
                            Q.Cast(target);
                        }
                    }


                    if (target.IsValidTarget(W.Range) && useE && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }
                    break;
                case 2:
                    if (R.Ready && E.Ready)
                    {
                        if (target.IsValidTarget(Q.Range - 50) && useE)
                        {

                            if (target != null)
                            {
                                R.Cast();
                            }
                        }
                    }

                    if (target.IsValidTarget(W.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }

                    if (useQ && Q.Ready && !Player.HasBuff("KarmaMantra"))
                    {
                        if (target.IsValidTarget(Q.Range))
                        {
                            Q.Cast(target);
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    break;
            }
        }


        protected override void SemiR()
        {
            if (RootMenu["rq"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (R.Cast())
                {
                    Q.Cast(Game.CursorPos);
                }
            }
            if (RootMenu["combo"]["save"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                if (R.Ready && target.Distance(Player) < W.Range)
                {
                    R.Cast();
                }
                if (W.Ready && target.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
                if (E.Ready && target.Distance(Player) < W.Range && !Player.HasBuff("KarmaMantra"))
                {
                    E.Cast();
                }
                if (Q.Ready && target.Distance(Player) < Q.Range && !Player.HasBuff("KarmaMantra"))
                {
                    Q.Cast(target);
                }
            }
            if (RootMenu["combo"]["chase"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                if (E.Ready && target.Distance(Player) < W.Range && !Player.HasBuff("KarmaMantra"))
                {
                    E.Cast();
                }
                if (W.Ready && target.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
                if (R.Ready && target.Distance(Player) < Q.Range)
                {
                    R.Cast();
                }
                if (Q.Ready && target.Distance(Player) < Q.Range && Player.HasBuff("KarmaMantra"))
                {
                    Q.Cast(target);
                }
                if (Q.Ready && target.Distance(Player) < Q.Range && !Player.HasBuff("KarmaMantra") && !R.Ready)
                {
                    Q.Cast(target);
                }
            }
        }


        protected override void Farming()
        {

            foreach (var minion in Support_AIO.Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!minion.IsValidTarget() || !minion.IsValidSpellTarget())
                {
                    return;
                }
                bool useQ = RootMenu["farming"]["useq"].Enabled;
                bool useW = RootMenu["farming"]["usew"].Enabled;



                if (minion.IsValidTarget(Q.Range) && minion != null && useQ)
                {
                    Q.Cast(minion);
                }
                if (minion.IsValidTarget(W.Range) && minion != null && useW)
                {
                    W.CastOnUnit(minion);
                }
            }
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
            

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            throw new NotImplementedException();
        }

        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (RootMenu["combo"]["support"].Enabled)
            {
                if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Lasthit) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
                {

                    if (e.Target.IsMinion && GameObjects.AllyHeroes
                            .Where(x => UnitExtensions.Distance(x, Player) < E.Range).Count() > 1)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    
        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuList("combomode", "Combo Mode", new[] { "R - Q", "R - W", "R - E"}, 1));
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
                ComboMenu.Add(new MenuKeyBind("chase", "Chase Combo", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuKeyBind("save", "Survive Combo", KeyCode.Z, KeybindType.Press));

            }
            RootMenu.Add(ComboMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
            }
            RootMenu.Add(DrawMenu);
            FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuBool("useq", "Use Q in Jungle"));
                FarmMenu.Add(new MenuBool("usew", "Use W in Jungle"));
            }
            RootMenu.Add(FarmMenu);
            Gapcloser.Attach(RootMenu, "W Anti-Gap");

            EvadeMenu = new Menu("wset", "Shielding");
            {
                EvadeMenu.Add(new MenuList("modes", "Shielding Mode", new[] {"Spells Detector", "ZLib"}, 1));
                var First = new Menu("first", "Spells Detector");
                SpellBlocking.EvadeManager.Attach(First);
                SpellBlocking.EvadeOthers.Attach(First);
                SpellBlocking.EvadeTargetManager.Attach(First);
                
                EvadeMenu.Add(First);
                var zlib = new Menu("zlib", "ZLib");

                Support_AIO.ZLib.Attach(EvadeMenu);


            }
            RootMenu.Add(new MenuKeyBind("rq", "RQ To Mouse", KeyCode.G, KeybindType.Press));
            RootMenu.Add(EvadeMenu);

            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
      
                if (target != null && Args.EndPosition.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
            
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 950);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 675);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 800);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 60, 1700, true, SkillshotType.Line);
        }
    }
}

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
using Aimtec.SDK.Events;
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
    class Braum : Champion
    {

        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;




            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target != null)
                {
                    if (target.IsValidTarget(Q.Range))
                    {


                        Q.Cast(target);
                    }
                }
            }
            if (RootMenu["combo"]["usew"].Enabled)
            {
                foreach (var enemy in GameObjects.EnemyHeroes
                    .Where(x => x.IsValidTarget(Q.Range+W.Range, true) && !x.IsDead && x != null))

                {
                    foreach (var ally in GameObjects.AllyHeroes
                        .Where(x => x.IsValidTarget(W.Range, true) && !x.IsMe && !x.IsDead && x != null).OrderByDescending(x => x.Distance(enemy)))

                    {
                        if (ally.Distance(enemy) < 500 && Player.Distance(ally) > 200 && Player.Distance(ally) < 650 &&
                            Player.Distance(enemy) > 300 && Player.HealthPercent() > 10)
                        {
                            W.Cast(ally);
                        }
                    }
                }
            }
            if (RootMenu["combo"]["user"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target != null)
                {
                    if (target.IsValidTarget(R.Range))
                    {
                        if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                        {
                            R.CastIfWillHit(target,
                                RootMenu["combo"]["hitr"].As<MenuSlider>().Value - 1);
                        }
                        if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                        {
                            R.Cast(target);
                        }

                    }
                }
            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["low"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(W.Range, true) && !x.IsMe && !x.IsDead && x != null).OrderBy(x => x.Health))

                {
                    foreach (var enemy in GameObjects.EnemyHeroes
                        .Where(x => x.IsValidTarget(R.Range, true, false, ally.ServerPosition) && !x.IsDead && x != null))

                    {
                        W.CastOnUnit(ally);
                        if (ally.Distance(Player) < 300)
                        {
                            if (!Player.IsDashing())
                            {
                                E.Cast(enemy.ServerPosition);
                            }
                        }
                    }
                }
            }
            if (RootMenu["we"]["key"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                             RootMenu["we"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value != 0)
                    .OrderByDescending(
                        x => RootMenu["we"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value))
                {
                    W.CastOnUnit(ally);
                    foreach (var enemy in GameObjects.EnemyHeroes
                        .Where(x => x.IsValidTarget(R.Range, true, false, ally.ServerPosition) && !x.IsDead &&
                                    x != null))

                    {
                        W.CastOnUnit(ally);
                        if (ally.Distance(Player) < 300)
                        {
                            if (!Player.IsDashing())
                            {
                                E.Cast(enemy.ServerPosition);
                            }
                        }
                    }
                }
            }
            if (RootMenu["combo"]["semir"].Enabled)
            {

                if (Extensions.GetBestEnemyHeroTargetInRange(R.Range) != null)
                {
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                    {
                        R.CastIfWillHit(Extensions.GetBestEnemyHeroTargetInRange(R.Range),
                            RootMenu["combo"]["hitr"].As<MenuSlider>().Value-1);
                    }
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                    {
                        R.Cast(Extensions.GetBestEnemyHeroTargetInRange(R.Range));
                    }
                }

            }
        }


        protected override void Farming()
        {
            throw new NotImplementedException();
        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
            }
            
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            bool useQ = RootMenu["harass"]["useq"].Enabled;

            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
                }
            }
        }
    
        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
          
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo with Logic"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits", 2, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuKeyBind("low", "W > E Lowest Health Ally", KeyCode.G, KeybindType.Press));
            }
            RootMenu.Add(ComboMenu);
            var WE = new Menu("we", "W > E Settings");
            WE.Add(new MenuKeyBind("key", "W > E Key", KeyCode.Z, KeybindType.Press));

            WE.Add(new MenuSeperator("meow", "0 - Disabled"));
            WE.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
            foreach (var target in GameObjects.AllyHeroes.Where(x => !x.IsMe))
            {

                WE.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));

            }
            RootMenu.Add(WE);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
            }
            RootMenu.Add(HarassMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");

            EvadeMenu = new Menu("wset", "Shielding");
            {

            
                BraumWE.EvadeManager.Attach(EvadeMenu);
                BraumWE.EvadeOthers.Attach(EvadeMenu);
                BraumWE.EvadeTargetManager.Attach(EvadeMenu);
                EvadeMenu.Add(new MenuSlider("health", "Don't W>E if My Health <=", 30, 0, 100));



            }
            RootMenu.Add(EvadeMenu);

            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    Q.Cast(Args.EndPosition);
                }
            
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1000);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 650);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 0);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1250);
            Q.SetSkillshot(0.25f, 60, 1700f, true, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(0.6f, 130, 1400, false, SkillshotType.Line);

        }
    }
}

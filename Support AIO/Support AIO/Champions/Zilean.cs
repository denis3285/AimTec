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
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Support_AIO;
using Support_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Zilean : Champion
    {
        private int delayyyyyyyyyyyy;

        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (RootMenu["combo"]["support"].Enabled)
            {
                if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Lasthit) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
                {
                    if (e.Target.IsMinion && GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() > 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

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
            if (RootMenu["combo"]["prioritye"].Enabled)
            {
                if (target.IsValidTarget(E.Range) && useE)
                {

                    if (target != null)
                    {
                        if (E.CastOnUnit(target))
                        {
                            delayyyyyyyyyyyy = 300 + Game.TickCount;
                        }
                    }
                }
                if (delayyyyyyyyyyyy < Game.TickCount)
                {
                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            Q.Cast(target);
                        }
                    }
                    if (useW && target.IsValidTarget(Q.Range) && !Q.Ready && Player.Mana >=
                        Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost)
                    {
                        if (target != null)
                        {
                            W.Cast();
                            Q.Cast(target);
                        }
                    }

                }
            }
            if (!RootMenu["combo"]["prioritye"].Enabled)
            {
                if (target.IsValidTarget(E.Range) && useE)
                {

                    if (target != null)
                    {
                        E.CastOnUnit(target);
                    }
                }
                if (target.IsValidTarget(Q.Range) && useQ)
                {

                    if (target != null)
                    {
                        Q.Cast(target);
                    }
                }
                if (useW && target.IsValidTarget(Q.Range) && !Q.Ready && Player.Mana >=
                    Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost)
                {
                    if (target != null)
                    {
                        W.Cast();
                        Q.Cast(target);
                    }
                }

            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["rusage"].As<MenuList>().Value == 1)
            {
                foreach (var en in GameObjects.AllyHeroes)
                {
                    if (en != null && en.IsValidTarget(R.Range, true) && en.Distance(Player) < R.Range)
                    {
                        if(ZLib.Menu["whitelist"][en.ChampionName.ToLower()].Enabled)
                        {
                            if (en.HealthPercent() <= RootMenu["combo"]["hitr"].As<MenuSlider>().Value && !en.IsRecalling() && !Player.IsRecalling() && en.CountEnemyHeroesInRange(R.Range*2) > 0)
                            {
                                R.CastOnUnit(en);
                            }
                        }
                    }
                }
            }
            if (RootMenu["misc"]["autoq"].Enabled)
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
            if (RootMenu["flee"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                E.CastOnUnit(Player);
            }
            if (RootMenu["qwq"].Enabled)
            {
                var pos = (Game.CursorPos - Player.ServerPosition).Normalized();
                if (Player.Distance(Game.CursorPos) < 800)
                {
                    Q.Cast(Game.CursorPos);
                }
                if (Player.Distance(Game.CursorPos) > 800)
                {
                    Q.Cast(Player.ServerPosition + pos * 800);
                }
                if (!Q.Ready && Player.Mana >= Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost)
                {
                    W.Cast();
                }
                if (Player.Distance(Game.CursorPos) < 800)
                {
                    Q.Cast(Game.CursorPos);
                }
                if (Player.Distance(Game.CursorPos) > 800)
                {
                    Q.Cast(Player.ServerPosition + pos * 800);
                }
            }
            if (RootMenu["combo"]["slow"].Enabled)
            {
                if (Player.HasBuffOfType(BuffType.Slow))
                {
                    E.CastOnUnit(Player);
                }
                foreach (var en in GameObjects.AllyHeroes)
                {
                    if (!en.IsDead && en.Distance(Player) < E.Range && en.HasBuffOfType(BuffType.Slow))
                    {
                        E.CastOnUnit(en);
                    }
                }
            }
        }

        protected override void Farming()
        {
            foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                bool useQ = RootMenu["farming"]["useq"].Enabled;
                bool useW = RootMenu["farming"]["usew"].Enabled;



                if (minion.IsValidTarget(Q.Range) && minion != null && useQ)
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                            minion.ServerPosition)) >= RootMenu["farming"]["hitq"].As<MenuSlider>().Value)
                    {
                        Q.Cast(minion);
                    }
                }
                if (minion.IsValidTarget(Q.Range) && minion != null && useW)
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, false,
                            minion.ServerPosition)) >= RootMenu["farming"]["hitq"].As<MenuSlider>().Value)
                    {
                        if (!Q.Ready && Player.Mana >= Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost)
                        {
                            W.Cast();
                        }
                    }
                }
            }
        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
        }


        protected override void Drawings()
        {
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
        }

        protected override void Killsteal()
        {
            
        }

        protected override void Harass()
        {
            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"].Enabled;

                bool useW = RootMenu["harass"]["usew"].Enabled;

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
                if (useW && target.IsValidTarget(Q.Range))
                {
                    if (target != null && !Q.Ready && Player.Mana >= Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost)
                    {
                        W.Cast();
                        Q.Cast(target);
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W for Q Reset"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("prioritye", "Priority E Usage First", false));
                ComboMenu.Add(new MenuBool("slow", "Use Auto E on Slowed Ally"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("rusage", "R Usage", new[] { "If Incoming Damage Kills", "At X Health" }, 0));
                ComboMenu.Add(new MenuSlider("hitr", "If X Health <= (Health Mode)", 20, 1, 100));

                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);


            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));

                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("usew", "Use W for Q Reset"));
            }
            RootMenu.Add(HarassMenu);
            
            KillstealMenu = new Menu("misc", "Misc.");
            {
                KillstealMenu.Add(new MenuBool("autoq", "Auto Q on CC"));
            }
            RootMenu.Add(KillstealMenu);

            FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
                FarmMenu.Add(new MenuSlider("hitq", "^- if hits X", 3, 1, 6));
                FarmMenu.Add(new MenuBool("usew", "Use W to Reset Q"));
            }
            RootMenu.Add(FarmMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);
            var zlib = new Menu("zlib", "ZLib");

            Support_AIO.ZLib.Attach(RootMenu);
            RootMenu.Add(new MenuKeyBind("qwq", "Q-W-Q to Mouse", KeyCode.T, KeybindType.Press));
            RootMenu.Add(new MenuKeyBind("flee", "Flee Key", KeyCode.G, KeybindType.Press));
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 900);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 550);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 900);
            Q.SetSkillshot(0.75f, 105f, int.MaxValue, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}

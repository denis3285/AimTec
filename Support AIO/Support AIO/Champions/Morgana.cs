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
    class Morgana : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;




            var t = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!t.IsValidTarget())
            {

                return;
            }

            if (t.IsValidTarget(Q.Range) && useQ)
            {

                if (t != null)
                {
                    Q.Cast(t);
                }
            }
            if (t.IsValidTarget(W.Range) && useW)
            {

                if (t != null)
                {
                    if (!RootMenu["combo"]["cc"].Enabled)
                    {
                        W.Cast(t);
                    }
                    if (RootMenu["combo"]["cc"].Enabled)
                    {

                        if (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                            t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                            t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                            t.HasBuffOfType(BuffType.Suppression))
                        {
                            W.Cast(t);
                        }
                    }
                }

            }
            if (t.IsValidTarget(R.Range) && RootMenu["combo"]["user"].Enabled)
            {

                if (t != null)
                {
                    if (Player.CountEnemyHeroesInRange(R.Range) >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                    {
                        R.Cast();
                    }
                }
            }
        }

        protected override void SemiR()
        {
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
        }


        protected override void Farming()
        {
            foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                if (Player.ManaPercent() >= RootMenu["farming"]["mana"].As<MenuSlider>().Value)
                {
                  
                    bool useW = RootMenu["farming"]["usew"].Enabled;



                    if (minion.IsValidTarget(W.Range) && minion != null && useW)
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(275, false, false,
                                minion.ServerPosition)) >= RootMenu["farming"]["hitw"].As<MenuSlider>().Value)
                        {
                            W.Cast(minion);
                        }
                    }
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
                Render.Circle(Player.Position, E.Range, 40, Color.Crimson);
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
                var t = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!t.IsValidTarget())
                {

                    return;
                }


                if (t.IsValidTarget(Q.Range) && RootMenu["harass"]["useq"].Enabled)
                {

                    if (t != null)
                    {
                        Q.Cast(t);
                    }
                }
                if (t.IsValidTarget(W.Range) && RootMenu["harass"]["usew"].Enabled)
                {

                    if (t != null)
                    {
                        if (!RootMenu["harass"]["cc"].Enabled)
                        {
                            W.Cast(t);
                        }
                        if (RootMenu["harass"]["cc"].Enabled)
                        {

                            if (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                                t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                                t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                                t.HasBuffOfType(BuffType.Suppression))
                            {
                                W.Cast(t);
                            }
                        }
                    }

                }
            }
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("cc", "^- Only W if Immobile"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                HarassMenu.Add(new MenuBool("cc", "^- Only W if Immobile"));

            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
               
                FarmMenu.Add(new MenuBool("usew", "Use W to Farm"));
                FarmMenu.Add(new MenuSlider("hitw", "^- if Hits X Minions", 3, 1, 6));

            }
            RootMenu.Add(FarmMenu);
            KillstealMenu = new Menu("misc", "Misc.");
            {

                KillstealMenu.Add(new MenuBool("autoq", "Auto Q on CC"));
               

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);

            EvadeMenu = new Menu("wset", "Shielding");
            {

                MorganaSpellBlocking.EvadeManager.Attach(EvadeMenu);
                MorganaSpellBlocking.EvadeOthers.Attach(EvadeMenu);
                MorganaSpellBlocking.EvadeTargetManager.Attach(EvadeMenu);
             
            }
            RootMenu.Add(EvadeMenu);

            RootMenu.Attach();
        }

        
   

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1175);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 900);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 800);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 625);
            Q.SetSkillshot(0.25f, 80, 1300, true, SkillshotType.Line);
            W.SetSkillshot(0.50f, 150, 2200, false, SkillshotType.Circle);
        }
    }
}

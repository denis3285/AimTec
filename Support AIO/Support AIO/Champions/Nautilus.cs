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
    class Nautilus : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;




            var t = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!t.IsValidTarget())
            {

                return;
            }

            if (t.IsValidTarget(Q.Range) && useQ)
            {

                if (t != null)
                {

                    if (!Extensions.AnyWallInBetween(Player.ServerPosition, t.ServerPosition))
                    {
                        Q.Cast(t);
                    }
                }
            }
            if (t.IsValidTarget(E.Range) && useE)
            {

                if (t != null)
                {
                    E.Cast();
                }
            }
            if (t.IsValidTarget(R.Range) && RootMenu["combo"]["user"].Enabled)
            {

                if (t != null)
                {
                    if (!RootMenu["black"][t.ChampionName.ToLower()].Enabled)
                    {
                        R.CastOnUnit(t);
                    }
                }
            }
        }

        protected override void SemiR()
        {

        }


        protected override void Farming()
        {
            foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                if (Player.ManaPercent() >= RootMenu["farming"]["mana"].As<MenuSlider>().Value)
                {
                  
                    bool useW = RootMenu["farming"]["usew"].Enabled;

                   

                    if (minion.IsValidTarget(E.Range) && minion != null && useW)
                    {
                        Console.WriteLine("2");
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(E.Range, false, false,
                                Player.ServerPosition)) >= RootMenu["farming"]["hitw"].As<MenuSlider>().Value)
                        {
                            E.Cast();
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
                        
                        if (!Extensions.AnyWallInBetween(Player.ServerPosition, t.ServerPosition))
                        {
                            Q.Cast(t);
                        }
                    }
                }
                if (t.IsValidTarget(E.Range) && RootMenu["harass"]["usee"].Enabled)
                {

                    if (t != null)
                    {
                        E.Cast();
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
  
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                WhiteList = new Menu("black", "R Black List");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                    }
                }

                ComboMenu.Add(WhiteList);
            }
            RootMenu.Add(ComboMenu);
            KillstealMenu = new Menu("wset", "W Settings");
            NautilusSelfShield.EvadeManager.Attach(KillstealMenu);
            NautilusSelfShield.EvadeOthers.Attach(KillstealMenu);
            NautilusSelfShield.EvadeTargetManager.Attach(KillstealMenu);
            RootMenu.Add(KillstealMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));


            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
               
                FarmMenu.Add(new MenuBool("usew", "Use E to Farm"));
                FarmMenu.Add(new MenuSlider("hitw", "^- if Hits X Minions", 3, 1, 6));

            }
            RootMenu.Add(FarmMenu);

            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1000);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 600);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 825);
            Q.SetSkillshot(0.25f, 87, 2000, true, SkillshotType.Line, false, HitChance.None);
          
        }
    }
}

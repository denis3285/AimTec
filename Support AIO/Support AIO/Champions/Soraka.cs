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
    class Soraka : Champion
    {
      
        


        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (RootMenu["combo"]["support"].Enabled)
            {
                if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Lasthit) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
                {
                    if (e.Target.IsMinion && GameObjects.AllyHeroes.Any(x => x.Distance(Player) < 1000 && !x.IsMe))
                    {
                        e.Cancel = true;
                    }
                }
            }
        }


     
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
   
            bool useE = RootMenu["combo"]["usee"].Enabled;
        
            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (target.IsValidTarget(E.Range) && useE)
            {

                if (target != null)
                {
                    E.Cast(target);
                }
            }
            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
                }
            }

        }

        protected override void SemiR()
        {
            if (RootMenu["heal"]["autor"].Enabled && !RootMenu["heal"]["semi"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsAlly && !x.IsRecalling() &&
                         x.HealthPercent() <=
                         RootMenu["heal"]["rhealth"].As<MenuSlider>().Value && x.CountEnemyHeroesInRange(3000) > 0 &&
                         !RootMenu["black"][x.ChampionName.ToLower()].As<MenuBool>().Enabled))
                {
                    if (ally != null && !ally.IsDead)
                    {
                        R.Cast();
                    }
                }

            }
            if (RootMenu["heal"]["autow"].Enabled)
            {



                switch (RootMenu["heal"]["mode"].As<MenuList>().Value)
                {
                    case 0:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent() >=
                                     RootMenu["heal"]["me"].As<MenuSlider>().Value && x.HealthPercent() <=
                                     RootMenu["heal"]["ally"].As<MenuSlider>().Value &&
                                     RootMenu["white"][x.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            .OrderByDescending(x => x.TotalAttackDamage))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 1:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent() >=
                                     RootMenu["heal"]["me"].As<MenuSlider>().Value && x.HealthPercent() <=
                                     RootMenu["heal"]["ally"].As<MenuSlider>().Value &&
                                     RootMenu["white"][x.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            .OrderByDescending(x => x.TotalAbilityDamage))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 2:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent() >=
                                     RootMenu["heal"]["me"].As<MenuSlider>().Value && x.HealthPercent() <=
                                     RootMenu["heal"]["ally"].As<MenuSlider>().Value &&
                                     RootMenu["white"][x.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            .OrderBy(x => x.Health))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 3:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent() >=
                                     RootMenu["heal"]["me"].As<MenuSlider>().Value && x.HealthPercent() <=
                                     RootMenu["heal"]["ally"].As<MenuSlider>().Value &&
                                     RootMenu["white"][x.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            .OrderByDescending(x => x.MaxHealth))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 4:
                        foreach (var ally in GameObjects.AllyHeroes.Where(
                            x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                 Player.HealthPercent() >=
                                 RootMenu["heal"]["me"].As<MenuSlider>().Value && x.HealthPercent() <=
                                 RootMenu["white"][x.ChampionName.ToLower() + "hp"].As<MenuSlider>().Value &&
                                 RootMenu["white"][x.ChampionName.ToLower()].As<MenuBool>().Enabled))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;



                }


            }
        }



        protected override void Farming()
        {
            throw new NotImplementedException();
        }


        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };

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
            if (RootMenu["heal"]["semi"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsAlly && !x.IsRecalling() &&
                         x.HealthPercent() <=
                         RootMenu["heal"]["rhealth"].As<MenuSlider>().Value &&
                         !RootMenu["black"][x.ChampionName.ToLower()].As<MenuBool>().Enabled))
                {
                    if (ally != null)
                    {
                        Vector2 maybeworks;
                        var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
                        var xaOffset = (int) maybeworks.X;
                        var yaOffset = (int) maybeworks.Y;



                        Render.Text(xaOffset - 50, yaOffset - 150, Color.Red, "ALLY IS LOW - PRESS R",
                            RenderTextFlags.VerticalCenter);


                    }
                }
            }
            if (RootMenu["drawings"]["toggle"].Enabled)
            {
                Vector2 maybeworks;
                var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
                var xaOffset = (int)maybeworks.X;
                var yaOffset = (int)maybeworks.Y;

                if (RootMenu["harass"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.GreenYellow, "HARASS: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!RootMenu["harass"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.Red, "HARASS: OFF",
                        RenderTextFlags.VerticalCenter);
                }
            }

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }
      

        }

        protected override void Killsteal()
        {
            
        }

        protected override void Harass()
        {
            if (RootMenu["harass"]["toggle"].Enabled)
            {
                bool useQ = RootMenu["harass"]["useq"].Enabled;

                bool useE = RootMenu["harass"]["usee"].Enabled;

                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                if (target.IsValidTarget(E.Range) && useE)
                {

                    if (target != null)
                    {
                        E.Cast(target);
                    }
                }
                if (target.IsValidTarget(Q.Range) && useQ)
                {

                    if (target != null)
                    {
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
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
           HarassMenu= new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuKeyBind("toggle", "Toggle Key", KeyCode.T, KeybindType.Toggle));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
               
            }
            
            RootMenu.Add(ComboMenu);
            RootMenu.Add(HarassMenu);
            WhiteList = new Menu("heal", "Healing");
            {
                WhiteList.Add(new MenuBool("autow", "Enable W Healing"));
                WhiteList.Add(new MenuList("mode", "Healing Priority", new[] { "Most AD", "Most AP", "Least Health", "Least Health (Squishies)", "Whitelist" }, 3));
                WhiteList.Add(new MenuSlider("ally", "Ally Health Percent <=", 50));
                WhiteList.Add(new MenuSlider("me", "Don't W if my Health <=", 30));
                WhiteList.Add(new MenuBool("autor", "Enable R Healing"));
                WhiteList.Add(new MenuBool("semi", "^- Semi Manual  R", true));
                WhiteList.Add(new MenuSlider("rhealth", "Ally R Health <=", 20));
            }
            RootMenu.Add(WhiteList);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("toggle", "Draw Toggle"));
            }
            RootMenu.Add(DrawMenu);
            FarmMenu = new Menu("white", "W White List");
            {
                FarmMenu.Add(new MenuSeperator("meow", "Health Percent only works if Whitelist Mode"));
                foreach (var target in GameObjects.AllyHeroes)
                {
                    if (!target.IsMe)
                    {
                        FarmMenu.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                        FarmMenu.Add(new MenuSlider(target.ChampionName.ToLower() + "hp", "^- Health Percent: ", 100, 0, 100));
                    }
                }
            }
            RootMenu.Add(FarmMenu);
            WhiteList = new Menu("black", "R Black List");
            {
                foreach (var target in GameObjects.AllyHeroes)
                {
                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                }
            }
            RootMenu.Add(WhiteList);

            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 800);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 550);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 925);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.5f, 80, 1750f, false, SkillshotType.Circle, false, HitChance.None);
            E.SetSkillshot(0.5f, 50, 1750f, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}

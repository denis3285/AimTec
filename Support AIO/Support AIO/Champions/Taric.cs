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
    class Taric : Champion
    {

        protected override void Combo()
        {


            if (E.Ready && RootMenu["combo"]["usee"].Enabled)
            {
                if (RootMenu["combo"]["epriority"].Enabled)
                {
                                   var t = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (t.IsValidTarget(E.Range) && t != null)
                    {

                        if (t.IsValidTarget(E.Range))
                        {
                            E.Cast(t);
                        }
                    }
                
                    if (RootMenu["combo"]["fromally"].Enabled)
                    {
                        var enemyInBounceRange =
                            GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(1280 + E.Range));
                        if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > E.Range)
                        {

                            foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.IsAlly && x != null && !x.IsMe))
                            {
                                if (ally != null && !ally.IsDead && ally.HasBuff("taricwallybuff") &&
                                    ally.Distance(Player) < 1280)
                                {

                                    var meowmeow =
                                        GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(E.Range, false,
                                            false, ally.ServerPosition));
                                    if (meowmeow != null)
                                    {
                                        var meow = E.GetPrediction(meowmeow, ally.ServerPosition, ally.ServerPosition);

                                        E.Cast(meow.CastPosition);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!RootMenu["combo"]["epriority"].Enabled)
                {

                    if (RootMenu["combo"]["fromally"].Enabled)
                    {
                        var enemyInBounceRange =
                            GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(1280 + E.Range));
                        if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > E.Range)
                        {

                            foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.IsAlly && x != null && !x.IsMe))
                            {
                                if (ally != null && !ally.IsDead && ally.HasBuff("taricwallybuff") &&
                                    ally.Distance(Player) < 1280)
                                {

                                    var meowmeow =
                                        GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(E.Range, false,
                                            false, ally.ServerPosition));
                                    if (meowmeow != null)
                                    {
                                        var meow = E.GetPrediction(meowmeow, ally.ServerPosition, ally.ServerPosition);

                                        E.Cast(meow.CastPosition);
                                    }
                                }
                            }
                        }
                    }
                    var t = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (t.IsValidTarget(E.Range) && t != null)
                    {

                        if (t.IsValidTarget(E.Range))
                        {
                            E.Cast(t);
                        }
                    }
                }
            }
        }



        protected override void SemiR()
        {

            if (RootMenu["heal"]["autow"].Enabled)
            {
                if (RootMenu["heal"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                {
                    foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= Q.Range && x.IsAlly && !x.IsRecalling() && x.HealthPercent() <=
                             RootMenu["heal"][x.ChampionName.ToLower() + "hp"].As<MenuSlider>().Value &&
                             RootMenu["heal"][x.ChampionName.ToLower()].As<MenuBool>().Enabled))
                    {
                        if (ally != null && !ally.IsDead)
                        {
                            Q.Cast();
                        }
                    }

                    var daggers = ObjectManager.Get<GameObject>()
                        .Where(d => d.IsValid && !d.IsDead && d.Distance(Player) <= 1280 &&
                                    d.Name == "Taric_Base_W_buff_indicator.troy");
                    foreach (var dagger in daggers)
                    {

                        if (dagger != null)
                        {
                            foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(dagger) <= Q.Range && x.IsAlly && !x.IsRecalling() &&
                                     x.HealthPercent() <=
                                     RootMenu["heal"][x.ChampionName.ToLower() + "hp"].As<MenuSlider>().Value &&
                                     RootMenu["heal"][x.ChampionName.ToLower()].As<MenuBool>().Enabled))
                            {
                                if (ally != null && !ally.IsDead)
                                {
                                    Q.Cast();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};

        public static int SxOffset(Obj_AI_Hero target)
        {

            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }

        protected override void Farming()
        {

        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Wheat);
                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsAlly && x != null))
                {
                    if (ally != null && !ally.IsDead && ally.HasBuff("taricwallybuff") && ally.Distance(Player) < 1280)
                    {
                        Render.Circle(ally.Position, Q.Range, 40, Color.Wheat);
                    }
                }
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawwm"].Enabled)
            {
                Render.Circle(Player.Position, 1280, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Crimson);
            }

            if (RootMenu["drawings"]["drawea"].Enabled)
            {

                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsAlly && x != null))
                {
                    if (ally != null && !ally.IsDead && ally.HasBuff("taricwallybuff") && ally.Distance(Player) < 1280)
                    {
                        Render.Circle(ally.Position, E.Range, 40, Color.Crimson);
                    }
                }
            
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
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
            
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("fromally", "^- Use from Ally"));
                ComboMenu.Add(new MenuBool("epriority", "^- Priority E from Me"));

            }

            RootMenu.Add(ComboMenu);
            WhiteList = new Menu("heal", "Healing");
            {
                WhiteList.Add(new MenuBool("autow", "Enable Q  Healing"));
                WhiteList.Add(new MenuSlider("mana", "Mana Manager", 20, 0, 100));
                foreach (var target in GameObjects.AllyHeroes)
                {

                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                    WhiteList.Add(new MenuSlider(target.ChampionName.ToLower() + "hp", "^- Health Percent: ", 80, 0, 100));

                }
            }
            RootMenu.Add(WhiteList);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawwm", "Draw W Max Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawea", "Draw E Range from Allies"));
            }
            RootMenu.Add(DrawMenu);
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
            RootMenu.Add(EvadeMenu);
 
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 325);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 800);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 600);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 400);
            E.SetSkillshot(0.2f, 50, 3000, false, SkillshotType.Line, false);
        }
    }
}

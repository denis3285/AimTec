using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Orbwalking;
using Aimtec;
using Aimtec.SDK;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Support_AIO;
using Support_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Leona : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;


            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            
            if (target.IsValidTarget(E.Range) && useE && RootMenu["whitelist"][target.ChampionName.ToLower()]
                    .As<MenuBool>().Enabled)
            {

                if (target != null)
                {
                    E.Cast(target);
                }
            }
            if (target.IsValidTarget(300) && useQ)
            {

                if (target != null)
                {
                    if (RootMenu["combo"]["eq"].Enabled)
                    {
                        if (target.HasBuff("leonazenithbladeroot"))
                        {

                            Q.Cast();
                        }
                    }
                    if (!RootMenu["combo"]["eq"].Enabled)
                    {

                        Q.Cast();

                    }
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.Cast();
                }
            }


            if (useR)
            {



                if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                {
                    if (target != null &&
                        target.CountEnemyHeroesInRange(300) >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value &&
                        target.IsValidTarget(R.Range))
                    {
                        R.Cast(target);
                    }
                }
                if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                {
                    if (target != null &&

                        target.IsValidTarget(R.Range))
                    {
                        R.Cast(target);
                    }
                }


            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["semir"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }


                if (target != null &&
                    target.IsValidTarget(R.Range))
                {
                    R.Cast(target);
                }
            }
        }

        
    

    protected override void Farming()
        {
            throw new NotImplementedException();
        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
        }

        protected override void Drawings()
        {
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
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
            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useW = RootMenu["harass"]["usew"].Enabled;
            bool useE = RootMenu["harass"]["usee"].Enabled;



            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

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
            if (target.IsValidTarget(1000) && useQ)
            {

                if (target != null)
                {
                    if (RootMenu["harass"]["eq"].Enabled)
                    {
                        if (target.HasBuff("leonazenithbladeroot"))
                        {

                            Q.Cast();
                        }
                    }
                    if (!RootMenu["harass"]["eq"].Enabled)
                    {

                        Q.Cast();

                    }
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.Cast();
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
                ComboMenu.Add(new MenuBool("eq", "Use Q only if E Hits"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits", 2, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));


            }
            RootMenu.Add(ComboMenu);
            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("eq", "Use Q only if E Hits"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));


            }
            RootMenu.Add(HarassMenu);
            WhiteList = new Menu("whitelist", "E Whitelist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                }
            }
            RootMenu.Add(WhiteList);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);



            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 0);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 275);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 875);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1200);

            E.SetSkillshot(0.25f, 20, 2400, false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(1.3f, 300, float.MaxValue, false, SkillshotType.Circle);
        }
    }
}

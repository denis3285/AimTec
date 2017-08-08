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
using Support_AIO;
using Support_AIO.Bases;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Blitzcrank : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;
            float enemies = RootMenu["combo"]["hitr"].As<MenuSlider>().Value;
            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
            {

                if (target != null && !RootMenu["black"][target.ChampionName.ToLower()].Enabled)
                {
                    Q.Cast(target);
                }
            }
            if (E.Ready && useE)
            {

                if (target != null)
                {
                    if (!RootMenu["combo"]["eq"].Enabled && target.IsValidTarget(E.Range))
                    {
                        E.Cast();
                    }
                    if (RootMenu["combo"]["eq"].Enabled)
                    {
                        if (target.HasBuff("rocketgrab2"))
                        {
                            E.Cast();
                        }
                    }
                }
            }

            if (R.Ready && useR && target.IsValidTarget(R.Range))
            {

                if (target != null && enemies <= Player.CountEnemyHeroesInRange(R.Range))
                {
                    R.Cast();
                }
            }
        }

        protected override void SemiR()
        {

            if (RootMenu["qset"]["autoq"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {
                    return;
                }

                if (Q.Ready && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        var pred = Q.GetPrediction(target);
                        if (pred.HitChance == HitChance.Impossible)
                        {

                            Q.Cast(pred.CastPosition);


                        }
                    }
                }
            }
            if (RootMenu["qset"]["grabq"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {
                    return;
                }

                if (Q.Ready && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        Q.Cast(target);
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
            if (RootMenu["drawings"]["qmin"].Enabled)
            {
                Render.Circle(Player.Position, RootMenu["qset"]["minq"].As<MenuSlider>().Value, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["qmax"].Enabled)
            {
                Render.Circle(Player.Position, RootMenu["qset"]["maxq"].As<MenuSlider>().Value, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }
        }

        protected override void Killsteal()
        {
            if (Q.Ready &&
                RootMenu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = Q.GetBestKillableHero(DamageType.Magical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(Player.GetFullAttackRange(bestTarget)) &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) > bestTarget.Health)
                {
                    Q.Cast(bestTarget);
                }
            }

            if (R.Ready &&
                RootMenu["killsteal"]["ksr"].Enabled)
            {
                var bestTarget = R.GetBestKillableHero(DamageType.Magical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(Player.GetFullAttackRange(bestTarget)) &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.R) > bestTarget.Health)
                {
                    R.Cast();
                }
            }
        }

        protected override void Harass()
        {
            throw new NotImplementedException();
        }

        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);

            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q"));
                ComboMenu.Add(new MenuBool("usee", "Use E "));
                ComboMenu.Add(new MenuBool("eq", "^- Only if Q Landed"));
                ComboMenu.Add(new MenuBool("user", "Use R"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 2, 1, 5));
            }
            RootMenu.Add(ComboMenu);
            var QSet = new Menu("qset", "Q Settings");
            {
                QSet.Add(new MenuKeyBind("grabq", "Grab Q", KeyCode.T, KeybindType.Press));
                QSet.Add(new MenuBool("autoq", "Use Auto Q on Dash", true));
                QSet.Add(new MenuSlider("minq", "Min Q Range", 300, 10, 400));
                QSet.Add(new MenuSlider("maxq", "Max Q Range", 900, 500, 900));
            }
            RootMenu.Add(QSet);
            WhiteList = new Menu("black", "Black List");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                }
            }

            RootMenu.Add(WhiteList);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("qmax", "Draw Q Max."));
                DrawMenu.Add(new MenuBool("qmin", "Draw Q Min."));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
            }
            RootMenu.Add(DrawMenu);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KillstealMenu.Add(new MenuBool("ksr", "Killsteal with R"));
            }
            RootMenu.Add(KillstealMenu);
        

            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 900);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 300);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 600);

            Q.SetSkillshot(0.25f, 70, 2000, true, SkillshotType.Line);
        }
    }
}

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
    class Brand : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;

            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }
            switch (RootMenu["combo"]["combomode"].As<MenuList>().Value)
            {
                case 1:

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
                            switch (RootMenu["combo"]["qmode"].As<MenuList>().Value)
                            {
                                case 0:
                                    Q.Cast(target);
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        Q.Cast(target);
                                    }
                                    break;
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }
                    break;
                case 0:
                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }
                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].As<MenuList>().Value)
                            {
                                case 0:
                                    Q.Cast(target);
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        Q.Cast(target);
                                    }
                                    break;
                            }
                        }
                    }

                    break;
                case 3:
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }
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
                            switch (RootMenu["combo"]["qmode"].As<MenuList>().Value)
                            {
                                case 0:
                                    Q.Cast(target);
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        Q.Cast(target);
                                    }
                                    break;
                            }
                        }
                    }

                    break;
                case 2:
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }

                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].As<MenuList>().Value)
                            {
                                case 0:
                                    Q.Cast(target);
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        Q.Cast(target);
                                    }
                                    break;
                            }
                        }
                    }
                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }


                    break;

            }
            if (useR)
            {
                switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                {
                    case 0:
                        if (target != null)
                        {
                            if (!RootMenu["combo"]["minion"].Enabled)
                            {
                                if (target.IsValidTarget(R.Range) && target.CountEnemyHeroesInRange(750) >=
                                    RootMenu["combo"]["bounce"].As<MenuSlider>().Value && target.HealthPercent() <=
                                    RootMenu["combo"]["hp"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                            if (RootMenu["combo"]["minion"].Enabled)
                            {
                                if (target.IsValidTarget(R.Range) &&
                                    (target.CountEnemyHeroesInRange(750) +
                                     GameObjects.EnemyMinions.Count(h => h.IsValidTarget(750, false, false,
                                         target.ServerPosition)) >=
                                     RootMenu["combo"]["bounce"].As<MenuSlider>().Value) && target.HealthPercent() <=
                                    RootMenu["combo"]["hp"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                        }
                        break;
                    case 1:
                        if (target != null)
                        {
                            if (!RootMenu["combo"]["minion"].Enabled)
                            {
                                if (target.IsValidTarget(R.Range) && target.CountEnemyHeroesInRange(750) >=
                                    RootMenu["combo"]["bounce"].As<MenuSlider>().Value && target.Health <=
                                    Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.E) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                            if (RootMenu["combo"]["minion"].Enabled)
                            {
                                if (target.IsValidTarget(R.Range) &&
                                    (target.CountEnemyHeroesInRange(750) +
                                     GameObjects.EnemyMinions.Count(h => h.IsValidTarget(750, false, false,
                                         target.ServerPosition)) >=
                                     RootMenu["combo"]["bounce"].As<MenuSlider>().Value) && target.Health <=
                                    Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.E) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                        }
                        break;
                }
            }
        }


        protected override void SemiR()
        {
        }

        protected override void Farming()
        {
        
        }

        protected override void LastHit()
        {
    
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
                Render.Circle(Player.Position, E.Range, 40, Color.Aquamarine);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range * 2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                           
                            var heroUnit = unit as Obj_AI_Hero;
                            int width = 103;

                            int xOffset = SxOffset(heroUnit);
                            int yOffset = SyOffset(heroUnit);
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;
                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float)(barPos.X + (unit.Health >
                                                    Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                    Player.GetSpellDamage(unit, SpellSlot.W) +
                                                    Player.GetSpellDamage(unit, SpellSlot.E) +
                                                    Player.GetSpellDamage(unit, SpellSlot.R)
                                            ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                       Player.GetSpellDamage(unit, SpellSlot.W) +
                                                                       Player.GetSpellDamage(unit, SpellSlot.E) +
                                                                       Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                       unit.MaxHealth * 100 / 100)
                                            : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
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
                    if (e.Target.IsMinion && GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() > 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }


        protected override void Killsteal()
        {
            if (Q.Ready &&
                RootMenu["ks"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health && bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
            if (W.Ready &&
                RootMenu["ks"]["ksw"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health && bestTarget.IsValidTarget(W.Range))
                {
                    W.Cast(bestTarget);
                }
            }
            if (E.Ready &&
                RootMenu["ks"]["kse"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health && bestTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(bestTarget);
                }
            }
            if (R.Ready &&
                RootMenu["ks"]["ksr"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(R, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.R) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    if (bestTarget.CountEnemyHeroesInRange(750) >=
                        RootMenu["ks"]["bounce"].As<MenuSlider>().Value)
                    {
                        R.CastOnUnit(bestTarget);
                    }
                }
            }
        }

        protected override void Harass()
        {
            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"].Enabled;
                bool useW = RootMenu["harass"]["usew"].Enabled;
                bool useE = RootMenu["harass"]["usee"].Enabled;

                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                switch (RootMenu["harass"]["harassmode"].As<MenuList>().Value)
                {
                    case 1:

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
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        break;
                    case 0:
                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }

                        break;
                    case 3:
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
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
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }

                        break;
                    case 2:
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }

                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }


                        break;

                }
            }
        }


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);

            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuList("combomode", "Combo Mode", new[] { "E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E" }, 1));
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuList("qmode", "Q Mode", new[] { "Always", "Only Stun" }, 1));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("rmode", "R Mode", new[] { "If X Health", "Only if Killable" }, 1));

                ComboMenu.Add(new MenuSlider("bounce", "Use R if Bounces On X Targets", 1, 1, 5));
                ComboMenu.Add(new MenuBool("minion", "^- Include Minions for Bounce"));
          
                ComboMenu.Add(new MenuSlider("hp", "If Health <= ( If X Health Mode)", 50, 1, 100));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));

                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuList("qmode", "Q Mode", new[] {"Always", "Only Stun"}, 1));
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
                HarassMenu.Add(new MenuList("harassmode", "Harass Mode", new[] {"E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E"},
                    1));

            }
            RootMenu.Add(HarassMenu);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killseal with Q"));
                KillstealMenu.Add(new MenuBool("ksw", "Killseal with W"));
                KillstealMenu.Add(new MenuBool("kse", "Killseal with E"));
                KillstealMenu.Add(new MenuBool("ksr", "Killseal with R"));
                KillstealMenu.Add(new MenuSlider("bounce", "^- Only if Bounces on X Enemies", 1, 1, 5));

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1050);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 900);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 625);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 750);
            Q.SetSkillshot(0.25f, 75f, 1600, true, SkillshotType.Line);
            W.SetSkillshot(0.85f, 200f, 2900, false, SkillshotType.Circle);
        }
    }
}

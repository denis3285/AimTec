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
using Aimtec.SDK.Prediction.Collision;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Support_AIO;
using Support_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Zyra : Champion
    {

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
            bool useR = RootMenu["combo"]["user"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;

            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {

                return;
            }
            switch (RootMenu["combo"]["mode"].As<MenuList>().Value)
            {
                case 0:
                {
                    if (target.IsValidTarget(W.Range) && useW && (Q.Ready || E.Ready))
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
                            if (Q.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }
                    if (target.IsValidTarget(R.Range) && target != null && useR)
                    {
                        switch (RootMenu["combo"]["rusage"].As<MenuList>().Value)
                        {
                            case 0:
                                if (RootMenu["combo"]["rhit"].As<MenuSlider>().Value <=
                                    target.CountEnemyHeroesInRange(500))
                                {

                                    R.Cast(target);
                                }
                                break;

                            case 1:
                                if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.Cast(target);
                                }
                                break;

                        }
                    }
                    if (target.IsValidTarget(E.Range) && useE)
                    {
                        if (target != null)
                        {
                            if (E.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }

                }
                    break;
                case 1:
                {
                    if (target.IsValidTarget(W.Range) && useW && (Q.Ready || E.Ready))
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
                            if (E.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }
                        if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            if (Q.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }
                    if (target.IsValidTarget(R.Range) && target != null && useR)
                    {
                        switch (RootMenu["combo"]["rusage"].As<MenuList>().Value)
                        {
                            case 0:
                                if (RootMenu["combo"]["rhit"].As<MenuSlider>().Value <=
                                    target.CountEnemyHeroesInRange(500))
                                {

                                    R.Cast(target);
                                }
                                break;

                            case 1:
                                if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.Cast(target);
                                }
                                break;

                        }
                    }
 
                }
                    break;
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
                if (target.IsValidTarget(R.Range) && target != null)
                {


                    R.Cast(target);


                }

            }
            if (RootMenu["misc"]["autoe"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(E.Range)))
                {

                    E.Cast(target);
                }

            }

        }

        protected override void Farming()
        {
        }

        protected override void LastHit()
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
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
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
                                (float) (barPos.X + (unit.Health >
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E) +
                                                          Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }
            if (RootMenu["drawings"]["drawseed"].Enabled)
            {
                foreach (var seed in GameObjects.AllGameObjects)
                {
                    if (seed.Name.Contains("Zyra_Base_W_Seed") && !seed.IsDead && seed.IsValid &&
                        seed.Distance(Player) < 2000 && seed != null)
                    {
                        Render.Circle(seed.ServerPosition, 100, 40, Color.Wheat);
                    }
                }
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
                bool useE = RootMenu["harass"]["usee"].Enabled;

                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                if (target.IsValidTarget(W.Range) && useW && (Q.Ready || E.Ready))
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
                        if (Q.Cast(target))
                        {
                            if (target.IsValidTarget(W.Range) && useW)
                            {
                                W.Cast(target);
                            }
                        }
                    }
                }
                if (target.IsValidTarget(E.Range) && useE)
                {
                    if (target != null)
                    {
                        if (E.Cast(target))
                        {
                            if (target.IsValidTarget(W.Range) && useW)
                            {
                                W.Cast(target);
                            }
                        }
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
                ComboMenu.Add(new MenuList("mode", "Combo Usage", new[] { "Q > W > R > E > W", "E > W > R > Q > W" }, 0));
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("rusage", "R Usage", new[] { "If Hits X Enemies", "If Killable with Combo" }, 0));
                ComboMenu.Add(new MenuSlider("rhit", "If X Enemies <= (If Hits X Enemies Mode)", 2, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);


            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
            }
            RootMenu.Add(HarassMenu);
            
            KillstealMenu = new Menu("misc", "Misc.");
            {
                KillstealMenu.Add(new MenuBool("autoe", "Auto E on CC"));
            }
            RootMenu.Add(KillstealMenu);

            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawseed", "Draw Seeds"));
            }
            RootMenu.Add(DrawMenu);


            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Attach();
        }
        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

                if (target != null && Args.EndPosition.Distance(Player) < E.Range)
                {
                    E.Cast(Args.EndPosition);
                }
            
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 800);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 850);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1000);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 700);
            Q.SetSkillshot(0.85f, 140f, 3000, false, SkillshotType.Line, false, HitChance.None);
            W.SetSkillshot(0.25f, 150, 1000f, false, SkillshotType.Circle, false, HitChance.None);
            E.SetSkillshot(0.25f, 60, 1300f,  false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(0.3f, 60, 1000f, false, SkillshotType.Circle);
        }
    }
}

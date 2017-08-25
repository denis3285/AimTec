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
    class Ivern : Champion
    {
        private int meowdelay;
        private int hmmm;

        private void RFollow()
        {
            if (Player.HasBuff("IvernRRecast"))
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(1000);
                if (target.IsValidTarget())
                {
                    if (target.IsValidTarget(1000) && target != null && !target.IsDead)
                    {

                        R.Cast(target.Position);
                    }
                }
                if (Player.CountEnemyHeroesInRange(1000) == 0 && hmmm < Game.TickCount)
                {

                    var teddybear = ObjectManager.Get<GameObject>()
                        .Where(d => d.IsValid && !d.IsDead && d.Distance(Player) <= 1000 &&
                                    d.Name == "IvernMinion");
                    {
                        foreach (var hello in teddybear)
                        {
                            


                            if (R.Cast(Player.ServerPosition.Extend(hello.ServerPosition, 150)))
                            {
                                hmmm = 300 + Game.TickCount;
                            }


                        }
                    }
                }
            }
        }

        protected override void Combo()
        {
            if (RootMenu["combo"]["useq"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target != null)
                    {
                        if (!Player.HasBuff("ivernqallyjump") && target.IsValidTarget(Q.Range))
                        {
                            Q.Cast(target);
                        }
                        if (RootMenu["combo"]["gapq"].Enabled)
                        {
                            if (Player.HasBuff("ivernqallyjump"))
                            {
                                switch (RootMenu["combo"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                }
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 1:
                                        if (target.HasBuff("IvernQ") && target.CountEnemyHeroesInRange(600) == 1)
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            if (RootMenu["combo"]["usew"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target != null)
                    {
                        if (target.IsValidTarget(W.Range) && meowdelay < Game.TickCount)
                        {
                            if (target.Distance(Player) > 300 && target.Distance(Player) < 500 &&
                                !Player.HasBuff("ivernwpassive"))
                            {
                                if (W.Cast(Player.ServerPosition.Extend(target.ServerPosition, 150f)))
                                {
                                    meowdelay = 500 + Game.TickCount;
                                }
                            }
                        }

                    }
                }
            }
            if (RootMenu["combo"]["usee"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsValidTarget(E.Range, true) && x.Distance(Player) < W.Range && !x.IsDead))
                {
                    foreach (var targeto in GameObjects.EnemyHeroes.Where(
                        x => x.Distance(ally) < 380 && x != null && !x.IsDead).OrderByDescending(x => x.Distance(ally)))
                    {
                        E.CastOnUnit(ally);
                    }

                }
                var teddybear = ObjectManager.Get<GameObject>()
                    .Where(d => d.IsValid && !d.IsDead && d.Distance(Player) <= E.Range &&
                                d.Name == "IvernMinion");
                {
                    foreach (var hello in teddybear)
                    {


                        foreach (var targeto in GameObjects.EnemyHeroes.Where(
                            x => x.Distance(hello) < 380 && x != null && !x.IsDead))
                        {
                            E.CastOnUnit(hello);
                        }

                    }
                }
            }
            if (RootMenu["combo"]["user"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target != null && target.IsValidTarget(E.Range))
                    {
                        switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (target.HealthPercent() < 50)
                                {
                                    R.Cast(target);
                                }
                                break;
                            case 1:
                                if (Player.CountEnemyHeroesInRange(E.Range) >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                                {
                                    R.Cast(target);
                                }
                                break;
                        }
                    }
                }
            }
        }

        protected override void SemiR()
        {
            RFollow();

            if (RootMenu["misc"]["autoq"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(Q.Range)))
                {
                    if (!Player.HasBuff("ivernqallyjump"))
                    {
                        Q.Cast(target);
                    }
                }

            }
        }


        protected override void Farming()
        {
            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (!minion.IsValidTarget() && minion != null)
                    {
                        return;
                    }
                    if (RootMenu["farming"]["lane"]["usee"].Enabled)
                    {
                        if (Extensions.GetEnemyLaneMinionsTargetsInRange(380).Count >= RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                        {
                            E.Cast();
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
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {



            if (RootMenu["harass"]["useq"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target != null)
                    {
                        if (!Player.HasBuff("ivernqallyjump") && target.IsValidTarget(Q.Range))
                        {
                            Q.Cast(target);
                        }
                        if (RootMenu["harass"]["gapq"].Enabled)
                        {
                            if (Player.HasBuff("ivernqallyjump"))
                            {
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        Q.Cast(target);
                                        break;
                                }
                                switch (RootMenu["harass"]["qmode"].As<MenuList>().Value)
                                {
                                    case 1:
                                        if (target.HasBuff("IvernQ") && target.CountEnemyHeroesInRange(600) == 1)
                                        {
                                            Q.Cast(target);
                                        }
                                        break;
                                }
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("gapq", "^- Use Q2 to GapClose"));
                ComboMenu.Add(new MenuList("qmode", "Q2 Usage", new[] { "Always", "Only in 1v1" }, 1));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E for Damage"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("rmode", "R Usage", new[] { "If Killable", "If X Enemies in Range" }, 0));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X", 2, 1, 5));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("gapq", "^- Use Q2 to GapClose"));
                HarassMenu.Add(new MenuList("qmode", "Q2 Usage", new[] {"Always", "Only in 1v1"}, 1));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if Hits X", 3, 1, 6));
            }

            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
    

            }
            RootMenu.Add(DrawMenu);
            var MiscMenu = new Menu("misc", "Misc.");
            {
                MiscMenu.Add(new MenuBool("autoq", "Auto Q on CC'd"));

            }
            RootMenu.Add(MiscMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");

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

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                if (Player.HasBuff("ivernqallyjump"))
                {
                    Q.Cast(Args.EndPosition);
                }
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1075);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 850);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 750);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.3f, 87, 1300, true, SkillshotType.Line);

        }
    }
}

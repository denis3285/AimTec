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
    class Annie : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;

            bool useR = RootMenu["combo"]["user"].Enabled;
            if (useR)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget() && target != null)
                {

                    if (target.IsValidTarget(R.Range) &&
                        target.Health >= RootMenu["combo"]["wasteR"].As<MenuSlider>().Value)
                    {

                        if (target.CountEnemyHeroesInRange(290) >=
                            RootMenu["combo"]["hitsr"].As<MenuSlider>().Value)
                        {
                            if (!RootMenu["blacklist"][target.ChampionName.ToLower()].Enabled)
                            {
                                R.Cast(target);
                            }
                        }
                        if (RootMenu["combo"]["kill"].Enabled)
                        {
                            if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) +
                                Player.GetSpellDamage(target, SpellSlot.W) + Player.GetSpellDamage(target, SpellSlot.R))
                            {
                                if (!RootMenu["blacklist"][target.ChampionName.ToLower()].Enabled)
                                {
                                    R.Cast(target);
                                }
                            }
                        }
                    }

                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(W.Range))
                    {

                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }

                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range))
                    {

                        if (target != null)
                        {
                            Q.CastOnUnit(target);
                        }
                    }

                }
            }

        }


        protected override void SemiR()
        {
            if (RootMenu["combo"]["flashr"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range + 380);

                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value <= target.CountEnemyHeroesInRange(290))
                {
                    if (R.Ready)
                    {
                        if (Flash.Ready && Flash != null && target.IsValidTarget() && target != null)
                        {
                            if (target.Distance(Player) < R.Range + 380)
                            {
                                if (!RootMenu["blacklist"][target.ChampionName.ToLower()].Enabled)
                                {

                                    var meowmeow = R.GetPrediction(target);
                                    if (R.Cast(meowmeow.CastPosition))
                                    {
                                        Flash.Cast(target.ServerPosition);
                                    }
                                }
                            }
                        }

                    }
                }
            }

            RFollow();
            if (RootMenu["stack"].Enabled)
            {

                if (!Player.HasBuff("pyromania_particle") && !Player.IsRecalling() &&
                    RootMenu["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                {
                    E.Cast();
                }
            }
        }

        protected override void Farming()
        {
            if (!RootMenu["stack"].Enabled)
            {


                if (!RootMenu["combo"]["support"].Enabled)
                {

                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (!minion.IsValidTarget())
                            {
                                return;
                            }

                            if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    Q.CastOnUnit(minion);
                                }
                                if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                }
                            }
                            if (RootMenu["farming"]["lane"]["usew"].Enabled)
                            {
                                if (minion.IsValidTarget(W.Range) && minion != null)
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                            minion.ServerPosition)) >=
                                        RootMenu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                    {
                                        W.Cast(minion);
                                    }
                                }
                            }
                        }
                    }

                    if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle
                            .Where(m => m.IsValidTarget(Q.Range)).ToList())
                        {
                            if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                return;
                            }
                            bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                            bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.CastOnUnit(jungleTarget);
                                }
                            }
                            if (useW)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    W.Cast(jungleTarget.ServerPosition);
                                }
                            }
                        }
                    }
                }

                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (!minion.IsValidTarget())
                            {
                                return;
                            }

                            if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    Q.CastOnUnit(minion);
                                }
                                if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                }
                            }
                            if (RootMenu["farming"]["lane"]["usew"].Enabled)
                            {
                                if (minion.IsValidTarget(W.Range) && minion != null)
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                            minion.ServerPosition)) >=
                                        RootMenu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                    {
                                        W.Cast(minion);
                                    }
                                }
                            }
                        }
                    }
                    if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle
                            .Where(m => m.IsValidTarget(Q.Range)).ToList())
                        {
                            if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                return;
                            }
                            bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                            bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.CastOnUnit(jungleTarget);
                                }
                            }
                            if (useW)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    W.Cast(jungleTarget.ServerPosition);
                                }
                            }
                        }
                    }
                }
            }
            if (RootMenu["stack"].Enabled && !Player.HasBuff("pyromania_particle"))
            {


                if (!RootMenu["combo"]["support"].Enabled)
                {

                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (!minion.IsValidTarget())
                            {
                                return;
                            }

                            if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    Q.CastOnUnit(minion);
                                }
                                if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                }
                            }
                            if (RootMenu["farming"]["lane"]["usew"].Enabled)
                            {
                                if (minion.IsValidTarget(W.Range) && minion != null)
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                            minion.ServerPosition)) >=
                                        RootMenu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                    {
                                        W.Cast(minion);
                                    }
                                }
                            }
                        }
                    }

                    if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle
                            .Where(m => m.IsValidTarget(Q.Range)).ToList())
                        {
                            if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                return;
                            }
                            bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                            bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.CastOnUnit(jungleTarget);
                                }
                            }
                            if (useW)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    W.Cast(jungleTarget.ServerPosition);
                                }
                            }
                        }
                    }
                }

                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (!minion.IsValidTarget())
                            {
                                return;
                            }

                            if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                            {


                                if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    Q.CastOnUnit(minion);
                                }
                                if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["lastq"].Enabled)
                                {
                                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                }
                            }
                            if (RootMenu["farming"]["lane"]["usew"].Enabled)
                            {
                                if (minion.IsValidTarget(W.Range) && minion != null)
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                            minion.ServerPosition)) >=
                                        RootMenu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                    {
                                        W.Cast(minion);
                                    }
                                }
                            }
                        }
                    }
                    if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle
                            .Where(m => m.IsValidTarget(Q.Range)).ToList())
                        {
                            if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                return;
                            }
                            bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                            bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.CastOnUnit(jungleTarget);
                                }
                            }
                            if (useW)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    W.Cast(jungleTarget.ServerPosition);
                                }
                            }
                        }
                    }
                }
            }
        }
        


        protected override void LastHit()
        {
            if (!RootMenu["stack"].Enabled)
            {


                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (RootMenu["farming"]["lane"]["lastqs"].Enabled)
                    {

                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                            {

                                Q.CastOnUnit(minion);

                            }
                        }
                    }
                }
                if (!RootMenu["combo"]["support"].Enabled)
                {
                    if (RootMenu["farming"]["lane"]["lastqs"].Enabled)
                    {

                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                            {

                                Q.CastOnUnit(minion);

                            }
                        }
                    }
                }
            }
            if (RootMenu["stack"].Enabled && !Player.HasBuff("pyromania_particle"))
            {


                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (RootMenu["farming"]["lane"]["lastqs"].Enabled)
                    {

                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                            {

                                Q.CastOnUnit(minion);

                            }
                        }
                    }
                }
                if (!RootMenu["combo"]["support"].Enabled)
                {
                    if (RootMenu["farming"]["lane"]["lastqs"].Enabled)
                    {

                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                            {

                                Q.CastOnUnit(minion);

                            }
                        }
                    }
                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;

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
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;
            if (RootMenu["drawings"]["drawstack"].Enabled)
            {
                if (RootMenu["stack"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.GreenYellow, "SAVE STACKS: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!RootMenu["stack"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.Red, "SAVE STACKS: OFF",
                        RenderTextFlags.VerticalCenter);
                }
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawrf"].Enabled)
            {
                Render.Circle(Player.Position, R.Range + 380, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
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
                                (float) (barPos.X + (unit.Health >
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +

                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.W) +

                                                                        Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +

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
                    if (e.Target.IsMinion &&
                        GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() > 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
            if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
            {
                if (Player.Mana > 100 && RootMenu["combo"]["disableAA"].Enabled)
                {
                    if (e.Target.IsHero)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void RFollow()
        {
            if (Player.HasBuff("infernalguardiantimer"))
            {

                if (RootMenu["combo"]["control"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(1500);
                    if (target.IsValidTarget())
                    {
                        if (target.IsValidTarget(1500) && target != null && !target.IsDead)
                        {
                            var teddybear = ObjectManager.Get<GameObject>()
                                .Where(d => d.IsValid && !d.IsDead  &&
                                            d.Name == "Tibbers");
                            {
                                foreach (var hello in teddybear)
                                {

                                    if (target.Distance(hello.ServerPosition) > 200)
                                    {


                                        R.Cast(target.Position);
                                    }
                                    
                                }
                            }
                        }
                    }
                    if (Player.CountEnemyHeroesInRange(1500) == 0 && hmmm < Game.TickCount)
                    {


                        var teddybear = ObjectManager.Get<GameObject>()
                            .Where(d => d.IsValid && !d.IsDead &&
                                        d.Name == "Tibbers");
                        {
                            foreach (var hello in teddybear)
                            {
                                if (R.Cast(Player.ServerPosition.Extend(hello.ServerPosition, 150)))
                                {
                                    hmmm = 200 + Game.TickCount;
                                }
                            }
                        }




                    }
                }
            }
        }

    

    protected override void Killsteal()
        {
            if (!RootMenu["combo"]["support"].Enabled)
            {
                if (Q.Ready &&
                    RootMenu["ks"]["ksq"].Enabled)
                {
                    var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(Q.Range))
                    {
                        Q.CastOnUnit(bestTarget);
                    }
                }
                if (W.Ready &&
                    RootMenu["ks"]["ksw"].Enabled)
                {
                    var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(W.Range))
                    {
                        W.Cast(bestTarget);
                    }
                }
            }
        }

        protected override void Harass()
        {
            if (!RootMenu["stack"].Enabled)
            {
                if (!RootMenu["combo"]["support"].Enabled)
                {
                    if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
                    {
                        bool useQ = RootMenu["harass"]["useq"].Enabled;
                        bool useW = RootMenu["harass"]["usew"].Enabled;

                        if (useW)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        W.Cast(target);
                                    }
                                }

                            }
                        }
                        if (useQ)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        Q.CastOnUnit(target);
                                    }
                                }

                            }
                        }

                    }

                }

                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
                    {
                        bool useQ = RootMenu["harass"]["useq"].Enabled;
                        bool useW = RootMenu["harass"]["usew"].Enabled;

                        if (useW)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        W.Cast(target);
                                    }
                                }

                            }
                        }
                        if (useQ)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        Q.CastOnUnit(target);
                                    }
                                }

                            }
                        }

                    }
                }
            }
            if (RootMenu["stack"].Enabled && !Player.HasBuff("pyromania_particle"))
            {

                if (RootMenu["combo"]["support"].Enabled &&
                    GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() == 0)
                {
                    if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
                    {
                        bool useQ = RootMenu["harass"]["useq"].Enabled;
                        bool useW = RootMenu["harass"]["usew"].Enabled;

                        if (useW)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        W.Cast(target);
                                    }
                                }

                            }
                        }
                        if (useQ)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        Q.CastOnUnit(target);
                                    }
                                }

                            }
                        }

                    }
                }
                if (!RootMenu["combo"]["support"].Enabled)
                {
                    if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
                    {
                        bool useQ = RootMenu["harass"]["useq"].Enabled;
                        bool useW = RootMenu["harass"]["usew"].Enabled;

                        if (useW)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        W.Cast(target);
                                    }
                                }

                            }
                        }
                        if (useQ)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target.IsValidTarget(Q.Range))
                                {

                                    if (target != null)
                                    {
                                        Q.CastOnUnit(target);
                                    }
                                }

                            }
                        }

                    }

                }

            }
        }

        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }
            var ally = args.Target as Obj_AI_Hero;
            var target = args.Sender as Obj_AI_Hero;
            if (ally != null && target != null)
            {

                if (RootMenu["combo"]["usee"].Enabled && args.SpellData.Name.Contains("BasicAttack"))
                {
                    if (ally.IsHero && ally.IsMe && target.IsHero)
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
            RootMenu.Add(new MenuKeyBind("stack", "Save Stacks Toggle", KeyCode.T, KeybindType.Toggle));
            RootMenu.Add(new MenuSlider("mana", "^- Mana Manager", 50));
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Auto E on Enemy Auto Attacks"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitsr", "If Hits X Enemies", 3, 0, 5));
                ComboMenu.Add(new MenuBool("kill", "Use R in 1v1 if Killable"));
                ComboMenu.Add(new MenuSlider("wasteR", "^- Dont waste R if Enemy HP lower than", 100, 0, 300));
                
                ComboMenu.Add(new MenuKeyBind("flashr", "Flash-R Key", KeyCode.G, KeybindType.Press));
                ComboMenu.Add(new MenuSlider("hitr", "^- If Hits X Enemies", 3, 0, 5));
                ComboMenu.Add(new MenuBool("disableAA", "Disable AA", false));
                ComboMenu.Add(new MenuBool("control", "Auto Bear Control"));
                ComboMenu.Add(new MenuBool("support", "Support Mode", false));
            }
            RootMenu.Add(ComboMenu);
            var BlackList = new Menu("blacklist", "R Blacklist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                }
            }
            RootMenu.Add(BlackList);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));

                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));

            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("lastqs", "Use Q in Last Hit"));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("lastq", "^- Only for Last Hit"));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitw", "^- if Hits X", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killseal with Q"));
                KillstealMenu.Add(new MenuBool("ksw", "Killseal with W"));

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawrf", "Draw Flash - R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawstack", "Draw Stack Toggle"));
            }
           
     
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 625);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 580);
            E = new Aimtec.SDK.Spell(SpellSlot.E);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 600);
            W.SetSkillshot(0.5f, 150, float.MaxValue, false, SkillshotType.Cone);
            R.SetSkillshot(0.2f, 85, float.MaxValue, false, SkillshotType.Circle);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);
        }
    }
}

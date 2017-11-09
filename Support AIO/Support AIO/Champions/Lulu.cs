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
    class Lulu : Champion
    {
        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            var attack = sender as Obj_AI_Hero;
            var target = args.Target as Obj_AI_Hero;
            if (attack != null && attack.IsAlly && attack.IsHero)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["combo"]["wset"]["ally"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>()
                                 .Value != 0)
                    .OrderByDescending(
                        x => RootMenu["combo"]["wset"]["ally"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>()
                            .Value))
                {

                    if (target != null && args.SpellData.Name.Contains("BasicAttack") &&
                        attack.Distance(Player) < E.Range && !attack.IsDead && attack == ally)
                    {
                        W.CastOnUnit(attack);
                    }
                }
            }

        }


        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;
            bool useEQ = RootMenu["combo"]["useeq"].Enabled;
            if (useEQ)
            {

                if (Q.Ready)
                {
                    foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (ally != null && !ally.IsMe)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, ally.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {

                                if (ally.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(ally);
                                }
                            }
                        }
                    }
                    foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (minion != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, minion.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (minion.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                    foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (target != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, target.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (target.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }
                    }
                }


                foreach (var pixs in GameObjects.AllGameObjects)
                {

                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {

                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")) && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                    }


                }
            }
            if (E.Ready)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range) && target != null)
                {

                    if (target != null)
                    {
                        switch (RootMenu["combo"]["emode"].As<MenuList>().Value)
                        {
                            case 0:
                                E.CastOnUnit(target);
                                break;
                            case 1:
                              
                                if (Player.CountAllyHeroesInRange(E.Range) == 0 ||
                                    target.HealthPercent() < 5 && Player.HealthPercent() > 20)
                                {
                                    E.CastOnUnit(target);
                                }
                                break;
                            case 2:

                                break;
                        }
                    }


                }
            }
            if (useR)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(R.Range, true)))
                {
                    if (ally != null)
                    {
                        if (ally.HealthPercent() <= RootMenu["combo"]["hp"].As<MenuSlider>().Value)
                        {
                            if (ally.CountEnemyHeroesInRange(350) >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                            {
                                R.CastOnUnit(ally);
                            }
                        }
                    }
                }

            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var enemy in GameObjects.EnemyHeroes.Where(
                                x => x.Distance(Player) <= W.Range &&
                                     RootMenu["combo"]["wset"]["enemy"][x.ChampionName.ToLower() + "priority"]
                                         .As<MenuSlider>()
                                         .Value != 0)
                            .OrderByDescending(
                                x => RootMenu["combo"]["wset"]["enemy"][x.ChampionName.ToLower() + "priority"]
                                    .As<MenuSlider>()
                                    .Value))
                        {
                            W.CastOnUnit(enemy);
                        }
                    }


                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget(Q.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var pixs in GameObjects.AllGameObjects)
                        {
                            if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                            {
                                if (pixs.Distance(target) < Player.Distance(target))
                                {
                                    Q.From = pixs.ServerPosition;
                                    Q.Cast(target);
                                }
                                if (pixs.Distance(target) > Player.Distance(target))
                                {
                                    Q.From = Player.ServerPosition;
                                    Q.Cast(target);
                                }
                            }
                        }
                    }
                }
            }
            
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["semir"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x=> x.IsValidTarget(R.Range, true) && !x.IsDead && x != null).OrderBy(x=> x.Health))

                {
                    R.CastOnUnit(ally);
                }
            }
            if (RootMenu["flee"]["fleekey"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (W.Ready)
                {
                    W.Cast();
                }
            }
            if (RootMenu["we"]["key"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["we"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value != 0)
                    .OrderByDescending(
                        x => RootMenu["we"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value))
                {
                    W.CastOnUnit(ally);
                    E.CastOnUnit(ally);
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
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawpix"].Enabled)
            {
                foreach (var pix in GameObjects.AllGameObjects)
                {
                    if (pix.Name == "RobotBuddy" && pix.IsValid && pix != null && !pix.IsDead && pix.Team == Player.Team)
                    {
                        Render.Circle(pix.ServerPosition, 60, 40, Color.HotPink);
                    }
                }

            }
            if (RootMenu["drawings"]["pixranges"].Enabled)
            {
                foreach (var pixs in GameObjects.AllGameObjects)
                {
                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                pix.HasBuff("lulufaerieattackaid"))
                            {
                                Render.Circle(pixs.ServerPosition, Q.Range, 40, Color.GreenYellow);
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                pix.HasBuff("lulufaerieburn"))
                            {
                                Render.Circle(pixs.ServerPosition, Q.Range, 40, Color.GreenYellow);
                            }
                        }

                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                 pix.HasBuff("luluevision")))
                            {

                                Render.Circle(pixs.ServerPosition, Q.Range, 40, Color.GreenYellow);
                            }
                        }
                    }
                }
            }
            if (RootMenu["drawings"]["damage"].Enabled)
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
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.E))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }

        protected override void Killsteal()
        {
            if (Q.Ready &&
                RootMenu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    foreach (var pixs in GameObjects.AllGameObjects)
                    {
                        if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                        {
                            if (pixs.Distance(bestTarget) < Player.Distance(bestTarget))
                            {
                                Q.From = pixs.ServerPosition;
                                Q.Cast(bestTarget);
                            }
                            if (pixs.Distance(bestTarget) > Player.Distance(bestTarget))
                            {
                                Q.From = Player.ServerPosition;
                                Q.Cast(bestTarget);
                            }
                        }
                    }
                }
            }
            if (E.Ready &&
                RootMenu["killsteal"]["kse"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(bestTarget);
                }
            }
            if (Q.Ready && RootMenu["killsteal"]["kseq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHeroEQ(DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range + Q.Range) && bestTarget.Distance(Player) > Q.Range)
                {
                    foreach (var ally in GameObjects.AllyHeroes
                        .Where(x => x.IsValidTarget(E.Range, true) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(ally);
                    }
                    foreach (var minion in GameObjects.Minions
                        .Where(x => x.IsValidTarget(E.Range) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(minion);
                    }
                    foreach (var enemy in GameObjects.EnemyHeroes
                        .Where(x => x.IsValidTarget(E.Range) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(enemy);
                    }
                    foreach (var pixs in GameObjects.AllGameObjects)
                    {
                        if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                        {
                            foreach (var pix in GameObjects.AllyHeroes)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(bestTarget) < Q.Range)
                                {
                                    Q.From = pix.ServerPosition;
                                    Q.Cast(bestTarget);
                                }
                            }
                            foreach (var pix in GameObjects.EnemyHeroes)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(bestTarget) < Q.Range)
                                {
                                    Q.From = pix.ServerPosition;
                                    Q.Cast(bestTarget);
                                }
                            }
                            foreach (var pix in GameObjects.EnemyMinions)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")))
                                {

                                    Q.From = pix.ServerPosition;
                                    Q.Cast(bestTarget);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void Harass()
        {
            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useE = RootMenu["harass"]["usee"].Enabled;

            bool useEQ = RootMenu["harass"]["useeq"].Enabled;
            if (useEQ)
            {

                if (Q.Ready)
                {
                    foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (ally != null && !ally.IsMe)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, ally.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (ally.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(ally);
                                }
                            }
                        }
                    }
                    foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (minion != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, minion.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (minion.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                    foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (target != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, target.ServerPosition));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (target.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }
                    }
                }


                foreach (var pixs in GameObjects.AllGameObjects)
                {

                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {

                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, false,
                                    false, pix.ServerPosition));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")) && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.ServerPosition;
                                    Q.Cast(enemyInBounceRange);

                                }
                            }
                        }
                    }


                }
            }
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range) && target != null)
                {

                    if (target != null)
                    {
                        E.CastOnUnit(target);
                    }
                }


            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget(Q.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var pixs in GameObjects.AllGameObjects)
                        {
                            if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                            {
                                if (pixs.Distance(target) < Player.Distance(target))
                                {
                                    Q.From = pixs.ServerPosition;
                                    Q.Cast(target);
                                }
                                if (pixs.Distance(target) > Player.Distance(target))
                                {
                                    Q.From = Player.ServerPosition;
                                    Q.Cast(target);
                                }
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
                            .Where(x => UnitExtensions.Distance(x, Player) < 1000).Count() > 1)
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
                ComboMenu.Add(new MenuBool("useeq", "Use E > Q Extended in Combo", false));
                var WSettings = new Menu("wset", "W Settings");
                WSettings.Add(new MenuBool("usew", "Use W in Combo"));
                var EnemySet = new Menu("enemy", "Enemy Settings");
                EnemySet.Add(new MenuSeperator("meow", "0 - Disabled"));
                EnemySet.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                foreach (var target in GameObjects.EnemyHeroes)
                {

                    EnemySet.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));

                }
                var AllySet = new Menu("ally", "Ally Settings");
                AllySet.Add(new MenuSeperator("meow", "0 - Disabled"));
                AllySet.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                foreach (var target in GameObjects.AllyHeroes)
                {

                    AllySet.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));

                }
                ComboMenu.Add(WSettings);
                WSettings.Add(EnemySet);
                WSettings.Add(AllySet);
                
                ComboMenu.Add(new MenuList("emode", "E Mode on Enemy", new[] { "Always", "Logic", "Never"}, 1));
                var RSettings = new Menu("rset", "R Settings");
                RSettings.Add(new MenuBool("user", "Use R in Combo"));
                RSettings.Add(new MenuSlider("hitr", "^- if Knocks Up X Enemies", 2, 0, 5));
                RSettings.Add(new MenuSlider("hp", "^- if Ally is Lower than X Health", 20, 0, 100));
                RSettings.Add(new MenuBool("autor", "Auto R if Incoming Damage will Kill"));
                RSettings.Add(new MenuKeyBind("semir", "Semi-R on Lowest Health Ally", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(RSettings);
                ComboMenu.Add(new MenuBool("support", "Support Mode", false));

            }

            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 30, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Harass with Q"));
                HarassMenu.Add(new MenuBool("usee", "Harass with E"));
                HarassMenu.Add(new MenuBool("useeq", "Harass with E > Q Extended"));

            }
            RootMenu.Add(HarassMenu);
            var WE = new Menu("we", "W > E Settings");
            WE.Add(new MenuKeyBind("key", "W > E Key", KeyCode.Z, KeybindType.Press));

            WE.Add(new MenuSeperator("meow", "0 - Disabled"));
            WE.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
            foreach (var target in GameObjects.AllyHeroes)
            {

                WE.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));

            }
            RootMenu.Add(WE);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawpix", "Draw Pix Position"));
                DrawMenu.Add(new MenuBool("pixranges", "Draw Ranges from Pix"));
                DrawMenu.Add(new MenuBool("damage", "Draw damages"));
            }
            RootMenu.Add(DrawMenu);
            Gapcloser.Attach(RootMenu, "W Anti-Gap");

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
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KillstealMenu.Add(new MenuBool("kse", "Killsteal with E"));
                KillstealMenu.Add(new MenuBool("kseq", "Killsteal with E > Q"));
            }
            RootMenu.Add(KillstealMenu);
            FarmMenu = new Menu("flee", "Flee");
            {
                FarmMenu.Add(new MenuKeyBind("fleekey", "Fleey Key", KeyCode.G, KeybindType.Press));

            }
            RootMenu.Add(FarmMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
         

                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    W.CastOnUnit(target);
                }
            
        }


        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 875);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 650);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 650);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 900);
            Q.SetSkillshot(0.25f, 60, 1400, false, SkillshotType.Line, false, HitChance.None);
        }
    }
}

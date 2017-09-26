using System.CodeDom;
using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Damage.JSON;

namespace Gnar_By_Kornis
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.TargetSelector;
    using Aimtec.SDK.Util.Cache;
    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.Util;


    using Spell = Aimtec.SDK.Spell;

    internal class Gnar
    {
        public static Menu Menu = new Menu("Gnar By Kornis", "Gnar by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, Q2, W, E, E2, R;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            Q2 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 500);
            E = new Spell(SpellSlot.E, 475);
            E2 = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 480);
            Q.SetSkillshot(0.25f, 60, 1400, true, SkillshotType.Line);
            Q2.SetSkillshot(0.25f, 60, 1200, true, SkillshotType.Line, false, HitChance.None);
            W.SetSkillshot(0.25f, 80, 1200, false, SkillshotType.Line, false, HitChance.None);
            E.SetSkillshot(0.5f, 150, float.MaxValue, false, SkillshotType.Circle);
            E2.SetSkillshot(0.6f, 60, 1500, false, SkillshotType.Circle, false, HitChance.None);

        }

        public Gnar()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("block", "Block Spell Cast if Bar is RED"));
                var Mega = new Menu("mega", "Mega-Gnar Settings");
                Mega.Add(new MenuBool("useq", "Use Q in Combo"));
                Mega.Add(new MenuBool("usew", "Use W in Combo"));
                Mega.Add(new MenuBool("usee", "Use E with Logic"));
                Mega.Add(new MenuBool("user", "Use R in Combo"));
                Mega.Add(new MenuList("rmode", "R Mode:", new[] {"At X Health", "If Killable with Combo"}, 0));
                Mega.Add(new MenuBool("autor", "Auto R"));
                Mega.Add(new MenuSlider("hitr", "^- Only if Hits X", 3, 1, 5));
                Mega.Add(new MenuSlider("hp", "At X Health (Health Mode)", 50, 0, 100));
                ComboMenu.Add(Mega);
                var Mini = new Menu("mini", "Mini-Gnar Settings");
                Mini.Add(new MenuBool("useq", "Use Q in Combo"));
                Mini.Add(new MenuBool("usee", "Use E with Logic"));
                Mini.Add(new MenuSlider("suicidallikeme", "Don't jump in X Enemies", 3, 0, 5));
                Mini.Add(new MenuBool("usegap", "^- Use E to GapClose if Killable"));
                ComboMenu.Add(Mini);
            }

            Menu.Add(ComboMenu);

            var HarassMenu = new Menu("harass", "Harass");
            {
                var Mega = new Menu("mega", "Mega-Gnar Settings");
                Mega.Add(new MenuBool("useq", "Use Q in Combo"));
                Mega.Add(new MenuBool("usew", "Use W in Combo"));
                Mega.Add(new MenuBool("usee", "Use E with Logic"));
                var Mini = new Menu("mini", "Mini-Gnar Settings");
                Mini.Add(new MenuBool("useq", "Use Q in Combo"));
                Mini.Add(new MenuBool("usee", "Use E with Logic"));
                HarassMenu.Add(Mega);
                HarassMenu.Add(Mini);

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            {
                var Mega = new Menu("mega", "Mega-Gnar Settings");
                Mega.Add(new MenuBool("useq", "Use Q in Combo"));
                Mega.Add(new MenuBool("usew", "Use W in Combo"));
                Mega.Add(new MenuSlider("hitw", "^- Only if Hits X", 3, 1, 5));

                FarmMenu.Add(Mega);
                var Mini = new Menu("mini", "Mini-Gnar Settings");
                Mini.Add(new MenuBool("useq", "Use Q in Combo"));
                FarmMenu.Add(Mini);
            }
            Menu.Add(FarmMenu);
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q Mini/Mega"));
                KSMenu.Add(new MenuBool("ksw", "Killsteal with Mega W"));

            }
            Menu.Add(KSMenu);
            var MiscMenu = new Menu("misc", "Misc.");
            {
                MiscMenu.Add(new MenuKeyBind("toggle", "E Under-Turret Toggle", KeyCode.T, KeybindType.Toggle));
            }
            Menu.Add(MiscMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Menu.Add(DrawMenu);
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;

            LoadSpells();
            Console.WriteLine("Gnar by Kornis - Loaded");
        }

        public class LaneclearResult
        {
            public LaneclearResult(int hit, Vector3 cp)
            {
                this.numberOfMinionsHit = hit;
                this.CastPosition = cp;
            }

            public int numberOfMinionsHit = 0;
            public Vector3 CastPosition;
        }

        public LaneclearResult GetLineClearLocation(float range, float width)
        {
            var minions = ObjectManager.Get<Obj_AI_Base>()
                .Where(x => x.IsValidTarget(W.Range) && x.IsValidSpellTarget());

            var positions = minions.Select(x => x.ServerPosition).ToList();

            var locations = new List<Vector3>();

            locations.AddRange(positions);

            var max = positions.Count();

            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (positions[j] != positions[i])
                    {
                        locations.Add((positions[j] + positions[i]) / 2);
                    }
                }
            }


            HashSet<LaneclearResult> results = new HashSet<LaneclearResult>();

            foreach (var p in locations)
            {
                var rect = new Gnar_By_Kornis.Rectangle(Player.Position, p, width);

                var count = 0;

                foreach (var m in minions)
                {
                    if (rect.Contains(m.Position))
                    {
                        count++;
                    }
                }

                results.Add(new LaneclearResult(count, p));
            }

            var maxhit = results.MaxBy(x => x.numberOfMinionsHit);

            return maxhit;
        }


        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private Vector3 hello2;
        private Vector3 hello;

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }

        private void Render_OnPresent()
        {
            if (!Player.HasBuff("gnartransform"))
            {
                if (Menu["drawings"]["drawq"].Enabled)
                {
                    Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
                }

                if (Menu["drawings"]["drawe"].Enabled)
                {
                    Render.Circle(Player.Position, E.Range, 40, Color.Crimson);
                }
            }
            if (Player.HasBuff("gnartransform") || Player.HasBuff("gnartransformsoon"))
            {
                if (Menu["drawings"]["drawq"].Enabled)
                {
                    Render.Circle(Player.Position, Q2.Range, 40, Color.CornflowerBlue);
                }

                if (Menu["drawings"]["drawe"].Enabled)
                {
                    Render.Circle(Player.Position, E2.Range, 40, Color.Crimson);
                }
                if (Menu["drawings"]["draww"].Enabled)
                {
                    Render.Circle(Player.Position, W.Range, 40, Color.Crimson);
                }
                if (Menu["drawings"]["drawr"].Enabled)
                {
                    Render.Circle(Player.Position, R.Range, 40, Color.CornflowerBlue);
                }
            }
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;

            if (Menu["misc"]["toggle"].Enabled)
            {
                Render.Text(xaOffset - 60, yaOffset + 30, Color.Wheat, "Use Under-Turret: ON",
                    RenderTextFlags.VerticalCenter);
            }
            if (!Menu["misc"]["toggle"].Enabled)
            {
                Render.Text(xaOffset - 60, yaOffset + 30, Color.Wheat, "Use Under-Turret: OFF",
                    RenderTextFlags.VerticalCenter);
            }
            if (Menu["drawings"]["drawdamage"].Enabled)
            {
                double QDamage = 0;
                double WDamage = 0;
                double EDamage = 0;
                double RDamage = 0;

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range + R.Range))
                    .ToList()
                    .ForEach(
                        unit =>
                        { 
                            if (!Player.HasBuff("gnartransform"))
                            {
                                QDamage = Player.GetSpellDamage(unit, SpellSlot.Q);
                                EDamage = Player.GetSpellDamage(unit, SpellSlot.E);
                            }
                            if (Player.HasBuff("gnartransform") || Player.HasBuff("gnartransformsoon"))
                            { 
                                QDamage = Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.SecondForm);
                                EDamage = Player.GetSpellDamage(unit, SpellSlot.E, DamageStage.SecondForm);
                                WDamage = Player.GetSpellDamage(unit, SpellSlot.W, DamageStage.SecondForm);
                                RDamage = Player.GetSpellDamage(unit, SpellSlot.R, DamageStage.Collision);
                            }
                            var heroUnit = unit as Obj_AI_Hero;
                            int width = 103;
                            int height = 8;
                            int xOffset = SxOffset(heroUnit);
                            int yOffset = SyOffset(heroUnit);
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;
                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health >
                                                     QDamage + EDamage + WDamage + RDamage
                                             ? width * ((unit.Health - (QDamage + EDamage + WDamage + RDamage)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < QDamage + EDamage + WDamage + RDamage
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }
        }

        private void Game_OnUpdate()
        {
            if (Player.IsDead || MenuGUI.IsChatOpen())
            {
                return;
            }
            Killsteal();

            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    OnCombo();
                    break;
                case OrbwalkingMode.Mixed:
                    OnHarass();
                    break;
                case OrbwalkingMode.Laneclear:

                    Clearing();
                    Jungle();
                    break;

            }
            if (Menu["combo"]["mega"]["autor"].Enabled)
            {
                var target = GetBestEnemyHeroTargetInRange(R.Range);
                if (!target.IsValidTarget())
                {
                    return;
                }
                if (target == null)
                {
                    return;
                }
                if (target.IsValidTarget(520) && Player.CountEnemyHeroesInRange(470) >=
                    Menu["combo"]["mega"]["hitr"].As<MenuSlider>().Value)
                {

                    if (Player.Distance(target) <= 490)
                    {

                        var pushdistance = 500;
                        var targetpos = target.ServerPosition;
                        var pushidrection =
                            (targetpos - Player.ServerPosition.Extend(targetpos, pushdistance))
                            .Normalized();
                        var checkdistance = pushdistance / 40;
                        for (var i = 0; i <= 37; i++)
                        {

                            var finalpos = targetpos + (pushidrection * checkdistance * i);
                            if (IsWall(finalpos, true))
                            {

                                hello = finalpos;

                            }
                            else hello = new Vector3(0, 0, 0);
                        }
                        var pushdistance2 = 500;
                        var targetpos2 = target.ServerPosition;
                        var pushidrection2 = (targetpos2 - Player.ServerPosition).Normalized();
                        var checkdistance2 = pushdistance2 / 40;
                        for (var i = 0; i <= 37; i++)
                        {

                            var finalpos2 = targetpos2 + (pushidrection2 * checkdistance2 * i);
                            if (IsWall(finalpos2, true))
                            {
                                hello2 = finalpos2;

                            }
                            else hello2 = new Vector3(0, 0, 0);
                        }
                        if (!hello.IsZero && !hello2.IsZero)
                        {
                            if (hello.Distance(Player) > hello2.Distance(Player))
                            {
                                R.Cast(hello2);
                            }
                            if (hello.Distance(Player) < hello2.Distance(Player))
                            {
                                R.Cast(hello);
                            }
                        }
                        if (hello2.IsZero && !hello.IsZero)
                        {

                            R.Cast(hello);
                        }
                        if (hello.IsZero && !hello2.IsZero)
                        {

                            R.Cast(hello2);
                        }
                    }

                }

            }

        }

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidTarget(range)).ToList();
        }


        private void Clearing()
        {

            float hitW = Menu["farming"]["mega"]["hitw"].As<MenuSlider>().Value;

            if (!Player.HasBuff("gnartransform"))
            {
                if (Menu["farming"]["mini"]["useq"].Enabled)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {

                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            Q.Cast(minion);
                        }
                    }
                }
            }
            if (Player.HasBuff("gnartransform"))
            {
                if (Menu["farming"]["mega"]["useq"].Enabled)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q2.Range))
                    {

                        if (minion.IsValidTarget(Q2.Range) && minion != null)
                        {
                            Q2.Cast(minion);
                        }
                    }
                }
                if (Menu["farming"]["mega"]["usew"].Enabled)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {
                        var hits = GetLineClearLocation(W.Range, 100);
                        if (minion.IsValidTarget(W.Range) && minion != null && hits.numberOfMinionsHit >= hitW)


                        {
                            W.Cast(hits.CastPosition);
                        }
                    }
                }
            }
        }


        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.Jungle.Where(m => !GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range))
                .ToList();
        }

        private void Jungle()
        {
           
            foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range-300))
            {
             
                if (!Player.HasBuff("gnartransform"))
                {
                   
                    if (Menu["farming"]["mini"]["useq"].Enabled)
                    {

                        
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            Q.Cast(minion);
                        }

                    }
                }
                if (Player.HasBuff("gnartransform"))
                {
                    if (Menu["farming"]["mega"]["useq"].Enabled)
                    {

                        if (minion.IsValidTarget(Q2.Range) && minion != null)
                        {
                            Q2.Cast(minion);
                        }

                    }
                    if (Menu["farming"]["mega"]["usew"].Enabled)
                    {

                        var hits = GetLineClearLocation(W.Range, 100);
                        if (minion.IsValidTarget(W.Range) && minion != null)


                        {
                            W.Cast(hits.CastPosition);
                        }

                    }
                }
            }
        }



        public static Obj_AI_Hero GetBestKillableHero(Spell spell, DamageType damageType = DamageType.True,
            bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(spell.Range).FirstOrDefault(t => t.IsValidTarget());
        }

        public static Obj_AI_Hero GetEGAP(DamageType damageType = DamageType.True, bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(Q.Range + E.Range)
                .FirstOrDefault(t => t.IsValidTarget());
        }

        private void Killsteal()
        {
            if (!Player.HasBuff("gnartransform"))
            {
                if (Q.Ready &&
                    Menu["killsteal"]["ksq"].Enabled)
                {
                    var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);
                    if (bestTarget != null)
                    {
                        if (Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                            bestTarget.IsValidTarget(Q.Range))
                        {
                            var pred = Q.GetPrediction(bestTarget);
                            Q.Cast(bestTarget);
                            if (bestTarget.Health <= Player.GetSpellDamage(bestTarget, SpellSlot.Q) / 2)
                            {

                                for (var i = 0; i < 17; i++)
                                {
                                    ;

                                    var colliding = pred.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                                    var test = bestTarget.ServerPosition.Extend(Player.ServerPosition,
                                        bestTarget.Distance(Player) - 60 * i);

                                    if (colliding.Count > 0)
                                    {

                                        if (bestTarget.Distance(Player) > 700 && bestTarget.Distance(Player) < 1000)
                                        {



                                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + bestTarget.BoundingRadius &&
                                                        m.Distance(bestTarget) < 310 &&
                                                        colliding[0].Distance(bestTarget) < 310)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }
                                            }
                                            foreach (var m in GameObjects.Jungle.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + bestTarget.BoundingRadius &&
                                                        m.Distance(bestTarget) < 310 &&
                                                        colliding[0].Distance(bestTarget) < 310)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }
                                            }

                                        }
                                        if (bestTarget.Distance(Player) < 700 && bestTarget.Distance(Player) < 1000)
                                        {
                                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + bestTarget.BoundingRadius &&
                                                        m.Distance(bestTarget) < 350 &&
                                                        colliding[0].Distance(bestTarget) < 350)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }

                                            }
                                            foreach (var m in GameObjects.Jungle.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + bestTarget.BoundingRadius &&
                                                        m.Distance(bestTarget) < 350 &&
                                                        colliding[0].Distance(bestTarget) < 350)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Player.HasBuff("gnartransform") || Player.HasBuff("gnartransformsoon"))
            { 
                if (Q2.Ready &&
                    Menu["killsteal"]["ksq"].Enabled)
                {
                    var bestTarget = GetBestKillableHero(Q2, DamageType.Magical, false);
                    if (bestTarget != null)
                    {
                        if (Player.GetSpellDamage(bestTarget, SpellSlot.Q, DamageStage.SecondForm) >=
                            bestTarget.Health &&
                            bestTarget.IsValidTarget(Q2.Range))
                        {
                            var collisions =
                                (IList<Obj_AI_Base>)E.GetPrediction(bestTarget).CollisionObjects;
                            if (collisions.Any())
                            {
                                if (collisions.All(c => GetAllGenericUnitTargets().Contains(c)))
                                {
                                    return;
                                }
                            }
                            Q2.Cast(bestTarget);

                        }
                    }
                }
                if (W.Ready &&
                    Menu["killsteal"]["ksw"].Enabled)
                {
                    var bestTarget = GetBestKillableHero(W, DamageType.Magical, false);
                    if (bestTarget != null)
                    {
                        if (Player.GetSpellDamage(bestTarget, SpellSlot.W, DamageStage.SecondForm) >=
                            bestTarget.Health &&
                            bestTarget.IsValidTarget(W.Range))
                        {
                            W.Cast(bestTarget);
                        }
                    }
                }
            }
        }



        public static Obj_AI_Hero GetBestEnemyHeroTarget()
        {
            return GetBestEnemyHeroTargetInRange(float.MaxValue);
        }

        public static Obj_AI_Hero GetBestEnemyHeroTargetInRange(float range)
        {
            var ts = TargetSelector.Implementation;
            var target = ts.GetTarget(range);
            if (target != null && target.IsValidTarget())
            {
                return target;
            }

            var firstTarget = ts.GetOrderedTargets(range)
                .FirstOrDefault(t => t.IsValidTarget());
            if (firstTarget != null)
            {
                return firstTarget;
            }

            return null;
        }
        public static List<Obj_AI_Base> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }
        public static List<Obj_AI_Minion> GetAllGenericMinionsTargets()
        {
            return GetAllGenericMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetAllGenericMinionsTargetsInRange(float range)
        {
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range)).ToList();
        }
        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
        }
        public static bool IsWall(Vector3 pos, bool includeBuildings = false)
        {
            var point = NavMesh.WorldToCell(pos).Flags;
            return point.HasFlag(NavCellFlags.Wall) || includeBuildings && point.HasFlag(NavCellFlags.Building);
        }

        private void OnCombo()
        {


            if (!Menu["combo"]["block"].Enabled)
            {
                if (Menu["combo"]["mega"]["user"].Enabled)
                {
                    var targets = GetBestEnemyHeroTargetInRange(R.Range);
                    if (targets.IsValidTarget())
                    {

                        switch (Menu["combo"]["mega"]["rmode"].As<MenuList>().Value)
                        {
                            case 0:

                                if (targets.IsValidTarget(520))
                                {
                                    if (targets.HealthPercent() <= Menu["combo"]["mega"]["hp"].As<MenuSlider>().Value)
                                    {
                                        if (Player.Distance(targets) <= 490)
                                        {

                                            var pushdistance = 500;
                                            var targetpos = targets.ServerPosition;
                                            var pushidrection =
                                                (targetpos - Player.ServerPosition.Extend(targetpos, pushdistance))
                                                .Normalized();
                                            var checkdistance = pushdistance / 40;
                                            for (var i = 0; i <= 37; i++)
                                            {

                                                var finalpos = targetpos + (pushidrection * checkdistance * i);
                                                if (IsWall(finalpos, true))
                                                {

                                                    hello = finalpos;

                                                }
                                                else hello = new Vector3(0, 0, 0);
                                            }
                                            var pushdistance2 = 500;
                                            var targetpos2 = targets.ServerPosition;
                                            var pushidrection2 = (targetpos2 - Player.ServerPosition).Normalized();
                                            var checkdistance2 = pushdistance2 / 40;
                                            for (var i = 0; i <= 37; i++)
                                            {

                                                var finalpos2 = targetpos2 + (pushidrection2 * checkdistance2 * i);
                                                if (IsWall(finalpos2, true))
                                                {
                                                    hello2 = finalpos2;

                                                }
                                                else hello2 = new Vector3(0, 0, 0);
                                            }
                                            if (!hello.IsZero && !hello2.IsZero)
                                            {
                                                if (hello.Distance(Player) > hello2.Distance(Player))
                                                {
                                                    R.Cast(hello2);
                                                }
                                                if (hello.Distance(Player) < hello2.Distance(Player))
                                                {
                                                    R.Cast(hello);
                                                }
                                            }
                                            if (hello2.IsZero && !hello.IsZero)
                                            {

                                                R.Cast(hello);
                                            }
                                            if (hello.IsZero && !hello2.IsZero)
                                            {

                                                R.Cast(hello2);
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (targets.IsValidTarget(520))
                                {
                                    if (targets.Health <= Player.GetSpellDamage(targets, SpellSlot.Q,
                                            DamageStage.SecondForm) +
                                        Player.GetSpellDamage(targets, SpellSlot.W, DamageStage.SecondForm) +
                                        Player.GetSpellDamage(targets, SpellSlot.E, DamageStage.SecondForm) +
                                        Player.GetSpellDamage(targets, SpellSlot.R, DamageStage.Collision))
                                    {
                                        if (Player.Distance(targets) <= 490)
                                        {

                                            var pushdistance = 500;
                                            var targetpos = targets.ServerPosition;
                                            var pushidrection =
                                                (targetpos - Player.ServerPosition.Extend(targetpos, pushdistance))
                                                .Normalized();
                                            var checkdistance = pushdistance / 40;
                                            for (var i = 0; i <= 37; i++)
                                            {

                                                var finalpos = targetpos + (pushidrection * checkdistance * i);
                                                if (IsWall(finalpos, true))
                                                {

                                                    hello = finalpos;

                                                }
                                                else hello = new Vector3(0, 0, 0);
                                            }
                                            var pushdistance2 = 500;
                                            var targetpos2 = targets.ServerPosition;
                                            var pushidrection2 = (targetpos2 - Player.ServerPosition).Normalized();
                                            var checkdistance2 = pushdistance2 / 40;
                                            for (var i = 0; i <= 37; i++)
                                            {

                                                var finalpos2 = targetpos2 + (pushidrection2 * checkdistance2 * i);
                                                if (IsWall(finalpos2, true))
                                                {
                                                    hello2 = finalpos2;

                                                }
                                                else hello2 = new Vector3(0, 0, 0);
                                            }
                                            if (!hello.IsZero && !hello2.IsZero)
                                            {
                                                if (hello.Distance(Player) > hello2.Distance(Player))
                                                {
                                                    R.Cast(hello2);
                                                }
                                                if (hello.Distance(Player) < hello2.Distance(Player))
                                                {
                                                    R.Cast(hello);
                                                }
                                            }
                                            if (hello2.IsZero && !hello.IsZero)
                                            {

                                                R.Cast(hello);
                                            }
                                            if (hello.IsZero && !hello2.IsZero)
                                            {

                                                R.Cast(hello2);
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                if (Player.HasBuff("gnartransform") || Player.HasBuff("gnartransformsoon"))
                {

                    if (Menu["combo"]["mega"]["usee"].Enabled)
                    {

                        var target = GetBestEnemyHeroTargetInRange(E2.Range);
                        if (target.IsValidTarget())
                        {
                            if (target.IsValidTarget(E2.Range))
                            {


                                if (target != null)
                                {

                                    if (Menu["misc"]["toggle"].Enabled)
                                    {

                                        E2.Cast(target);
                                    }
                                    if (!Menu["misc"]["toggle"].Enabled)
                                    {
                                        if (!target.IsUnderEnemyTurret())
                                        {
                                            E2.Cast(target);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (W.Ready && Menu["combo"]["mega"]["usew"].Enabled)
                    {
                        var target = GetBestEnemyHeroTargetInRange(W.Range);
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
                    if (Q2.Ready && Menu["combo"]["mega"]["useq"].Enabled)
                    {
                        var target = GetBestEnemyHeroTargetInRange(Q2.Range);
                        if (target.IsValidTarget())
                        {
                            if (target.IsValidTarget(Q2.Range))
                            {


                                if (target != null)
                                {

                                    var collisions =
                                        (IList<Obj_AI_Base>)E.GetPrediction(target).CollisionObjects;
                                    if (collisions.Any())
                                    {
                                        if (collisions.All(c => GetAllGenericUnitTargets().Contains(c)))
                                        {
                                            return;
                                        }
                                    }
                                    Q2.Cast(target);



                                }
                            }

                        }
                    }
                }

                if (!Player.HasBuff("gnartransform"))
                {
                    if (Q.Ready && Menu["combo"]["mini"]["useq"].Enabled)
                    {
                        var target = GetBestEnemyHeroTargetInRange(Q.Range);
                        if (target.IsValidTarget())
                        {
                            if (target.IsValidTarget(Q.Range))
                            {


                                if (target != null)
                                {
                                    Q.Cast(target);


                                }
                                var pred = Q.GetPrediction(target);


                                for (var i = 0; i < 17; i++)
                                {

                                    var colliding = pred.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                                    var test = target.ServerPosition.Extend(Player.ServerPosition,
                                        target.Distance(Player) - 60 * i);
                             
                                    if (colliding.Count > 0)
                                        {

                                        if (target.Distance(Player) > 700 && target.Distance(Player) < 1000)
                                        {



                                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + target.BoundingRadius && m.Distance(target) < 310 && colliding[0].Distance(target) < 310)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }
                                            }
                                            foreach (var m in GameObjects.Jungle.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + target.BoundingRadius && m.Distance(target) < 310 && colliding[0].Distance(target) < 310)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }
                                            }

                                        }
                                        if (target.Distance(Player) < 700 && target.Distance(Player) < 1000)
                                        {
                                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                               x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + target.BoundingRadius && m.Distance(target) < 350 && colliding[0].Distance(target) < 350)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }

                                            }
                                            foreach (var m in GameObjects.Jungle.Where(x =>
                                                x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                            {
                                                if (m != null)
                                                {

                                                    if (test.Distance(m) <= 100 + target.BoundingRadius && m.Distance(target) < 350 && colliding[0].Distance(target) < 350)
                                                    {
                                                        Q.Cast(pred.CastPosition);
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (E.Ready && Menu["combo"]["mini"]["usee"].Enabled)
                    {
                        var target = GetBestEnemyHeroTargetInRange(E.Range + 500);
                        if (target.IsValidTarget())
                        {
                            if (target.IsValidTarget(E.Range + 500))
                            {


                                if (target != null)
                                {

                                    if (Menu["misc"]["toggle"].Enabled)
                                    {
                                        if (!Menu["combo"]["mini"]["usegap"].Enabled)
                                        {
                                            if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                            {
                                                E.Cast(target.ServerPosition);

                                            }
                                        }
                                        if (Menu["combo"]["mini"]["usegap"].Enabled)
                                        {
                                            if (target.Health <=
                                                Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Collision))
                                            {
                                                if (target.Distance(Player) > 500)
                                                {
                                                    if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                    {
                                                        E.Cast(target.ServerPosition);

                                                    }
                                                }
                                            }
                                            if (target.Health <=
                                                Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) ||
                                                target.BuffManager.GetBuffCount("gnarwproc", false) == 2 &&
                                                Player.GetSpellDamage(target, SpellSlot.E) * 3 > target.Health)
                                            {

                                                if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                {
                                                    E.Cast(target.ServerPosition);

                                                }

                                            }
                                        }
                                    }
                                    if (!Menu["misc"]["toggle"].Enabled)
                                    {
                                        if (!Menu["combo"]["mini"]["usegap"].Enabled)
                                        {
                                            if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                            {
                                                E.Cast(target.ServerPosition);

                                            }
                                        }
                                        if (Menu["combo"]["mini"]["usegap"].Enabled)
                                        {
                                            if (target.Health <=
                                                Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Collision))
                                            {
                                                if (target.Distance(Player) > 500)
                                                {
                                                    if (!target.IsUnderEnemyTurret())
                                                    {
                                                        if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                        {
                                                            E.Cast(target.ServerPosition);

                                                        }
                                                    }
                                                }
                                            }
                                            if (target.Health <=
                                                Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) ||
                                                target.BuffManager.GetBuffCount("gnarwproc", false) == 2 &&
                                                Player.GetSpellDamage(target, SpellSlot.E) * 3 > target.Health)
                                            {

                                                if (!target.IsUnderEnemyTurret())
                                                {
                                                    if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                    {
                                                        E.Cast(target.ServerPosition);

                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
            }
            if (Menu["combo"]["block"].Enabled)
            {
                if (Player.Mana < 100)
                {
                    if (R.Ready && Menu["combo"]["mega"]["user"].Enabled)
                    {
                        var targets = GetBestEnemyHeroTargetInRange(R.Range);
                        if (targets.IsValidTarget())
                        {

                            switch (Menu["combo"]["mega"]["rmode"].As<MenuList>().Value)
                            {
                                case 0:

                                    if (targets.IsValidTarget(520))
                                    {
                                        if (targets.HealthPercent() <= Menu["combo"]["mega"]["hp"].As<MenuSlider>().Value)
                                        {
                                            if (Player.Distance(targets) <= 490)
                                            {

                                                var pushdistance = 500;
                                                var targetpos = targets.ServerPosition;
                                                var pushidrection =
                                                    (targetpos - Player.ServerPosition.Extend(targetpos, pushdistance))
                                                    .Normalized();
                                                var checkdistance = pushdistance / 40;
                                                for (var i = 0; i <= 37; i++)
                                                {

                                                    var finalpos = targetpos + (pushidrection * checkdistance * i);
                                                    if (IsWall(finalpos, true))
                                                    {

                                                        hello = finalpos;

                                                    }
                                                    else hello = new Vector3(0, 0, 0);
                                                }
                                                var pushdistance2 = 500;
                                                var targetpos2 = targets.ServerPosition;
                                                var pushidrection2 = (targetpos2 - Player.ServerPosition).Normalized();
                                                var checkdistance2 = pushdistance2 / 40;
                                                for (var i = 0; i <= 37; i++)
                                                {

                                                    var finalpos2 = targetpos2 + (pushidrection2 * checkdistance2 * i);
                                                    if (IsWall(finalpos2, true))
                                                    {
                                                        hello2 = finalpos2;

                                                    }
                                                    else hello2 = new Vector3(0, 0, 0);
                                                }
                                                if (!hello.IsZero && !hello2.IsZero)
                                                {
                                                    if (hello.Distance(Player) > hello2.Distance(Player))
                                                    {
                                                        R.Cast(hello2);
                                                    }
                                                    if (hello.Distance(Player) < hello2.Distance(Player))
                                                    {
                                                        R.Cast(hello);
                                                    }
                                                }
                                                if (hello2.IsZero && !hello.IsZero)
                                                {

                                                    R.Cast(hello);
                                                }
                                                if (hello.IsZero && !hello2.IsZero)
                                                {

                                                    R.Cast(hello2);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    if (targets.IsValidTarget(520))
                                    {
                                        if (targets.Health <= Player.GetSpellDamage(targets, SpellSlot.Q,
                                                DamageStage.SecondForm) +
                                            Player.GetSpellDamage(targets, SpellSlot.W, DamageStage.SecondForm) +
                                            Player.GetSpellDamage(targets, SpellSlot.E, DamageStage.SecondForm) +
                                            Player.GetSpellDamage(targets, SpellSlot.R, DamageStage.Collision))
                                        {
                                            if (Player.Distance(targets) <= 490)
                                            {

                                                var pushdistance = 500;
                                                var targetpos = targets.ServerPosition;
                                                var pushidrection =
                                                    (targetpos - Player.ServerPosition.Extend(targetpos, pushdistance))
                                                    .Normalized();
                                                var checkdistance = pushdistance / 40;
                                                for (var i = 0; i <= 37; i++)
                                                {

                                                    var finalpos = targetpos + (pushidrection * checkdistance * i);
                                                    if (IsWall(finalpos, true))
                                                    {

                                                        hello = finalpos;

                                                    }
                                                    else hello = new Vector3(0, 0, 0);
                                                }
                                                var pushdistance2 = 500;
                                                var targetpos2 = targets.ServerPosition;
                                                var pushidrection2 = (targetpos2 - Player.ServerPosition).Normalized();
                                                var checkdistance2 = pushdistance2 / 40;
                                                for (var i = 0; i <= 37; i++)
                                                {

                                                    var finalpos2 = targetpos2 + (pushidrection2 * checkdistance2 * i);
                                                    if (IsWall(finalpos2, true))
                                                    {
                                                        hello2 = finalpos2;

                                                    }
                                                    else hello2 = new Vector3(0, 0, 0);
                                                }
                                                if (!hello.IsZero && !hello2.IsZero)
                                                {
                                                    if (hello.Distance(Player) > hello2.Distance(Player))
                                                    {
                                                        R.Cast(hello2);
                                                    }
                                                    if (hello.Distance(Player) < hello2.Distance(Player))
                                                    {
                                                        R.Cast(hello);
                                                    }
                                                }
                                                if (hello2.IsZero && !hello.IsZero)
                                                {

                                                    R.Cast(hello);
                                                }
                                                if (hello.IsZero && !hello2.IsZero)
                                                {

                                                    R.Cast(hello2);
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (Player.HasBuff("gnartransform"))
                    {
                        if (E2.Ready && Menu["combo"]["mega"]["usee"].Enabled)
                        {
                            var target = GetBestEnemyHeroTargetInRange(E2.Range);
                            if (target.IsValidTarget())
                            {
                                if (target.IsValidTarget(E2.Range))
                                {


                                    if (target != null)
                                    {
                                        if (Menu["misc"]["toggle"].Enabled)
                                        {
                                            E2.Cast(target);
                                        }
                                        if (!Menu["misc"]["toggle"].Enabled)
                                        {
                                            if (!target.IsUnderEnemyTurret())
                                            {
                                                E2.Cast(target);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (W.Ready && Menu["combo"]["mega"]["usew"].Enabled)
                        {
                            var target = GetBestEnemyHeroTargetInRange(W.Range);
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
                        if (Q2.Ready && Menu["combo"]["mega"]["useq"].Enabled)
                        {
                            var target = GetBestEnemyHeroTargetInRange(Q2.Range);
                            if (target.IsValidTarget())
                            {
                                if (target.IsValidTarget(Q2.Range))
                                {


                                    if (target != null)
                                    {


                                        var collisions =
                                            (IList<Obj_AI_Base>)E.GetPrediction(target).CollisionObjects;
                                        if (collisions.Any())
                                        {
                                            if (collisions.All(c => GetAllGenericUnitTargets().Contains(c)))
                                            {
                                                return;
                                            }
                                        }
                                        Q2.Cast(target);



                                    }
                                }
                            }
                        }
                    }
                    if (!Player.HasBuff("gnartransform"))
                    {
                        if (Q.Ready && Menu["combo"]["mini"]["useq"].Enabled)
                        {
                            var target = GetBestEnemyHeroTargetInRange(Q.Range);
                            if (target.IsValidTarget())
                            {
                                if (target.IsValidTarget(Q.Range))
                                {


                                    if (target != null)
                                    {
                                        Q.Cast(target);


                                    }
                                }
                            }
                        }

                        if (E.Ready && Menu["combo"]["mini"]["usee"].Enabled)
                        {
                            var target = GetBestEnemyHeroTargetInRange(E.Range + 500);
                            if (target.IsValidTarget())
                            {
                                if (target.IsValidTarget(E.Range + 50))
                                {


                                    if (target != null)
                                    {
                                        if (Menu["misc"]["toggle"].Enabled)
                                        {
                                            if (!Menu["combo"]["mini"]["usegap"].Enabled)
                                            {
                                                if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                {
                                                    E.Cast(target.ServerPosition);

                                                }
                                            }
                                            if (Menu["combo"]["mini"]["usegap"].Enabled)
                                            {
                                                if (target.Health <=
                                                    Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Collision))
                                                {
                                                    if (target.Distance(Player) > 500)
                                                    {
                                                        if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                        {
                                                            E.Cast(target.ServerPosition);

                                                        }
                                                    }
                                                }
                                                if (target.Health <=
                                                    Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) ||
                                                    target.BuffManager.GetBuffCount("gnarwproc", false) == 2 &&
                                                    Player.GetSpellDamage(target, SpellSlot.E) * 3 > target.Health)
                                                {

                                                    if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                    {
                                                        E.Cast(target.ServerPosition);

                                                    }

                                                }
                                            }
                                        }
                                        if (!Menu["misc"]["toggle"].Enabled)
                                        {
                                            if (!Menu["combo"]["mini"]["usegap"].Enabled)
                                            {
                                                if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                {
                                                    E.Cast(target.ServerPosition);

                                                }
                                            }
                                            if (Menu["combo"]["mini"]["usegap"].Enabled)
                                            {
                                                if (target.Health <=
                                                    Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Collision))
                                                {
                                                    if (target.Distance(Player) > 500)
                                                    {
                                                        if (!target.IsUnderEnemyTurret())
                                                        {
                                                            if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                            {
                                                                E.Cast(target.ServerPosition);

                                                            }
                                                        }
                                                    }
                                                }
                                                if (target.Health <=
                                                    Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) +
                                                    Player.GetSpellDamage(target, SpellSlot.W, DamageStage.SecondForm) ||
                                                    target.BuffManager.GetBuffCount("gnarwproc", false) == 2 &&
                                                    Player.GetSpellDamage(target, SpellSlot.E) * 3 > target.Health)
                                                {

                                                    if (!target.IsUnderEnemyTurret())
                                                    {
                                                        if (Menu["combo"]["mini"]["suicidallikeme"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(800))
                                                        {
                                                            E.Cast(target.ServerPosition);

                                                        }
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
            }
        }




        private void OnHarass()
        {
            if (!Menu["combo"]["block"].Enabled)
            {
                if (Player.HasBuff("gnartransform") || Player.HasBuff("gnartransformsoon"))
                {
                    var target = GetBestEnemyHeroTargetInRange(Q2.Range);

                    if (W.Ready && Menu["harass"]["mega"]["usew"].Enabled && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            W.Cast(target);
                        }
                    }
                    if (Q2.Ready && Menu["harass"]["mega"]["useq"].Enabled && target.IsValidTarget(Q2.Range))
                    {
                        if (target != null)
                        {
                            var collisions =
                                (IList<Obj_AI_Base>)E.GetPrediction(target).CollisionObjects;
                            if (collisions.Any())
                            {
                                if (collisions.All(c => GetAllGenericUnitTargets().Contains(c)))
                                {
                                    return;
                                }
                            }
                            Q2.Cast(target);

                        }
                    }

                    if (E2.Ready && Menu["harass"]["mega"]["usee"].Enabled && target.IsValidTarget(E2.Range))
                    {
                        if (target != null)
                        {
                            if (Menu["misc"]["toggle"].Enabled)
                            {
                                E2.Cast(target);
                            }
                            if (!Menu["misc"]["toggle"].Enabled)
                            {
                                if (!target.IsUnderEnemyTurret())
                                {
                                    E2.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (!Player.HasBuff("gnartransform"))
                {
                    var target = GetBestEnemyHeroTargetInRange(Q.Range);

                    if (Q.Ready && Menu["harass"]["mini"]["useq"].Enabled && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            Q.Cast(target);
                        }
                         var pred = Q.GetPrediction(target);


                        for (var i = 0; i < 17; i++)
                        {

                            var colliding = pred.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                            var test = target.ServerPosition.Extend(Player.ServerPosition,
                                target.Distance(Player) - 60 * i);

                            if (colliding.Count > 0)
                            {

                                if (target.Distance(Player) > 700 && target.Distance(Player) < 1000)
                                {



                                    foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                        x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                    {
                                        if (m != null)
                                        {

                                            if (test.Distance(m) <= 100 + target.BoundingRadius &&
                                                m.Distance(target) < 310 && colliding[0].Distance(target) < 310)
                                            {
                                                Q.Cast(pred.CastPosition);
                                            }
                                        }
                                    }
                                    foreach (var m in GameObjects.Jungle.Where(x =>
                                        x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                    {
                                        if (m != null)
                                        {

                                            if (test.Distance(m) <= 100 + target.BoundingRadius &&
                                                m.Distance(target) < 310 && colliding[0].Distance(target) < 310)
                                            {
                                                Q.Cast(pred.CastPosition);
                                            }
                                        }
                                    }

                                }
                                if (target.Distance(Player) < 700 && target.Distance(Player) < 1000)
                                {
                                    foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                        x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                    {
                                        if (m != null)
                                        {

                                            if (test.Distance(m) <= 100 + target.BoundingRadius &&
                                                m.Distance(target) < 350 && colliding[0].Distance(target) < 350)
                                            {
                                                Q.Cast(pred.CastPosition);
                                            }
                                        }

                                    }
                                    foreach (var m in GameObjects.Jungle.Where(x =>
                                        x.Distance(Player) < Q.Range && x != null && x.IsValidTarget()))
                                    {
                                        if (m != null)
                                        {

                                            if (test.Distance(m) <= 100 + target.BoundingRadius &&
                                                m.Distance(target) < 350 && colliding[0].Distance(target) < 350)
                                            {
                                                Q.Cast(pred.CastPosition);
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                    if (E.Ready && Menu["harass"]["mini"]["usee"].Enabled && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (Menu["misc"]["toggle"].Enabled)
                            {
                                if (Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm) >= target.Health)
                                {
                                    if (target.Distance(Player) > 500)
                                    {
                                        E.Cast(target.ServerPosition);
                                    }
                                }
                            }
                            if (!Menu["misc"]["toggle"].Enabled)
                            {
                                if (!target.IsUnderEnemyTurret())
                                {
                                    if (Player.GetSpellDamage(target, SpellSlot.Q) +
                                        Player.GetSpellDamage(target, SpellSlot.W) +
                                        Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm) >=
                                        target.Health)
                                    {
                                        if (target.Distance(Player) > 500)
                                        {
                                            E.Cast(target.ServerPosition);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Menu["combo"]["block"].Enabled)
            {
                
                if (Player.Mana < 100)
                {
                    if (Player.HasBuff("gnartransform"))
                    {
                        var target = GetBestEnemyHeroTargetInRange(Q2.Range);

                        if (W.Ready && Menu["harass"]["mega"]["usew"].Enabled && target.IsValidTarget(W.Range))
                        {
                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        if (Q2.Ready && Menu["harass"]["mega"]["useq"].Enabled && target.IsValidTarget(Q2.Range))
                        {
                            if (target != null)
                            {
                                var collisions =
                                    (IList<Obj_AI_Base>)E.GetPrediction(target).CollisionObjects;
                                if (collisions.Any())
                                {
                                    if (collisions.All(c => GetAllGenericUnitTargets().Contains(c)))
                                    {
                                        return;
                                    }
                                }
                                Q2.Cast(target);

                            }
                        }

                        if (E2.Ready && Menu["harass"]["mega"]["usee"].Enabled && target.IsValidTarget(E2.Range))
                        {
                            if (target != null)
                            {
                                if (Menu["misc"]["toggle"].Enabled)
                                {
                                    E2.Cast(target);
                                }
                                if (!Menu["misc"]["toggle"].Enabled)
                                {
                                    if (!target.IsUnderEnemyTurret())
                                    {
                                        E2.Cast(target);
                                    }
                                }
                            }
                        }
                    }
                    if (!Player.HasBuff("gnartransform"))
                    {
                        var target = GetBestEnemyHeroTargetInRange(Q.Range);

                        if (Q.Ready && Menu["harass"]["mini"]["useq"].Enabled && target.IsValidTarget(Q.Range))
                        {
                            if (target != null)
                            {
                                Q.Cast(target);
                            }
                        }
                        if (E.Ready && Menu["harass"]["mini"]["usee"].Enabled && target.IsValidTarget(Q.Range))
                        {
                            if (target != null)
                            {
                                if (Menu["misc"]["toggle"].Enabled)
                                {
                                    if (Player.GetSpellDamage(target, SpellSlot.Q) +
                                        Player.GetSpellDamage(target, SpellSlot.W) +
                                        Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm) >=
                                        target.Health)
                                    {
                                        if (target.Distance(Player) > 500)
                                        {
                                            E.Cast(target.ServerPosition);
                                        }
                                    }
                                }
                                if (!Menu["misc"]["toggle"].Enabled)
                                {
                                    if (!target.IsUnderEnemyTurret())
                                    {
                                        if (Player.GetSpellDamage(target, SpellSlot.Q) +
                                            Player.GetSpellDamage(target, SpellSlot.W) +
                                            Player.GetSpellDamage(target, SpellSlot.R, DamageStage.SecondForm) >=
                                            target.Health)
                                        {
                                            if (target.Distance(Player) > 500)
                                            {
                                                E.Cast(target.ServerPosition);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
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
    class Nami : Champion
    {
        public class DrawHelper
        {
            public void DrawLine(Vector3 start, Vector3 end, Color color)
            {
                var screenStart = start.ToScreenPosition();
                var screenEnd = end.ToScreenPosition();
                Render.Line(screenStart, screenEnd, color);
            }
        }


        public abstract class Polygon : DrawHelper
        {


            public List<Vector3> Points = new List<Vector3>();

            public abstract void Draw(Color color);

            public List<IntPoint> ClipperPoints
            {
                get
                {
                    List<IntPoint> clipperpoints = new List<IntPoint>();

                    foreach (var p in this.Points)
                    {
                        clipperpoints.Add(new IntPoint(p.X, p.Z));
                    }

                    return clipperpoints;
                }
            }

            public bool Contains(Vector3 point)
            {
                var p = new IntPoint(point.X, point.Z);
                var inpolygon = Clipper.PointInPolygon(p, this.ClipperPoints);
                return inpolygon == 1;

            }
        }
        public class Rectangle : Polygon
        {
            public Rectangle(Vector3 startPosition, Vector3 endPosition, float width)
            {
                var direction = (startPosition - endPosition).Normalized();
                var perpendicular = Perpendicular(direction);

                var leftBottom = startPosition + width * perpendicular;
                var leftTop = startPosition - width * perpendicular;

                var rightBottom = endPosition - width * perpendicular;
                var rightLeft = endPosition + width * perpendicular;

                this.Points.Add(leftBottom);
                this.Points.Add(leftTop);
                this.Points.Add(rightBottom);
                this.Points.Add(rightLeft);
            }


            public Vector3 Perpendicular(Vector3 v)
            {
                return new Vector3(-v.Z, v.Y, v.X);
            }

            public override void Draw(Color color)
            {
                if (Points.Count < 4)
                {
                    return;
                }

                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var p2 = (Points.Count - 1 == i) ? 0 : (i + 1);
                    this.DrawLine(Points[i], Points[p2], color);
                }
            }
        }
        public class Results
        {
            public Results(int hit, Vector3 cp)
            {
                this.numberOfMinionsHit = hit;
                this.CastPosition = cp;
            }

            public int numberOfMinionsHit = 0;
            public Vector3 CastPosition;
        }

        public static Results GetLinePosition(float range, float width)
        {
            var enemies = GameObjects.EnemyHeroes.Where(x => x.IsValidSpellTarget(range));

            var positions = enemies.Select(x => x.ServerPosition).ToList();

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

            HashSet<Results> results = new HashSet<Results>();

            foreach (var p in locations)
            {
                var rect = new Rectangle(Player.ServerPosition, Player.ServerPosition.Extend(p, R.Range), width);

                var count = 0;

                foreach (var m in enemies)
                {
                    if (rect.Contains(m.ServerPosition))
                    {
                        count++;
                    }
                }

                results.Add(new Results(count, p));
            }

            var maxhit = results.MaxBy(x => x.numberOfMinionsHit);

            return maxhit;
        }


        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                Q.Cast(Args.EndPosition);
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



            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
                }
            }

            if (RootMenu["combo"]["user"].Enabled)
            {
                var result = GetLinePosition(R.Range, 240);

                if (result != null)
                {
                    if (result.numberOfMinionsHit >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value &&
                        RootMenu["combo"]["allyr"].As<MenuSlider>().Value <= Player.CountAllyHeroesInRange(1000))
                    {
                        R.Cast(result.CastPosition);
                    }
                }

            }
            var targets = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
            switch (RootMenu["combo"]["wmode"].As<MenuList>().Value)
            {

                case 0:
                {
                    return;
                }
                case 1:
                    if (targets.IsValidTarget(W.Range) && targets != null)
                    {

                        if (targets.CountAllyHeroesInRange(690) >= 0)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    break;
                case 2:
                    foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.IsValidTarget(W.Range, true) && x.Distance(Player) < W.Range && !x.IsDead))
                    {
                        foreach (var targeto in GameObjects.EnemyHeroes.Where(
                            x => x.Distance(ally) < W.Range && x != null && !x.IsDead))
                        {


                            if (ally.Distance(targeto) < 690 && ally.IsAlly)
                            {
                                if (ally.CountEnemyHeroesInRange(690) > 0)
                                {
                                    W.CastOnUnit(ally);
                                }
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
                var result = GetLinePosition(R.Range, 240);

                if (result != null)
                {
                    if (result.numberOfMinionsHit >= RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                    {
                        R.Cast(result.CastPosition);
                    }
                }

            }
            if (RootMenu["misc"]["autoq"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(Q.Range)))
                {

                    Q.Cast(target);
                }

            }
            if (RootMenu["white"]["enable"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["white"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value != 0 &&
                             x.HealthPercent() <= RootMenu["white"][x.ChampionName.ToLower() + "hp"].As<MenuSlider>().Value)
                    .OrderByDescending(x => RootMenu["white"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value)
                    .Where(x => x.Distance(Player) < W.Range).OrderBy(x => x.Health))
                {


                    if (ally != null && !ally.IsDead)
                    {
                        W.Cast(ally);
                    }
                }

            }

        }







        protected override void Farming()
        {

        }

        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            var attack = sender as Obj_AI_Hero;
            var target = args.Target as Obj_AI_Hero;
            if (attack != null && attack.IsAlly && attack.IsHero)
            {
                if (RootMenu["ewhite"][attack.ChampionName.ToLower()].Enabled && RootMenu["combo"]["usee"].Enabled)
                {
                    if (target != null && args.SpellData.Name.Contains("BasicAttack") && attack.Distance(Player) < E.Range && !attack.IsDead)
                    {
                        E.Cast(attack);
                    }
                }
            }

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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuList("wmode", "W On Target Mode:",
                    new[] { "Never", "If Bounces to Ally / Me", "Bounce from Ally to Target" }, 1));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X", 2, 1, 5));
                ComboMenu.Add(new MenuSlider("allyr", "^- if X Nearby Allies", 1, 1, 4));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));

                ComboMenu.Add(new MenuBool("support", "Support Mode"));

            }

            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("misc", "Misc.");
            {
                HarassMenu.Add(new MenuBool("autoq", "Auto Q on CC"));


            }

            RootMenu.Add(HarassMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
            }
            RootMenu.Add(DrawMenu);
            FarmMenu = new Menu("white", "W Settings");
            {
                FarmMenu.Add(new MenuBool("enable", "Use Auto W"));
                FarmMenu.Add(new MenuSeperator("meow", "Priority 0 - Disabled"));
                FarmMenu.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                foreach (var target in GameObjects.AllyHeroes)
                {

                    FarmMenu.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));
                    FarmMenu.Add(new MenuSlider(target.ChampionName.ToLower() + "hp", "^- Health Percent: ", 50, 0,
                        100));
                }

            }
            RootMenu.Add(FarmMenu);
            KillstealMenu = new Menu("ewhite", "E WhiteList");
            {

                foreach (var target in GameObjects.AllyHeroes)
                {

                    KillstealMenu.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));


                }
            }
            RootMenu.Add(KillstealMenu);

            Gapcloser.Attach(RootMenu, "Q Anti-Gap");

            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 875);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 725);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 800);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1000);
            Q.SetSkillshot(1f, 110, float.MaxValue, false, SkillshotType.Circle);
            R.SetSkillshot(0.5f, 260f, 850, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}

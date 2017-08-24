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
    class Sona : Champion
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
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;
            float hit = RootMenu["combo"]["hitr"].As<MenuSlider>().Value;
            var target = Extensions.GetBestEnemyHeroTargetInRange(W2.Range);

            if (!target.IsValidTarget())
            {

                return;
            }
            if (target.IsValidTarget(Q.Range) && useQ && target.Distance(Player) < Q.Range)
            {

                if (target != null)
                {
                    Q.Cast();
                }
            }

            if (target.IsValidTarget(E.Range) && target.Distance(Player) < E.Range && useE)
            {

                if (target != null)
                {
                    E.Cast();
                }
            }

            if (useR)
            {

                if (Extensions.GetBestEnemyHeroTargetInRange(R.Range) != null)
                {
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                    {
                        R.CastIfWillHit(Extensions.GetBestEnemyHeroTargetInRange(R.Range),
                            RootMenu["combo"]["hitr"].As<MenuSlider>().Value - 1);
                    }
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                    {
                        R.Cast(Extensions.GetBestEnemyHeroTargetInRange(R.Range));
                    }
                }

            }

        }

        protected override void SemiR()
        {

            if (RootMenu["misc"]["autoe"].Enabled)
            {
                if (Player.HasBuffOfType(BuffType.Slow))
                {
                    E.Cast();
                }
                foreach (var en in GameObjects.AllyHeroes)
                {
                    if (!en.IsDead && en.Distance(Player) < E.Range && en.HasBuffOfType(BuffType.Slow))
                    {
                        E.Cast();
                    }
                }
            }
            if (Bases.Champion.RootMenu["wset"]["modes"].As<MenuList>().Value == 2)
            {

                if (RootMenu["wset"]["three"]["autow"].Enabled)
                {
                    if (Player.ManaPercent() >= RootMenu["wset"]["three"]["mana"].As<MenuSlider>().Value)
                    {
                        foreach (var en in GameObjects.AllyHeroes)
                        {
                            if (!en.IsDead && en.Distance(Player) < W2.Range && !en.IsMe && en.HealthPercent() <=
                                RootMenu["wset"]["three"]["ally"].As<MenuSlider>().Value && !Player.IsRecalling())
                            {
                                W2.Cast();
                            }
                            if (!en.IsDead && en.IsMe && en.HealthPercent() <=
                                RootMenu["wset"]["three"]["me"].As<MenuSlider>().Value && !Player.IsRecalling())
                            {
                                W2.Cast();
                            }
                        }
                    }
                }
            }
            if (RootMenu["combo"]["semir"].Enabled)
            {
                if (Extensions.GetBestEnemyHeroTargetInRange(R.Range) != null)
                {
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value > 1)
                    {
                        R.CastIfWillHit(Extensions.GetBestEnemyHeroTargetInRange(R.Range),
                            RootMenu["combo"]["hitr"].As<MenuSlider>().Value - 1);
                    }
                    if (RootMenu["combo"]["hitr"].As<MenuSlider>().Value == 1)
                    {
                        R.Cast(Extensions.GetBestEnemyHeroTargetInRange(R.Range));
                    }
                }

            }
        }

        protected override void Farming()
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
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
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
                                                    Player.GetSpellDamage(unit, SpellSlot.R)
                                            ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                       Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                       unit.MaxHealth * 100 / 100)
                                            : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }

        protected override void Killsteal()
        {
            
        }

        protected override void Harass()
        {
            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useE = RootMenu["harass"]["usee"].Enabled;
            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                if (!target.IsValidTarget())
                {

                    return;
                }
                if (target.IsValidTarget(Q.Range) && useQ)
                {

                    if (target != null)
                    {
                        Q.Cast();
                    }
                }

                if (target.IsValidTarget(
                    ) && useE)
                {

                    if (target != null)
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

            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", "^- if Hits", 2, 1, 5));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
            }
            RootMenu.Add(HarassMenu);
            HarassMenu = new Menu("misc", "Misc.");
            {
                HarassMenu.Add(new MenuBool("autoe", "Auto E if Slowed"));
              
            }

            RootMenu.Add(HarassMenu);

            EvadeMenu = new Menu("wset", "Healing");
            {
                EvadeMenu.Add(new MenuList("modes", "Healing Mode", new[] {"Spells Detector", "ZLib", "Healing Mode"},
                    1));
                var First = new Menu("first", "Spells Detector");
                SpellBlocking.EvadeManager.Attach(First);
                SpellBlocking.EvadeOthers.Attach(First);
                SpellBlocking.EvadeTargetManager.Attach(First);
                EvadeMenu.Add(First);
                var zlib = new Menu("zlib", "ZLib");
                var three = new Menu("three", "Healing Mode");
                three.Add(new MenuBool("autow", "Enable Healing"));
                three.Add(new MenuSlider("ally", "Ally Health Percent <=", 50));
                three.Add(new MenuSlider("me", "My Health Percent <=", 50));
                three.Add(new MenuSlider("mana", "Mana Manager", 20));
                EvadeMenu.Add(three);
                Support_AIO.ZLib.Attach(EvadeMenu);


            }

            RootMenu.Add(EvadeMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));

            }
            RootMenu.Add(DrawMenu);
            Gapcloser.Attach(RootMenu, "R Anti-Gap");


            RootMenu.Attach();
        }
        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
       

                if (target != null && Args.EndPosition.Distance(Player) < R.Range)
                {
                    R.Cast(Args.EndPosition);
                }
            
        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 850);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 400);
            W2 = new Aimtec.SDK.Spell(SpellSlot.W, 1000);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 400);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 900);
            R.SetSkillshot(0.5f, 115, 3000f, false, SkillshotType.Line);
        }
    }
}

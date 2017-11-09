using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    class Bard : Champion
    {
        private int yikes;

        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            var target = TargetSelector.GetTarget(Q.Range);
            if (target == null || !useQ)
            {
                return;
            }
            if (RootMenu["combo"]["stunq"].Enabled)
            {
                QSmite(target);
            }
            if (!RootMenu["combo"]["stunq"].Enabled)
            {
                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
            }
        }

        protected override void SemiR()
        {
            if (RootMenu["Flee"]["flee"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                W.Cast(Player.ServerPosition);
            }
            if (RootMenu["combo"]["white"]["enable"].Enabled)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() && !x.IsDead && 
                             RootMenu["combo"]["white"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value != 0 &&
                             x.HealthPercent() <= RootMenu["combo"]["white"][x.ChampionName.ToLower() + "hp"].As<MenuSlider>().Value)
                    .OrderByDescending(x => RootMenu["combo"]["white"][x.ChampionName.ToLower() + "priority"].As<MenuSlider>().Value)
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
        public static void DrawCircleOnMinimap(
            Vector3 center,
            float radius,
            Color color,
            int thickness = 1,
            int quality = 100)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float)Math.Sin(angle))
                );
            }
            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                Vector2 aonScreen;
                Vector2 bonScreen;

                Render.WorldToMinimap(a, out aonScreen);
                Render.WorldToMinimap(b, out bonScreen);

                Render.Line(aonScreen, bonScreen, color);
            }
        }

        protected override void Drawings()
        {
            if (RootMenu["drawings"]["drawchime"].Enabled)
            {
                var daggers = ObjectManager.Get<GameObject>()
                    .Where(d => d.IsValid && !d.IsDead && d.Distance(Player) <= 300000 &&
                                d.Name == "BardChimeMinion");
                foreach (var dagger in daggers)
                {
                   
                    Render.Circle(dagger.ServerPosition, 50, 10, Color.Coral);
                }
            }

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                if (Player.GetSpell(SpellSlot.R).Level == 0)
                {
                    DrawCircleOnMinimap(Player.Position, 0, Color.Wheat);

                }
                if (Player.GetSpell(SpellSlot.R).Level >= 1)
                {
                   
                    DrawCircleOnMinimap(Player.Position, 3400, Color.Wheat);
                }
            }

        }

        protected override void Killsteal()
        {
            if (Q.Ready &&
                RootMenu["killsteal"]["useq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
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
                ComboMenu.Add(new MenuBool("stunq", "^- Only if Stuns"));
                var meowmeow = new Menu("white", "W Settings");
                {
                    meowmeow.Add(new MenuBool("enable", "Use Auto W"));
                    meowmeow.Add(new MenuSeperator("meow", "Priority 0 - Disabled"));
                    meowmeow.Add(new MenuSeperator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        meowmeow.Add(new MenuSlider(target.ChampionName.ToLower() + "priority", target.ChampionName + " Priority: ", 1, 0, 5));
                        meowmeow.Add(new MenuSlider(target.ChampionName.ToLower() + "hp", "^- Health Percent: ", 50, 0,
                            100));
                    }

                }
                ComboMenu.Add(meowmeow);
                ComboMenu.Add(new MenuBool("support", "Support Mode", false));
              

            }
            RootMenu.Add(ComboMenu);

            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useq", "Use Q to Killsteal"));
            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range Minimap"));
                DrawMenu.Add(new MenuBool("drawchime", "Draw Chime Positions"));


            }
            RootMenu.Add(DrawMenu);
            var FleeMenu = new Menu("Flee", "Flee");
            {
                FleeMenu.Add(new MenuKeyBind("flee", "Use W to Flee", KeyCode.G, KeybindType.Press));
            }
            RootMenu.Add(FleeMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                Q.Cast(Args.EndPosition);
            }

        }

        public static bool IsWall(Vector3 pos, bool includeBuildings = false)
        {
            var point = NavMesh.WorldToCell(pos).Flags;
            return point.HasFlag(NavCellFlags.Wall) || includeBuildings && point.HasFlag(NavCellFlags.Building);
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
                            .Where(x => UnitExtensions.Distance(x, Player) < Q.Range).Count() > 1)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        public void QSmite(Obj_AI_Base target)
        {
           
            var pred = Q.GetPrediction(target);
            var pred2 = Q.GetPrediction(target, target.ServerPosition,
                target.ServerPosition.Extend(Player.ServerPosition, 300));
            var objects = pred.CollisionObjects;


            float pushdistance = 300;
            var targetpos = target.ServerPosition;
            var pushidrection =
                (targetpos - Player.ServerPosition.Extend(targetpos, -pushdistance))
                .Normalized();
            var checkdistance = pushdistance / 4.9f;
            for (var i = 0; i <= 5; i++)
            {

                var finalpos = targetpos + (pushidrection * checkdistance * i);

                if (IsWall(finalpos, true))
                {

                    yikes = Game.TickCount + 100;
                    if (!objects.Any())
                    {
                        Q.Cast(target);
                    }
                }


            }
            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                x.Distance(Player) < Q.Range + 300 && x != null && x.IsValidTarget()))
            {
                if (m != null)
                {

                    for (var i = 0; i < 5; i++)
                    {


                        var test = Player.ServerPosition.Extend(m.ServerPosition,
                            m.Distance(Player) + 60 * i);
                        if (test.Distance(target) <= 80  + m.BoundingRadius)
                        {
                            if (pred.CollisionObjects.Count == 1)
                            {
                                Q.Cast(pred.CastPosition);
                            }
                        }
                        var test2 = Player.ServerPosition.Extend(target.ServerPosition,
                            target.Distance(Player) + 60 * i);

                        if (test2.Distance(m) <= 80 + m.BoundingRadius)
                        {
                            

                            if (pred.CollisionObjects.Count == 0)
                            {
                                Q.Cast(pred.CastPosition);
                            }
                        }

                    }

                }
            }


            foreach (var ms in Extensions.GetEnemyHeroesTargetsInRange(Q.Range + 300))
            {
                if (ms != null)
                {
                    if (ms.NetworkId != target.NetworkId)
                    {
                        for (var i = 0; i < 5; i++)
                        {


                            var test = Player.ServerPosition.Extend(ms.ServerPosition,
                                ms.Distance(Player) + 63 * i);
                            if (test.Distance(target) < 70 + ms.BoundingRadius)
                            {

                                if (pred.CollisionObjects.Count == 0)
                                {
                                    Q.Cast(pred.CastPosition);
                                }
                            }

                        }
                    }

                    if (ms.NetworkId == target.NetworkId)
                    {
                        for (var i = 0; i < 5; i++)
                        {


                            var test = Player.ServerPosition.Extend(target.ServerPosition,
                                target.Distance(Player) + 63 * i);
                            foreach (var mso in Extensions.GetEnemyHeroesTargetsInRange(Q.Range + 300))
                            {
                                if (mso.NetworkId != target.NetworkId)
                                {
                                    if (test.Distance(mso) < 70 + mso.BoundingRadius)
                                    {

                                        if (pred.CollisionObjects.Count == 0)
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




        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 950);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 800);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 0);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.3f, 60, 1500, true, SkillshotType.Line);
        }

        protected override void LastHit()
        {
        }
    }
}

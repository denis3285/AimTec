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
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Prediction.Collision;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;
using Geometry = Potato_AIO.Bases.Geometry;

namespace Potato_AIO.Champions
{
    class Sion : Champion
    {
        public static Vector2 QCastPos = new Vector2();

        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(900);



            if (Q.Ready && useQA && target.IsValidTarget(900))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        if (Q.IsCharging)
                        {
                            if (RootMenu["combo"]["autoq"].Enabled)
                            {
                                var start = Aimtec.SDK.Extensions.Vector3Extensions.To2D(Player.ServerPosition);

                                var rectangle = new Geometry.Rectangle(Vector3Extensions.To2D(Player.ServerPosition),
                                    start.Extend(QCastPos, Q.Range), 170);
                                Console.WriteLine(rectangle.IsOutside(Vector3Extensions.To2D(target.ServerPosition)));
                                if (rectangle.IsOutside(Vector3Extensions.To2D(target.ServerPosition)))
                                {
                                    Q.ShootChargedSpell(Game.CursorPos, true);
                                }
                                if (Meowmeowtimer < Game.TickCount)
                                {
                                    Q.ShootChargedSpell(Game.CursorPos, true);
                                }
                            }
                        }
                        if (!Q.IsCharging)
                        {
                            if (Q.Ready && target.Distance(Player) < 580)
                            {
                                var meow = Q.GetPrediction(target);

                                Q.StartCharging(meow.CastPosition);
                            }
                        }
                        if (Q.IsCharging)
                        {
                            if (Meowmeowtimer < Game.TickCount)
                            {
                                Q.ShootChargedSpell(Game.CursorPos, true);
                            }

                        }
                    }
                }
            }
            if (E.Ready && useE)
            {
                var targetz = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (targetz != null && targetz.IsValidTarget(E.Range))
                {
                    E.Cast(targetz);
                }
                if (RootMenu["combo"]["extendede"].Enabled)
                {
                    var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range + 650);
                    if (targets != null && targets.IsValidTarget(E.Range + 650))
                    {
                        var test = E2.GetPrediction(targets);
                        for (var i = 0; i < 22; i++)
                        {
                            var rectangle = new Geometry.Rectangle(Vector3Extensions.To2D(Player.ServerPosition),
                                Vector3Extensions.To2D(test.CastPosition), 100);
                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                x.Distance(Player) < E.Range && x != null && x.IsValidTarget()))
                            {
                                if (rectangle.IsInside(Vector3Extensions.To2D(m.ServerPosition)))
                                {
                                    var colliding = test.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                                    var zzz = Player.ServerPosition.Extend(test.CastPosition,
                                        Player.Distance(test.CastPosition) - 60 * i);
                                    if (colliding.Count > 0 &&
                                        test.CastPosition.Distance(zzz) <= 100 + targets.BoundingRadius)
                                    {

                                        if (!Extensions.AnyWallInBetween(m.ServerPosition, test.CastPosition))
                                        {
                                            if (test.HitChance >= HitChance.None)
                                            {
                                                E2.Cast(test.CastPosition);
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


        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe && args.SpellData.Name == "SionQ")
            {
                Meowmeowtimer = RootMenu["combo"]["timerq"].As<MenuSlider>().Value + Game.TickCount;
                QCastPos = Aimtec.SDK.Extensions.Vector3Extensions.To2D(args.End);
            }
        }

        protected override void Farming()
        {

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;

                bool useE = RootMenu["farming"]["lane"]["useW"].Enabled;


                if (useQ)
                {
                    if (Q.Ready)
                    {

                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {


                                var result = FarmHelper.GetLineClearLocation(Q.Range, 180);

                                if (result != null)
                                {
                                    if (result.numberOfMinionsHit >=
                                        RootMenu["farming"]["lane"]["qhit"].As<MenuSlider>().Value)
                                    {
                                        if (!Q.IsCharging)
                                        {
                                            Q.StartCharging(result.CastPosition);
                                        }
                                        if (Q.IsCharging && Meowmeowtimer < Game.TickCount)
                                        {
                                            Q.ShootChargedSpell(result.CastPosition, true);
                                        }



                                    }

                                }
                            }
                        }
                    }
                }




                if (useE)
                {
                    if (W.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {
                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {

                                if (!Player.HasBuff("sionwshieldstacks"))
                                {
                                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range, false, false,
                                            Player.ServerPosition)) >= RootMenu["farming"]["lane"]["whit"]
                                            .As<MenuSlider>().Value)
                                    {
                                        W.Cast();
                                    }
                                }

                            }
                        }


                    }
                }



                foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(Q.Range))
                    .ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                    if (manapercents < Player.ManaPercent())
                    {
                        bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;


                        if (RootMenu["farming"]["jungle"]["useW"].Enabled && jungleTarget.IsValidTarget(W.Range))
                        {
                           
                            if (!Player.HasBuff("sionwshieldstacks"))
                            {

                                W.Cast();

                            }
                        }
                        if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range - 100))
                        {
                            if (!Q.IsCharging)
                            {
                                Q.StartCharging(jungleTarget.ServerPosition);
                            }
                            if (Q.IsCharging && Meowmeowtimer < Game.TickCount)
                            {
                                Q.ShootChargedSpell(jungleTarget.ServerPosition, true);
                            }
                        }
                        if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready &&
                            jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
                        }


                    }



                }
                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                    if (manapercents < Player.ManaPercent())
                    {

                        if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range - 100))
                        {
                            if (!Q.IsCharging)
                            {
                                Q.StartCharging(jungleTarget.ServerPosition);
                            }
                            if (Q.IsCharging && Meowmeowtimer < Game.TickCount)
                            {
                                Q.ShootChargedSpell(jungleTarget.ServerPosition, true);
                            }
                        }
                        if (RootMenu["farming"]["jungle"]["useW"].Enabled && W.Ready &&
                            jungleTarget.IsValidTarget(W.Range))
                        {
                            
                            if (!Player.HasBuff("sionwshieldstacks"))
                            {

                                W.Cast();

                            }
                        }
                        if (RootMenu["farming"]["jungle"]["useE"].Enabled && E.Ready &&
                            jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
                        }


                    }
                }
            }
        }



        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimes;

        public int Hmmmmm { get; private set; }
        public int Meowmeowtimer { get; private set; }

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
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                          
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
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawemax"].Enabled)
            {
                Render.Circle(Player.Position, E2.Range,50, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position,750, 50, Color.LightGreen);
            }
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


        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useE"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {

                    E.Cast(bestTarget);
                }
            }
            if (RootMenu["killsteal"]["useEmax"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(E2, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E, DamageStage.Empowered) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E2.Range))
                {

                    var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range + 650);
                    if (targets != null && targets.IsValidTarget(E.Range + 650))
                    {
                        var test = E2.GetPrediction(targets);
                        for (var i = 0; i < 22; i++)
                        {
                            var rectangle = new Geometry.Rectangle(Vector3Extensions.To2D(Player.ServerPosition),
                                Vector3Extensions.To2D(test.CastPosition), 100);
                            foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                x.Distance(Player) < E.Range && x != null && x.IsValidTarget()))
                            {
                                if (rectangle.IsInside(Vector3Extensions.To2D(m.ServerPosition)))
                                {
                                    var colliding = test.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                                    var zzz = Player.ServerPosition.Extend(test.CastPosition,
                                        Player.Distance(test.CastPosition) - 60 * i);
                                    if (colliding.Count > 0 &&
                                        test.CastPosition.Distance(zzz) <= 100 + targets.BoundingRadius)
                                    {

                                        if (!Extensions.AnyWallInBetween(m.ServerPosition, test.CastPosition))
                                        {
                                            if (test.HitChance >= HitChance.None)
                                            {
                                                E2.Cast(test.CastPosition);
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



        protected override void Harass()
        {


            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (manapercent < Player.ManaPercent())
            {

                if (Q.Ready && useQA && target.IsValidTarget(900))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (Q.IsCharging)
                            {
                                if (RootMenu["harass"]["autoq"].Enabled)
                                {
                                    var start = Aimtec.SDK.Extensions.Vector3Extensions.To2D(Player.ServerPosition);

                                    var rectangle = new Geometry.Rectangle(
                                        Vector3Extensions.To2D(Player.ServerPosition),
                                        start.Extend(QCastPos, Q.Range), 170);
                                    Console.WriteLine(
                                        rectangle.IsOutside(Vector3Extensions.To2D(target.ServerPosition)));
                                    if (rectangle.IsOutside(Vector3Extensions.To2D(target.ServerPosition)))
                                    {
                                        Q.ShootChargedSpell(Game.CursorPos, true);
                                    }
                                    if (Meowmeowtimer < Game.TickCount)
                                    {
                                        Q.ShootChargedSpell(Game.CursorPos, true);
                                    }
                                }
                            }
                            if (!Q.IsCharging)
                            {
                                if (Q.Ready && target.Distance(Player) < 580)
                                {
                                    var meow = Q.GetPrediction(target);

                                    Q.StartCharging(meow.CastPosition);
                                }
                            }
                            if (Q.IsCharging)
                            {
                                if (Meowmeowtimer < Game.TickCount)
                                {
                                    Q.ShootChargedSpell(Game.CursorPos, true);
                                }

                            }
                        }
                    }
                }
                if (E.Ready && useE)
                {
                    var targetz = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (targetz != null && targetz.IsValidTarget(E.Range))
                    {
                        E.Cast(targetz);
                    }
                    if (RootMenu["harass"]["extendede"].Enabled)
                    {
                        var targets = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range + 650);
                        if (targets != null && targets.IsValidTarget(E.Range + 650))
                        {
                            var test = E2.GetPrediction(targets);
                            for (var i = 0; i < 22; i++)
                            {
                                var rectangle = new Geometry.Rectangle(Vector3Extensions.To2D(Player.ServerPosition),
                                    Vector3Extensions.To2D(test.CastPosition), 100);
                                foreach (var m in GameObjects.EnemyMinions.Where(x =>
                                    x.Distance(Player) < E.Range && x != null && x.IsValidTarget()))
                                {
                                    if (rectangle.IsInside(Vector3Extensions.To2D(m.ServerPosition)))
                                    {
                                        var colliding = test.CollisionObjects.OrderBy(o => o.Distance(Player)).ToList();
                                        var zzz = Player.ServerPosition.Extend(test.CastPosition,
                                            Player.Distance(test.CastPosition) - 60 * i);
                                        if (colliding.Count > 0 &&
                                            test.CastPosition.Distance(zzz) <= 100 + targets.BoundingRadius)
                                        {

                                            if (!Extensions.AnyWallInBetween(m.ServerPosition, test.CastPosition))
                                            {
                                                if (test.HitChance >= HitChance.None)
                                                {
                                                    E2.Cast(test.CastPosition);
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


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo"));
                ComboMenu.Add(new MenuSlider("timerq", "^- Release Q After X ms.", 1000, 0, 2000));
                ComboMenu.Add(new MenuBool("autoq", "Release Q if Enemy is going outside of it"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("extendede", "^- Use Extended E"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass"));
                HarassMenu.Add(new MenuSlider("timerq", "^- Release Q After X ms.", 1000, 0, 2000));
                HarassMenu.Add(new MenuBool("autoq", "Release Q if Enemy is going outside of it"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("extendede", "^- Use Extended E"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("qhit", "^- If Hits X Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("whit", "^- If Hits X Minions", 3, 1, 6));


            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useE", "Use E to Killsteal"));
                KillstealMenu.Add(new MenuBool("useEmax", "Use Extended E to Killsteal"));
            }
            RootMenu.Add(KillstealMenu);     
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawemax", "Draw E Max Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            var zzzzzz = new Menu("wset", "W Settings");
            WShield.EvadeManager.Attach(zzzzzz);
            WShield.EvadeOthers.Attach(zzzzzz);
            WShield.EvadeTargetManager.Attach(zzzzzz);
            RootMenu.Add(zzzzzz);
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                E.Cast(Args.EndPosition);


            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 650);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 550);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 610);
            E2 = new Aimtec.SDK.Spell(SpellSlot.E, 725+ 650);

            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.65f, 70, float.MaxValue, false, SkillshotType.Line);
            Q.SetCharged("SionQ", "SionQ", 900, 900, 0.5f);
            E.SetSkillshot(0.25f, 70, 1800f, false, SkillshotType.Line);
            E2.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.Line);

        }

        protected override void SemiR()
        {

          
        }

        protected override void LastHit()
        {
        }
    }
}

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
    class TahmKench : Champion
    {
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
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range))
                .ToList();
        }

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargetsInRange(float range)
        {
            return Support_AIO.Bases.GameObjects.Jungle
                .Where(m => !Support_AIO.Bases.GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range))
                .ToList();
        }

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidTarget(range)).ToList();
        }


        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range))
                .Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
        }

        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;




            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }


            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {

                    if (!RootMenu["combo"]["stuns"].Enabled)
                    {
                        Q.Cast(target);
                    }
                    if (RootMenu["combo"]["stuns"].Enabled)
                    {
                        if (target.HasBuff("tahmkenchpdevourable"))
                        {
                            Q.Cast(target);
                        }
                    }
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null && target.HasBuff("tahmkenchpdevourable"))
                {
                    W.CastOnUnit(target);
                }
            }
        }

        protected override void SemiR()
        {



        }


        protected override void Farming()
        {
            if (RootMenu["farming"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (!minion.IsValidTarget() && minion != null)
                    {
                        return;
                    }
                    if (RootMenu["farming"]["useq"].Enabled)
                    {
                        if (minion.IsValidTarget(Q.Range))
                        {
                            Q.Cast(minion);
                        }
                    }
                    if (RootMenu["farming"]["usew"].Enabled)
                    {
                        if (minion.IsValidTarget(W.Range))
                        {
                            W.Cast(minion);
                        }
                    }
                }
                foreach (var minion in Support_AIO.Bases.Extensions.GetGenericJungleMinionsTargetsInRange(Q.Range))
                {
                    if (!minion.IsValidTarget() && minion != null)
                    {
                        return;
                    }
                    if (RootMenu["farming"]["useq"].Enabled)
                    {
                        if (minion.IsValidTarget(Q.Range))
                        {
                            Q.Cast(minion);
                        }
                    }
                    if (RootMenu["farming"]["usew"].Enabled)
                    {
                        if (minion.IsValidTarget(W.Range))
                        {
                            W.Cast(minion);
                        }
                    }
                }
            }
        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
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
                        center.X + radius * (float) Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float) Math.Sin(angle))
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
                    return;

                }
                if (Player.GetSpell(SpellSlot.R).Level == 1)
                {

                    DrawCircleOnMinimap(Player.Position, 4500, Color.Wheat);
                }

                if (Player.GetSpell(SpellSlot.R).Level == 2)
                {

                    DrawCircleOnMinimap(Player.Position, 5500, Color.Wheat);
                }
                if (Player.GetSpell(SpellSlot.R).Level == 3)
                {

                    DrawCircleOnMinimap(Player.Position, 6500, Color.Wheat);
                }
            }

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            throw new NotImplementedException();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                Q.Cast(Args.EndPosition);
            }


            if (target != null)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x => x.Distance(Player) < W.Range && x != null))
                {
                    if (ally.Distance(Args.EndPosition) < 300 && ally.Distance(Player) < W.Range && !ally.IsMe)
                    {
                        W.CastOnUnit(ally);
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
                ComboMenu.Add(new MenuBool("stuns", "^- Only if Stuns", false));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
            }
            RootMenu.Add(ComboMenu);
     
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range on Map"));

            }
            RootMenu.Add(DrawMenu);

            FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));

                FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
                FarmMenu.Add(new MenuBool("usew", "Use W to Farm"));

            }
            RootMenu.Add(FarmMenu);

            EvadeMenu = new Menu("wset", "Shielding/Eating");
            {
                EvadeMenu.Add(new MenuList("modes", "Shielding Mode", new[] {"Spells Detector", "ZLib"}, 1));
                var First = new Menu("first", "Spells Detector");
                TahmShielding.EvadeManager.Attach(First);
                TahmShielding.EvadeOthers.Attach(First);
                TahmShielding.EvadeTargetManager.Attach(First);
                EvadeMenu.Add(First);
                var zlib = new Menu("zlib", "ZLib");

                Support_AIO.ZLib.Attach(EvadeMenu);


            }
            RootMenu.Add(EvadeMenu);
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Attach();
        }



        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 800);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 380);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 400);
            
            Q.SetSkillshot(0.25f, 70f, 2700, true, SkillshotType.Line);

        }
    }
}

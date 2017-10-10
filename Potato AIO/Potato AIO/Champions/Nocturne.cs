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
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Nocturne : Champion
    {
        protected override void Combo()
        {

            if (E.Ready && RootMenu["combo"]["usee"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast(target);
                        }
                    }
                }


            }
            if (Q.Ready && RootMenu["combo"]["useq"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (R.Ready && RootMenu["combo"]["user"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (RootMenu["combo"]["hmm"].As<MenuSlider>().Value >= target.CountEnemyHeroesInRange(900))
                            {
                                if (target.Distance(Player) >= RootMenu["combo"]["minr"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                        }
                    }
                }


            }



        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;


            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;

            if (manapercent < Player.ManaPercent())
            {
                if (Q.Ready)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {

                            Q.Cast(minion);


                        }
                    }
                }


                if (useE)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {

                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            E.Cast(minion);
                        }

                    }
                }
            }
            float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

            if (manapercents < Player.ManaPercent())
            {
                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;

                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                    if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
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
                                (float) (barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.R) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.R) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.R) +
                                Player.GetSpellDamage(unit, SpellSlot.W) + Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }

            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                DrawCircleOnMinimap(Player.ServerPosition, R.Range, Color.Wheat, 3, 40);
                Render.Circle(Player.Position, R.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.CornflowerBlue);
            }
        }

        protected override void Harass()
        {
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;


            if (manapercent < Player.ManaPercent())
            {
                if (E.Ready && RootMenu["combo"]["usee"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                    if (target.IsValidTarget(E.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                E.Cast(target);
                            }
                        }
                    }


                }
                if (Q.Ready && RootMenu["combo"]["useq"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                    if (target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q.Cast(target);
                            }
                        }
                    }


                }
            }
        }
        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {


                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["items"].Enabled)
                {

                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                        Player.HasItem(ItemId.RavenousHydra))
                    {
                        var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                        if (slot != null)
                        {
                            var spellslot = slot.SpellSlot;
                            if (spellslot != SpellSlot.Unknown &&
                                Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                            {
                                Player.SpellBook.CastSpell(spellslot);
                            }
                        }
                    }
                }
            }

            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
            {


                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["items"].Enabled)
                {

                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                        Player.HasItem(ItemId.RavenousHydra))
                    {
                        var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                        if (slot != null)
                        {
                            var spellslot = slot.SpellSlot;
                            if (spellslot != SpellSlot.Unknown &&
                                Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                            {
                                Player.SpellBook.CastSpell(spellslot);
                            }
                        }
                    }
                }
            }
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear))
            {

                Obj_AI_Minion hero = e.Target as Obj_AI_Minion;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }


                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range))
                    .ToList())
                {
                    if (hero == jungleTarget)
                    {
                        if (jungleTarget.IsValidTarget() && !jungleTarget.UnitSkinName.Contains("Plant"))
                        {


                            if (RootMenu["combo"]["items"].Enabled)
                            {
                                if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                    Player.HasItem(ItemId.RavenousHydra))
                                {
                                    var items = new[]
                                        {ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra};
                                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                    if (slot != null)
                                    {
                                        var spellslot = slot.SpellSlot;
                                        if (spellslot != SpellSlot.Unknown &&
                                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                                        {
                                            Player.SpellBook.CastSpell(spellslot);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }




                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (minion != null && hero == minion)
                    {
                        if (RootMenu["combo"]["items"].Enabled)
                        {
                            if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                Player.HasItem(ItemId.RavenousHydra))
                            {
                                var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                                var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                if (slot != null)
                                {
                                    var spellslot = slot.SpellSlot;
                                    if (spellslot != SpellSlot.Unknown &&
                                        Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                                    {
                                        Player.SpellBook.CastSpell(spellslot);
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("minr", " ^- Min. R Range", 500, 0, 600));
                ComboMenu.Add(new MenuSlider("hmm", "Don't R in X Enemies", 3, 2, 5));
                ComboMenu.Add(new MenuBool("items", "Use Items"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);              
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            KillstealMenu = new Menu("wset", "W Settings");
            WShield.EvadeManager.Attach(KillstealMenu);
            WShield.EvadeOthers.Attach(KillstealMenu);
            WShield.EvadeTargetManager.Attach(KillstealMenu);
            RootMenu.Add(KillstealMenu);
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                E.Cast(target);
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1200);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 400);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 425);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 2500);

            Q.SetSkillshot(0.25f, 90, 1300, false, SkillshotType.Line, false, HitChance.None);
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
        protected override void SemiR()
        {
            if (Player.GetSpell(SpellSlot.R).Level == 1)
            {
                R.Range = 2500;
            }
            if (Player.GetSpell(SpellSlot.R).Level == 2)
            {
                R.Range = 3250;
            }
            if (Player.GetSpell(SpellSlot.R).Level ==3)
            {
                R.Range = 4000;
            }


        }

        protected override void LastHit()
        {

        }

        protected override void Killsteal()
        {
        }
    }
}

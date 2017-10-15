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

namespace Potato_AIO.Champions
{
    class RekSai : Champion
    {
        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;


            if (Player.HasBuff("RekSaiW"))
            {
                if (RootMenu["combo"]["useEQ"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E2.Range);

                    if (E2.Ready && target.IsValidTarget(E2.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (target.Distance(Player) > 500)
                                {
                                    E2.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (useQA)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q2.Range);
                    if (Q.Ready && target.IsValidTarget(Q2.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q2.Cast(target);
                            }
                        }
                    }
                }
                if (RootMenu["combo"]["useW"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                W.Cast();
                            }
                        }
                    }
                }
            }
            if (!Player.HasBuff("RekSaiW"))
            {
                if (useQA && !RootMenu["combo"]["useQAA"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                    if (Q.Ready && target.IsValidTarget(Q.Range))
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
                if (RootMenu["combo"]["useE"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (E.Ready && target.IsValidTarget(E.Range))
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

                if (RootMenu["combo"]["useW"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead && !target.HasBuff("RekSaiKnockupImmune") && !Player.HasBuff("RekSaiQ"))
                            {
                                W.Cast();
                            }
                        }
                    }
                }
            }
            if (RootMenu["combo"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (R.Ready && target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (target.Distance(Player) > RootMenu["combo"]["minrange"].As<MenuSlider>().Value)
                        {
                            if (RootMenu["combo"]["turret"].Enabled)
                            {
                                R.Cast(target);
                            }
                            if (!RootMenu["combo"]["turret"].Enabled)
                            {
                                if (!target.IsUnderEnemyTurret())
                                {
                                    R.Cast(target);
                                }
                            }
                        }

                    }
                }
            }
        }
    


    protected override void Farming()
        {



            bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
            bool useW = RootMenu["farming"]["lane"]["useE"].Enabled;

            if (useQ)
            {

                if (Q.Ready)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            if (!Player.HasBuff("RekSaiW"))
                            {
                                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(Q.Range, false, false,
                                        Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitQ"]
                                        .As<MenuSlider>().Value)
                                {

                                    Q.Cast();
                                }
                            }
                        }
                    }

                }
            }
            if (useW)
            {
                if (E.Ready)
                {
                    if (RootMenu["farming"]["lane"]["lastE"].Enabled)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                if (!Player.HasBuff("RekSaiW"))
                                {
                                    if (Player.GetSpellDamage(minion, SpellSlot.E) >= minion.Health)
                                    {
                                        E.Cast(minion);
                                    }
                                }
                            }
                        }
                    }
                    if (!RootMenu["farming"]["lane"]["lastE"].Enabled)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                if (!Player.HasBuff("RekSaiW"))
                                {
                                    E.Cast(minion);
                                }
                            }
                        }
                    }
                }
            }


            foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(Q2.Range))
                .ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;



                if (Player.HasBuff("RekSaiW"))
                {
                    if (useQs && Q2.Ready && jungleTarget.IsValidTarget(Q2.Range))
                    {
                        Q2.Cast(jungleTarget);
                    }
                    if (useWs && W2.Ready && jungleTarget.IsValidTarget(W2.Range))
                    {
                        W2.Cast();
                    }

                }
                if (!Player.HasBuff("RekSaiW"))
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }
                    if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
                    }
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) &&
                        !jungleTarget.HasBuff("RekSaiKnockupImmune"))
                    {
                        if (!Orbwalker.Implementation.IsWindingUp)
                        {
                            W.Cast();
                        }
                    }

                }
            }

            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q2.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;



                if (Player.HasBuff("RekSaiW"))
                {
                    if (useQs && Q2.Ready && jungleTarget.IsValidTarget(Q2.Range))
                    {
                        Q2.Cast(jungleTarget);
                    }
                    if (useWs && W2.Ready && jungleTarget.IsValidTarget(W2.Range))
                    {
                        W2.Cast();
                    }

                }
                if (!Player.HasBuff("RekSaiW"))
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }
                    if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(jungleTarget);
                    }
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range) &&
                        !jungleTarget.HasBuff("RekSaiKnockupImmune") && !Player.HasBuff("RekSaiQ"))
                    {
                        W.Cast();
                    }

                }

            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimer;

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }


        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "RekSaiW")
                {
                    meowmeowtimer = 1000 + Game.TickCount;
                    Orbwalker.Implementation.AttackingEnabled = false;

                }
            }
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
                            double Qdamage = 0;
                            double Wdamage = 0;
                            double Edamage = 0;
                            double Rdamage = Player.GetSpellDamage(unit, SpellSlot.R);


                            if (Player.HasBuff("RekSaiW"))
                            {
                                Qdamage = Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.SecondForm);
                                Wdamage = Player.GetSpellDamage(unit, SpellSlot.W);
                                Edamage = 0;
                            }
                            if (!Player.HasBuff("RekSaiW"))
                            {
                                Qdamage = Player.GetSpellDamage(unit, SpellSlot.Q);
                                Wdamage = Player.GetSpellDamage(unit, SpellSlot.W);
                                Edamage = Player.GetSpellDamage(unit, SpellSlot.E);
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
                                (float) (barPos.X + (unit.Health > Qdamage + Wdamage + Edamage + Rdamage
                                             ? width * ((unit.Health -
                                                         (Qdamage + Wdamage + Edamage + Rdamage)) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Qdamage + Wdamage + Edamage + Rdamage
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;
            if (RootMenu["drawings"]["drawtoggle"].Enabled)
            {
                if (RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("R Under-Turret: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.GreenYellow);

                }
                if (!RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("R Under-Turret: OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.Red);

                }
            }
            if (Player.HasBuff("RekSaiW"))
            {
                if (RootMenu["drawings"]["drawe"].Enabled)
                {
                    Render.Circle(Player.Position, E2.Range, 50, Color.LightGreen);
                }
                if (RootMenu["drawings"]["drawr"].Enabled)
                {
                    Render.Circle(Player.Position, R.Range, 50, Color.Wheat);
                }
                if (RootMenu["drawings"]["drawq"].Enabled)
                {
                    Render.Circle(Player.Position, Q2.Range, 50, Color.LightGreen);
                }
            }
            if (!Player.HasBuff("RekSaiW"))
            {
                if (RootMenu["drawings"]["drawe"].Enabled)
                {
                    Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
                }
                if (RootMenu["drawings"]["drawr"].Enabled)
                {
                    Render.Circle(Player.Position, R.Range, 50, Color.Wheat);
                }

            }
        }



        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useQ"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(Q2, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q, DamageStage.SecondForm) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q2.Range))
                {
                    if (Player.HasBuff("RekSaiW"))
                    {
                        Q2.Cast(bestTarget);
                    }
                }
            }
            if (RootMenu["killsteal"]["useE"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    if (Player.HasBuff("RekSaiW"))
                    {
                        E.Cast(bestTarget);
                    }
                }
            }
        }



        protected override void Harass()
        {


            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;



            if (Player.HasBuff("RekSaiW"))
            {
                if (RootMenu["harass"]["useEQ"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E2.Range);

                    if (E2.Ready && target.IsValidTarget(E2.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (target.Distance(Player) > 500)
                                {
                                    E2.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (useQA)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q2.Range);
                    if (Q.Ready && target.IsValidTarget(Q2.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q2.Cast(target);
                            }
                        }
                    }
                }
                if (RootMenu["harass"]["useW"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                W.Cast();
                            }
                        }
                    }
                }
            }
            if (!Player.HasBuff("RekSaiW"))
            {
                if (useQA && !RootMenu["combo"]["useQAA"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                    if (Q.Ready && target.IsValidTarget(Q.Range))
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
                if (RootMenu["harass"]["useE"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (E.Ready && target.IsValidTarget(E.Range))
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

                if (RootMenu["harass"]["useW"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead && !target.HasBuff("RekSaiKnockupImmune") && !Player.HasBuff("RekSaiQ"))
                            {
                                W.Cast();
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
                ComboMenu.Add(new MenuBool("useQAA", "^- Use Unburrowed Q Only for AA Reset"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useEQ", " ^- Use Burrowed E to GapClose"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuKeyBind("turret", "R Under Turret Toggle", KeyCode.T, KeybindType.Toggle));
                ComboMenu.Add(new MenuSlider("minrange", "Min. R Range", 300, 0, 500));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("useQAA", "^- Use Unburrowed Q Only for AA Reset"));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("useEQ", " ^- Use Burrowed E to GapClose"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Unburrowed Q to Farm"));
                LaneClear.Add(new MenuSlider("hitQ", "^- If Hits X Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("useE", "Use Unburrowed E to Farm"));
                LaneClear.Add(new MenuBool("lastE", "^- Only for Last Hit"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use Unburrowed E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Burrowed Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("useE", "Use Unburrowed E to Killsteal"));

            }
            RootMenu.Add(KillstealMenu);

            var Flee = new Menu("flee", "Flee");
            {
                Flee.Add(new MenuKeyBind("flee", "Flee Key", KeyCode.Z, KeybindType.Press));
            }
            RootMenu.Add(Flee);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Toggle"));

            }
            Gapcloser.Attach(RootMenu, "Burrowed W Anti-Gap");
            RootMenu.Add(DrawMenu);

            RootMenu.Attach();
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
                if (RootMenu["combo"]["useQAA"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();
                    }
                }
                if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
                {

                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                        Player.HasItem(ItemId.RavenousHydra))
                    {
                        var items = new[] {ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra};
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
                if (RootMenu["harass"]["useQAA"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();
                    }
                }
                if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
                {

                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                        Player.HasItem(ItemId.RavenousHydra))
                    {
                        var items = new[] {ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra};
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




                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (minion != null && hero == minion)
                    {
                        if (RootMenu["combo"]["items"].Enabled)
                        {
                            if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                Player.HasItem(ItemId.RavenousHydra))
                            {
                                var items = new[] {ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra};
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

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                if (Player.HasBuff("RekSaiW"))
                {
                    W.Cast();
                }


            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 325);
            Q2 = new Aimtec.SDK.Spell(SpellSlot.Q, 1450f);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 250);
            W2 = new Aimtec.SDK.Spell(SpellSlot.W, 250);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 310);
            E2 = new Aimtec.SDK.Spell(SpellSlot.E, 750);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 1500);
            Q2.SetSkillshot(0.4f, 70, 1950, true, SkillshotType.Line, false, HitChance.None);
            E2.SetSkillshot(0.25f, 60, 1600, false, SkillshotType.Line, false, HitChance.None);


        }

        protected override void SemiR()
        {
            if (meowmeowtimer < Game.TickCount)
            {
                Orbwalker.Implementation.AttackingEnabled = true;
            }
            if (RootMenu["flee"]["flee"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (Player.HasBuff("RekSaiW"))
                {
                    E2.Cast(Game.CursorPos);
                }
            }
        }

        protected override void LastHit()
        {
            if (RootMenu["farming"]["lane"]["lastE"].Enabled)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (minion.IsValidTarget(E.Range) && minion != null)
                    {
                        if (!Player.HasBuff("RekSaiW"))
                        {
                            if (Player.GetSpellDamage(minion, SpellSlot.E) >= minion.Health)
                            {
                                E.Cast(minion);
                            }
                        }
                    }
                }
            }
        }
    }
}

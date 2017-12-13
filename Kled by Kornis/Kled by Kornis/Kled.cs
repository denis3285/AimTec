using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Events;

namespace Kled_By_Kornis
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

    internal class Kled
    {
        public static Menu Menu = new Menu("Kled By Kornis", "Kled by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, Q2;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 760);
            Q2 = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 550);
            Q.SetSkillshot(0.25f, 40, 1850, false, SkillshotType.Line, false, HitChance.None);
            Q2.SetSkillshot(0.25f, 80, 3000, false, SkillshotType.Line, false, HitChance.None);
            E.SetSkillshot(0.25f, 60, 2200, false, SkillshotType.Line);

        }

        public Kled()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                var Skaarl = new Menu("Skaarl", "Shaarl Settings");
                Skaarl.Add(new MenuBool("useq", "Use Q in Combo"));
                Skaarl.Add(new MenuBool("usee", "Use E in Combo with Logic"));
                var Kled = new Menu("Kled", "Kled Settings");
                Kled.Add(new MenuBool("useq", "Use Q in Combo"));
                Kled.Add(new MenuList("qmode", "Q Mode", new[] {"Logic", "Always"}, 0));
                Kled.Add(new MenuBool("gap", "Use for Smart Gapclosing"));
                ComboMenu.Add(Skaarl);
                ComboMenu.Add(Kled);
                ComboMenu.Add(new MenuBool("items", "Use Items for AA Reset"));

            }

            Menu.Add(ComboMenu);

            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Skaarl's Q to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use Skaarl's E to Harass"));
                HarassMenu.Add(new MenuBool("useqk", "Use Kled's Q to Harass"));

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");

            var Jungle = new Menu("jungle", "Jungle Clear");
            Jungle.Add(new MenuBool("useq", "Use Skaarl's Q in Jungle"));
            Jungle.Add(new MenuBool("usee", "Use Skaarl's E in Jungle"));
            Jungle.Add(new MenuBool("useqk", "Use Kled's Q in Jungle"));
            var lane = new Menu("lane", "Lane Clear");
            lane.Add(new MenuBool("useq", "Use Skaarl's Q in Lane Clear"));
            lane.Add(new MenuBool("useqk", "Use Kled's Q in Lane clear"));
            FarmMenu.Add(Jungle);
            FarmMenu.Add(lane);
            Menu.Add(FarmMenu);

            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                var Skaarl = new Menu("Skaarl", "Shaarl Settings");
                Skaarl.Add(new MenuBool("useq", "Killsteal with Q"));
                Skaarl.Add(new MenuBool("usee", "Killsteal with E"));
                var Kled = new Menu("Kled", "Kled Settings");
                Kled.Add(new MenuBool("useq", "Killsteal with Q"));

                KSMenu.Add(Skaarl);
                KSMenu.Add(Kled);
            }
            Menu.Add(KSMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("damage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Minimap"));
            }
            Menu.Add(DrawMenu);
            Menu.Attach();
            Gapcloser.Attach(Menu, "Q Anti-GapClose");
            Orbwalker.PostAttack += OnPostAttack;
            Render.OnPresent += Render_OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
            Game.OnUpdate += Game_OnUpdate;
            LoadSpells();
            Console.WriteLine("Kled by Kornis - Loaded");
        }

        private void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
            if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready && !target.IsDashing() &&
                target.IsValidTarget(Q.Range))
            {

                Q.Cast(Args.EndPosition);

            }
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["items"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (Menu["combo"]["items"].Enabled)
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

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
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

        private void Render_OnPresent()
        {
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
            {
                if (Menu["drawings"]["drawq"].Enabled)
                {
                    Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
                }

                if (Menu["drawings"]["drawe"].Enabled)
                {
                    Render.Circle(Player.Position, E.Range, 40, Color.CornflowerBlue);
                }
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
            {
                if (Menu["drawings"]["drawq"].Enabled)
                {
                    Render.Circle(Player.Position, Q2.Range, 40, Color.CornflowerBlue);
                }
            }
            if (Menu["drawings"]["drawr"].Enabled)
            {
                if (Player.GetSpell(SpellSlot.R).Level == 0)
                {
                    DrawCircleOnMinimap(Player.Position, 0, Color.Wheat); ;

                }
                if (Player.GetSpell(SpellSlot.R).Level == 1)
                {

                    DrawCircleOnMinimap(Player.Position, 3500, Color.Wheat);
                }

                if (Player.GetSpell(SpellSlot.R).Level == 2)
                {

                    DrawCircleOnMinimap(Player.Position, 4000, Color.Wheat);
                }
                if (Player.GetSpell(SpellSlot.R).Level == 3)
                {

                    DrawCircleOnMinimap(Player.Position, 4500, Color.Wheat);
                }
            }
            if (Menu["drawings"]["damage"].Enabled)
            {
                double QDamage = 0;
                double WDamage = 0;
                double EDamage = 0;

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range*2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                      
                            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
                            {
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        20 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.04);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 2)
                                {
                                    double meow =
                                        30 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.045);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        40 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.05);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        50 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.055);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        60 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.06);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                QDamage = Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.SecondForm);

                            }
                            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                            {
                                QDamage = Player.GetSpellDamage(unit, SpellSlot.Q) +
                                          Player.GetSpellDamage(unit, SpellSlot.Q, DamageStage.Detonation);

                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        20 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.04);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 2)
                                {
                                    double meow =
                                        30 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.045);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        40 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.05);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        50 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.055);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                if (Player.SpellBook.GetSpell(SpellSlot.W).Level == 1)
                                {
                                    double meow =
                                        60 + ((Math.Abs(Player.FlatPhysicalDamageMod / 100) * 0.05) *
                                              Player.TotalAttackDamage) + (unit.Health * 0.06);
                                    double hello = Player.CalculateDamage(unit, DamageType.Physical, meow);
                                    WDamage = hello;
                                }
                                EDamage = Player.GetSpellDamage(unit, SpellSlot.E, DamageStage.SecondForm);


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
                                                     QDamage + EDamage + WDamage
                                             ? width * ((unit.Health - (QDamage + EDamage + WDamage)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < QDamage + EDamage + WDamage
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
            Killsteal();


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

        public static List<Obj_AI_Base> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range))
                .Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
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
            bool useQ = Menu["farming"]["lane"]["useq"].Enabled;
            bool useQk = Menu["farming"]["lane"]["useqk"].Enabled;
            if (useQk)
            {
                foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
                    {

                        if (minion.IsValidTarget(Q2.Range) && minion != null)
                        {

                            Q2.Cast(minion);

                        }
                    }
                }
            }
            if (useQ)
            {
                foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                    {

                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {

                            Q.Cast(minion);

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
            foreach (var jungleTarget in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || !jungleTarget.IsValidSpellTarget())
                {
                    return;
                }
                bool useQ = Menu["farming"]["jungle"]["useq"].Enabled;
                bool useE = Menu["farming"]["jungle"]["usee"].Enabled;
                bool useQk = Menu["farming"]["jungle"]["useqk"].Enabled;
                if (useQk)
                {
                    foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range))
                    {
                        if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
                        {

                            if (minion.IsValidTarget(Q2.Range) && minion != null)
                            {

                                Q2.Cast(minion);

                            }
                        }
                    }
                }
                if (useE)
                {
                    foreach (var minion in GetGenericJungleMinionsTargetsInRange(E.Range))
                    {
                        if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                        {

                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {

                                E.Cast(minion);

                            }
                        }
                    }
                }
                if (useQ)
                {
                    foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range))
                    {
                        if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {

                                Q.Cast(minion);

                            }
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

        private void Killsteal()
        {
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
            {
                if (Menu["killsteal"]["Skaarl"]["useq"].Enabled)
                {


                    var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);


                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(Q.Range))
                    {
                        if (Q.GetPrediction(bestTarget).CastPosition.Distance(Player) < Q.Range - 50)
                        {
                            Q.Cast(bestTarget);
                        }
                    }
                }
                if (Menu["killsteal"]["Skaarl"]["usee"].Enabled)
                {


                    var bestTarget = GetBestKillableHero(E, DamageType.Magical, false);


                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(bestTarget);
                    }

                }
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
            {
                if (Menu["killsteal"]["Skaarl"]["useq"].Enabled)
                {


                    var bestTarget = GetBestKillableHero(Q2, DamageType.Magical, false);


                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.Q, DamageStage.SecondForm) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(Q2.Range))
                    {
                        if (Q.GetPrediction(bestTarget).CastPosition.Distance(Player) < Q.Range - 100)
                        {
                            Q2.Cast(bestTarget);
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

        public static bool AnyWallInBetween(Vector3 startPos, Vector2 endPos)
        {
            for (var i = 0; i < startPos.Distance(endPos); i++)
            {
                var point = NavMesh.WorldToCell(startPos.Extend(endPos, i));
                if (point.Flags.HasFlag(NavCellFlags.Wall | NavCellFlags.Building))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnCombo()
        {

            bool useQs = Menu["combo"]["Skaarl"]["useq"].Enabled;
            bool useEs = Menu["combo"]["Skaarl"]["usee"].Enabled;
            bool useQk = Menu["combo"]["Kled"]["useq"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {
                return;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
            {
                if (useQk)
                {
                    if (Q.Ready && target.IsValidTarget(Q2.Range))
                    {

                        if (target != null)
                        {
                            if (Menu["combo"]["Kled"]["gap"].Enabled)
                            {
                                if (target.Distance(Player) > 400 && target.Distance(Player) < 600 &&
                                    target.Health < Player.Health - 150 &&
                                    !target.IsUnderEnemyTurret() &&
                                    target.Health > Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondCast))
                                {
                                    Q2.Cast(Player.ServerPosition.Extend(target.ServerPosition, -Q2.Range));
                                }
                            }
                            if (Q.GetPrediction(target).CastPosition.Distance(Player) < Q.Range - 100)
                            {
                                switch (Menu["combo"]["Kled"]["qmode"].As<MenuList>().Value)
                                {

                                    case 0:
                                        if (Player.Mana >= 80)
                                        {
                                            Q2.Cast(target);
                                        }
                                        if (Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) >=
                                            target.Health)
                                        {
                                            Q2.Cast(target);
                                        }
                                        if (Player.Health < target.Health && Player.HealthPercent() < 30)
                                        {

                                            Q2.Cast(target);
                                        }
                                        break;
                                    case 1:
                                        Q2.Cast(target);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
            {
                if (useEs)
                {
                    if (E.Ready && target.IsValidTarget(E.Range))
                    {

                        if (target != null)
                        {



                            if (!Player.HasBuff("KledE2"))
                            {
                                E.Cast(target);
                            }
                            if (target.HasBuff("klede2target"))
                            {
                                if (Player.HasBuff("KledE2") &&
                                    Game.ClockTime - Player.BuffManager.GetBuff("KledE2").StartTime > 2.5)
                                {

                                    E.Cast();
                                }
                                if (Player.HasBuff("KledE2") &&
                                    target.Distance(Player) > 300)
                                {

                                    E.Cast();
                                }
                                if (Player.HasBuff("KledE2") &&
                                    Player.GetSpellDamage(target, SpellSlot.E) > target.Health ||
                                    Player.GetSpellDamage(target, SpellSlot.E) +
                                    Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.Detonation) + Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                                {

                                    E.Cast();
                                }
                            }
                        }
                    }
                }
                if (useQs)
                {
                    if (Q.Ready && target.IsValidTarget(Q.Range))
                    {

                        if (target != null)
                        {
                            if (Q.GetPrediction(target).CastPosition.Distance(Player) < Q.Range-50)
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                }
                
            }
        }


        private void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;
            bool useQk = Menu["harass"]["useqk"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(Q.Range);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (useQk)
            {
                if (target != null)
                {
                    if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledRiderQ")
                    {

                        if (target.IsValidTarget(Q2.Range) && target != null)
                        {
                            if (Q.GetPrediction(target).CastPosition.Distance(Player) < Q.Range - 100)
                            {
                                Q2.Cast(target);
                            }
                        }
                    }

                }
            }
            if (useE)
            {
                if (target != null)
                {

                    if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                    {

                        if (target.IsValidTarget(E.Range) && target != null)
                        {

                            if (!Player.HasBuff("KledE2"))
                            {
                                E.Cast(target);
                            }
                            if (target.HasBuff("klede2target"))
                            {
                                if (Player.HasBuff("KledE2") &&
                                    Game.ClockTime - Player.BuffManager.GetBuff("KledE2").StartTime > 2.5)
                                {

                                    E.Cast();
                                }
                            }

                        }
                    }
                }

            }
            if (useQ)
            {
                if (target != null)
                {

                    if (Player.SpellBook.GetSpell(SpellSlot.Q).Name == "KledQ")
                    {


                        if (target.IsValidTarget(Q.Range) && target != null)
                        {

                            if (Q.GetPrediction(target).CastPosition.Distance(Player) < Q.Range - 50)
                            {
                                Q.Cast(target);
                            }

                        }
                    }

                }
            }
           
        }
    }
}

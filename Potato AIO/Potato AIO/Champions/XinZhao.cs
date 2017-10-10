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
using Aimtec.SDK.Events;
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
    class XinZhao : Champion
    {

        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useQAA = RootMenu["combo"]["useQAA"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;
            if (RootMenu["combo"]["items"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(600);
                if (target != null)
                {

                    if (Player.HasItem(ItemId.BladeoftheRuinedKing) || Player.HasItem(ItemId.BilgewaterCutlass))
                    {
                        var items = new[] {ItemId.BladeoftheRuinedKing, ItemId.BilgewaterCutlass};
                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                        if (slot != null)
                        {
                            var spellslot = slot.SpellSlot;
                            if (spellslot != SpellSlot.Unknown &&
                                Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                            {
                                Player.SpellBook.CastSpell(spellslot, target);
                            }
                        }
                    }

                }
            }
            if (useQA)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (Q.Ready && !useQAA && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q.Cast();
                            }
                        }
                    }
                }
            }
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (E.Ready && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (target.Distance(Player) > RootMenu["combo"]["minE"].As<MenuSlider>().Value)
                        {
                            E.CastOnUnit(target);
                        }
                        if (Player.GetSpellDamage(target, SpellSlot.E) >= target.Health)
                        {
                            E.CastOnUnit(target);
                        }
                        if (target.IsDashing())
                        {
                            E.CastOnUnit(target);
                        }

                    }
                }
            }
            if (RootMenu["combo"]["useW"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (W.Ready && target.IsValidTarget(W.Range))
                {

                    if (target != null)
                    {

                        if (RootMenu["combo"]["priorE"].Enabled)
                        {
                            if (!E.Ready)
                            {
                                W.Cast(target);
                            }
                        }
                        if (!RootMenu["combo"]["priorE"].Enabled)
                        {
                            
                                W.Cast(target);
                            
                        }

                    }
                }
            }
            if (RootMenu["combo"]["useR"].Enabled && R.Ready)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget(R.Range))
                {

                    if (target != null)
                    {
                        if (Player.CountEnemyHeroesInRange(R.Range) >= RootMenu["combo"]["minR"].As<MenuSlider>().Value)
                        {

                            R.Cast();

                        }
                    }
                }
            }



        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;


            if (RootMenu["farming"]["lane"]["useQ"].Enabled)
            {
                if (Q.Ready)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {

                            Q.Cast();


                        }
                    }
                }
            }


            if (useE)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                            minion.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                    {

                        if (minion.IsValidTarget(E.Range) && minion != null && !Player.HasBuff("GarenE"))
                        {
                            E.CastOnUnit(minion);
                        }
                    }
                }
            }
            if (RootMenu["farming"]["lane"]["useW"].Enabled)
            {

                if (W.Ready)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {
                        if (minion.IsValidTarget(W.Range) && minion != null)
                        {

                            W.Cast(minion);


                        }
                    }
                }
            }
            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(W.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                if (useQs && Q.Ready && jungleTarget.IsValidTarget(300))
                {
                    Q.Cast();
                }
                if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(jungleTarget);
                }
                if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range))
                {
                    W.Cast(jungleTarget);
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

        static double GetR(Obj_AI_Base target)
        {

            double meow = 0;
            double meoww = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 75;

            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 175;

            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 275;

            }
            double calc = target.Health;
            double mmmmm = calc * 0.15;
            double hhh = Player.TotalAttackDamage - Player.BaseAttackDamage;
            double full = meow + mmmmm + hhh;

            double damage = Player.CalculateDamage(target, DamageType.Physical, full);
            return damage;

        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawrdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
                    .ToList()
                    .ForEach(
                        unit =>
                        {

                            double Rdamage;

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
                                                     Player.GetSpellDamage(unit, SpellSlot.R) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R,
                                                         DamageStage.DamagePerSecond) * 7
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.R) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.R,
                                                                            DamageStage.DamagePerSecond) * 7)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.R) +
                                Player.GetSpellDamage(unit, SpellSlot.R, DamageStage.DamagePerSecond) * 7
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }


        }



        protected override void Killsteal()
        {

            if (RootMenu["combo"]["autor"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(R, DamageType.Physical, false);

                if (bestTarget != null &&
                    GetR(bestTarget) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    R.Cast();
                }
            }


        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!RootMenu["combo"]["useQAA"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (Q.Cast())
                {
                    Orbwalker.Implementation.ResetAutoAttackTimer();
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
                if (!RootMenu["harass"]["useQAA"].Enabled)
                {
                    return;
                }

                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (Q.Cast())
                {
                    Orbwalker.Implementation.ResetAutoAttackTimer();
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


                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range))
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

        protected override void Harass()
        {

            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useQAA = RootMenu["harass"]["useQAA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            if (useQA)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (Q.Ready && !useQAA && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                Q.Cast();
                            }
                        }
                    }
                }
            }
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (E.Ready && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (target.Distance(Player) > RootMenu["harass"]["minE"].As<MenuSlider>().Value)
                        {
                            E.CastOnUnit(target);
                        }
                        if (Player.GetSpellDamage(target, SpellSlot.E) >= target.Health)
                        {
                            E.CastOnUnit(target);
                        }
                        if (target.IsDashing())
                        {
                            E.CastOnUnit(target);
                        }

                    }
                }
            }
            if (RootMenu["harass"]["useW"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (W.Ready && target.IsValidTarget(W.Range))
                {

                    if (target != null)
                    {


                        W.Cast(target);

                    }
                }
            }
           
        }




        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            RootMenu.Add(new MenuKeyBind("insec", "Insec Key", KeyCode.T, KeybindType.Press));
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo", true));
                ComboMenu.Add(new MenuBool("useQAA", "^- Only for AA Reset", false));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("priorE", "^- Priority E"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuSlider("minE", "Min. E Range", 250, 0, 400));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("minR", "^- R if X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuBool("autor", "Auto R if Killable"));
                ComboMenu.Add(new MenuBool("items", "Use Items in Combo"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass", true));
                HarassMenu.Add(new MenuBool("useQAA", "^- Only for AA Reset", false));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuSlider("minE", "Min. E Range", 250, 0, 400));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "Min. minion for E", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            DrawMenu = new Menu("drawings", "Drawings");
            {

                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawrdamage", "Draw R Damage"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 300);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 800);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 650);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 500);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);
            W.SetSkillshot(0.4f, 40f, 2000, false, SkillshotType.Line);
        }

        protected override void SemiR()
        {
            if (RootMenu["insec"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range + 410);
                if (R.Ready)
                {
                    if (Flash.Ready && Flash != null && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(380))
                        {

                            foreach (var ally in GameObjects.AllyHeroes)
                            {
                                if (ally != null && UnitExtensions.Distance(ally, Player) < 1500 && !ally.IsMe)
                                {
                                    if (Flash.Cast(target.ServerPosition.Extend(ally.ServerPosition, -100)))
                                    {
                                        R.Cast();

                                    }
                                }


                            }
                            if (GameObjects.AllyHeroes.Where(x => UnitExtensions.Distance(x, Player) < E.Range)
                                    .Count() == 1)
                            {
                                if (Flash.Cast(target.ServerPosition.Extend(Player.ServerPosition, -100)))
                                {
                                    R.Cast();

                                }
                            }


                        }
                    }
                }
            }
        }

        protected override void LastHit()
        {

        }
    }
}
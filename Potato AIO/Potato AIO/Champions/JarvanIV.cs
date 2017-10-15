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
    class JarvanIV : Champion
    {
        protected override void Combo()
        {

            bool useQA = RootMenu["combo"]["useQA"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (E.Ready && useE && target.IsValidTarget(E.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        if (RootMenu["combo"]["useEQ"].Enabled)
                        {
                            if (Q.Ready)
                            {
                                var meowpred = E.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50)))
                                    {
                                        Q.Cast(target);
                                    }
                                }
                                if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                {
                                    if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80)))
                                    {
                                        Q.Cast(target);
                                    }
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                {
                                    if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120)))
                                    {
                                        Q.Cast(target);
                                    }
                                }
                                if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                {
                                    if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150)))
                                    {
                                        Q.Cast(target);
                                    }
                                }
                            }
                        }
                        if (!RootMenu["combo"]["useEQ"].Enabled)
                        {
                            E.Cast(target);
                        }
                    }
                }
            }

            if (R.Ready && RootMenu["combo"]["useR"].Enabled && target.IsValidTarget(R.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead&& !Player.HasBuff("JarvanIVCataclysm"))
                    {
                        if (target.CountEnemyHeroesInRange(300) >= RootMenu["combo"]["hitR"].As<MenuSlider>().Value)
                        {
                            R.Cast(target);
                        }
                        if (RootMenu["combo"]["forceR"].Enabled)
                        {
                            if (Player.GetSpellDamage(target, SpellSlot.Q) +
                                Player.GetSpellDamage(target, SpellSlot.E) +
                                Player.GetSpellDamage(target, SpellSlot.R) >= target.Health)
                            {
                                R.Cast(target);
                            }
                        }
                    }
                }
            }
            if (Q.Ready && useQA && target.IsValidTarget(Q.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        Q.Cast(target);
                    }
                }
            }
            if (W.Ready && RootMenu["combo"]["useW"].Enabled && target.IsValidTarget(W.Range))
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


        protected override void Farming()
        {

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {

                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
                bool useW = RootMenu["farming"]["lane"]["useW"].Enabled;

                if (useW)
                {

                    if (W.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {
                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {
                                if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range, false, false,
                                        Player.ServerPosition)) >= RootMenu["farming"]["lane"]["hitW"]
                                        .As<MenuSlider>().Value)
                                {

                                    W.Cast();
                                }
                            }
                        }

                    }
                }
                if (useQ)
                {
                    if (Q.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                var result = FarmHelper.GetLineClearLocation(Q.Range, 105);

                                if (result != null)
                                {
                                    if (result.numberOfMinionsHit >=
                                        RootMenu["farming"]["lane"]["hitQ"].As<MenuSlider>().Value)
                                    {
                                        Q.Cast(result.CastPosition);
                                    }


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

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;

                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range))
                    {
                        W.Cast();
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
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
            
                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                    if (useWs && W.Ready && jungleTarget.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }

                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimes;

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
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E) +
                                                          Player.GetSpellDamage(unit, SpellSlot.R))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
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
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["draweqr"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range+R.Range, 50, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["draweqflash"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range + 420, 50, Color.CornflowerBlue);
            }
        }



        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useQ"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
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
        }



        protected override void Harass()
        {


            bool useQA = RootMenu["harass"]["useQA"].Enabled;
            bool useE = RootMenu["harass"]["useE"].Enabled;
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;

            var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (manapercent < Player.ManaPercent())
            {
                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (RootMenu["harass"]["useEQ"].Enabled)
                            {
                                if (Q.Ready)
                                {
                                    var meowpred = E.GetPrediction(target);
                                    if (target.Distance(Player) <= 300)
                                    {
                                        if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50)))
                                        {
                                            Q.Cast(target);
                                        }
                                    }
                                    if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                    {
                                        if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80)))
                                        {
                                            Q.Cast(target);
                                        }
                                    }
                                    if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                    {
                                        if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120)))
                                        {
                                            Q.Cast(target);
                                        }
                                    }
                                    if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                    {
                                        if (E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150)))
                                        {
                                            Q.Cast(target);
                                        }
                                    }
                                }
                            }
                            if (!RootMenu["harass"]["useEQ"].Enabled)
                            {
                                E.Cast(target);
                            }
                        }
                    }
                }

            }
            if (Q.Ready && useQA && target.IsValidTarget(Q.Range))
            {
                if (target != null)
                {
                    if (!target.IsDead)
                    {
                        Q.Cast(target);
                    }
                }
            }
            if (W.Ready && RootMenu["harass"]["useW"].Enabled && target.IsValidTarget(W.Range))
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


        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQA", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useEQ", " ^- Wait for Q"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitR", "^- If Hits X Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuBool("forceR", "Force R if Killable with Combo"));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQA", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));
                HarassMenu.Add(new MenuBool("useEQ", " ^- Wait for Q"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hitQ", "^- If Hits X Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("useW", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitW", "^- If Hits X Minions", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("useE", "Use E to Killsteal"));

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
                DrawMenu.Add(new MenuBool("draww", "Draw W Range", false));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("draweqr", "Draw E > Q > R Range"));
                DrawMenu.Add(new MenuBool("draweqflash", "Draw E > Q > Flash Range"));

            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            var zzzzzz = new Menu("wset", "W Shielding");
            WShield.EvadeManager.Attach(zzzzzz);
            WShield.EvadeOthers.Attach(zzzzzz);
            WShield.EvadeTargetManager.Attach(zzzzzz);
            RootMenu.Add(zzzzzz);
            RootMenu.Add(DrawMenu);
            RootMenu.Add(new MenuKeyBind("eqr", "E > Q > R on Key", KeyCode.T, KeybindType.Press));
            RootMenu.Add(new MenuKeyBind("eqflash", "E > Q > Flash on Key", KeyCode.G, KeybindType.Press));

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




                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
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
        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                W.Cast();



            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 770);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 625);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 860);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 650);
            Q.SetSkillshot(0.55f, 70, float.MaxValue, false, SkillshotType.Line, false, HitChance.None);
            E.SetSkillshot(0.65f, 70, float.MaxValue, false, SkillshotType.Circle, false, HitChance.None);
                     if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);


        }

        protected override void SemiR()
        {
            if (RootMenu["flee"]["flee"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (Player.Mana > Player.GetSpell(SpellSlot.Q).Cost + Player.GetSpell(SpellSlot.E).Cost)
                {
                    if (E.Cast(Game.CursorPos))
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }
            }
            if (RootMenu["eqr"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (Player.Mana > Player.GetSpell(SpellSlot.Q).Cost + Player.GetSpell(SpellSlot.E).Cost +
                    Player.GetSpell(SpellSlot.R).Cost + 30 && R.Ready && !Player.HasBuff("JarvanIVCataclysm"))
                {
                    var target = TargetSelector.Implementation.GetTarget(Q.Range + R.Range);
                    if (target != null)
                    {
                        var meow = E.GetPrediction(target);
                        if (E.Cast(meow.CastPosition))
                        {
                            Q.Cast(meow.CastPosition);
                        }
                        if (target.IsValidTarget(R.Range))
                        {
                            
                            R.CastOnUnit(target);
                        }
                    }
                }
            }
            if (RootMenu["eqflash"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (Player.Mana > Player.GetSpell(SpellSlot.Q).Cost + Player.GetSpell(SpellSlot.E).Cost)
                {

                    var target = TargetSelector.Implementation.GetTarget(Q.Range + 420);
                    if (target != null)
                    {
                        if (Flash != null && Flash.Ready)
                        {
                            var meow = E.GetPrediction(target);
                            if (E.Cast(meow.CastPosition))
                            {
                                Q.Cast(meow.CastPosition);

                            }
                            if (target.Distance(Player) <= 420 && Player.HasBuff("jarvanivdragonstrikeph"))
                            {
                                Flash.Cast(target.ServerPosition.Extend(Player.ServerPosition, -100));
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

using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Damage.JSON;

namespace Fiora_By_Kornis
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

    internal class Fiora
    {
        public static Menu Menu = new Menu("Fiora By Kornis", "Fiora by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R;



        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 500);
            W = new Spell(SpellSlot.W, 750);
            E = new Spell(SpellSlot.E, 0);
            R = new Spell(SpellSlot.R, 500);
            W.SetSkillshot(0.75f, 80, 2000, false, SkillshotType.Line);

        }
        internal static Menu EvadeMenu { get; set; }
        public Fiora()
        {
            Orbwalker.Attach(Menu);

            var ComboMenu = new Menu("combo", "Combo");
            ComboMenu.Add(new MenuBool("items", "Use Items"));
            var QSet = new Menu("qset", "Q Settings");
            {
                QSet.Add(new MenuBool("useq", "Use Q in Combo"));
                QSet.Add(new MenuBool("vital", "Use Q to Passive Vital"));
                QSet.Add(new MenuBool("rvital", "Use Q to R Vital"));
                QSet.Add(new MenuBool("prevital", "Use Q in Pre-Vital Position", false));
                QSet.Add(new MenuBool("block", "Block Q to Vital if Player is Inside Vital"));
                QSet.Add(new MenuKeyBind("turret", "Enable Under Turret Toggle", KeyCode.Z, KeybindType.Toggle));
            }
             EvadeMenu = new Menu("wset", "W Settings");
            {
                SpellBlocking.EvadeManager.Attach(EvadeMenu);
               SpellBlocking.EvadeOthers.Attach(EvadeMenu);
              SpellBlocking.EvadeTargetManager.Attach(EvadeMenu);
            }
            var ESet = new Menu("eset", "E Settings");
            {
                ESet.Add(new MenuBool("usee", "Use E in Combo"));
                ESet.Add(new MenuBool("aae", "^- Only for AA Reset"));

            }
            var RSet = new Menu("rset", "R Settings");
            {
                RSet.Add(new MenuBool("user", "Use R in Combo"));
                RSet.Add(new MenuList("rmode", "R Mode", new[] {"At X Health", "If Killable"}, 1));
                RSet.Add(new MenuSlider("waster", "Don't waste R if Enemy HP lower than", 100, 0, 500));
                RSet.Add(new MenuSlider("hp", "R if Target has Health Percent (R Health Mode)", 55, 1));
                RSet.Add(new MenuSlider("vital", "R Vitals for Damage (R Killable)", 3, 1, 4));
                RSet.Add(new MenuBool("selected", "R Only Selected"));

            }
            Menu.Add(ComboMenu);
            ComboMenu.Add(QSet);
            ComboMenu.Add(EvadeMenu);
            ComboMenu.Add(ESet);
            ComboMenu.Add(RSet);
            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("vital", "Use Q to Passive Vital"));
                HarassMenu.Add(new MenuBool("prevital", "Use Q in Pre-Vital Position", false));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
                HarassMenu.Add(new MenuBool("aae", "^- Only for AA Reset"));

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            FarmMenu.Add(new MenuBool("items", "Use Items"));
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));

            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));

            }
            Menu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KSMenu.Add(new MenuBool("ksw", "Killsteal with W"));
            }
            Menu.Add(KSMenu);
            var FleeMenu = new Menu("flee", "Flee");
            {

                FleeMenu.Add(new MenuBool("useq", "Use Q to Flee"));
                FleeMenu.Add(new MenuKeyBind("key", "Flee Key:", KeyCode.G, KeybindType.Press));
            }
            Menu.Add(FleeMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawvital", "Draw Vitals"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Toggle"));
            }
            Menu.Add(DrawMenu);
            Menu.Attach();


            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
        
            Orbwalker.PostAttack += OnPostAttack;
            LoadSpells();

            Console.WriteLine("Fiora by Kornis - Loaded");
        }


        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["eset"]["aae"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                E.Cast();
                if (!E.Ready)
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
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Mixed))
            {
                if (!Menu["harass"]["aae"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                E.Cast();
                if (!E.Ready)
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

            if (Orbwalker.Mode.Equals(OrbwalkingMode.Laneclear))
            {
                if (!Menu["farming"]["items"].Enabled)
                {
                    return;
                }
                Obj_AI_Minion hero = args.Target as Obj_AI_Minion;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }

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

// This is wrong, but gives the same and correct Vital Damage. :3 // Don't bully for this. :c
        static double GetR(Obj_AI_Base target)
        {
            double test = (Player.TotalAttackDamage - Player.BaseAttackDamage) * 0.045;
            double aa = test * 0.02;
            double health = target.MaxHealth * aa / 2;
            double full = health + 200;
            double damage = Player.CalculateDamage(target, DamageType.True, full);
            return damage;

        }



        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
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

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidTarget(range)).ToList();
        }

        private void Render_OnPresent()
        {
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;

            if (Menu["drawings"]["drawtoggle"].Enabled)
            {
                if (Menu["combo"]["qset"]["turret"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.HotPink, "Use Under-Turret: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["combo"]["qset"]["turret"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.HotPink, "Use Under-Turret: OFF",
                        RenderTextFlags.VerticalCenter);
                }
            }
       
            
            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }

            if (Menu["drawings"]["drawdamage"].Enabled)
            {
                double QDamage = 0;
                double WDamage = 0;
                double RDamage = 0;

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
                    .ToList()
                    .ForEach(
                        unit =>
                        {

                            if (Q.Ready)
                            {
                                QDamage = Player.GetSpellDamage(unit, SpellSlot.Q);
                            }
                            if (W.Ready)
                            {
                                WDamage = Player.GetSpellDamage(unit, SpellSlot.W);
                            }
                            if (R.Ready)
                            {
                                RDamage = GetR(unit) * (Menu["combo"]["rset"]["vital"].As<MenuSlider>().Value-1);
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
                                                     QDamage +
                                                     WDamage +
                                                     RDamage + GetR(unit)

                                             ? width * ((unit.Health - (QDamage +
                                                                        WDamage +
                                                                        RDamage + GetR(unit))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < QDamage +
                                WDamage +
                                RDamage + GetR(unit)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
            if (Menu["drawings"]["drawvital"].Enabled)
            {
                foreach (var passive in GameObjects.AllGameObjects)
                {
                    if (passive.IsVisible)
                    {
                        if (passive.Name == "Fiora_Base_Passive_NW.troy" ||
                            passive.Name == "Fiora_Base_Passive_NW_Warning.troy" ||
                            passive.Name == "Fiora_Base_Passive_NW_Timeout.troyy" ||
                            passive.Name == "Fiora_Base_R_Mark_NW_FioraOnly.troy" ||
                            passive.Name == "Fiora_Base_R_NW_Timeout_FioraOnly.troy" && passive.IsValid)
                        {
                            var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                passive.ServerPosition.Z);
                            Render.Circle(miau, 40, 50, Color.LimeGreen);

                        }
                        if (passive.Name == "Fiora_Base_Passive_SE.troy" ||
                            passive.Name == "Fiora_Base_Passive_SE_Warning.troy" ||
                            passive.Name == "Fiora_Base_Passive_SE_Timeout.troy" ||
                            passive.Name == "Fiora_Base_R_Mark_SE_FioraOnly.troy" ||
                            passive.Name == "Fiora_Base_R_SE_Timeout_FioraOnly.troy" && passive.IsValid)
                        {
                            var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                passive.ServerPosition.Z);
                            Render.Circle(miau, 40, 50, Color.LimeGreen);

                        }
                        if (passive.Name == "Fiora_Base_Passive_NE.troy" ||
                            passive.Name == "Fiora_Base_Passive_NE_Warning.troy" ||
                            passive.Name == "Fiora_Base_Passive_NE_Timeout.troy" ||
                            passive.Name == "Fiora_Base_R_Mark_NE_FioraOnly.troy" ||
                            passive.Name == "Fiora_Base_R_NE_Timeout_FioraOnly.troy" && passive.IsValid)
                        {
                            var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                passive.ServerPosition.Z + 200);
                            Render.Circle(miau, 40, 50, Color.LimeGreen);

                        }
                        if (passive.Name == "Fiora_Base_Passive_SW.troy" ||
                            passive.Name == "Fiora_Base_Passive_SW_Warning.troy" ||
                            passive.Name == "Fiora_Base_Passive_SW_Timeout.troy" ||
                            passive.Name == "Fiora_Base_R_Mark_SW_FioraOnly.troy" ||
                            passive.Name == "Fiora_Base_R_SW_Timeout_FioraOnly.troy" && passive.IsValid)
                        {
                            var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                passive.ServerPosition.Z - 200);
                            Render.Circle(miau, 40, 50, Color.LimeGreen);

                        }

                    }
                }
               
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
            if (Menu["flee"]["key"].Enabled)
            {
                Flee();
            }
            Killsteal();
        
        }
        

        public static Obj_AI_Hero GetBestKillableHero(Spell spell, DamageType damageType = DamageType.True,
            bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(spell.Range).FirstOrDefault(t => t.IsValidTarget());
        }

        private void Killsteal()
        {
            if (Q.Ready &&
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(W, DamageType.Physical, false);
                if (bestTarget != null)
                {
                    if (GetR(bestTarget) >= bestTarget.Health
                        && bestTarget.IsValidTarget(Q.Range + 200))
                    {
                        foreach (var passive in GameObjects.AllGameObjects)
                        {
                            if (passive.IsVisible)
                            {
                                if (passive.Name == "Fiora_Base_Passive_NW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NW_Timeout.troyy" ||
                                    passive.Name == "Fiora_Base_R_Mark_NW_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_NW_Timeout_FioraOnly.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SE_Timeout.troy" ||
                                    passive.Name == "Fiora_Base_R_Mark_SE_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_SE_Timeout_FioraOnly.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_NE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NE_Timeout.troy" ||
                                    passive.Name == "Fiora_Base_R_Mark_NE_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_NE_Timeout_FioraOnly.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SW_Timeout.troy" ||
                                    passive.Name == "Fiora_Base_R_Mark_SW_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_SW_Timeout_FioraOnly.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);
                                    }
                                }

                            }
                        }
                    }
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(bestTarget);
                    }

                }
            }

            if (W.Ready &&
                Menu["killsteal"]["ksw"].Enabled)
            {
                var bestTarget = GetBestKillableHero(W, DamageType.Physical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {
                    W.Cast(bestTarget);
                }
            }



        }


        public void Flee()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            bool useQ = Menu["flee"]["useq"].Enabled;
            if (useQ)
            {

                Q.Cast(Game.CursorPos);
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
            if (target != null && target.IsValidTarget() && !Invulnerable.Check(target))
            {
                return target;
            }

            var firstTarget = ts.GetOrderedTargets(range)
                .FirstOrDefault(t => t.IsValidTarget() && !Invulnerable.Check(t));
            if (firstTarget != null)
            {
                return firstTarget;
            }

            return null;
        }

        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.Jungle.Where(m => !GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range)).ToList();
        }
        private void Clearing()
        {
            bool useQ = Menu["farming"]["lane"]["useq"].Enabled;
            bool useE = Menu["farming"]["lane"]["usee"].Enabled;
            float manapercent = Menu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                if (useQ)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {


                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            Q.CastOnUnit(minion);
                        }
                    }
                }
                if (useE)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(300))
                    {


                        if (minion.IsValidTarget(300) && minion != null)
                        {

                            E.Cast();
                        }
                    }
                }
            }
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
                float manapercent = Menu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (useQ && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.CastOnUnit(jungleTarget);
                    }
                    if (useE && jungleTarget.IsValidTarget(300))
                    {
                        E.Cast();
                    }
                }

            }
        }
        public void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useE = Menu["harass"]["aae"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(W.Range);
            if (!target.IsValidTarget())
            {
                return;
            }
            if (Player.ManaPercent() >= Menu["harass"]["mana"].As<MenuSlider>().Value)
            {
                if (useQ)
                {
                    foreach (var passive in GameObjects.AllGameObjects)
                    {
                        if (passive.IsVisible)
                        {


                            if (Menu["harass"]["vital"].Enabled)
                            {
                                if (passive.Name == "Fiora_Base_Passive_NW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NW_Timeout.troyy"
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {


                                        Q.Cast(miau);

                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SE_Timeout.troy"
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {

                                        Q.Cast(miau);

                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_NE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NE_Timeout.troy"
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {

                                        Q.Cast(miau);

                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SW_Timeout.troy"
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);

                                    }
                                }

                            }

                            if (Menu["harass"]["prevital"].Enabled)
                            {
                                if (
                                    passive.Name == "Fiora_Base_Passive_NW_Warning.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {

                                        Q.Cast(miau);

                                    }

                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SE_Warning.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);

                                    }
                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_NE_Warning.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        Q.Cast(miau);

                                    }
                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SW_Warning.troy" && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {

                                        Q.Cast(miau);

                                    }

                                }
                            }
                        }
                    }
                }

                if (useE)
                {
                    if (target != null && target.IsValidTarget(300) && !Menu["harass"]["aae"].Enabled)
                    {
                        E.Cast();
                    }
                }
            }
        }


        private void OnCombo()
        {
            bool useQ = Menu["combo"]["qset"]["useq"].Enabled;
            bool useE = Menu["combo"]["eset"]["usee"].Enabled;
            bool useR = Menu["combo"]["rset"]["user"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(W.Range);

            if (!target.IsValidTarget())
            {
                return;
            }
            var miau1 = new Vector3();
            var miau2 = new Vector3();
            var miau3 = new Vector3();
            var miau4 = new Vector3();
            if (useQ)
            {
                foreach (var passive in GameObjects.AllGameObjects)
                {
                    if (passive.IsVisible && passive.Distance(target) < 10)
                    {
                        if (Menu["combo"]["qset"]["turret"].Enabled)
                        {
                            if (Menu["combo"]["qset"]["vital"].Enabled)
                            {
                                if (Menu["combo"]["qset"]["rvital"].Enabled)
                                {
                                    if (
                                        passive.Name == "Fiora_Base_R_Mark_NW_FioraOnly.troy" ||
                                        passive.Name == "Fiora_Base_R_NW_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                    {
                                        miau1 = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                            passive.ServerPosition.Z);
   

                                    }
                                    if (
                                        passive.Name == "Fiora_Base_R_Mark_SE_FioraOnly.troy" ||
                                        passive.Name == "Fiora_Base_R_SE_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                    {
                                        miau2 = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                            passive.ServerPosition.Z);
                        

                                    }
                                    if (
                                        passive.Name == "Fiora_Base_R_Mark_NE_FioraOnly.troy" ||
                                        passive.Name == "Fiora_Base_R_NE_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                    {
                                        miau3 = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                            passive.ServerPosition.Z + 200);
                                        

                                    }
                                    if (
                                        passive.Name == "Fiora_Base_R_Mark_SW_FioraOnly.troy" ||
                                        passive.Name == "Fiora_Base_R_SW_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                    {
                                        miau4 = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                            passive.ServerPosition.Z - 200);
                                       
                      
                                    }
                                   
                                }
                                if (passive.Name == "Fiora_Base_Passive_NW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NW_Timeout.troyy"
                                    && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SE_Timeout.troy"
                                    && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_NE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NE_Timeout.troy"
                                    && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SW_Timeout.troy"
                                    && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }
                                }

                            }
                            
                            if (Menu["combo"]["qset"]["prevital"].Enabled)
                            {
                                if (
                                    passive.Name == "Fiora_Base_Passive_NW_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SE_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_NE_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SW_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                            }
                        }
                        if (!Menu["combo"]["qset"]["turret"].Enabled)
                        {
                            if (Menu["combo"]["qset"]["rvital"].Enabled)
                            {
                                if (
                                    passive.Name == "Fiora_Base_R_Mark_NW_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_NW_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    miau1 = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                   

                                }
                                if (
                                    passive.Name == "Fiora_Base_R_Mark_SE_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_SE_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                   miau2 = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                  

                                }
                                if (
                                    passive.Name == "Fiora_Base_R_Mark_NE_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_NE_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                   miau3 = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                  

                                }
                                if (
                                    passive.Name == "Fiora_Base_R_Mark_SW_FioraOnly.troy" ||
                                    passive.Name == "Fiora_Base_R_SW_Timeout_FioraOnly.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    miau4 = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                
                                }
                            }
                            if (Menu["combo"]["qset"]["vital"].Enabled)
                            {
                                if (passive.Name == "Fiora_Base_Passive_NW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NW_Timeout.troyy" && passive.Distance(target) < 200
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range)
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled && !miau.PointUnderEnemyTurret())
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SE_Timeout.troy" && passive.Distance(target) < 200
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_NE.troy" ||
                                    passive.Name == "Fiora_Base_Passive_NE_Timeout.troy" && passive.Distance(target) < 200
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (passive.Name == "Fiora_Base_Passive_SW.troy" ||
                                    passive.Name == "Fiora_Base_Passive_SW_Timeout.troy" && passive.Distance(target) < 200
                                    && passive.IsValid)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }
                                }

                            }
                            

                            if (Menu["combo"]["qset"]["prevital"].Enabled)
                            {
                                if (
                                    passive.Name == "Fiora_Base_Passive_NW_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X + 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SE_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X - 200, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }
                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_NE_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z + 200);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }
                                }
                                if (
                                    passive.Name == "Fiora_Base_Passive_SW_Warning.troy" && passive.IsValid && passive.Distance(target) < 200)
                                {
                                    var miau = new Vector3(passive.ServerPosition.X, passive.ServerPosition.Y,
                                        passive.ServerPosition.Z - 200);
                                    if (miau.Distance(Player) < Q.Range && !miau.PointUnderEnemyTurret())
                                    {
                                        if (!Menu["combo"]["qset"]["block"].Enabled)
                                        {
                                            Q.Cast(miau);
                                        }
                                        if (Menu["combo"]["qset"]["block"].Enabled && miau.Distance(Player) > 150)
                                        {
                                            Q.Cast(miau);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                if (miau1.Distance(Player) < 150 || miau2.Distance(Player) < 150 ||
                    miau3.Distance(Player) < 150 || miau4.Distance(Player) < 150)

                {
                 
                    return;
                }
                if (miau1.Distance(Player) < Q.Range)
                {

                    if (!Menu["combo"]["qset"]["block"].Enabled)
                    {
                        Q.Cast(miau1);
                    }

                    if (Menu["combo"]["qset"]["block"].Enabled && miau1.Distance(Player) > 150)
                    {

                        Q.Cast(miau1);
                    }
                }
                if (miau2.Distance(Player) < Q.Range)
                {

                    if (!Menu["combo"]["qset"]["block"].Enabled)
                    {

                        Q.Cast(miau2);
                    }

                    if (Menu["combo"]["qset"]["block"].Enabled && miau2.Distance(Player) > 150)
                    {

                        Q.Cast(miau2);
                    }
                }
                if (miau3.Distance(Player) < Q.Range)
                {

                    if (!Menu["combo"]["qset"]["block"].Enabled)
                    {
                        Q.Cast(miau3);
                    }

                    if (Menu["combo"]["qset"]["block"].Enabled && miau3.Distance(Player) > 150)
                    {

                        Q.Cast(miau3);
                    }
                }
                if (miau4.Distance(Player) < Q.Range)
                {

                    if (!Menu["combo"]["qset"]["block"].Enabled)
                    {
                        Q.Cast(miau4);
                    }

                    if (Menu["combo"]["qset"]["block"].Enabled && miau4.Distance(Player) > 150)
                    {

                        Q.Cast(miau4);
                    }
                }
            }

            if (useE)
            {
                if (target != null && target.IsValidTarget(300) && !Menu["combo"]["eset"]["aae"].Enabled)
                {
                    E.Cast();
                }
            }
            var selected = TargetSelector.GetSelectedTarget();
            if (useR)
            {

                if (Menu["combo"]["rset"]["selected"].Enabled)

                {
                    if (selected != null && selected.IsValidTarget(R.Range))
                    {
                        switch (Menu["combo"]["rset"]["rmode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (selected.HealthPercent() <= Menu["combo"]["rset"]["hp"].As<MenuSlider>().Value &&
                                    selected.Health >= Menu["combo"]["rset"]["waster"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(selected);
                                }

                                break;
                            case 1:
                                if (selected.Health <= GetR(selected) * Menu["combo"]["rset"]["vital"].As<MenuSlider>().Value + Player.GetSpellDamage(selected, SpellSlot.Q) +
                                    Player.GetSpellDamage(selected, SpellSlot.W) && selected.Health >=
                                    Menu["combo"]["rset"]["waster"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(selected);
                                }

                                break;
                        }
                    }
                }
                if (!Menu["combo"]["rset"]["selected"].Enabled)

                {
                    if (target != null && target.IsValidTarget(R.Range))
                    {
                        switch (Menu["combo"]["rset"]["rmode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (target.HealthPercent() <=
                                    Menu["combo"]["rset"]["hp"].As<MenuSlider>().Value &&
                                    target.Health >= Menu["combo"]["rset"]["waster"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }

                                break;
                            case 1:
                                if (target.Health <= GetR(target) * Menu["combo"]["rset"]["vital"].As<MenuSlider>().Value +
                                    Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) && target.Health >=
                                    Menu["combo"]["rset"]["waster"].As<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }

                                break;

                        }
                    }
                }
            }
        }
    }
}
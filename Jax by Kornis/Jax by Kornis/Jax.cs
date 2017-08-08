using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Events;

namespace Jax_By_Kornis
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

    internal class Jax
    {
        public static Menu Menu = new Menu("Jax By Kornis", "Jax by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R, Smites;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 300);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 0);
            var smiteSlot = Player.SpellBook.Spells.FirstOrDefault(x => x.SpellData.Name.ToLower().Contains("smite"));
            if (smiteSlot != null)
            {

                Smites = new Spell(smiteSlot.Slot, 700);

            }
        }

        public Jax()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            var QSet = new Menu("qset", "Q Settings");
            {
                QSet.Add(new MenuList("qmode", "Combo Mode", new[] {"Q>E", "E>Q"}, 1));
                QSet.Add(new MenuBool("useq", "Use Q in Combo"));
                QSet.Add(new MenuSlider("rangeq", "Q Min. Range", 300, 1, 400));
            }
            var WSet = new Menu("wset", "W Settings");
            {
                WSet.Add(new MenuBool("usew", "Use W in Combo"));
                WSet.Add(new MenuBool("waa", "^- Only for W AA Reset"));
            }
            var ESet = new Menu("eset", "E Settings");
            {
                ESet.Add(new MenuList("emode", "E Mode", new[] {"Instant", "Delayed"}, 1));
                ESet.Add(new MenuKeyBind("under", "Mode Changing", KeyCode.T, KeybindType.Toggle));
                ESet.Add(new MenuBool("usee", "Use E in Combo"));
                ESet.Add(new MenuSlider("delay", "E Delay", 1000, 0, 2000));

            }
            var RSet = new Menu("rset", "R Settings");
            {
                RSet.Add(new MenuBool("user", "Use R"));
                RSet.Add(new MenuSlider("enemies", "Min. enemies for R", 2, 1, 5));
                RSet.Add(new MenuSlider("hp", "Min. HP for R", 30, 10, 100));
            }
            ComboMenu.Add(new MenuBool("items", "Use Items"));
            ComboMenu.Add(new MenuBool("hydraa", "^- Hyra use Always", false));


            Menu.Add(ComboMenu);
            ComboMenu.Add(QSet);
            ComboMenu.Add(WSet);
            ComboMenu.Add(ESet);
            ComboMenu.Add(RSet);
            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));

                HarassMenu.Add(new MenuList("qmode", "Harass Mode", new[] {"Q>E", "E>Q"}, 1));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuSlider("rangeq", "Q Min. Range", 300, 1, 400));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("waa", "^- Only for W AA Reset"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            {
                FarmMenu.Add(new MenuKeyBind("toggle", "Farm Toggle", KeyCode.Z, KeybindType.Toggle));
                FarmMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
                FarmMenu.Add(new MenuBool("usew", "Use W to Farm"));
                FarmMenu.Add(new MenuBool("usee", "Use E to Farm"));
                FarmMenu.Add(new MenuSlider("delay", "E Delay", 1000, 0, 2000));
            }
            Menu.Add(FarmMenu);
            var SmiteMenu = new Menu("smite", "Smite Settings");
            {
                SmiteMenu.Add(new MenuBool("SmiteUse", "Use Smite on Monsters"));
                SmiteMenu.Add(new MenuBool("SmiteUseHeroes", "Use Smite on Champions"));
                SmiteMenu.Add(new MenuKeyBind("smitekey", "Smite Toggle", KeyCode.Z, KeybindType.Toggle));
            }
            Menu.Add(SmiteMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawsmite", "Draw Smite"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw E Toggle"));
                DrawMenu.Add(new MenuBool("toggles", "Draw Farm Toggle"));

            }
            Menu.Add(DrawMenu);
            var FleeMenu = new Menu("flee", "Wardjump");
            {
                FleeMenu.Add(new MenuBool("useq", "Use Q to Wardjump"));
                FleeMenu.Add(new MenuKeyBind("key", "Wardjump Key:", KeyCode.G, KeybindType.Press));
            }
            Menu.Add(FleeMenu);
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.PostAttack += OnPostAttack;
            LoadSpells();
            Console.WriteLine("Jax by Kornis - Loaded");
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};

        private static readonly string[] SmiteMobs =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air",
            "SRU_Dragon_Elder", "SRU_Baron", "SRU_RiftHerald"
        };

        private static int delay;
        private float hello;
        private float helloo;
        private float helloooo;

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["wset"]["waa"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                W.Cast();
                if (!W.Ready && !Menu["combo"]["hydraa"].Enabled)
                {
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
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Mixed))
            {
                if (!Menu["harass"]["waa"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                W.Cast();
                if (!W.Ready)
                {
                    if (!W.Ready && !Menu["combo"]["hydraa"].Enabled)
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
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Laneclear))
            {
                if (!Menu["farming"]["usew"].Enabled)
                {
                    return;
                }
                Obj_AI_Minion hero = args.Target as Obj_AI_Minion;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                W.Cast();
            }


        }

        private static int SmiteDamages
        {
            get
            {
                int[] Hello = new int[]
                    {390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000};

                return Hello[Player.Level-1];
            }
        }

        public static void SmiteUse()
        {

            if (Menu["smite"]["smitekey"].Enabled)
            {
                if (Smites != null)
                {
                    
                    if (Menu["smite"]["SmiteUse"].Enabled)
                    {
                        var minion = GameObjects.Jungle.Where(x => x.IsValidTarget(Smites.Range));
                        foreach (var m in minion)
                        {
                            if (m != null && SmiteMobs.Contains(m.UnitSkinName))
                            {

                                if (m.Distance(Player) <= Smites.Range)
                                {


                                    if (SmiteDamages >= m.Health)
                                    {

                                        Smites.Cast(m);

                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }

        private void Render_OnPresent()
        {
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;

            if (Menu["drawings"]["drawtoggle"].Enabled)
            {
                switch (Menu["combo"]["eset"]["emode"].As<MenuList>().Value)
                {
                    case 0:
                        Render.Text(xaOffset - 50, yaOffset + 40, Color.Yellow, "E Mode: Instant",
                            RenderTextFlags.VerticalCenter);
                        break;
                    case 1:

                        Render.Text(xaOffset - 50, yaOffset + 40, Color.Yellow, "E Mode: Delayed",
                            RenderTextFlags.VerticalCenter);

                        break;
                }
            }
            if (Menu["drawings"]["toggles"].Enabled)
            {
                if (Menu["farming"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 25, Color.LawnGreen, "Farm: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["farming"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 25, Color.Red, "Farm: OFF",
                        RenderTextFlags.VerticalCenter);
                }
            }
            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawsmite"].Enabled)
            {

                if (Menu["smite"]["smitekey"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.LimeGreen, "Smite: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["smite"]["smitekey"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.Red, "Smite:  OFF",
                        RenderTextFlags.VerticalCenter);



                }
            }
        }



        // Don't bully me for this :c
        public static void WardJump()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            if (!Q.Ready)
            {
                return;
            }
            Vector3 wardJumpPosition = (Player.Position.Distance(Game.CursorPos) < 600)
                ? Game.CursorPos
                : Player.Position.Extend(Game.CursorPos, 600);
            Obj_AI_Base entityToWardJump = ObjectManager.Get<Obj_AI_Base>().ToArray().FirstOrDefault(obj =>
                obj.Position.Distance(wardJumpPosition) < 150
                && (obj is Obj_AI_Minion || obj is Obj_AI_Base)
                && !obj.IsMe && !obj.IsDead
                && obj.Position.Distance(Player.Position) < Q.Range);

            if (entityToWardJump != null)
            {
                Q.Cast(entityToWardJump);
            }
            else if (delay < Game.TickCount)
            {

                if (Player.HasItem(ItemId.TrackersKnife))
                {

                    var items = new[] {ItemId.TrackersKnife};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {

                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.ControlWard))
                {
                    var items = new[] {ItemId.ControlWard};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.WardingTotemTrinket))
                {
                    var items = new[] {ItemId.WardingTotemTrinket};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.RubySightstone))
                {
                    var items = new[] {ItemId.RubySightstone};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.Sightstone))
                {
                    var items = new[] {ItemId.Sightstone};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.EnchantmentWarrior))
                {
                    var items = new[] {ItemId.EnchantmentWarrior};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.EnchantmentCinderhulk))
                {
                    var items = new[] {ItemId.EnchantmentCinderhulk};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.EnchantmentRunicEchoes))
                {
                    var items = new[] {ItemId.EnchantmentRunicEchoes};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
                        }
                    }
                }
                if (Player.HasItem(ItemId.EnchantmentBloodrazor))
                {
                    var items = new[] {ItemId.EnchantmentBloodrazor};
                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                    if (slot != null)
                    {
                        var spellslot = slot.SpellSlot;
                        if (spellslot != SpellSlot.Unknown &&
                            Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                        {
                            if (!AnyWallInBetween(wardJumpPosition))
                            {
                                if (Player.SpellBook.CastSpell(spellslot, wardJumpPosition))
                                {
                                    delay = Game.TickCount + 1000;
                                }
                                Q.Cast(ObjectManager.Get<Obj_AI_Base>().ToArray()
                                    .FirstOrDefault(obj => obj.Position.Distance(wardJumpPosition) < 150 &&
                                                           obj is Obj_AI_Minion &&
                                                           obj.Position.Distance(Player.Position) < Q.Range));
                            }
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
            SmiteUse();
            if (Menu["combo"]["eset"]["under"].Enabled)
            {

                Menu["combo"]["eset"]["emode"].As<MenuList>().Value = 1;

            }
            if (!Menu["combo"]["eset"]["under"].Enabled)
            {
                Menu["combo"]["eset"]["emode"].As<MenuList>().Value = 0;
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
            if (Menu["flee"]["key"].Enabled && Menu["flee"]["useq"].Enabled)
            {
                WardJump();
            }

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
            if (Menu["farming"]["toggle"].Enabled)
            {
                bool useQ = Menu["farming"]["useq"].Enabled;
                bool useE = Menu["farming"]["usee"].Enabled;
                float delay = Menu["farming"]["delay"].As<MenuSlider>().Value;
                float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;

                foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {


                    if (manapercent < Player.ManaPercent())
                    {
                        if (useQ && minion.IsValidTarget(Q.Range))
                        {
                            Q.CastOnUnit(minion);
                        }
                        if (useE && minion.IsValidTarget(E.Range))
                        {
                            if (!Player.HasBuff("JaxCounterStrike"))
                            {
                                if (E.Cast())
                                {
                                    hello = Game.TickCount + delay;
                                }
                            }
                            if (Player.HasBuff("JaxCounterStrike"))
                            {
                                if (hello < Game.TickCount)
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
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
            if (Menu["farming"]["toggle"].Enabled)
            {
                foreach (var jungleTarget in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || !jungleTarget.IsValidSpellTarget())
                    {
                        return;
                    }
                    bool useQ = Menu["farming"]["useq"].Enabled;
                    bool useE = Menu["farming"]["usee"].Enabled;
                    float delay = Menu["farming"]["delay"].As<MenuSlider>().Value;
                    float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;
                    if (manapercent < Player.ManaPercent())
                    {
                        if (useQ && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.CastOnUnit(jungleTarget);
                        }
                        if (useE && jungleTarget.IsValidTarget(E.Range))
                        {
                            if (!Player.HasBuff("JaxCounterStrike"))
                            {
                                if (E.Cast())
                                {
                                    hello = Game.TickCount + delay;
                                }
                            }
                            if (Player.HasBuff("JaxCounterStrike"))
                            {
                                if (hello < Game.TickCount)
                                {
                                    E.CastOnUnit(jungleTarget);
                                }
                            }
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

        public static bool AnyWallInBetween(Vector3 startPos)
        {

            var point = NavMesh.WorldToCell(startPos);
            if (point.Flags.HasFlag(NavCellFlags.Wall | NavCellFlags.Building))
            {
                return true;
            }


            return false;
        }


        private void OnCombo()
        {

            bool useQ = Menu["combo"]["qset"]["useq"].Enabled;
            bool useW = Menu["combo"]["wset"]["usew"].Enabled;
            bool useE = Menu["combo"]["eset"]["usee"].Enabled;
            bool useR = Menu["combo"]["rset"]["user"].Enabled;
            float delay = Menu["combo"]["eset"]["delay"].As<MenuSlider>().Value;
            float hp = Menu["combo"]["rset"]["hp"].As<MenuSlider>().Value;
            float enemies = Menu["combo"]["rset"]["enemies"].As<MenuSlider>().Value;
            var target = GetBestEnemyHeroTargetInRange(Q.Range+200);


            if (!target.IsValidTarget())
            {
                return;
            }
            if (Menu["smite"]["smitekey"].Enabled)
            {
                if (Smites != null)
                {
                    if (Menu["smite"]["SmiteUseHeroes"].Enabled)
                    {

                        if (target.IsValidTarget(Smites.Range) && target != null)
                        {
                            Smites.CastOnUnit(target);
                        }
                    }
                }
            }
            if (Menu["combo"]["items"].Enabled)
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
            if (Menu["combo"]["hydraa"].Enabled && target.Distance(Player) < 400)
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
            switch (Menu["combo"]["qset"]["qmode"].As<MenuList>().Value)
            {
                case 0:
                    if (useQ && target.IsValidTarget(Q.Range) && target != null)
                    {
                        if (target.Distance(Player) > Menu["combo"]["qset"]["rangeq"].As<MenuSlider>().Value)
                        {
                            Q.CastOnUnit(target);
                        }
                        if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                        {
                            Q.CastOnUnit(target);
                        }
                        if (target.IsDashing())
                        {
                            Q.CastOnUnit(target);
                        }
                    }
                    if (useW && !Menu["combo"]["wset"]["waa"].Enabled && target != null)
                    {
                        if (target.IsValidTarget(300))
                        {
                            W.Cast();
                        }
                    }
                    switch (Menu["combo"]["eset"]["emode"].As<MenuList>().Value)
                    {
                        case 0:
                            if (useE && target != null && target.IsValidTarget(E.Range))
                            {
                                if (!Player.HasBuff("JaxCounterStrike"))
                                {

                                    E.Cast();
                                }
                                if (Player.HasBuff("JaxCounterStrike"))
                                {

                                    E.CastOnUnit(target);
                                }
                            }


                            break;
                        case 1:
                            if (useE && target != null && target.IsValidTarget(E.Range))
                            {
                                if (!Player.HasBuff("JaxCounterStrike"))
                                {
                                    if (E.Cast())
                                    {
                                        helloooo = Game.TickCount + delay;
                                    }
                                }
                                if (Player.HasBuff("JaxCounterStrike"))
                                {
                                    if (helloooo < Game.TickCount)
                                    {
                                        E.CastOnUnit(target);
                                    }
                                }
                            }
                            break;

                    }
                    break;
                case 1:
                    if (target.IsValidTarget(Q.Range + 100) && target != null)
                    {
                        switch (Menu["combo"]["eset"]["emode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (useE && target != null)
                                {
                                    if (!Player.HasBuff("JaxCounterStrike"))
                                    {

                                        E.Cast();
                                    }
                                    if (Player.HasBuff("JaxCounterStrike"))
                                    {

                                        E.CastOnUnit(target);
                                    }
                                }


                                break;
                            case 1:
                                if (useE && target != null)
                                {
                                    if (!Player.HasBuff("JaxCounterStrike"))
                                    {
                                        if (E.Cast())
                                        {
                                            helloooo = Game.TickCount + delay;
                                        }
                                    }
                                    if (Player.HasBuff("JaxCounterStrike"))
                                    {
                                        if (helloooo < Game.TickCount)
                                        {
                                            E.CastOnUnit(target);
                                        }
                                    }
                                }
                                break;

                        }
                    }
                    if (helloo < Game.TickCount)
                    {
                        if (useQ && target != null)
                        {
                            if (target.Distance(Player) > Menu["combo"]["qset"]["rangeq"].As<MenuSlider>().Value)
                            {
                                Q.CastOnUnit(target);
                            }
                            if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                            {
                                Q.CastOnUnit(target);
                            }
                            if (target.IsDashing())
                            {
                                Q.CastOnUnit(target);
                            }
                        }
                    }
                    if (!E.Ready)
                    {
                        if (useQ && target != null)
                        {
                            if (target.Distance(Player) > Menu["combo"]["qset"]["rangeq"].As<MenuSlider>().Value)
                            {
                                Q.Cast(target);
                            }
                            if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                            {
                                Q.Cast(target);
                            }
                            if (target.IsDashing())
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useW && !Menu["combo"]["wset"]["waa"].Enabled && target != null)
                    {
                        if (target.IsValidTarget(300))
                        {
                            W.Cast();
                        }
                    }
                    break;
            }
            if (useR)
            {
                if (target != null && enemies <= Player.CountEnemyHeroesInRange(700) && Player.HealthPercent() <= hp)
                {
                    R.Cast();
                }
            }
        }


        private void OnHarass()
        {

            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useW = Menu["harass"]["usew"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(Q.Range+200);
            float manapercent = Menu["harass"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                if (!target.IsValidTarget())
                {
                    return;
                }
                switch (Menu["harass"]["qmode"].As<MenuList>().Value)
                {
                    case 0:
                        if (useQ && target.IsValidTarget(Q.Range) && target != null)
                        {
                            if (target.Distance(Player) > Menu["harass"]["rangeq"].As<MenuSlider>().Value)
                            {
                                Q.CastOnUnit(target);
                            }
                            if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                            {
                                Q.CastOnUnit(target);
                            }
                            if (target.IsDashing())
                            {
                                Q.CastOnUnit(target);
                            }
                        }
                        if (useW && !Menu["harass"]["waa"].Enabled && target != null)
                        {
                            if (target.IsValidTarget(300))
                            {
                                W.Cast();
                            }
                        }
                        if (useE && target != null && target.IsValidTarget(E.Range))
                        {
                            if (!Player.HasBuff("JaxCounterStrike"))
                            {
                                
                                E.Cast();
                            }
                            if (Player.HasBuff("JaxCounterStrike"))
                            {

                                E.CastOnUnit(target);
                            }
                        }
                        break;
                    case 1:
                        if (target.IsValidTarget(Q.Range + 100) && target != null && useE)
                        {
                            if (!Player.HasBuff("JaxCounterStrike"))
                            {
                                if (E.Cast())
                                {
                                    helloo = Game.TickCount + 100;
                                }
                            }
                            if (Player.HasBuff("JaxCounterStrike"))
                            {
                                if (target.IsValidTarget(E.Range))
                                {

                                    E.CastOnUnit(target);

                                }
                            }
                        }
                        if (helloo < Game.TickCount)
                        {
                            if (useQ && target != null)
                            {
                                if (target.Distance(Player) > Menu["harass"]["rangeq"].As<MenuSlider>().Value)
                                {
                                    Q.CastOnUnit(target);
                                }
                                if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                                {
                                    Q.CastOnUnit(target);
                                }
                                if (target.IsDashing())
                                {
                                    Q.CastOnUnit(target);
                                }
                            }
                        }
                        if (!E.Ready)
                        {
                            if (useQ && target != null)
                            {
                                if (target.Distance(Player) > Menu["harass"]["rangeq"].As<MenuSlider>().Value)
                                {
                                    Q.CastOnUnit(target);
                                }
                                if (Player.GetSpellDamage(target, SpellSlot.Q) > target.Health)
                                {
                                    Q.CastOnUnit(target);
                                }
                                if (target.IsDashing())
                                {
                                    Q.CastOnUnit(target);
                                }
                            }
                        }
                        if (useW && !Menu["harass"]["waa"].Enabled && target != null)
                        {
                            if (target.IsValidTarget(300))
                            {
                                W.Cast();
                            }
                        }
                        break;
                }
            }
        }
    }
}

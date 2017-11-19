using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Damage.JSON;

namespace Rengar_By_Kornis
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

    internal class Rengar
    {
        public static Menu Menu = new Menu("Rengar By Kornis", "Rengar by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, Smites;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 425);
            E = new Spell(SpellSlot.E, 1000);
            Q.SetSkillshot(0.3f, 120, 2000, false, SkillshotType.Line);
            E.SetSkillshot(0.25f, 70f, 1500, true, SkillshotType.Line, false, HitChance.High);
            var smiteSlot = Player.SpellBook.Spells.FirstOrDefault(x => x.SpellData.Name.ToLower().Contains("smite"));
            if (smiteSlot != null)
            {

                Smites = new Spell(smiteSlot.Slot, 700);

            }

        }

        public Rengar()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuList("priority", "Priority: ", new[] { " Q ", " W ", " E " }, 0));
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("priorityq", "Priority Q if can kill Q + AA"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("autow", "^- Auto W if CC'd"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("changee", "^- Priority E if out of Q Range"));
                ComboMenu.Add(new MenuBool("bushe", "Don't use E from Bush", false));

            }
            Menu.Add(ComboMenu);
            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuList("priority", "Priority: ", new[] { " Q ", " W ", " E " }, 0));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
                FarmMenu.Add(new MenuBool("items", "Use Items"));
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("savestacks", "Save Stacks", false));
                LaneClear.Add(new MenuList("priority", "Priority: ", new[] { " Q ", " W ", " E " }, 0));
                LaneClear.Add(new MenuBool("useq", "Use Q to Clear"));
                LaneClear.Add(new MenuBool("usew", "Use W to Clear"));
                LaneClear.Add(new MenuSlider("hitw", "^- Min. Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("usee", "Use E to Clear"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("savestacks", "Save Stacks", false));
                JungleClear.Add(new MenuList("priority", "Priority: ", new[] { " Q ", " W ", " E " }, 0));
                JungleClear.Add(new MenuBool("useq", "Use Q to Clear"));
                JungleClear.Add(new MenuBool("usew", "Use W to Clear"));
                JungleClear.Add(new MenuBool("usee", "Use E to Clear"));
 
            }
            Menu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
            
            }
            Menu.Add(KSMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Smite Toggle"));
            }
            Menu.Add(DrawMenu);
            var SmiteMenu = new Menu("smite", "Smite Settings");
            {
                SmiteMenu.Add(new MenuBool("SmiteUse", "Use Smite on Monsters"));
                SmiteMenu.Add(new MenuBool("SmiteUseHeroes", "Use Smite on Champions"));
                SmiteMenu.Add(new MenuKeyBind("smitekey", "Smite Toggle", KeyCode.T, KeybindType.Toggle));
            }
            Menu.Add(SmiteMenu);
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.PostAttack += OnPostAttack;
            SpellBook.OnCastSpell += OnCastSpell;
            BuffManager.OnRemoveBuff += uh;
            LoadSpells();
            Console.WriteLine("Rengar by Kornis - Loaded");
        }
        private void uh(Obj_AI_Base sender, Buff buff)
        {
            if (sender.IsMe)
            {
               
                if (buff.Name == "RengarR")
                {
                 
                    reeeeeeeeeSTOPusingE = false;
                }
            }
        }

        private static readonly string[] SmiteMobs = {
            "SRU_Red", "SRU_Blue", "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air",
            "SRU_Dragon_Elder", "SRU_Baron","SRU_RiftHerald"};
        private static int SmiteDamages
        {
            get
            {
                int[] Hello = new int[] { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 };

                return Hello[Player.Level - 1];
            }
        }

        public void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.R && sender.IsMe)
            {
                reeeeeeeeeSTOPusingE = true;
            }

        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
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


        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };
        private bool reeeeeeeeeSTOPusingE;

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
            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["drawtoggle"].Enabled)
            {

                Vector2 meow;
                var heropos = Render.WorldToScreen(Player.Position, out meow);
                var xaOffset = (int)meow.X;
                var yaOffset = (int)meow.Y;
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
            if (Menu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(E.Range * 2))
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
                                (float)(barPos.X + (unit.Health >
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                         Player.GetSpellDamage(unit, SpellSlot.E) +
                                                         Player.GetSpellDamage(unit, SpellSlot.W))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }

        private void Game_OnUpdate()
        {
            SmiteUse();
            if (Player.IsDead || MenuGUI.IsChatOpen())
            {
                return;
            }
            
            Killsteal();
            if (Menu["combo"]["autow"].Enabled)
            {
                if (Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Stun) ||
          Player.HasBuffOfType(BuffType.Fear) || Player.HasBuffOfType(BuffType.Snare) ||
          Player.HasBuffOfType(BuffType.Taunt) || Player.HasBuffOfType(BuffType.Knockback) ||
          Player.HasBuffOfType(BuffType.Suppression))
                {
                    
                    if (Player.Mana == 4)
                    {
                        W.Cast();
                    }

                }
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


        }

        public static List<Obj_AI_Minion> GetAllGenericMinionsTargets()
        {
            return GetAllGenericMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetAllGenericMinionsTargetsInRange(float range)
        {
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range)).ToList();
        }

        public static List<Obj_AI_Base> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
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
            bool stacks = Menu["farming"]["lane"]["savestacks"].Enabled;
            bool useQ = Menu["farming"]["lane"]["useq"].Enabled;
            bool useW = Menu["farming"]["lane"]["usew"].Enabled;
            bool useE = Menu["farming"]["lane"]["usee"].Enabled;
            float hitsw = Menu["farming"]["lane"]["hitw"].As<MenuSlider>().Value;

            if (stacks)
            {
                if (Player.Mana == 4)
                {
                    return;
                }
            }
            if (Player.Mana == 4)
            {
                switch (Menu["farming"]["lane"]["priority"].As<MenuList>().Value)
                {
                    case 0:
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (useQ)
                            {
                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    Q.Cast(minion);
                                }
                            }
                        }
                        break;
                    case 1:
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {
                            if (useW)
                            {
                                if (minion.IsValidTarget(W.Range - 100) && minion != null &&
                                    hitsw <= GetEnemyLaneMinionsTargetsInRange(W.Range).Count)
                                {
                                    W.Cast();
                                }
                            }
                        }
                        break;
                    case 2:
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (useE)
                            {

                                if (minion.IsValidTarget(E.Range - 100) && minion != null)
                                {
                                    E.Cast();
                                }
                            }
                        }
                        break;
                }
            }
            if (Player.Mana < 4)
            {
                if (useE)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(E.Range))
                    {


                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            E.Cast(minion);
                        }
                    }
                }
                if (useW)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {


                        if (minion.IsValidTarget(W.Range - 100) && minion != null && hitsw <= GetEnemyLaneMinionsTargetsInRange(W.Range).Count)
                        {
                            W.Cast();
                        }
                    }
                }
                if (useQ)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
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
            return GameObjects.Jungle.Where(m => !GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range)).ToList();
        }

        private void Jungle()
        {
            foreach (var minion in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!minion.IsValidTarget() || !minion.IsValidSpellTarget())
                {
                    return;
                }
                bool stacks = Menu["farming"]["jungle"]["savestacks"].Enabled;
                bool useQ = Menu["farming"]["jungle"]["useq"].Enabled;
                bool useW = Menu["farming"]["jungle"]["usew"].Enabled;
                bool useE = Menu["farming"]["jungle"]["usee"].Enabled;

                if (stacks)
                {
                    if (Player.Mana == 4)
                    {
                        return;
                    }
                }


                if (Player.Mana == 4)
                {
                    switch (Menu["farming"]["jungle"]["priority"].As<MenuList>().Value)
                    {
                        case 0:

                            if (useQ)
                            {
                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    Q.Cast(minion);
                                }
                            }
                            break;
                        case 1:
                            if (useW)
                            {
                                if (minion.IsValidTarget(W.Range - 100) && minion != null)
                                {
                                    W.Cast();
                                }
                            }
                            break;
                        case 2:

                            if (useE)
                            {
                                if (minion.IsValidTarget(E.Range - 100) && minion != null)
                                {
                                    E.Cast(minion);
                                }
                            }
                            break;
                    }
                }
                if (Player.Mana < 4)
                {
                    if (useE)
                    {



                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            E.Cast(minion);
                        }
                    }

                    if (useW)
                    {



                        if (minion.IsValidTarget(W.Range - 100) && minion != null)
                        {
                            W.Cast();
                        }
                    }

                    if (useQ)
                    {


                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            Q.Cast(minion);
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
            if (Q.Ready &&
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(Q, DamageType.Physical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range) && !Player.HasBuff("RengarR"))
                {
                    Q.Cast(bestTarget);
                    if (bestTarget.Distance(Player) <= 420)
                    {
                        Q.Cast(Q.GetPrediction(bestTarget).CastPosition.Extend(Q.GetPrediction(bestTarget).UnitPosition, 100));
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

            bool useQ = Menu["combo"]["useq"].Enabled;
            bool useW = Menu["combo"]["usew"].Enabled;
            bool useE = Menu["combo"]["usee"].Enabled;
            bool ChangeE = Menu["combo"]["changee"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget() || Player.HasBuff("RengarR") || reeeeeeeeeSTOPusingE)
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
                if (Player.Mana == 4)
            {
                if (ChangeE)
                {
                    if (target.Distance(Player) > Q.Range)
                    {
                        if (target.IsValidTarget(E.Range) && target != null)
                        {
                            if (!Menu["combo"]["bushe"].Enabled)
                            {
                                E.Cast(target);

                            }
                            if (Menu["combo"]["bushe"].Enabled && !Player.HasBuff("rengarpassivebuff"))
                            {
                                E.Cast(target);

                            }
                        }
                    }
                }
                if (Menu["combo"]["priorityq"].Enabled)
                {

                    if (target.IsValidTarget(Q.Range) && target != null)
                    {
                        
                        if (Player.GetAutoAttackDamage(target) + Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.Empowered)*2 >=
                            target.Health)
                        {
                            Q.Cast(target);
                            if (target.Distance(Player) <= 420)
                            {
                                Q.Cast(Q.GetPrediction(target).CastPosition
                                    .Extend(Q.GetPrediction(target).UnitPosition, 100));
                            }
                        }
                    }

                }
                switch (Menu["combo"]["priority"].As<MenuList>().Value)
                {
                    case 0:


                        if (target.IsValidTarget(Q.Range) && target != null)
                        {
                            Q.Cast(target);
                            if (target.Distance(Player) <= 420)
                            {
                                Q.Cast(Q.GetPrediction(target).CastPosition.Extend(Q.GetPrediction(target).UnitPosition, 100));
                            }
                        }

                        break;
                    case 1:

                        if (target.IsValidTarget(W.Range - 100) && target != null)
                        {
                            W.Cast();
                        }

                        break;
                    case 2:


                        if (target.IsValidTarget(E.Range - 100) && target != null)
                        {
                            if (!Menu["combo"]["bushe"].Enabled)
                            {
                                if (E.Cast(target))
                                {
                                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                        Player.HasItem(ItemId.RavenousHydra))
                                    {
                                        var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                        if (slot != null && target.Distance(Player) < 400)
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
                            if (Menu["combo"]["bushe"].Enabled && !Player.HasBuff("rengarpassivebuff"))
                            {
                                if (E.Cast(target))
                                {
                                    if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                        Player.HasItem(ItemId.RavenousHydra))
                                    {
                                        var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                        if (slot != null && target.Distance(Player) < 400)
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

                        break;
                }
            }
            if (Player.Mana < 4)
            {
                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!Menu["combo"]["bushe"].Enabled)
                        {
                            if (E.Cast(target))
                            {
                                if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                    Player.HasItem(ItemId.RavenousHydra))
                                {
                                    var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                    if (slot != null && target.Distance(Player) < 400)
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
                        if (Menu["combo"]["bushe"].Enabled && !Player.HasBuff("rengarpassivebuff"))
                        {
                            if (E.Cast(target))
                            {
                                if (Player.HasItem(ItemId.TitanicHydra) || Player.HasItem(ItemId.Tiamat) ||
                                    Player.HasItem(ItemId.RavenousHydra))
                                {
                                    var items = new[] { ItemId.TitanicHydra, ItemId.Tiamat, ItemId.RavenousHydra };
                                    var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                                    if (slot != null && target.Distance(Player) < 400)
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
                if (W.Ready && useW && target.IsValidTarget(W.Range - 100))

                {
                    if (target != null)
                    {
                        W.Cast();
                    }
                }
                if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                 
                        Q.Cast(target);
                        if (target.Distance(Player) <= 420)
                        {
                            Q.Cast(Q.GetPrediction(target).CastPosition.Extend(Q.GetPrediction(target).UnitPosition, 100));
                        }
                    }

                }
            }
        }

        private void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useW = Menu["harass"]["usew"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {
                return;
            }
            if (Player.Mana == 4)
            {
                switch (Menu["harass"]["priority"].As<MenuList>().Value)
                {
                    case 0:


                        if (target.IsValidTarget(Q.Range) && target != null)
                        {
                            Q.Cast(target);
                            if (target.Distance(Player) <= 420)
                            {
                                Q.Cast(Q.GetPrediction(target).CastPosition.Extend(Q.GetPrediction(target).UnitPosition, 100));
                            }
                        }

                        break;
                    case 1:

                        if (target.IsValidTarget(W.Range - 100) && target != null)
                        {
                            W.Cast();
                        }

                        break;
                    case 2:


                        if (target.IsValidTarget(E.Range - 100) && target != null)
                        {
                            E.Cast(target);
                        }

                        break;
                }
            }
            if (Player.Mana < 4)
            {
                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        E.Cast(target);
                    }
                }
                if (W.Ready && useW && target.IsValidTarget(W.Range - 100))

                {
                    if (target != null)
                    {
                        W.Cast();
                    }
                }
                if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        Q.Cast(target);
                        if (target.Distance(Player) <= 420)
                        {
                            Q.Cast(Q.GetPrediction(target).CastPosition.Extend(Q.GetPrediction(target).UnitPosition, 100));
                        }
                    }

                }
            }
        }
    }
}
// :3
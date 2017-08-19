using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;

namespace Akali_By_Kornis
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

    internal class Akali
    {
        public static Menu Menu = new Menu("Akali By Kornis", "Akali by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R, Ignite, Flash;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 270);
            E = new Spell(SpellSlot.E, 300);
            R = new Spell(SpellSlot.R, 700);


        }

        public Akali()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("procq", "Priority Q Proc.", false));
                ComboMenu.Add(new MenuBool("usew", "Use W to Gapclose"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("saver", "Save X R Stacks", 1, 0, 3));
                ComboMenu.Add(new MenuSlider("minr", "Min. R Range", 200, 0, 400));
                ComboMenu.Add(new MenuKeyBind("flashe", "W > R GapClose", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            Menu.Add(ComboMenu);
           
            var HarassMenu = new Menu("harass", "Harass");
            {
             
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
                HarassMenu.Add(new MenuBool("autoe", "Use AUTO E if Enemy in Range"));
            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            FarmMenu.Add(new MenuBool("logic", "Use Farm Logic"));
            FarmMenu.Add(new MenuSlider("mana", "Energy Manager", 30));
            FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
            FarmMenu.Add(new MenuBool("lastq", "^- Use Q to Last Hit"));
            FarmMenu.Add(new MenuBool("aa", "^- Don't Last Hit in AA Range"));
            FarmMenu.Add(new MenuBool("usee", "Use E to Farm"));


            Menu.Add(FarmMenu);
         
     
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KSMenu.Add(new MenuBool("kse", "Killsteal with E"));
                KSMenu.Add(new MenuBool("ksr", "Killsteal with R"));
                KSMenu.Add(new MenuBool("ksgap", "Gapclose R for Q"));
            }
            Menu.Add(KSMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawwr", "Draw W+R Range"));
                DrawMenu.Add(new MenuBool("minr", "Draw min. R Range"));
                
            }
            Menu.Add(DrawMenu);
            var FleeMenu = new Menu("flee", "Flee");
            {
                FleeMenu.Add(new MenuBool("usew", "Use W to Flee"));
                FleeMenu.Add(new MenuBool("user", "Use R to Flee"));
                FleeMenu.Add(new MenuKeyBind("key", "Flee Key:", KeyCode.G, KeybindType.Press));
            }
            Menu.Add(FleeMenu);
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.PostAttack += OnPostAttack;
            LoadSpells();
            Console.WriteLine("Akali by Kornis - Loaded");
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            
            
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["procq"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }

                if (!Orbwalker.IsWindingUp)
                {
                     if (hero.IsValidTarget(E.Range))
                    {

                        E.Cast();
                    }
                }
            }
        }

        private void Render_OnPresent()
        {
            
            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["minr"].Enabled)
            {
                Render.Circle(Player.Position, Menu["combo"]["minr"].As<MenuSlider>().Value, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawwr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range+W.Range, 40, Color.CornflowerBlue);
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
                case OrbwalkingMode.Lasthit:
                    Lasthit();
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
            if (Menu["combo"]["flashe"].Enabled)
            {
               ComboW();
            }
            Killsteal();
            if (Menu["harass"]["autoe"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => t.IsValidTarget(E.Range) && !t.IsDead && t != null))
                {

                    E.Cast();
                }

            }

        }
        private void Lasthit()
        {
            if (Menu["farming"]["lastq"].Enabled)
            {

                foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {

                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                    {
                        if (Menu["farming"]["aa"].Enabled)
                        {
                            if (minion.Distance(Player) > 250)
                            {
                                Q.CastOnUnit(minion);
                            }
                        }
                        if (!Menu["farming"]["aa"].Enabled)
                        {
                            Q.CastOnUnit(minion);
                        }

                    }
                }
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
            bool useQ = Menu["farming"]["useq"].Enabled;
            bool useE = Menu["farming"]["usee"].Enabled;
            float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                if (!Menu["farming"]["logic"].Enabled)
                {
                    if (useQ)
                    {
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range)
                            .OrderBy(x => x.Distance(Player)))
                        {


                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                Q.CastOnUnit(minion);
                            }
                        }
                    }
                    if (useE)
                    {
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {


                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                E.Cast();
                            }
                        }
                    }
                }
                if (Menu["farming"]["logic"].Enabled)
                {

                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range).OrderBy(x => x.Distance(Player)))
                    {




                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            if (minion.Health > Player.GetSpellDamage(minion, SpellSlot.Q) +
                                Player.GetSpellDamage(minion, SpellSlot.E))
                            {
                                if (useQ)
                                {
                                    if (minion.Distance(Player) < 200)
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                    if (minion.Distance(Player) > 200 && minion.Distance(Player) < Q.Range)
                                    {
                                        Q.CastOnUnit(minion);
                                    }
                                }
                            }
                            if (minion.HasBuff("AkaliMota"))
                            {
                                if (minion.Health >
                                    Player.GetSpellDamage(minion, SpellSlot.E))
                                {
                                    if (useE)
                                    {
                                        if (minion.IsValidTarget(E.Range))
                                        {
                                            E.Cast();
                                            Player.IssueOrder(OrderType.AttackUnit, minion);
                                        }
                                    }
                                }
                            }


                            if (!minion.HasBuff("AkaliMota") && !Q.Ready)
                            {
                                if (minion.Health >
                                    Player.GetSpellDamage(minion, SpellSlot.E))
                                {
                                    if (useE)
                                    {
                                        if (minion.IsValidTarget(E.Range))
                                        {
                                            E.Cast();
                                        }
                                    }
                                }
                            }
                            if (useE)
                            {
                                if (minion.IsValidTarget(E.Range))
                                {
                                    if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q) +
                                        Player.GetSpellDamage(minion, SpellSlot.E))
                                    {
                                        E.Cast();
                                    }
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
            return GameObjects.Jungle.Where(m => !GameObjects.JungleSmall.Contains(m) && m.IsValidTarget(range)).ToList();
        }

        private void Jungle()
        {
            foreach (var jungleTarget in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || !jungleTarget.IsValidSpellTarget())
                {
                    return;
                }
                bool useQ = Menu["farming"]["useq"].Enabled;
                bool useE = Menu["farming"]["usee"].Enabled;
                float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (!Menu["farming"]["logic"].Enabled)
                    {
                        if (useQ)
                        {
                            foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range)
                                .OrderBy(x => x.Distance(Player)))
                            {


                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    Q.CastOnUnit(minion);
                                }
                            }
                        }
                        if (useE)
                        {
                            foreach (var minion in GetGenericJungleMinionsTargetsInRange(E.Range))
                            {


                                if (minion.IsValidTarget(E.Range) && minion != null)
                                {
                                    E.Cast();
                                }
                            }
                        }
                    }
                    if (Menu["farming"]["logic"].Enabled)
                    {
                        foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range)
                            .OrderBy(x => x.Distance(Player)))
                        {


                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                if (minion.Health > Player.GetSpellDamage(minion, SpellSlot.Q) +
                                    Player.GetSpellDamage(minion, SpellSlot.E))
                                {
                                    if (useQ)
                                    {
                                        if (minion.Distance(Player) < 200)
                                        {
                                            Q.CastOnUnit(minion);
                                        }
                                        if (minion.Distance(Player) > 200 && minion.Distance(Player) < Q.Range)
                                        {
                                            Q.CastOnUnit(minion);
                                        }
                                    }
                                }
                                if (minion.HasBuff("AkaliMota"))
                                {
                                    if (minion.Health >
                                        Player.GetSpellDamage(minion, SpellSlot.E))
                                    {
                                        if (useE)
                                        {
                                            if (minion.IsValidTarget(E.Range))
                                            {
                                                E.Cast();
                                                Player.IssueOrder(OrderType.AttackUnit, minion);
                                            }
                                        }
                                    }
                                }

                                if (!minion.HasBuff("AkaliMota") && !Q.Ready)
                                {
                                    if (minion.Health >
                                        Player.GetSpellDamage(minion, SpellSlot.E))
                                    {
                                        if (useE)
                                        {
                                            if (minion.IsValidTarget(E.Range))
                                            {
                                                E.Cast();
                                            }
                                        }
                                    }
                                }
                                if (useE)
                                {
                                    if (minion.IsValidTarget(E.Range))
                                    {
                                        if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q) +
                                            Player.GetSpellDamage(minion, SpellSlot.E))
                                        {
                                            E.Cast();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }


        private void Flee()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            bool useQ = Menu["flee"]["usew"].Enabled;
            bool useR = Menu["flee"]["user"].Enabled;
            if (useQ)
            {

                W.Cast(Game.CursorPos);
            }
            if (useR)
            {
                foreach (var en in ObjectManager.Get<Obj_AI_Base>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Game.CursorPos) < 200 && en.IsMinion)
                        {
                            R.CastOnUnit(en);
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
        public static Obj_AI_Hero GetBestKillableHeroa(Spell spell, DamageType damageType = DamageType.True,
            bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(Q.Range + R.Range).FirstOrDefault(t => t.IsValidTarget());
        }
        private void Killsteal()
        {
            if (Q.Ready &&
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.CastOnUnit(bestTarget);
                }
            }

            if (E.Ready &&
                Menu["killsteal"]["kse"].Enabled)
            {
                var bestTarget = GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
            }
            if (R.Ready &&
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(R, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.R) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    R.CastOnUnit(bestTarget);
                }
            }
            if (Q.Ready &&
                Menu["killsteal"]["ksgap"].Enabled)
            {
                var bestTarget = GetBestKillableHeroa(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range+R.Range))
                {
                    foreach (var en in ObjectManager.Get<Obj_AI_Base>().OrderBy(x => x.Distance(bestTarget)))
                    {
                        if (!en.IsDead &&
                            en.Distance(bestTarget) < Q.Range && en.Distance(Player) < R.Range && !en.IsAlly)
                        {

                            R.CastOnUnit(en);


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
        private void ComboW()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            bool useQ = Menu["combo"]["useq"].Enabled;
            bool useW = Menu["combo"]["usew"].Enabled;
            bool useE = Menu["combo"]["usee"].Enabled;
            bool useR = Menu["combo"]["user"].Enabled;

            var target = GetBestEnemyHeroTargetInRange(R.Range+W.Range);

            if (!target.IsValidTarget())
            {
                return;
            }

            var items = new[] { ItemId.HextechGunblade, ItemId.BilgewaterCutlass };
            if (Player.HasItem(ItemId.HextechGunblade) || Player.HasItem(ItemId.BilgewaterCutlass))
            {
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
            if (R.Ready && target.IsValidTarget(R.Range+W.Range))
            {

                if (target != null && target.Distance(Player) > R.Range)
                {

                    W.Cast(target.ServerPosition);
                }
            }
            if (R.Ready && useR && target.IsValidTarget(R.Range))
            {

                if (target != null && target.Distance(Player) >= Menu["combo"]["minr"].As<MenuSlider>().Value)
                {

                    R.CastOnUnit(target);
                }
            }
            if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
            {

                if (target != null)
                {

                    Q.CastOnUnit(target);
                }
            }
            if (E.Ready && useE && target.IsValidTarget(E.Range))
            {

                if (target != null)
                {

                    E.Cast();
                }
            }
            if (W.Ready && useW && target.IsValidTarget(W.Range + 120))
            {

                if (target != null)
                {
                    if (target.Distance(Player) >= 200)
                    {
                        W.Cast(target.ServerPosition);
                    }
                }
            }
        }
        private void OnCombo()
        {

            bool useQ = Menu["combo"]["useq"].Enabled;
            bool useW = Menu["combo"]["usew"].Enabled;
            bool useE = Menu["combo"]["usee"].Enabled;
            bool useR = Menu["combo"]["user"].Enabled;

            var target = GetBestEnemyHeroTargetInRange(R.Range + W.Range);

            if (!target.IsValidTarget())
            {
                return;
            }

            var items = new[] { ItemId.HextechGunblade, ItemId.BilgewaterCutlass };
            if (Player.HasItem(ItemId.HextechGunblade) || Player.HasItem(ItemId.BilgewaterCutlass))
            {
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
            if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
            {

                if (target != null)
                {

                    Q.CastOnUnit(target);
                }
            }
            if (!Menu["combo"]["procq"].Enabled)
            {
                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {

                    if (target != null)
                    {

                       E.Cast();
                    }
                }
            }
            if (W.Ready && useW && target.IsValidTarget(W.Range + 120))
            {

                if (target != null)
                {
                    if (target.Distance(Player) >= 200)
                    {
                        W.Cast(target.ServerPosition);
                    }
                }
            }
            if (R.Ready && useR && target.IsValidTarget(R.Range))
            {

                if (target != null && target.Distance(Player) >= Menu["combo"]["minr"].As<MenuSlider>().Value && Player.SpellBook.GetSpell(SpellSlot.R).Ammo > Menu["combo"]["saver"].As<MenuSlider>().Value)
                {

                    R.CastOnUnit(target);
                }
            }

        }

        private void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useW = Menu["harass"]["usew"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;
            var target = GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {
                    return;
                }


                if (E.Ready && useE && target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        E.Cast(target);
                    }
                }
                if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        Q.Cast(target);
                    }
                }
            if (W.Ready && useW && target.IsValidTarget(W.Range + 120))
            {

                if (target != null)
                {
                    if (target.Distance(Player) >= 200)
                    {
                        W.Cast(target.ServerPosition);
                    }
                }
            }

        }
        
    }
}

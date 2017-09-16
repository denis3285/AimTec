using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;
using Aimtec.SDK.Damage.JSON;

namespace Darius_By_Kornis
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

    internal class Darius
    {
        public static Menu Menu = new Menu("Darius By Kornis", "Darius by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 425);
            W = new Spell(SpellSlot.W, 300);
            E = new Spell(SpellSlot.E, 490);

            R = new Spell(SpellSlot.R, 460);


        }

        public Darius()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("lockq", "Use Q Outside Lock", false));
                ComboMenu.Add(new MenuBool("qaa", "Don't Q if in AA Range", false));
                ComboMenu.Add(new MenuBool("check", "Checks for Auto Attack for Q (Smooth Combo)").SetToolTip("If tries to Auto Attack, don't cast Q"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("waa", "^- Only for AA Reset", false));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("eaa", "^- Only if out of AA Range", false));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("waster", "Don't waste R if Enemy HP lower than", 0, 0, 500));
                ComboMenu.Add(new MenuKeyBind("toggle", "R Toggle", KeyCode.T, KeybindType.Toggle));
            }
            Menu.Add(ComboMenu);

            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 0, 100));

                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            FarmMenu.Add(new MenuSlider("mana", "Mana Manager", 30));
            FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
            FarmMenu.Add(new MenuSlider("minq", "^- if Hits", 3, 1, 6));
            FarmMenu.Add(new MenuBool("usew", "Use W to Farm"));
            FarmMenu.Add(new MenuBool("lastw", "^- Use W for Last Hit"));


            Menu.Add(FarmMenu);


            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KSMenu.Add(new MenuBool("ksr", "Killsteal with R"));
                KSMenu.Add(new MenuSlider("waster", "Don't waste R if Enemy HP lower than", 0, 0, 500));

            }
            Menu.Add(KSMenu);
            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("rdamage", "Draw R Damage"));
                DrawMenu.Add(new MenuBool("stacks", "Draw Stack Count"));
                       DrawMenu.Add(new MenuBool("toggle", "Draw Toggle"));
            }
            Menu.Add(DrawMenu);

            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.PostAttack += OnPostAttack;
            LoadSpells();
            Console.WriteLine("Darius by Kornis - Loaded");
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {


            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["waa"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                W.Cast();
            }
        }

        static double GetQmelee(Obj_AI_Base target)
        {
            double meow = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 1)
            {
                meow = 40 + Player.TotalAttackDamage * 1;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 2)
            {
                meow = 70 + Player.TotalAttackDamage * 1.10;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 3)
            {
                meow = 100 + Player.TotalAttackDamage * 1.20;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 4)
            {
                meow = 130 + Player.TotalAttackDamage * 1.30;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 5)
            {
                meow = 160 + Player.TotalAttackDamage * 1.40;
            }

            double full = meow * 0.35;
            double damage = Player.CalculateDamage(target, DamageType.Physical, full);
            return damage;

        }

        static double GetQmax(Obj_AI_Base target)
        {
            double meow = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 1)
            {
                meow = 40 + Player.TotalAttackDamage * 1;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 2)
            {
                meow = 70 + Player.TotalAttackDamage * 1.10;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 3)
            {
                meow = 100 + Player.TotalAttackDamage * 1.20;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 4)
            {
                meow = 130 + Player.TotalAttackDamage * 1.30;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.Q).Level == 5)
            {
                meow = 160 + Player.TotalAttackDamage * 1.40;
            }

            double full = meow;
            double damage = Player.CalculateDamage(target, DamageType.Physical, full);
            return damage;

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

        private void Render_OnPresent()
        {

            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.CornflowerBlue);
            }
            if (Menu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["rdamage"].Enabled)
            {


                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range + R.Range))
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
                                (float) (barPos.X + (unit.Health >
                                                     GetR(unit)
                                             ? width * ((unit.Health - GetR(unit)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < GetR(unit)
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }
            if (Menu["drawings"]["stacks"].Enabled)
            {
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range + R.Range))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                            Vector2 maybeworks;
                            var heropos = Render.WorldToScreen(unit.Position, out maybeworks);
                            var xaOffset = (int) maybeworks.X;
                            var yaOffset = (int) maybeworks.Y;

                            if (unit.GetBuffCount("DariusHemo") > 0)
                            {
                                Render.Text(xaOffset - 40, yaOffset - 40, Color.LawnGreen,
                                    "Stacks: " + unit.GetBuffCount("DariusHemo"),
                                    RenderTextFlags.VerticalCenter);
                            }
                            if (unit.GetBuffCount("DariusHemo") == 0)
                            {
                                Render.Text(xaOffset - 40, yaOffset - 40, Color.LawnGreen,
                                    "Stacks: 1",
                                    RenderTextFlags.VerticalCenter);
                            }
                        });
            }
            if (Menu["drawings"]["toggle"].Enabled)
            {
                Vector2 maybeworks;
                var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
                var xaOffset = (int) maybeworks.X;
                var yaOffset = (int) maybeworks.Y;

                if (Menu["combo"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 60, yaOffset + 30, Color.GreenYellow, "R Usage: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["combo"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 60, yaOffset + 30, Color.Red, "R Usage: OFF",
                        RenderTextFlags.VerticalCenter);
                }

            }
        }

        static double GetR(Obj_AI_Base target)
        {
            double meow = 0;
            double hello = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 100 + (Player.TotalAttackDamage - Player.BaseAttackDamage) * 0.75;
                if (target.GetBuffCount("DariusHemo") == 0)
                {
                    hello = meow * 0.2;
                }
                if (target.GetBuffCount("DariusHemo") == 2)
                {
                    hello = meow * 0.4;
                }
                if (target.GetBuffCount("DariusHemo") == 3)
                {
                    hello = meow * 0.6;
                }
                if (target.GetBuffCount("DariusHemo") == 4)
                {
                    hello = meow * 0.8;
                }
                if (target.GetBuffCount("DariusHemo") == 5)
                {
                    hello = meow;
                }

            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 200 + (Player.TotalAttackDamage - Player.BaseAttackDamage) * 0.75;
                if (target.GetBuffCount("DariusHemo") == 0)
                {
                    hello = meow * 0.2;
                }
                if (target.GetBuffCount("DariusHemo") == 2)
                {
                    hello = meow * 0.4;
                }
                if (target.GetBuffCount("DariusHemo") == 3)
                {
                    hello = meow * 0.6;
                }
                if (target.GetBuffCount("DariusHemo") == 4)
                {
                    hello = meow * 0.8;
                }
                if (target.GetBuffCount("DariusHemo") == 5)
                {
                    hello = meow;
                }
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 300 + (Player.TotalAttackDamage - Player.BaseAttackDamage) * 0.75;
                if (target.GetBuffCount("DariusHemo") == 0)
                {
                    hello = meow * 0.2;
                }
                if (target.GetBuffCount("DariusHemo") == 2)
                {
                    hello = meow * 0.4;
                }
                if (target.GetBuffCount("DariusHemo") == 3)
                {
                    hello = meow * 0.6;
                }
                if (target.GetBuffCount("DariusHemo") == 4)
                {
                    hello = meow * 0.8;
                }
                if (target.GetBuffCount("DariusHemo") == 5)
                {
                    hello = meow;
                }
            }

            double full = meow + hello;
            double damage = Player.CalculateDamage(target, DamageType.True, full);
            return damage - 20;

        }

        private void Game_OnUpdate()
        {
            if (Player.HasBuff("dariusqcast"))
            {
                Orbwalker.AttackingEnabled = false;
            }
            else Orbwalker.AttackingEnabled = true;

            if (Player.IsDead || MenuGUI.IsChatOpen())
            {
                return;
            }
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    QLock();
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


            Killsteal();


        }

        private void Lasthit()
        {
            if (Menu["farming"]["lastw"].Enabled)
            {

                foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                {
                    double hello = Player.TotalAttackDamage + Player.TotalAttackDamage * 0.4;
                    double full = Player.CalculateDamage(minion, DamageType.Physical, hello);

                    if (minion.Health <= full)
                    {


                        if (W.Cast())
                        {
                            Player.IssueOrder(OrderType.AttackUnit, minion);
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
            bool useQ = Menu["farming"]["useq"].Enabled;
            bool useW = Menu["farming"]["usew"].Enabled;
            float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {

                if (useQ)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (GetEnemyLaneMinionsTargetsInRange(Q.Range).Count >=
                            Menu["farming"]["minq"].As<MenuSlider>().Value)
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                
                                if (!Orbwalker.IsWindingUp)
                                {
                                    Q.Cast();
                                }
                            }
                        }
                    }
                }

                if (Menu["farming"]["lastw"].Enabled)
                {

                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {
                        double hello = Player.TotalAttackDamage + Player.TotalAttackDamage * 0.4;
                        double full = Player.CalculateDamage(minion, DamageType.Physical, hello);

                        if (minion.Health <= full)
                        {


                            if (W.Cast())
                            {
                                Player.IssueOrder(OrderType.AttackUnit, minion);
                            }


                        }
                    }
                }
                if (useW)
                {
                    foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                    {


                        if (minion.IsValidTarget(W.Range) && minion != null)
                        {
                            W.Cast();

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
                bool useQ = Menu["farming"]["useq"].Enabled;
                bool useW = Menu["farming"]["usew"].Enabled;
                float manapercent = Menu["farming"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (useQ)
                    {
                        foreach (var minion in GetGenericJungleMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {
                                if (!Orbwalker.IsWindingUp)
                                {
                                    Q.Cast();
                                }
                            }
                        }

                    }
                    if (useW)
                    {
                        foreach (var minion in GetGenericJungleMinionsTargetsInRange(W.Range))
                        {


                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {
                                W.Cast();

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

        public static Obj_AI_Hero GetBestKillableHeroa(Spell spell, DamageType damageType = DamageType.True,
            bool ignoreShields = false)
        {
            return TargetSelector.Implementation.GetOrderedTargets(Q.Range + R.Range)
                .FirstOrDefault(t => t.IsValidTarget());
        }

        private void Killsteal()
        {
            if (Q.Ready &&
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    if (bestTarget.Distance(Player) < 240)
                    {
                        if (bestTarget.Health <= GetQmelee(bestTarget))
                        {
                            Q.CastOnUnit(bestTarget);
                        }
                    }
                    if (bestTarget.Distance(Player) > 240)
                    {
                        if (bestTarget.Health <= GetQmax(bestTarget))
                        {
                            Q.CastOnUnit(bestTarget);
                        }
                    }
                }
            }

            if (R.Ready &&
                Menu["killsteal"]["ksr"].Enabled)
            {
                var bestTarget = GetBestKillableHero(R, DamageType.Magical, false);
                if (bestTarget != null &&
                    GetR(bestTarget) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range) && bestTarget.Health >=
                    Menu["combo"]["waster"].As<MenuSlider>().Value)
                {
                    if (Menu["combo"]["toggle"].Enabled)
                    {
                        R.CastOnUnit(bestTarget);
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

        private void QLock()
        {
            if (Menu["combo"]["lockq"].Enabled)
            {
                {
                    var target = GetBestEnemyHeroTargetInRange(Q.Range);

                    
                    if (target.IsValidTarget(Q.Range) && target != null)
                    {
                        if (target.ServerPosition.Distance(Player.ServerPosition) < 240)
                        {
                            if (Player.HasBuff("dariusqcast"))
                            {
                                Orbwalker.Move(target.ServerPosition.Extend(Player.ServerPosition, 350));
                            }


                        }


                    }
                }
                {
                    var target = GetBestEnemyHeroTargetInRange(Q.Range+200);


                    if (target.IsValidTarget(Q.Range + 200) && target != null)
                    {
                        if (target.ServerPosition.Distance(Player.ServerPosition) > Q.Range-50)
                        {
                            if (Player.HasBuff("dariusqcast"))
                            {
                                Orbwalker.Move(target.ServerPosition);
                            }


                        }
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

            var target = GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {
                return;
            }
            if (W.Ready && useW && target.IsValidTarget(W.Range) && !Menu["combo"]["waa"].Enabled)
            {

                if (target != null)
                {

                    W.Cast();
                }
            }
            if (useQ)
            {
                if (Q.Ready && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        if (Menu["combo"]["qaa"].Enabled)
                        {
                            if (Player.Distance(target) > 300)
                            {
                                if (Menu["combo"]["check"].Enabled)
                                {
                                    if (!Orbwalker.IsWindingUp)
                                    {
                                        Q.Cast();
                                    }
                                }
                            }
                        }
                        if (!Menu["combo"]["qaa"].Enabled)
                        {
                            if (Menu["combo"]["check"].Enabled)
                            {
                                if (!Orbwalker.IsWindingUp)
                                {
                                    Q.Cast();
                                }
                            }

                        }
                        if (Menu["combo"]["qaa"].Enabled)
                        {
                            if (Player.Distance(target) > 300)
                            {
                                if (!Menu["combo"]["check"].Enabled)
                                {
                              
                                        Q.Cast();
                                    
                                }
                            }
                        }
                        if (!Menu["combo"]["qaa"].Enabled)
                        {
                            if (!Menu["combo"]["check"].Enabled)
                            {
                              
                                    Q.Cast();
                                
                            }

                        }
                    }
                }
            }
            if (useE)
            {
                if (E.Ready && target.IsValidTarget(E.Range))
                {

                    if (target != null)
                    {
                        if (Menu["combo"]["eaa"].Enabled)
                        {
                            if (Player.Distance(target) > 300)
                            {
                                E.CastOnUnit(target);
                            }
                        }
                        if (!Menu["combo"]["eaa"].Enabled)
                        {
                            E.CastOnUnit(target);

                        }
                    }
                }
            }
            if (R.Ready && useR && target.IsValidTarget(R.Range))
            {

                if (target != null && target.Health <= GetR(target) &&
                    target.Health >= Menu["combo"]["waster"].As<MenuSlider>().Value)
                {
                    if (Menu["combo"]["toggle"].Enabled)
                    {
                        R.CastOnUnit(target);
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

            if (E.Ready && useE && target.IsValidTarget(E.Range))
            {
                if (target != null)
                {
                    E.CastOnUnit(target);
                }
            }
            if (Q.Ready && useQ && target.IsValidTarget(Q.Range))
            {
                if (target != null)
                {
                    if (Menu["combo"]["check"].Enabled)
                    {
                        if (!Orbwalker.IsWindingUp)
                        {
                            Q.Cast();

                        }
                    }
                    if (!Menu["combo"]["check"].Enabled)
                    {
                        Q.Cast();

                    }
                }
            }
            if (W.Ready && useW && target.IsValidTarget(W.Range))
            {

                if (target != null)
                {
                    W.Cast();
                }
            }
        }

    }
}

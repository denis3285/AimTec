using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;

namespace Vi_By_Kornis
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

    internal class Vi
    {
        public static Menu Menu = new Menu("Vi By Kornis", "Vi by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R, Q2, Flash;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 725);
            Q2 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 240);
            R = new Spell(SpellSlot.R, 800);
            Q.SetSkillshot(0.5f, 80f, float.MaxValue, false, SkillshotType.Line, false, HitChance.None);
            Q.SetCharged("ViQ", "ViQ", 250, 725, 1);
            Q.SetSkillshot(0.5f, 80f, float.MaxValue, false, SkillshotType.Line, false, HitChance.None);
            Q2.SetSkillshot(0.5f, 80f, float.MaxValue, false, SkillshotType.Line, false, HitChance.None);
            Q2.SetCharged("ViQ", "ViQ", 250, 1100, 1);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Spell(SpellSlot.Summoner2, 425);

        }

        public Vi()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("eaa", "^- Only for AA Resets"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(
                    new MenuList("rmode", "R Usage", new[] {"At X Health", "Only if Killable with Combo"}, 1));
                ComboMenu.Add(new MenuSlider("health", "At X Health", 50));

                ComboMenu.Add(new MenuKeyBind("flashq", "Flash - Q", KeyCode.G, KeybindType.Press));
            }
            Menu.Add(ComboMenu);
            var BlackList = new Menu("blacklist", "R Blacklist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                }
            }
            Menu.Add(BlackList);
            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
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
                KSMenu.Add(new MenuBool("kse", "Killsteal with E"));
            }
            Menu.Add(KSMenu);

            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("flashq", "Draw Flash - Q"));

                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Menu.Add(DrawMenu);
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Orbwalker.PostAttack += OnPostAttack;
            Game.OnUpdate += Game_OnUpdate;
            LoadSpells();
            Console.WriteLine("Vi by Kornis - Loaded");
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo))
            {
                if (!Menu["combo"]["eaa"].Enabled)
                {
                    return;
                }
                Obj_AI_Hero hero = args.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                E.Cast();

            }
            if (Orbwalker.Mode.Equals(OrbwalkingMode.Laneclear))
            {
                if (Menu["farming"]["jungle"]["usee"].Enabled)
                {
                    foreach (var jungleTarget in GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
                    {
                        if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                        {
                            return;
                        }
                        Obj_AI_Minion hero = args.Target as Obj_AI_Minion;
                        if (hero == null || !hero.IsValid)
                        {
                            return;
                        }
                        if (E.Cast())
                        {
                            Orbwalker.ResetAutoAttackTimer();
                        }
                    }

                }
                if (Menu["farming"]["lane"]["usee"].Enabled)
                {
                    if (Menu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                    {
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {


                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {
                                Obj_AI_Minion hero = args.Target as Obj_AI_Minion;
                                if (hero == null || !hero.IsValid)
                                {
                                    return;
                                }
                                if (E.Cast())
                                {
                                    Orbwalker.ResetAutoAttackTimer();
                                }
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

        private void Render_OnPresent()
        {

            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Wheat);
            }
            if (Menu["drawings"]["flashq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range + 375, 40, Color.Wheat);
            }


            if (Menu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }

            if (Menu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range * 2))
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
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.E) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.W) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.R)
                                    ? Color.GreenYellow
                                    : Color.Orange);

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
                    
                    Jungle();
                    break;

            }

            if (Menu["combo"]["flashq"].Enabled)
            {
                FlashQ();
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

        private void FlashQ()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            if (Q.Ready)
            {
                if (Flash.Ready && Flash != null)
                {


                    if (!Player.HasBuff("ViQ"))
                    {
                        var qtarget = TargetSelector.GetTarget(Q.ChargedMaxRange + 500);

                        if (qtarget != null)
                        {
                            Q2.StartCharging(Game.CursorPos);
                        }
                    }
                    if (Player.HasBuff("ViQ"))
                    {
                        var qtarget = TargetSelector.GetTarget(Q2.Range);
                        var pred = Q2.GetPrediction(qtarget);
                        if (qtarget != null)
                        {
                            if (qtarget.IsValidTarget(Q2.Range))
                            {
                                if (Flash.Cast(qtarget.ServerPosition))
                                {
                                    Q2.Cast(pred.CastPosition);
                                }
                            }
                        }
                    }

                }
            }
        }




        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidTarget(range)).ToList();
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
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }
                bool useQ = Menu["farming"]["jungle"]["useq"].Enabled;
                bool useE = Menu["farming"]["jungle"]["usee"].Enabled;
                float manapercent = Menu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (useQ)
                    {
                        if (!Player.HasBuff("ViQ"))
                        {
                            
                                Q.StartCharging(Game.CursorPos);
                            
                        }
                        if (Player.HasBuff("ViQ"))
                        {
                            
                                if (jungleTarget.IsValidTarget(Q.Range))
                                {
                                    Q.Cast(jungleTarget);
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
            if (
                Menu["killsteal"]["ksq"].Enabled)
            {
                var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    if (!Player.HasBuff("ViQ"))
                    {

                        Q.StartCharging(Game.CursorPos);

                    }
                    if (Player.HasBuff("ViQ"))
                    {

                        if (bestTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(bestTarget);
                        }

                    }
                }
            }
            if (E.Ready &&
                Menu["killsteal"]["kse"].Enabled)
            {
                var bestTarget = GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(240))
                {
                    E.Cast();
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
            bool useE = Menu["combo"]["usee"].Enabled;

            bool useR = Menu["combo"]["user"].Enabled;
            if (useQ)
            {

                if (!Player.HasBuff("ViQ"))
                {
                    var qtarget = TargetSelector.GetTarget(Q.ChargedMaxRange + 400);

                    if (qtarget != null)
                    {
                        Q.StartCharging(Game.CursorPos);
                    }
                }
                if (Player.HasBuff("ViQ"))
                {
                    var qtarget = TargetSelector.GetTarget(Q.Range);

                    if (qtarget != null)
                    {
                        if (qtarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(qtarget);
                        }
                    }
                }
            }
            if (useE && !Menu["combo"]["eaa"].Enabled)
            {
                var etarget = TargetSelector.GetTarget(240);

                if (etarget != null)
                {
                    E.Cast();
                }
            }
            if (useR)
            {
                var rtarget = TargetSelector.GetTarget(R.Range);

                if (rtarget != null)
                {
                  
                    switch (Menu["combo"]["rmode"].As<MenuList>().Value)
                    {
                        case 0:
                            if (rtarget.HealthPercent() <= Menu["combo"]["health"].As<MenuSlider>().Value)
                            {
                                if (!Menu["blacklist"][rtarget.ChampionName.ToLower()].Enabled)
                                {
                                    R.CastOnUnit(rtarget);
                                }
                            }
                            break;
                        case 1:
                            if (rtarget.Health <= Player.GetSpellDamage(rtarget, SpellSlot.Q) +
                                Player.GetSpellDamage(rtarget, SpellSlot.E) +
                                Player.GetSpellDamage(rtarget, SpellSlot.W) +
                                Player.GetSpellDamage(rtarget, SpellSlot.R))
                            {
                                if (!Menu["blacklist"][rtarget.ChampionName.ToLower()].Enabled)
                                {
                                    R.CastOnUnit(rtarget);
                                }
                            }
                            break;
                    }
                }


            }
        }



        private void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;

            float manapercent = Menu["harass"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {

                if (useE)
                {
                    var etarget = TargetSelector.GetTarget(240);

                    if (etarget != null)
                    {
                        E.Cast();
                    }
                }
                if (useQ)
                {

                    if (!Player.HasBuff("ViQ"))
                    {
                        var qtarget = TargetSelector.GetTarget(Q.ChargedMaxRange + 400);

                        if (qtarget != null)
                        {
                            Q.StartCharging(Game.CursorPos);
                        }
                    }
                    if (Player.HasBuff("ViQ"))
                    {
                        var qtarget = TargetSelector.GetTarget(Q.Range);

                        if (qtarget != null)
                        {
                            if (qtarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast(qtarget);
                            }
                        }
                    }
                }
            }
        }
    }
}

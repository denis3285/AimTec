using System.Net.Configuration;
using System.Resources;
using System.Security.Authentication.ExtendedProtection;

namespace Poppy_By_Kornis
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

    internal class Poppy
    {
        public static Menu Menu = new Menu("Poppy By Kornis", "Poppy by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 430);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 480);
            R = new Spell(SpellSlot.R, 425);
            Q.SetSkillshot(0.6f, 80, float.MaxValue, false, SkillshotType.Line);
            R.SetCharged("PoppyR", "PoppyR", 425, 1100, 0.9f);
            R.SetSkillshot(0.6f, 80, float.MaxValue, false, SkillshotType.Line);
        }

        public Poppy()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("pick", "Pick Passive"));
                ComboMenu.Add(new MenuSlider("pickup", "^- Range", 400, 10, 600));
                ComboMenu.Add(new MenuBool("useq", "Use Q"));
                ComboMenu.Add(new MenuBool("usew", "Use W"));
                ComboMenu.Add(new MenuBool("usee", "Use E "));
                ComboMenu.Add(new MenuBool("wall", "^- Only to Walls"));
                ComboMenu.Add(new MenuBool("ignore", "^- Ignore if can Kill with Combo"));
                ComboMenu.Add(new MenuBool("user", "Use Instant R if Killable with Combo"));
                ComboMenu.Add(new MenuSlider("waste", "^- Don't Waste R if X Health", 100, 0, 300));
                ComboMenu.Add(new MenuKeyBind("rcast", "Charging R Aiming", KeyCode.T, KeybindType.Press));
            }

            Menu.Add(ComboMenu);

            var HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
                HarassMenu.Add(new MenuBool("wall", "^- Only to Walls"));

            }
            Menu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
            }
            Menu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KSMenu.Add(new MenuBool("kse", "Killsteal with E"));
                KSMenu.Add(new MenuBool("wall", "^- Include Wall Damage"));
            }
            Menu.Add(KSMenu);

            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawflash", "Draw Passive Position"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            Menu.Add(DrawMenu);
            AutoQ.Attach(Menu, "Anti-Gapclose");
            Menu.Attach();

            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += OnCreate;
            AutoQ.DashQ += OnDash;
            GameObject.OnDestroy += OnDestroy;
            LoadSpells();
            Console.WriteLine("Poppy by Kornis - Loaded");
        }

        private void OnDash(Obj_AI_Hero target, GapcloserArgs Args)
        {

            if (target != null && Args.EndPosition.Distance(Player) < 400 && W.Ready)
            {

                W.Cast();
            }

        }
        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };

        public bool Hello { get; private set; }

        private GameObject meow;

        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 1 : 10;
        }

        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 3 : 20;
        }
        public void OnCreate(GameObject obj)
        {
            if (obj != null && obj.IsValid)
            {
                if (obj.Name == "Poppy_Base_P_mis_ground.troy" || obj.Name == "Poppy_Base_P_reticle.troy")
                {

                    meow = obj;
                }
            }
        }
        public void OnDestroy(GameObject obj)
        {
            if (obj != null && obj.IsValid)
            {
                if (obj.Name == "Poppy_Base_P_mis_ground.troy" || obj.Name == "Poppy_Base_P_shield_activate.troy")
                {
                    meow = null;
                }
            }
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
            if (Menu["drawings"]["drawflash"].Enabled)
            {

                if (meow != null && meow.IsValid && !meow.IsDead)
                {
                    Render.Circle(meow.ServerPosition, 80, 100, Color.Red);
                }
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
                                (float)(barPos.X + (unit.Health >
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                         Player.GetSpellDamage(unit, SpellSlot.E) +
                                                         Player.GetSpellDamage(unit, SpellSlot.R))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
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
            if (Player.HasBuff("poppypassiveshield"))
            {
                meow = null;
            }
            Killsteal();
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
            if (Menu["combo"]["rcast"].Enabled)
            {
                if (R.Ready)
                {
                    var target = GetBestEnemyHeroTargetInRange(R.Range);

                    if (target.IsValidTarget())
                    {
                        if (target != null)
                        {

                            if (R.IsCharging)
                            {
                                R.Cast(target);
                            }


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
            bool useQ = Menu["farming"]["lane"]["useq"].Enabled;
            float manapercent = Menu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
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
            foreach (var jungleTarget in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || !jungleTarget.IsValidSpellTarget())
                {
                    return;
                }
                bool useQ = Menu["farming"]["jungle"]["useq"].Enabled;
                float manapercent = Menu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (useQ && jungleTarget.IsValidTarget(Q.Range))
                    {
                        Q.Cast(jungleTarget);
                    }
                }

            }
        }

        private void Flee()
        {
            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
            bool useQ = Menu["flee"]["useq"].Enabled;
            if (useQ)
            {

                Q.Cast(Player.ServerPosition.Extend(Game.CursorPos, -500));
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
                var bestTarget = GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
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
                    E.CastOnUnit(bestTarget);
                }
            }
            if (E.Ready &&
                Menu["killsteal"]["wall"].Enabled)
            {

                var bestTarget = GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E, Aimtec.SDK.Damage.JSON.DamageStage.Collision) + Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    for (var i = 0; i < 7; i++)
                    {


                        var test = Player.ServerPosition.Extend(bestTarget.ServerPosition, Player.Distance(bestTarget) + 70 * i);
                        DelayAction.Queue(150, () =>
                        {
                            if (IsWall(test, true))
                            {
                                E.CastOnUnit(bestTarget);
                            }
                        });


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
            bool useR = Menu["combo"]["user"].Enabled;

            if (meow != null && !meow.IsDead && meow.IsValid)
            {
                
                if (meow.Distance(Player) <= Menu["combo"]["pickup"].As<MenuSlider>().Value)
                {

                    Orbwalker.Move(meow.ServerPosition);
                }
            }
            if (R.Ready && useR)
            {
                var target = GetBestEnemyHeroTargetInRange(400);

                if (target.IsValidTarget())
                {
                    if (target != null && target.Health < Player.GetSpellDamage(target, SpellSlot.Q) + Player.GetSpellDamage(target, SpellSlot.E) + Player.GetSpellDamage(target, SpellSlot.R) && target.Health > Menu["combo"]["waste"].As<MenuSlider>().Value)
                    {

                        if (!R.IsCharging)
                        {
                            R.StartCharging(target.ServerPosition);
                        }
                        if (R.IsCharging && target.Distance(Player) < 400)
                        {
                            R.Cast(target.ServerPosition);
                        }



                    }
                }
            }
            if (E.Ready && useE)
            {
                var target = GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {
                    if (target != null)
                    {
                        if (Menu["combo"]["wall"].Enabled)
                        {
                            
                                for (var i = 0; i < 7; i++)
                                {


                                    var test = Player.ServerPosition.Extend(target.ServerPosition, Player.Distance(target) + 70 * i);
                                    DelayAction.Queue(150, () =>
                                    {
                                        if (IsWall(test, true))
                                        {
                                            E.CastOnUnit(target);
                                        }
                                    });


                                }
                            
                            if (Menu["combo"]["ignore"].Enabled && target.Health < Player.GetSpellDamage(target, SpellSlot.Q) + Player.GetSpellDamage(target, SpellSlot.E))
                            {
                                E.CastOnUnit(target);
                            }


                        }
                        if (!Menu["combo"]["wall"].Enabled)
                        {

                            E.CastOnUnit(target);
                        }
                    }
                }
            }
            if (Q.Ready && useQ)
            {
                var target = GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {

                            Q.Cast(target);


                        }
                    }
                }
            }
            if (W.Ready && useW)
            {
                var target = GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {

                            W.Cast();


                        }
                    }
                }
            }



        }


        public static bool IsWall(Vector3 pos, bool includeBuildings = false)
        {
            var point = NavMesh.WorldToCell(pos).Flags;
            return point.HasFlag(NavCellFlags.Wall) || includeBuildings && point.HasFlag(NavCellFlags.Building);
        }
        private void OnHarass()
        {
            bool useQ = Menu["harass"]["useq"].Enabled;
            bool useE = Menu["harass"]["usee"].Enabled;
            float manapercent = Menu["harass"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                if (Q.Ready && useQ)
                {
                    var target = GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(Q.Range))
                        {
                            if (target != null)
                            {

                                Q.Cast(target);


                            }
                        }
                    }
                }

                if (E.Ready && useE)
                {
                    var target = GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {
                        if (target != null)
                        {
                            if (Menu["harass"]["wall"].Enabled)
                            {
                                for (var i = 0; i < 7; i++)
                                {


                                    var test = Player.ServerPosition.Extend(target.ServerPosition, Player.Distance(target) + 70 * i);
                                    DelayAction.Queue(150, () =>
                                    {
                                        if (IsWall(test, true))
                                        {
                                            E.CastOnUnit(target);
                                        }
                                    });


                                }
                            }
                            if (!Menu["harass"]["wall"].Enabled)
                            {

                                E.CastOnUnit(target);



                            }

                        }
                    }
                }
            }
        }
    }
}

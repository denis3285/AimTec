

using Aimtec.SDK.Events;

namespace Syndra_By_Kornis
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

    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.Util;


    using Spell = Aimtec.SDK.Spell;

    internal class Syndra
    {
        public static Menu Menu = new Menu("Syndra By Kornis", "Syndra by Kornis", true);

        public static Orbwalker Orbwalker = new Orbwalker();

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public static Spell Q, W, E, R, EQ, Q2;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 800f);
            Q2 = new Spell(SpellSlot.Q, 800f);
            W = new Spell(SpellSlot.W, 950f);
            E = new Spell(SpellSlot.E, 700f);
            R = new Spell(SpellSlot.R, 675);
            EQ = new Spell(SpellSlot.Q, 1100);


        }

        static double GetR(Obj_AI_Base target)
        {
            double meow = 0;
            double extra = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 90;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 135;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 180;
            }
            double ap = Player.TotalAbilityDamage * 0.2;
            double main = (ap + meow) * 4;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Ammo > 3)
            {
                extra = (ap + meow) * (Player.SpellBook.GetSpell(SpellSlot.R).Ammo - 3);
            }
            double together = main + extra;
            double damage = Player.CalculateDamage(target, DamageType.Magical, together);
            return damage - 50;

        }

        public static float GetAngleByDegrees(float degrees)
        {
            return (float) (degrees * Math.PI / 180);
        }

        public Syndra()
        {
            Orbwalker.Attach(Menu);
            var ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useqe", "Use QE in Combo"));
                ComboMenu.Add(new MenuSlider("range", "^- QE Range", 1100, 800, 1150));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));

                var EngageMenu = new Menu("engage", "R Settings");
                EngageMenu.Add(new MenuList("rmode", "R Mode:", new[] {"Finisher", "Engaging"}, 0));
                EngageMenu.Add(new MenuKeyBind("rtoggle", "^- Toggle Key", KeyCode.G, KeybindType.Press));
                EngageMenu.Add(new MenuBool("user", "Use R in Combo"));
                EngageMenu.Add(new MenuSlider("waster", "^- Don't waste R if Enemy Health <=", 0, 0, 500));
                EngageMenu.Add(new MenuSlider("orb", "Min. Orbs for Engage", 5, 3, 6));
                EngageMenu.Add(new MenuBool("kill", "Only Enagage if Combo Damage can Kill"));
                ComboMenu.Add(EngageMenu);


                ComboMenu.Add(new MenuKeyBind("qe", "QE Key", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuList("qemode", "QE on Key Mode", new[] {"Target", "Mouse", "Logic"}, 1));
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
                HarassMenu.Add(new MenuKeyBind("qtoggle", "Auto Q Toggle", KeyCode.K, KeybindType.Toggle));
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("dashing", "Auto Q on Dash"));

                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("useqe", "Use QE in Harass"));

                HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));


            }
            Menu.Add(HarassMenu);
            var AAMenu = new Menu("aa", "AA Settings");
            {
                AAMenu.Add(new MenuBool("disable", "Disable AA", false));
                AAMenu.Add(new MenuSlider("level", "At what Level disable AA", 6, 1, 18));
                AAMenu.Add(new MenuKeyBind("aakey", "AA Disable Key: (Combo)", KeyCode.Space, KeybindType.Press));
            }
            Menu.Add(AAMenu);
            var FarmMenu = new Menu("farming", "Farming");
            FarmMenu.Add(new MenuKeyBind("toggle", "Farm Toggle", KeyCode.Z, KeybindType.Toggle));
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hitq", "^- if Hits X Minions", 2, 1, 6));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hitw", "^- if Hits X Minions", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));

            }
            Menu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            var KSMenu = new Menu("killsteal", "Killsteal");
            {
                KSMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KSMenu.Add(new MenuBool("ksw", "Killsteal with W"));
                KSMenu.Add(new MenuBool("ksr", "Killsteal with R"));
                KSMenu.Add(new MenuSlider("waster", "^- Don't waste R if Enemy Health <=", 0, 0, 500));

            }
            Menu.Add(KSMenu);

            var DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawqe", "Draw QE Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range", false));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range", false));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw R Damage"));
                DrawMenu.Add(new MenuBool("drawrmode", "Draw R Mode"));
                DrawMenu.Add(new MenuBool("toggles", "Draw Toggles"));

            }


            Menu.Add(DrawMenu);
            Menu.Add(new MenuList("pred", "Pred.", new[] {"Old Version", "New Version"}, 1));
            Menu.Add(new MenuList("preds", "Prediction Mode", new[] {"Core", "TimbelPred"}, 1));
            Gapcloser.Attach(Menu, "E Anti-GapClose");
            Syndra_By_Kornis.DashQ.AutoQ.Attach(Menu, "Auto Q on Dashes");
            Menu.Attach();


            Render.OnPresent += Render_OnPresent;
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += OnGapcloser;
            DashQ.AutoQ.DashQ += OnDash;
            Obj_AI_Base.OnProcessSpellCast += ProcessCast;
            Orbwalker.PreAttack += OnPreAttack;



            LoadSpells();
            Console.WriteLine("Syndra by Kornis - Loaded");
        }

        private void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready && !target.IsDashing() &&
                target.IsValidTarget(E.Range))
            {

                E.Cast(Args.EndPosition);

            }
        }

        private void OnDash(Obj_AI_Hero target, Syndra_By_Kornis.DashQ.GapcloserArgs Args)
        {
            if (Menu["harass"]["dashing"].Enabled && Q.Ready)
            {
                if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready)
                {

                    Q.Cast(Args.EndPosition);
                }
            }
        }

        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            if (Menu["aa"]["aakey"].Enabled)
            {
                if (Player.Mana > 100 && Menu["aa"]["disable"].Enabled &&
                    Menu["aa"]["level"].As<MenuSlider>().Value <= Player.Level)
                {
                    if (args.Target.IsHero)
                    {
                        args.Cancel = true;
                    }
                }
            }
        }

        private void ProcessCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (e.Sender.IsMe)
            {
                if (e.SpellData.Name == "SyndraQ")
                {
                    if (Orbwalker.Mode.Equals(OrbwalkingMode.Combo) || Orbwalker.Mode.Equals(OrbwalkingMode.Mixed))
                    {
                        var EndPos = e.End;
                        if (EndPos.Distance(Player) <= 850 && lastw < Game.TickCount)
                        {
                            DelayAction.Queue(100, () => E.Cast(EndPos));
                            zzzzzzzzzzzzzzzzzzz = Game.TickCount + 750;

                        }
                    }
                    if (Menu["combo"]["qe"].Enabled)
                    {
                        var EndPos = e.End;
                        if (EndPos.Distance(Player) <= 850 && lastw < Game.TickCount)
                        {
                            DelayAction.Queue(100, () => E.Cast(EndPos));

                        }
                    }
                }
                if (e.SpellData.Name == "SyndraW")
                {

                    W.LastCastAttemptT = Game.TickCount + 100;
                    lastw = Game.TickCount + Game.Ping + 20;
                    lastwe = Game.TickCount + Game.Ping + 200;
                    delayyyy = 1000 + Game.TickCount;

                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int time;
        private int lastwe;
        private int lastw;

        private int delayyyy;
        private int hello;
        private int lastqe;
        private int zzzzzzzzzzzzzzzzzzz;

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
            if (Menu["drawings"]["drawrmode"].Enabled)
            {
                if (Menu["combo"]["engage"]["rmode"].As<MenuList>().Value == 0)
                {

                    Render.Text(xaOffset - 50, yaOffset + 10, Color.HotPink, "R Mode: Finisher",
                        RenderTextFlags.VerticalCenter);
                }
                if (Menu["combo"]["engage"]["rmode"].As<MenuList>().Value == 1)
                {

                    Render.Text(xaOffset - 50, yaOffset + 10, Color.HotPink, "R Mode: Engaging",
                        RenderTextFlags.VerticalCenter);

                }
            }

            if (Menu["drawings"]["toggles"].Enabled)
            {
                if (Menu["farming"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 40, Color.GreenYellow, "Farm: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["farming"]["toggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 40, Color.Red, "Farm: OFF",
                        RenderTextFlags.VerticalCenter);
                }
                if (Menu["harass"]["qtoggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 60, Color.GreenYellow, "Auto Harass: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!Menu["harass"]["qtoggle"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 60, Color.Red, "Auto Harass: OFF",
                        RenderTextFlags.VerticalCenter);
                }
            }

            if (Menu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Wheat);
            }
            if (Menu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Wheat);
            }
            if (Menu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (Menu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Crimson);
            }
            if (Menu["drawings"]["drawqe"].Enabled)
            {
                Render.Circle(Player.Position, Menu["combo"]["range"].As<MenuSlider>().Value, 40, Color.HotPink);
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
                            int yOffset = SyOffset(heroUnit) - 5;
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;
                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health >

                                                     GetR(unit)
                                             ? width * ((unit.Health -
                                                         GetR(unit)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health <
                                GetR(unit)
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }

        }



        private void Game_OnUpdate()
        {

            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                R.Range = 750;
            }
            if (Menu["pred"].As<MenuList>().Value == 1)
            {

                Q.SetSkillshot(0.6f, 120, float.MaxValue, false, SkillshotType.Circle, false, HitChance.None);

                W.SetSkillshot(0.25f, 50, 1600, false, SkillshotType.Circle, false, HitChance.None);
                E.SetSkillshot(0.25f, (float) (45 * 0.5), 2500, false, SkillshotType.Circle);
                EQ.SetSkillshot(float.MaxValue, 55, 2000, false, SkillshotType.Circle);

            }
            if (Menu["pred"].As<MenuList>().Value == 0)
            {

                Q.SetSkillshot(0.50f, 52, float.MaxValue, false, SkillshotType.Circle, false, HitChance.None);
                Q2.SetSkillshot(0, 60, float.MaxValue, false, SkillshotType.Circle, false, HitChance.High);

                W.SetSkillshot(0.45f, 40, 2000, false, SkillshotType.Circle, false, HitChance.None);
                E.SetSkillshot(0.25f, (float) (45 * 0.5), 2500f, false, SkillshotType.Cone);
                EQ.SetSkillshot(0.4f, 80, 1900, false, SkillshotType.Line);
            }
            if (Player.IsDead || MenuGUI.IsChatOpen())
            {
                return;
            }

            Killsteal();
            if (Menu["harass"]["qtoggle"].Enabled)
            {
                var target = GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target != null)
                    {
                        if (Menu["preds"].As<MenuList>().Value == 1)
                        {
                            Q.Cast(TimbelPred.PredEx(target, 0.5f));
                        }
                        if (Menu["preds"].As<MenuList>().Value == 0)
                        {
                            Q.Cast(target);
                        }
                    }

                }
            }

            if (Menu["combo"]["qe"].Enabled && E.Ready)
            {
                var pos = (Game.CursorPos - Player.ServerPosition).Normalized();
                var target = GetBestEnemyHeroTargetInRange(EQ.Range);
                switch (Menu["combo"]["qemode"].As<MenuList>().Value)
                {
                    case 0:
                    {
                        if (!target.IsValidTarget())
                        {
                            return;
                        }
                        if (Menu["pred"].As<MenuList>().Value == 0)
                        {
                            if (target.Distance(Player) >= 800 && target.Distance(Player) <= 900)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.55f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                Q2.Cast(startpos);

                            }
                            if (target.Distance(Player) >= 900 && target.Distance(Player) <= 1000)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.7f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                Q2.Cast(startpos);

                            }
                            if (target.Distance(Player) >= 1000)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.83f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 30);


                                Q2.Cast(startpos);
                            }
                            if (target.Distance(Player) <= 800)
                            {
                                Q.Cast(target);
                            }
                        }
                        if (Menu["pred"].As<MenuList>().Value == 1 && target.Distance(Player) > E.Range)

                        {
                            EQ.Delay = E.Delay + Q.Range / E.Speed;

                            EQ.From = Player.ServerPosition.Extend(target.ServerPosition, Q.Range);

                            var pred = EQ.GetPrediction(target);
                            if (pred.HitChance >= HitChance.High)
                            {


                                Q.Cast(Player.ServerPosition.Extend(pred.CastPosition, Q.Range - 100));
                            }
                        }

                    }
                        break;
                    case 1:

                        if (Player.Distance(Game.CursorPos) < 800)
                        {
                            Q.Cast(Game.CursorPos);
                        }
                        if (Player.Distance(Game.CursorPos) > 800)
                        {
                            Q.Cast(Player.ServerPosition + pos * 800);
                        }

                        break;
                    case 2:

                        if (Player.CountEnemyHeroesInRange(Menu["combo"]["range"].As<MenuSlider>().Value) == 0)
                        {

                            if (Player.Distance(Game.CursorPos) < 800)
                            {
                                Q.Cast(Game.CursorPos);
                            }
                            if (Player.Distance(Game.CursorPos) > 800)
                            {
                                Q.Cast(Player.ServerPosition + pos * 800);
                            }
                        }
                        if (Player.CountEnemyHeroesInRange(Menu["combo"]["range"].As<MenuSlider>().Value) > 0)
                        {


                            if (!target.IsValidTarget())
                            {
                                return;
                            }
                            if (Menu["pred"].As<MenuList>().Value == 0)
                            {
                                if (target.Distance(Player) >= 800 && target.Distance(Player) <= 900)
                                {


                                    Q2.From = target.ServerPosition;
                                    Q2.Delay = 0.55f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                    var pred = Q2.GetPrediction(target);
                                    var ppos = Player.ServerPosition;
                                    var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                    Q2.Cast(startpos);

                                }
                                if (target.Distance(Player) >= 900 && target.Distance(Player) <= 1000)
                                {


                                    Q2.From = target.ServerPosition;
                                    Q2.Delay = 0.7f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                    var pred = Q2.GetPrediction(target);
                                    var ppos = Player.ServerPosition;
                                    var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                    Q2.Cast(startpos);

                                }
                                if (target.Distance(Player) >= 1000)
                                {


                                    Q2.From = target.ServerPosition;
                                    Q2.Delay = 0.83f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                    var pred = Q2.GetPrediction(target);
                                    var ppos = Player.ServerPosition;
                                    var startpos = ppos.Extend(pred.CastPosition, Q.Range - 30);


                                    Q2.Cast(startpos);
                                }
                                if (target.Distance(Player) <= 800)
                                {
                                    if (Menu["preds"].As<MenuList>().Value == 1)
                                    {
                                        Q.Cast(TimbelPred.PredEx(target, 0.5f));
                                    }
                                    if (Menu["preds"].As<MenuList>().Value == 0)
                                    {
                                        Q.Cast(target);
                                    }
                                }

                            }
                            if (Menu["pred"].As<MenuList>().Value == 1 && target.Distance(Player) > E.Range)
                            {
                                EQ.Delay = E.Delay + Q.Range / E.Speed;

                                EQ.From = Player.ServerPosition.Extend(target.ServerPosition, Q.Range);

                                var pred = EQ.GetPrediction(target);
                                if (pred.HitChance >= HitChance.High)
                                {


                                    Q.Cast(Player.ServerPosition.Extend(pred.CastPosition, Q.Range - 100));
                                }
                            }
                        }
                        break;
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
            if (Menu["combo"]["engage"]["rtoggle"].Enabled && time <= Game.TickCount)
            {
                if (Menu["combo"]["engage"]["rmode"].As<MenuList>().Value == 0)
                {
                    Menu["combo"]["engage"]["rmode"].As<MenuList>().Value = 1;
                    time = Game.TickCount + 300;
                    return;

                }
                if (Menu["combo"]["engage"]["rmode"].As<MenuList>().Value == 1)
                {
                    Menu["combo"]["engage"]["rmode"].As<MenuList>().Value = 0;
                    time = Game.TickCount + 300;
                    return;
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
            if (Menu["farming"]["toggle"].Enabled)
            {
                bool useQ = Menu["farming"]["lane"]["useq"].Enabled;
                bool useW = Menu["farming"]["lane"]["usew"].Enabled;
                float manapercent = Menu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
                if (manapercent < Player.ManaPercent())
                {
                    if (useQ)
                    {
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                    minion.ServerPosition)) >= Menu["farming"]["lane"]["hitq"].As<MenuSlider>().Value)
                            {

                                if (minion.IsValidTarget(Q.Range) && minion != null)
                                {
                                    Q.Cast(minion);
                                }
                            }
                        }
                    }
                    if (useW)
                    {
                        foreach (var minion in GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(225, false, false,
                                    minion.ServerPosition)) >= Menu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                            {

                                if (minion.IsValidTarget(W.Range) && minion != null)
                                {
                                    if (!Player.HasBuff("syndrawtooltip") && delayyyy < Game.TickCount)
                                    {
                                        var grab =
                                            GameObjects.EnemyMinions.FirstOrDefault(m => m.IsValidSpellTarget(W.Range));

                                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                                minion.ServerPosition)) >=
                                            Menu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                        {
                                            if (minion != null)
                                            {
                                                W.CastOnUnit(grab);
                                                delayyyy = Game.TickCount + 1000;
                                            }
                                        }

                                    }
                                    if (Player.HasBuff("syndrawtooltip"))
                                    {

                                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                                minion.ServerPosition)) >=
                                            Menu["farming"]["lane"]["hitw"].As<MenuSlider>().Value)
                                        {


                                            if (minion != null)
                                            {
                                                W.Cast(minion);

                                            }
                                        }
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
                    bool useQ = Menu["farming"]["jungle"]["useq"].Enabled;
                    bool useW = Menu["farming"]["jungle"]["usew"].Enabled;
                    float manapercent = Menu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;
                    if (manapercent < Player.ManaPercent())
                    {
                        if (useQ && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }
                        if (useW && jungleTarget.IsValidTarget(W.Range))
                        {
                            if (!Player.HasBuff("syndrawtooltip") && delayyyy < Game.TickCount)
                            {
                                var grab =
                                    GameObjects.Jungle.FirstOrDefault(
                                        m => m.Distance(Player) < W.Range && m.IsValidTarget());


                                if (jungleTarget != null)
                                {
                                    W.CastOnUnit(grab);
                                    delayyyy = Game.TickCount + 1000;
                                }


                            }
                            if (Player.HasBuff("syndrawtooltip"))
                            {


                                if (jungleTarget != null)
                                {
                                    W.Cast(Player.ServerPosition);

                                }
                            }
                        }
                    }
                }

            }


        }


        public GameObject Objects()
        {

            var orb = GameObjects.AllGameObjects.FirstOrDefault(
                x => x.Name == "Seed" && x.IsValid && !x.IsDead && x.Distance(Player) <= 960);
            var minion =
                GameObjects.EnemyMinions.FirstOrDefault(
                    m => m.Distance(Player) < W.Range && m.IsValidTarget());
            var jungle =
                GameObjects.Jungle.FirstOrDefault(
                    m => m.Distance(Player) < W.Range && m.IsValidTarget());
            if (orb != null)
            {
                return orb;
            }
            if (minion != null)
            {
                return minion;
            }


            if (jungle != null)
            {
                return jungle;
            }
            return null;
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
                    if (Menu["preds"].As<MenuList>().Value == 1)
                    {
                        Q.Cast(TimbelPred.PredEx(bestTarget, 0.5f));
                    }
                    if (Menu["preds"].As<MenuList>().Value == 0)
                    {
                        Q.Cast(bestTarget);
                    }
                }
            }
            if (W.Ready &&
                Menu["killsteal"]["ksw"].Enabled)
            {
                var bestTarget = GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {




                    if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState == 1)
                    {

                        if (bestTarget.IsValidTarget(W.Range) && W.Ready)
                        {

                            if (delayyyy <= Game.TickCount)
                            {


                                if (Objects() != null && zzzzzzzzzzzzzzzzzzz <= Game.TickCount &&
                                    Game.TickCount - W.LastCastAttemptT > Game.Ping + 300)
                                {

                                    W.Cast(Objects().ServerPosition);

                                    W.LastCastAttemptT = Game.TickCount;

                                }

                            }
                        }
                    }
                    if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState != 1 &&
                        Game.TickCount - W.LastCastAttemptT > Game.Ping + 100)
                    {

                        if (!bestTarget.HasBuff("SyndraEDebuff"))
                        {
                            if (Objects() != null)
                            {


                                if (lastqe < Game.TickCount)
                                {
                                    if (Menu["preds"].As<MenuList>().Value == 1)
                                    {
                                        W.Cast(TimbelPred.PredEx(bestTarget, 0.5f));
                                    }
                                    if (Menu["preds"].As<MenuList>().Value == 0)
                                    {
                                        W.Cast(bestTarget);
                                    }
                                }
                            }


                        }
                    }
                }
            }
            if (R.Ready &&
                Menu["killsteal"]["ksr"].Enabled)
            {
                var bestTarget = GetBestKillableHero(R, DamageType.Magical);
                if (bestTarget != null &&
                    GetR(bestTarget) > bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range) && bestTarget.Health >=
                    Menu["killsteal"]["waster"].As<MenuSlider>().Value)
                {
                    if (!Menu["blacklist"][bestTarget.ChampionName.ToLower()].Enabled)
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

        private void OnCombo()
        {

            if (Menu["combo"]["useq"].Enabled)
            {
                var target = GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {
                    if (target.Distance(Player) < Q.Range)
                    {

                        if (target != null)
                        {

                            if (Menu["preds"].As<MenuList>().Value == 1)
                            {
                                Q.Cast(TimbelPred.PredEx(target, 0.5f));
                            }
                            if (Menu["preds"].As<MenuList>().Value == 0)
                            {
                                Q.Cast(target);
                            }
                        }

                    }
                }
            }
            if (Menu["combo"]["usew"].Enabled)
            {

                var target = GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget() && target.Distance(Player) <= W.Range)
                {
                    if (target != null)
                    {


                        if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState == 1)
                        {

                            if (target.IsValidTarget(W.Range) && W.Ready)
                            {

                                if (delayyyy <= Game.TickCount)
                                {


                                    if (Objects() != null && zzzzzzzzzzzzzzzzzzz <= Game.TickCount &&
                                        Game.TickCount - W.LastCastAttemptT > Game.Ping + 300)
                                    {

                                        W.Cast(Objects().ServerPosition);

                                        W.LastCastAttemptT = Game.TickCount;

                                    }

                                }
                            }
                        }
                        if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState != 1 &&
                            Game.TickCount - W.LastCastAttemptT > Game.Ping + 100)
                        {

                            if (!target.HasBuff("SyndraEDebuff"))
                            {
                                if (Objects() != null)
                                {


                                    if (lastqe < Game.TickCount)
                                    {
                                        if (Menu["preds"].As<MenuList>().Value == 1)
                                        {
                                            W.Cast(TimbelPred.PredEx(target, 0.5f));
                                        }
                                        if (Menu["preds"].As<MenuList>().Value == 0)
                                        {
                                            W.Cast(target);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            if (E.Ready && Menu["combo"]["usee"].Enabled)
            {
                var target = GetBestEnemyHeroTargetInRange(1100);

                if (target.IsValidTarget())
                {
                    foreach (var orb in GameObjects.AllGameObjects.Where(x => x.Distance(Player) < 1000))
                    {
                        if (orb.Name == "Seed" && orb.IsValid && !orb.IsDead)
                        {
                            if (orb.Distance(Player) <= E.Range && Player.Distance(orb.ServerPosition) >= 100 &&
                                target.Distance(Player) <= 1100)
                            {

                                var enemyPred = EQ.GetPrediction(target);
                                var test = Player.Distance(enemyPred.CastPosition);
                                var miau = Player.ServerPosition.Extend(orb.Position, test);
                                if (miau.Distance(enemyPred.CastPosition) < EQ.Width + target.BoundingRadius - 60)
                                {
                                    E.Cast(orb.Position);
                                    lastqe = Game.TickCount + Game.Ping + 100;

                                }
                            }
                        }
                    }
                }
            }
            if (E.Ready && Menu["combo"]["useqe"].Enabled)

            {
                var target = GetBestEnemyHeroTargetInRange(EQ.Range);

                if (target.IsValidTarget() && target.Distance(Player) < Menu["combo"]["range"].As<MenuSlider>().Value)
                {
                    if (Menu["pred"].As<MenuList>().Value == 0)
                    {

                        if (target.Distance(Player) >= 800 && target.Distance(Player) <= 900)
                        {


                            Q2.From = target.ServerPosition;
                            Q2.Delay = 0.55f + Player.Distance(target.ServerPosition) / EQ.Speed;
                            var pred = Q2.GetPrediction(target);
                            var ppos = Player.ServerPosition;
                            var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                            Q2.Cast(startpos);

                        }
                        if (target.Distance(Player) >= 900 && target.Distance(Player) <= 1000)
                        {


                            Q2.From = target.ServerPosition;
                            Q2.Delay = 0.7f + Player.Distance(target.ServerPosition) / EQ.Speed;
                            var pred = Q2.GetPrediction(target);
                            var ppos = Player.ServerPosition;
                            var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                            Q2.Cast(startpos);

                        }
                        if (target.Distance(Player) >= 1000)
                        {


                            Q2.From = target.ServerPosition;
                            Q2.Delay = 0.80f + Player.Distance(target.ServerPosition) / EQ.Speed;
                            var pred = Q2.GetPrediction(target);
                            var ppos = Player.ServerPosition;
                            var startpos = ppos.Extend(pred.CastPosition, Q.Range - 30);


                            Q2.Cast(startpos);
                        }
                        if (target.Distance(Player) <= 800)
                        {
                            if (Menu["preds"].As<MenuList>().Value == 1)
                            {
                                Q.Cast(TimbelPred.PredEx(target, 0.5f));
                            }
                            if (Menu["preds"].As<MenuList>().Value == 0)
                            {
                                Q.Cast(target);
                            }
                        }


                    }
                    if (Menu["pred"].As<MenuList>().Value == 1 && target.Distance(Player) > E.Range)
                    {

                        EQ.Delay = E.Delay + Q.Range / E.Speed;

                        EQ.From = Player.ServerPosition.Extend(target.ServerPosition, Q.Range);

                        var pred = EQ.GetPrediction(target);

                        if (pred.HitChance >= HitChance.High)
                        {


                            Q.Cast(Player.ServerPosition.Extend(pred.CastPosition, Q.Range - 100));
                        }
                    }




                }
            }
            if (R.Ready && Menu["combo"]["engage"]["user"].Enabled)
            {
                var target = GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget())
                {
                    if (target.Health >= Menu["combo"]["engage"]["waster"].As<MenuSlider>().Value)
                    {
                        switch (Menu["combo"]["engage"]["rmode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (target != null)
                                {
                                    if (target.Health < GetR(target))
                                    {
                                        if (!Menu["blacklist"][target.ChampionName.ToLower()].Enabled)
                                        {
                                            R.CastOnUnit(target);
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (target != null)
                                {

                                    if (!Menu["blacklist"][target.ChampionName.ToLower()].Enabled)
                                    {
                                        if (Player.SpellBook.GetSpell(SpellSlot.R).Ammo >=
                                            Menu["combo"]["engage"]["orb"].As<MenuSlider>().Value)
                                        {
                                            if (!Menu["combo"]["engage"]["kill"].Enabled)
                                            {
                                                R.CastOnUnit(target);
                                            }
                                            if (Menu["combo"]["engage"]["kill"].Enabled)
                                            {
                                                double QDamage = Player.GetSpellDamage(target, SpellSlot.Q);
                                                double WDamage = Player.GetSpellDamage(target, SpellSlot.W);
                                                double EDamage = Player.GetSpellDamage(target, SpellSlot.E);
                                                double RDamage = GetR(target);

                                                if (target.Health <= QDamage + WDamage + EDamage + RDamage)
                                                {
                                                    R.CastOnUnit(target);
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }

        private void OnHarass()
        {
            if (Player.ManaPercent() >= Menu["harass"]["mana"].As<MenuSlider>().Value)
            {



                if (Menu["harass"]["useq"].Enabled)
                {
                    var target = GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget() && target.Distance(Player) < Q.Range)
                    {
                        if (target != null)
                        {
                            if (target.IsValidTarget(Q.Range))
                            {
                                if (Menu["preds"].As<MenuList>().Value == 1)
                                {
                                    Q.Cast(TimbelPred.PredEx(target, 0.5f));
                                }
                                if (Menu["preds"].As<MenuList>().Value == 0)
                                {
                                    Q.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (Menu["harass"]["usew"].Enabled)
                {

                    var target = GetBestEnemyHeroTargetInRange(W.Range);

                    if (target.IsValidTarget() && target.Distance(Player) <= W.Range)
                    {
                        if (target != null)
                        {


                            if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState == 1)
                            {

                                if (target.IsValidTarget(W.Range) && W.Ready)
                                {

                                    if (delayyyy <= Game.TickCount)
                                    {


                                        if (Objects() != null && zzzzzzzzzzzzzzzzzzz <= Game.TickCount &&
                                            Game.TickCount - W.LastCastAttemptT > Game.Ping + 300)
                                        {

                                            W.Cast(Objects().ServerPosition);

                                            W.LastCastAttemptT = Game.TickCount;

                                        }

                                    }
                                }
                            }
                            if (Player.SpellBook.GetSpell(SpellSlot.W).ToggleState != 1 &&
                                Game.TickCount - W.LastCastAttemptT > Game.Ping + 100)
                            {

                                if (!target.HasBuff("SyndraEDebuff"))
                                {
                                    if (Objects() != null)
                                    {


                                        if (lastqe < Game.TickCount)
                                        {
                                            if (Menu["preds"].As<MenuList>().Value == 1)
                                            {
                                                W.Cast(TimbelPred.PredEx(target, 0.5f));
                                            }
                                            if (Menu["preds"].As<MenuList>().Value == 0)
                                            {
                                                W.Cast(target);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (E.Ready && Menu["harass"]["usee"].Enabled)
                {
                    var target = GetBestEnemyHeroTargetInRange(1100);

                    if (target.IsValidTarget())
                    {
                        foreach (var orb in GameObjects.AllGameObjects.Where(x => x.Distance(Player) < 1000))
                        {
                            if (orb.Name == "Seed" && orb.IsValid && !orb.IsDead)
                            {
                                if (orb.Distance(Player) <= E.Range && Player.Distance(orb.ServerPosition) >= 100 &&
                                    target.Distance(Player) <= 1100)
                                {

                                    var enemyPred = EQ.GetPrediction(target);
                                    var test = Player.Distance(enemyPred.CastPosition);
                                    var miau = Player.ServerPosition.Extend(orb.Position, test);
                                    if (miau.Distance(enemyPred.CastPosition) < EQ.Width + target.BoundingRadius - 60)
                                    {
                                        E.Cast(orb.Position);
                                        lastqe = Game.TickCount + Game.Ping + 100;

                                    }
                                }
                            }
                        }
                    }
                }
                if (E.Ready && Menu["harass"]["useqe"].Enabled)

                {
                    var target = GetBestEnemyHeroTargetInRange(EQ.Range);

                    if (target.IsValidTarget() &&
                        target.Distance(Player) < Menu["combo"]["range"].As<MenuSlider>().Value)
                    {
                        if (Menu["pred"].As<MenuList>().Value == 0)
                        {

                            if (target.Distance(Player) >= 800 && target.Distance(Player) <= 900)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.55f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                Q2.Cast(startpos);

                            }
                            if (target.Distance(Player) >= 900 && target.Distance(Player) <= 1000)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.7f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 100);


                                Q2.Cast(startpos);

                            }
                            if (target.Distance(Player) >= 1000)
                            {


                                Q2.From = target.ServerPosition;
                                Q2.Delay = 0.83f + Player.Distance(target.ServerPosition) / EQ.Speed;
                                var pred = Q2.GetPrediction(target);
                                var ppos = Player.ServerPosition;
                                var startpos = ppos.Extend(pred.CastPosition, Q.Range - 30);


                                Q2.Cast(startpos);
                            }
                            if (target.Distance(Player) <= 800)
                            {
                                if (Menu["preds"].As<MenuList>().Value == 1)
                                {
                                    Q.Cast(TimbelPred.PredEx(target, 0.5f));
                                }
                                if (Menu["preds"].As<MenuList>().Value == 0)
                                {
                                    Q.Cast(target);
                                }
                            }


                        }
                        if (Menu["pred"].As<MenuList>().Value == 1 && target.Distance(Player) > E.Range)
                        {

                            EQ.Delay = E.Delay + Q.Range / E.Speed;

                            EQ.From = Player.ServerPosition.Extend(target.ServerPosition, Q.Range);

                            var pred = EQ.GetPrediction(target);

                            if (pred.HitChance >= HitChance.High)
                            {


                                Q.Cast(Player.ServerPosition.Extend(pred.CastPosition, Q.Range - 100));
                            }
                        }




                    }
                }
            }
        }
    }
}
// ;> 

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
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Support_AIO;
using Support_AIO.Bases;
using Support_AIO.Handlers;
using Support_AIO.SpellBlocking;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Malzahar : Champion
    {
        internal override void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            if (sender.IsMe)
            {
                if ((e.Slot == SpellSlot.Q || e.Slot == SpellSlot.W || e.Slot == SpellSlot.E ||
                     e.Slot == SpellSlot.Item1 || e.Slot == SpellSlot.Item2 || e.Slot == SpellSlot.Item3 ||
                     e.Slot == SpellSlot.Item4 || e.Slot == SpellSlot.Item5 || e.Slot == SpellSlot.Item6) &&
                    Player.HasBuff("malzaharrsound"))

                {
                    e.Process = false;

                }
            }
        }

        protected override void Combo()
        {
            if (Player.HasBuff("malzaharrsound"))
            {
                return;
            }

            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useE = RootMenu["combo"]["usee"].Enabled;

            switch (RootMenu["combo"]["combomode"].As<MenuList>().Value)
            {
                case 0:
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    if (RootMenu["combo"]["user"].Enabled)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                        if (target.IsValidTarget() && !W.Ready && !Q.Ready)
                        {

                            if (target != null && target.IsValidTarget(R.Range))
                            {
                                switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        if (target.Health < Player.GetSpellDamage(target, SpellSlot.Q) +
                                            Player.GetSpellDamage(target, SpellSlot.W) +
                                            Player.GetSpellDamage(target, SpellSlot.E) +
                                            Player.GetSpellDamage(target, SpellSlot.R) +
                                            Player.GetSpellDamage(target, SpellSlot.R,
                                                DamageStage.DamagePerSecond))
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                    case 1:
                                        if (target.HealthPercent()  < RootMenu["combo"]["health"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (RootMenu["combo"]["user"].Enabled)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                        if (target.IsValidTarget() && !W.Ready && !Q.Ready)
                        {

                            if (target != null && target.IsValidTarget(R.Range))
                            {
                                switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        if (target.Health < Player.GetSpellDamage(target, SpellSlot.Q) +
                                            Player.GetSpellDamage(target, SpellSlot.W) +
                                            Player.GetSpellDamage(target, SpellSlot.E) +
                                            Player.GetSpellDamage(target, SpellSlot.R) +
                                            Player.GetSpellDamage(target, SpellSlot.R,
                                                DamageStage.DamagePerSecond))
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                    case 1:
                                        if (target.HealthPercent() < RootMenu["combo"]["health"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                    }

                    break;
                case 2:
                    if (RootMenu["combo"]["user"].Enabled)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(R.Range))
                            {
                                switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        if (target.Health < Player.GetSpellDamage(target, SpellSlot.Q) +
                                            Player.GetSpellDamage(target, SpellSlot.W) +
                                            Player.GetSpellDamage(target, SpellSlot.E) +
                                            Player.GetSpellDamage(target, SpellSlot.R) +
                                            Player.GetSpellDamage(target, SpellSlot.R,
                                                DamageStage.DamagePerSecond))
                                        {
                                            if (R.Cast(target))
                                            {
                                                hewwo = 300 + Game.TickCount;
                                            }
                                        }
                                        break;
                                    case 1:
                                        if (target.HealthPercent() < RootMenu["combo"]["health"].As<MenuSlider>().Value)
                                        {
                                            if (R.Cast(target))
                                            {
                                                hewwo = 300 + Game.TickCount;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    if (hewwo < Game.TickCount)
                    {
                        if (useW)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target != null && target.IsValidTarget(Q.Range))
                                {
                                    W.Cast(target.ServerPosition);
                                }
                            }
                        }
                        if (useQ)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                            if (target.IsValidTarget())
                            {

                                if (target != null && target.IsValidTarget(Q.Range))
                                {
                                    Q.Cast(target);
                                }
                            }
                        }
                        if (useE)
                        {
                            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                            if (target.IsValidTarget())
                            {

                                if (target != null && target.IsValidTarget(E.Range))
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }

                    }
                    break;
                case 3:
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (RootMenu["combo"]["user"].Enabled)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                        if(target.IsValidTarget() && !W.Ready && !Q.Ready)
                        {


                            if (target != null && target.IsValidTarget(R.Range))
                            {
                                switch (RootMenu["combo"]["rmode"].As<MenuList>().Value)
                                {
                                    case 0:
                                        if (target.Health < Player.GetSpellDamage(target, SpellSlot.Q) +
                                            Player.GetSpellDamage(target, SpellSlot.W) +
                                            Player.GetSpellDamage(target, SpellSlot.E) +
                                            Player.GetSpellDamage(target, SpellSlot.R) +
                                            Player.GetSpellDamage(target, SpellSlot.R,
                                                DamageStage.DamagePerSecond))
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                    case 1:
                                        if (target.HealthPercent() < RootMenu["combo"]["health"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
        }


        protected override void SemiR()
        {
            if (RootMenu["flashr"].Enabled)
            {
                if (!Player.HasBuff("malzaharrsound"))
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range + 410);
                    if (R.Ready)
                    {
                        if (Flash.Ready && Flash != null)
                        {
                            Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                            if (target.IsValidTarget())
                            {
                                if (target.Distance(Player) > R.Range)
                                {
                                    if (R.Cast(target))
                                    {
                                        Flash.Cast(target.ServerPosition);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Player.HasBuff("malzaharrsound"))
            {
                Orbwalker.Implementation.MovingEnabled = false;
                Orbwalker.Implementation.AttackingEnabled = false;
            }
            if (!Player.HasBuff("malzaharrsound"))
            {
                Orbwalker.Implementation.MovingEnabled = true;
                Orbwalker.Implementation.AttackingEnabled = true;
            }
        }


        protected override void Farming()
        {

            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var minion in Support_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (!minion.IsValidTarget())
                    {
                        return;
                    }
                    if (RootMenu["farming"]["lane"]["usee"].Enabled)
                    {
                        foreach (var lowest in Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range)
                            .OrderBy(x => x.Health))
                        {
                            if (lowest != null)
                            {

                                if (lowest.IsValidTarget(E.Range))
                                {
                                    E.CastOnUnit(lowest);
                                }
                            }
                        }
                    }
                    if (RootMenu["farming"]["lane"]["useq"].Enabled)
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(150, false, false,
                                    minion.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                            {
                                Q.Cast(minion);
                            }
                        }
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;
                    bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;
                    bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;
                    float manapercent = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;
                    
                        if (useQ)
                        {
                            if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast(jungleTarget);
                            }
                        }
                    if (useW)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            W.Cast(jungleTarget.ServerPosition);
                        }
                    }
                    if (useE)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.CastOnUnit(jungleTarget);
                        }
                    }




                }
            }
        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
        }


        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };
        private int hewwo;

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

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range + 410, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["damage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range * 2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {

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
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R) +
                                                     Player.GetSpellDamage(unit, SpellSlot.R,
                                                         DamageStage.DamagePerSecond)
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.W) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.E) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.R) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.R,
                                                                            DamageStage.DamagePerSecond))) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +
                                Player.GetSpellDamage(unit, SpellSlot.R) +
                                Player.GetSpellDamage(unit, SpellSlot.R,
                                    DamageStage.DamagePerSecond)
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }


        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useW = RootMenu["harass"]["usew"].Enabled;
            bool useE = RootMenu["harass"]["usee"].Enabled;

            switch (RootMenu["harass"]["harassmode"].As<MenuList>().Value)
            {
                case 0:
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    break;
                case 1:
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
     
                    break;
                case 2:
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }


                    break;
                case 3:
                    if (useE)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(E.Range))
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }
                    if (useW)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    if (useQ)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (target.IsValidTarget())
                        {

                            if (target != null && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast(target);
                            }
                        }
                    }

                    break;
            }
        }
    
        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuList("combomode", "Combo Mode", new[] { "Q > W > E > R", "Q > E > W > R", "R > W > Q > E", "E > W > Q > R"}, 1));
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("rmode", "R Usage", new[] { "If Killable", "At X Health" }, 0));
                ComboMenu.Add(new MenuSlider("health", "If X Health", 50));
                
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuList("harassmode", "Harass Mode", new[] { "Q > W > E", "Q > E > W", "W > Q > E", "E > W > Q" }, 1));
                HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
 
            }
            RootMenu.Add(HarassMenu);

            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if Hits X", 2, 1, 6));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("flashr", "Draw Flash R Range"));
                DrawMenu.Add(new MenuBool("damage", "Draw Damage"));
            }

            RootMenu.Add(DrawMenu);
            RootMenu.Add(new MenuKeyBind("flashr", "Flash - R", KeyCode.T, KeybindType.Press));
            RootMenu.Attach();
        }


        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 900);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 150);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 650);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 700);
            Q.SetSkillshot(0.9f, 60, float.MaxValue, false, SkillshotType.Line, false, HitChance.None);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);
        }
    }
}

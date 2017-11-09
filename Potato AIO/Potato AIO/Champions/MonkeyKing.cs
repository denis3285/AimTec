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
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class MonkeyKing : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"].Enabled;
           

            bool useR = RootMenu["combo"]["user"].Enabled;
           
            if (RootMenu["combo"]["usee"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range))
                    {
                        
                        if (target != null && target.Distance(Player) >=
                            RootMenu["combo"]["erange"].As<MenuSlider>().Value)
                        {
                            if (RootMenu["combo"]["turret"].Enabled)
                            {
                                E.CastOnUnit(target);
                            }
                            if (!RootMenu["combo"]["turret"].Enabled && !target.IsUnderEnemyTurret())
                            {
                                E.CastOnUnit(target);
                            }
                        }
                    }

                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range) && !RootMenu["combo"]["qaa"].Enabled)
                    { 

                        if (target != null)
                        {
                            Q.CastOnUnit(target);
                        }
                    }

                }
            }
            if (useR && !Player.HasBuff("MonkeyKingSpinToWin") )
            { 
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget() && target != null)
                {

                    if (target.IsValidTarget(R.Range))
                    {

                        if (Player.CountEnemyHeroesInRange(R.Range) >=
                            RootMenu["combo"]["hitsr"].As<MenuSlider>().Value)
                        {
                            if (RootMenu["combo"]["qr"].Enabled)
                            {
                                if(Player.HasBuff("MonkeyKingDoubleAttack") && target.Distance(Player) > 250)
                                {

                                    R.Cast();
                                }
                                if (!Player.HasBuff("MonkeyKingDoubleAttack"))
                                {

                                    R.Cast();
                                }
                            }
                            if (!RootMenu["combo"]["qr"].Enabled)
                            {
                            
                                    R.Cast();
                                
                            }
                        }
                        if (RootMenu["combo"]["kill"].Enabled && target.Health >= RootMenu["combo"]["wasteR"].As<MenuSlider>().Value)
                        { 
                            if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) + Player.GetAutoAttackDamage(target)+
                                Player.GetSpellDamage(target, SpellSlot.E) + Player.GetSpellDamage(target, SpellSlot.R)*4)
                            {
                                if (RootMenu["combo"]["qr"].Enabled)
                                {
                                    if (Player.HasBuff("MonkeyKingDoubleAttack") && target.Distance(Player) > 250)
                                    {

                                        R.Cast();
                                    }
                                    if (!Player.HasBuff("MonkeyKingDoubleAttack"))
                                    {


                                        R.Cast();
                                    }
                                }
                                if (!RootMenu["combo"]["qr"].Enabled)
                                {

                                    R.Cast();

                                }
                            }
                        }
                    }

                }
            }

        }


        protected override void Farming()
        {

            if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (!minion.IsValidTarget())
                    {
                        return;
                    }

                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["qaa"].Enabled)
                        {
                            Q.Cast();
                        }
                    }
                    if (RootMenu["farming"]["lane"]["usee"].Enabled)
                    {
                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(190, false, false,
                                    minion.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                            {
                                E.CastOnUnit(minion);
                            }
                        }
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(E.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                    bool useW = RootMenu["farming"]["jungle"]["usee"].Enabled;


                    if (useQ && !RootMenu["farming"]["jungle"]["qaa"].Enabled)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast();
                        }
                    }
                    if (useW)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.CastOnUnit(jungleTarget);
                        }
                    }
                }
            }

        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;

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
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;
            if (RootMenu["drawings"]["drawtoggle"].Enabled)
            {
                if (RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("E Under-Turret: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None, Color.GreenYellow);
            
                }
                if (!RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("E Under-Turret: OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None, Color.Red);

                }
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }

            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }

            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(E.Range * 2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                            double Rdamage;
                            if (R.Ready)
                            {
                                Rdamage = Player.GetSpellDamage(unit, SpellSlot.R) * 4;
                            }
                            else Rdamage = 0;
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
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) + Player.GetAutoAttackDamage(unit) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +

                                                     Rdamage
                                             ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q)+ Player.GetAutoAttackDamage(unit) +
                                                                        Player.GetSpellDamage(unit, SpellSlot.E) +

                                                                        Rdamage)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) + Player.GetAutoAttackDamage(unit)+
                                Player.GetSpellDamage(unit, SpellSlot.E) +

                                Rdamage
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }




        protected override void Killsteal()
        {
          
                if (Q.Ready &&
                    RootMenu["ks"]["ksq"].Enabled)
                {
                    var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Physical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.Q)+ Player.GetAutoAttackDamage(bestTarget) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(Q.Range))
                    {
                        if (Q.Cast())
                        {
                            Player.IssueOrder(OrderType.AttackUnit, bestTarget);
                        }
                    }
                }
                if (E.Ready &&
                    RootMenu["ks"]["kse"].Enabled)
                {
                    var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Physical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                        bestTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(bestTarget);
                    }
                }
            
        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {
            if (Player.HasBuff("MonkeyKingDoubleAttack"))
            {
               
                return;
            }
            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }
                if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
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
                if (RootMenu["harass"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }
                if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
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
                if (RootMenu["farming"]["jungle"]["qaa"].Enabled)
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range))
                            .ToList())
                        {
                            if (hero == jungleTarget)
                            {
                                if (jungleTarget.IsValidTarget() && !jungleTarget.UnitSkinName.Contains("Plant"))
                                {
                                    if (Q.Ready)
                                    {
                                        Q.Cast();

                                    }

                                    if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
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
                    }
                }

                if (RootMenu["farming"]["lane"]["qaa"].Enabled)
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                    {
                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (minion != null && hero == minion)
                            {
                                if (Q.Ready)
                                {
                                    Q.Cast();

                                }
                            }
                            if (RootMenu["combo"]["items"].Enabled && !Q.Ready)
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
        }
        protected override void Harass()
        {

            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"].Enabled;
                bool useW = RootMenu["harass"]["usee"].Enabled;

                if (useW)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range) && target.Distance(Player) >= RootMenu["combo"]["erange"].As<MenuSlider>().Value)
                        {

                            if (target != null)
                            {
                                if (RootMenu["combo"]["turret"].Enabled)
                                {
                                    E.CastOnUnit(target);
                                }
                                if (!RootMenu["combo"]["turret"].Enabled && !target.IsUnderEnemyTurret())
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }

                    }
                }
                if (useQ && !RootMenu["harass"]["qaa"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(Q.Range))
                        {

                            if (target != null)
                            {
                                Q.Cast();
                            }
                        }

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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuSlider("erange", "^- Min. E Range", 200, 0, 400));
                ComboMenu.Add(new MenuKeyBind("turret", "E Under Turret Toggle", KeyCode.T, KeybindType.Toggle));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitsr", "If Hits X Enemies", 3, 0, 5));
                ComboMenu.Add(new MenuBool("kill", "Use R in 1v1 if Killable"));
                ComboMenu.Add(new MenuSlider("wasteR", "^- Dont waste R if Enemy HP lower than", 100, 0, 300));
                ComboMenu.Add(new MenuBool("qr", "^- Don't R if Has Q Buff and in Auto Attack range"));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
                ComboMenu.Add(new MenuBool("follow", "Follow Mouse while Casting R"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if Hits X", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                KillstealMenu.Add(new MenuBool("kse", "Killsteal with E"));

            }
            RootMenu.Add(KillstealMenu);

            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Under-Turret Toggle"));
            }
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < 400 && W.Ready)
            {
                W.Cast();
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 365);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 650);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 310);

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["follow"].Enabled)
            {
                if (Player.HasBuff("MonkeyKingSpinToWin"))
                {
                    Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                }
            }
            
            if (Player.HasBuff("MonkeyKingSpinToWin"))
            {
                Orbwalker.Implementation.AttackingEnabled = false;
            }
            else Orbwalker.Implementation.AttackingEnabled = true;
        }

        protected override void LastHit()
        {
   
        }
    }
}

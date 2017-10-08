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
    class Shyvana : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useQ"].Enabled;



            if (RootMenu["combo"]["useE"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range))
                    {

                        if (target != null)
                        {
                          
                                E.Cast(target);
                            
                        }
                    }

                }
            }
            if (RootMenu["combo"]["useW"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

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
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range) && !RootMenu["combo"]["useQAA"].Enabled)
                    {

                        if (target != null)
                        {
                            Q.CastOnUnit(target);
                        }
                    }

                }
            }
           
            

        }


        protected override void Farming()
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
                    if (RootMenu["farming"]["lane"]["usew"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(W.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(W.Range, false, false,
                                    Player.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                            {
                                W.Cast();
                            }
                        }
                    }
                    if (RootMenu["farming"]["lane"]["usee"].Enabled)
                    {
                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            
                                E.CastOnUnit(minion);
                            
                        }
                    }
                }
            
     
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(E.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                    bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;
                    bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                    if (useQ && !RootMenu["farming"]["jungle"]["qaa"].Enabled)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast();
                        }
                    }
                    if (useW)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(W.Range))
                        {
                            W.Cast();
                        }
                    }
                    if (useE)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
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

      
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Wheat);
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
                                Rdamage = Player.GetSpellDamage(unit, SpellSlot.R);
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
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.W) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) +

                                                     Rdamage
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.W) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E) +

                                                          Rdamage)) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.W) +
                                Player.GetSpellDamage(unit, SpellSlot.E) +

                                Rdamage
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }




        protected override void Killsteal()
        {

        }

        internal override void PostAttack(object sender, PostAttackEventArgs e)
        {

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["useQAA"].Enabled)
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

            
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear))
            {

                Obj_AI_Minion hero = e.Target as Obj_AI_Minion;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["farming"]["jungle"]["qaa"].Enabled)
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

                if (RootMenu["farming"]["lane"]["qaa"].Enabled)
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
            }
        }

        protected override void Harass()
        {

  
        }



        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useQAA", " ^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("useW", "Use W"));
                ComboMenu.Add(new MenuBool("useE", "Use E"));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if Hits X", 3, 0, 6));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);

            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }

            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }
   
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 300);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 320);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 925);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 850);
            E.SetSkillshot(0.25f, 0, 2100, false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(0.25f, 150f, 1500f, false, SkillshotType.Line, false, HitChance.None);
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["semir"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                if (R.Ready)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                    if (target.IsValidTarget(R.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                R.Cast(target);
                            }
                        }
                    }


                }
            }
        }

        protected override void LastHit()
        {
   
        }
    }
}

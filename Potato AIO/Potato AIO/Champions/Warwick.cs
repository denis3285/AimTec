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
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Menu.Config;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Warwick : Champion
    {
        protected override void Combo()
        {
    
            if (RootMenu["combo"]["useq"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

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
            if (RootMenu["combo"]["usee"].Enabled && !Player.IsDashing())
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range))
                    {

                        if (target != null)
                        {
                            if (!Player.HasBuff("WarwickE"))
                            {
                                if (E.Cast())
                                {
                                    rdelay = Game.TickCount + RootMenu["combo"]["edelay"].As<MenuSlider>().Value;
                                }
                            }
                            if (Player.HasBuff("WarwickE"))
                            {
                                if (rdelay < Game.TickCount)
                                {
                                    E.Cast();
                                }
                            }
                        }
                    }

                }

            }
            if (RootMenu["combo"]["user"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(R.Range))
                    {
                        if (RootMenu["combo"]["turret"].Enabled)
                        {
                            R.Cast(target);
                        }
                        if (!RootMenu["combo"]["turret"].Enabled)
                        {
                            if (!target.IsUnderEnemyTurret())
                            {
                                R.Cast(target);
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
                foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
                    if (!minion.IsValidTarget())
                    {
                        return;
                    }

                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range))
                        {
                            
                                Q.Cast(minion);
                            

                        }

                    }
                    if (RootMenu["farming"]["lane"]["usee"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range))
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(E.Range, false, false,
                                    Player.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hite"].As<MenuSlider>().Value)
                            {
                                E.Cast();
                            }

                        }

                    }
                  
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(Q.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;
                    bool useE = RootMenu["farming"]["jungle"]["usee"].Enabled;


                    if (useQ)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }
                    }
                    if (useE)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast();
                        }
                    }
                    
                }
            }

        }

        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin" };
        private int hmmm;
        private int rdelay;

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
            var xaOffset = (int)maybeworks.X;
            var yaOffset = (int)maybeworks.Y;
            if (RootMenu["drawings"]["drawtoggle"].Enabled)
            {
                if (RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("R Under-Turret: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None, Color.GreenYellow);

                }
                if (!RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("R Under-Turret: OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None, Color.Red);

                }
            }

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
 
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Wheat);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                DrawCircleOnMinimap(Player.Position,  W.Range, Color.Wheat, 3, 40);
            }
        }


        protected override void Killsteal()
        {
 
            if (Q.Ready &&
                RootMenu["ks"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Physical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                   Q.Cast(bestTarget);
                }
            }
 


        }





        protected override void Harass()
        {

            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {

                if (RootMenu["harass"]["useq"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

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
                if (RootMenu["harass"]["usee"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(E.Range))
                        {

                            if (target != null)
                            {
                                if (!Player.HasBuff("WarwickE"))
                                {
                                    if (E.Cast())
                                    {
                                        rdelay = Game.TickCount + RootMenu["harass"]["edelay"].As<MenuSlider>().Value;
                                    }
                                }
                                if (Player.HasBuff("WarwickE"))
                                {
                                    if (rdelay < Game.TickCount)
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
                if (RootMenu["combo"]["items"].Enabled)
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
                if (RootMenu["combo"]["items"].Enabled)
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



        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("autoq", "Auto Q on Enemy Dashes"));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuSlider("edelay", "^- E Delay", 1000, 0, 2500));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuKeyBind("turret", "R Under Turret Toggle", KeyCode.T, KeybindType.Toggle));
                ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", KeyCode.G, KeybindType.Press));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
                HarassMenu.Add(new MenuSlider("edelay", "^- E Delay", 1000, 0, 2500)); ;
            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hite", "^- if X Minions in Range", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("ks", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("ksq", "Killseal with Q"));
            }
            RootMenu.Add(KillstealMenu);


            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Minimap"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Toggle"));
            }
         
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }


        public static void DrawCircleOnMinimap(
            Vector3 center,
            float radius,
            Color color,
            int thickness = 1,
            int quality = 100)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float)Math.Sin(angle))
                );
            }
            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                Vector2 aonScreen;
                Vector2 bonScreen;

                Render.WorldToMinimap(a, out aonScreen);
                Render.WorldToMinimap(b, out bonScreen);

                Render.Line(aonScreen, bonScreen, color);
            }
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 350);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 4000);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 375);
            R = new Aimtec.SDK.Spell(SpellSlot.R, Player.MoveSpeed * 2.5f);
            R.SetSkillshot(0.25f, 110, 2000, false, SkillshotType.Line);
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["autoq"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && target.IsDashing())
                        {
                            Q.Cast(target);
                        }
                    }
                }

            }
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
            R.Range = Player.MoveSpeed * 2f;
        }

        protected override void LastHit()
        {


        }
    }
}

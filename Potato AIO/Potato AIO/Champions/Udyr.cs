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
    class Udyr : Champion
    {
        protected override void Combo()
        {
            if (RootMenu["combo"]["whp"].As<MenuSlider>().Value > Player.HealthPercent())
            {
                if (RootMenu["combo"]["usew"].Enabled && W.Ready)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(300);

                    if (target.IsValidTarget())
                    {

                        if (target.IsValidTarget(300))
                        {
                            W.Cast();
                        }
                    }
                }
            }
            bool useQ = RootMenu["combo"]["useq"].Enabled;
           

            bool useR = RootMenu["combo"]["user"].Enabled;
            switch (RootMenu["combo"]["stance"].As<MenuList>().Value)
            {
                case 0:
                    if (RootMenu["combo"]["usee"].Enabled && E.Ready)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(800);

                        
                        if (target.IsValidTarget() && (Player.GetRealBuffCount("UdyrPhoenixStance") != 3 || (Player.GetRealBuffCount("UdyrPhoenixStance") == 3 && target.Distance(Player) > 300)))
                        { 
                            if (target.IsValidTarget(800))
                            {
                                E.Cast();

                            }
                        }
                    }
                    var targe = Extensions.GetBestEnemyHeroTargetInRange(300);

                    if (targe.IsValidTarget())
                    {

                        if (targe.IsValidTarget(300) && targe.HasBuff("UdyrBearStunCheck") ||
                            Player.GetSpell(SpellSlot.E).Level == 0 || !RootMenu["combo"]["usee"].Enabled)
                        {
                            if (useR)
                            {
                                R.Cast();
                            }
                            if ((!R.Ready || !useR) && Player.GetRealBuffCount("UdyrPhoenixStance") != 3 ||
                                Player.GetSpell(SpellSlot.R).Level == 0)
                            {
                                if (RootMenu["combo"]["usew"].Enabled)
                                {
                                    W.Cast();
                                }
                            }
                            if ((!W.Ready || !RootMenu["combo"]["usew"].Enabled) && Player.GetRealBuffCount("UdyrPhoenixStance") != 3 ||
                                Player.GetSpell(SpellSlot.W).Level == 0)
                            {
                                if (RootMenu["combo"]["useq"].Enabled)
                                {
                                    Q.Cast();
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (RootMenu["combo"]["usee"].Enabled && E.Ready)
                    {
                        var target = Extensions.GetBestEnemyHeroTargetInRange(800);

                        if (target.IsValidTarget() && (Player.GetRealBuffCount("UdyrPhoenixStance") != 3 || (Player.GetRealBuffCount("UdyrPhoenixStance") == 3 && target.Distance(Player) > 300)))
                        {

                            if (target.IsValidTarget(800))
                            {
                                E.Cast();

                            }
                        }
                    }
                    var targ = Extensions.GetBestEnemyHeroTargetInRange(300);

                    if (targ.IsValidTarget())
                    {

                        if (targ.IsValidTarget(300) && targ.HasBuff("UdyrBearStunCheck") ||
                            Player.GetSpell(SpellSlot.E).Level == 0 || !RootMenu["combo"]["usee"].Enabled)
                        {
                            if (useQ)
                            {
                                Q.Cast();
                            }
                            if ((!Q.Ready || !useQ) ||
                                Player.GetSpell(SpellSlot.Q).Level == 0)
                            {
                                if (RootMenu["combo"]["user"].Enabled)
                                {
                                    R.Cast();
                                }
                            }
                            if ((!R.Ready || !RootMenu["combo"]["user"].Enabled) && Player.GetRealBuffCount("UdyrPhoenixStance") != 3 ||
                                Player.GetSpell(SpellSlot.R).Level == 0)
                            {
                                if (RootMenu["combo"]["usew"].Enabled)
                                {
                                    W.Cast();
                                }
                            }
                        }
                    }
                    break;
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
                    bool useQ = RootMenu["farming"]["lane"]["useq"].Enabled;

                    bool useW = RootMenu["farming"]["lane"]["usee"].Enabled;
                    bool useR = RootMenu["farming"]["lane"]["user"].Enabled;
                    switch (RootMenu["farming"]["stance"].As<MenuList>().Value)
                    {
                        case 0:
                            if (useR)
                            {
                                if (minion != null && minion.IsValidTarget(300))
                                {
                                    R.Cast();
                                }
                            }
                            if ((!R.Ready || !useR) && Player.GetRealBuffCount("UdyrPhoenixStance") == 1)
                            {
                                if (minion != null && minion.IsValidTarget(300))
                                {
                                    if (useW || Player.GetSpell(SpellSlot.R).Level == 0)
                                    {
                                        W.Cast();
                                    }
                                }
                            }
                            break;
                        case 1:
                            if (useQ)
                            {
                                if (minion != null && minion.IsValidTarget(300))
                                {
                                    Q.Cast();
                                }
                            }
                            if ((!Q.Ready || !useQ) || Player.GetSpell(SpellSlot.Q).Level == 0)
                            {
                                if (minion != null && minion.IsValidTarget(300))
                                {
                                    if (useR || Player.GetSpell(SpellSlot.R).Level == 0)
                                    {
                                        R.Cast();
                                    }
                                }
                            }
                            if ((!R.Ready || !useR) && useW && Player.GetRealBuffCount("UdyrPhoenixStance") == 1 || Player.GetSpell(SpellSlot.R).Level == 0)
                            {
                                if (minion != null && minion.IsValidTarget(300))
                                {

                                    W.Cast();

                                }
                            }
                            break;
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Potato_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(300)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                    bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;
                    bool useR = RootMenu["farming"]["jungle"]["user"].Enabled;
                    switch (RootMenu["farming"]["stance"].As<MenuList>().Value)
                    {
                        case 0:
                            if (useR)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(300))
                                {
                                    R.Cast();
                                }
                            }
                            if (!R.Ready && Player.GetRealBuffCount("UdyrPhoenixStance") == 1)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(300))
                                {
                                    if (useW || Player.GetSpell(SpellSlot.R).Level == 0)
                                    {
                                        W.Cast();
                                    }
                                }
                            }
                            break;
                        case 1:
                            if (useQ)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(300))
                                {
                                    Q.Cast();
                                }
                            }
                            if (!Q.Ready || Player.GetSpell(SpellSlot.Q).Level == 0)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(300))
                                {
                                    if (useR || Player.GetSpell(SpellSlot.R).Level == 0)
                                    {
                                        R.Cast();
                                    }
                                }
                            }
                            if (!R.Ready && useW && Player.GetRealBuffCount("UdyrPhoenixStance") == 1 || Player.GetSpell(SpellSlot.R).Level == 0)
                            {
                                if (jungleTarget != null && jungleTarget.IsValidTarget(300))
                                {
                                    
                                       W.Cast();
                                    
                                }
                            }
                            break;
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
                Render.Circle(Player.Position, E.Range, 40, Color.CornflowerBlue);
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
                if (RootMenu["combo"]["items"].Enabled)
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

                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range))
                    .ToList())
                {
                    if (hero == jungleTarget)
                    {
                        if (jungleTarget.IsValidTarget() && !jungleTarget.UnitSkinName.Contains("Plant"))
                        {

                            if (RootMenu["combo"]["items"].Enabled)
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


                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (minion != null && hero == minion)
                    {

                        if (RootMenu["combo"]["items"].Enabled)
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
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuSlider("whp", "^- Priority W if X Health", 30, 0, 100));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuList("stance", "Stance Rotations", new[] { "E > R > W > Q", "E > Q > R > W" }, 0));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }

            RootMenu.Add(ComboMenu);

            FarmMenu = new Menu("farming", "Farming");
            FarmMenu.Add(new MenuList("stance", "Stance Rotations", new[] { "R - W - R", "Q - R - W" }, 0));
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuBool("user", "Use R to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 30));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
                JungleClear.Add(new MenuBool("user", "Use R to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("flee", "Flee");
            {
                KillstealMenu.Add(new MenuKeyBind("flee", "Flee Key", KeyCode.G, KeybindType.Press));
                KillstealMenu.Add(new MenuBool("fleestun", "Stun with E while Flee"));

            }
            RootMenu.Add(KillstealMenu);

            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {

                DrawMenu.Add(new MenuBool("drawe", "Draw E Engage Range"));
               
            }
           
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 0);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 800);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);

        }

        protected override void SemiR()
        {
            if (RootMenu["flee"]["flee"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (E.Ready)
                {
                    E.Cast();
                }
                if (RootMenu["flee"]["fleestun"].Enabled)
                {
                    foreach (var meow in GameObjects.EnemyHeroes)
                    {
                        if (meow != null && meow.IsValidTarget(300))
                        {
                            if (!meow.HasBuff("UdyrBearStunCheck") && Player.HasBuff("UdyrBearStance"))
                            {
                                Player.IssueOrder(OrderType.AttackUnit, meow);
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

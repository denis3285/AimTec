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
using Aimtec.SDK.Events;
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
    class Trundle : Champion
    {
        private int meowmeow;

        protected override void Combo()
        {
            bool useE = RootMenu["combo"]["usee"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useQAA = RootMenu["combo"]["qaa"].Enabled;
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range - 80);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(E.Range) && target.Distance(Player) > 300)
                    {

                        if (target != null)
                        {
                            E.Cast(Vector3Extensions.To2D(target.ServerPosition)
                                .Extend(Vector3Extensions.To2D(Player.ServerPosition), -150).To3D());
                        }
                    }
                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(RootMenu["combo"]["wmin"].As<MenuSlider>().Value))
                    {

                        if (target != null)
                        {
                            W.Cast(Vector3Extensions.To2D(Player.ServerPosition).Extend(Vector3Extensions.To2D(target.ServerPosition), 300).To3D());

                        }
                    }
                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(Q.Range) && !useQAA)
                    {

                        if (target != null)
                        {
                            Q.Cast();
                        }
                    }
                }
            }
            if (RootMenu["combo"]["user"].Enabled &&
                Player.HealthPercent() <= RootMenu["combo"]["health"].As<MenuSlider>().Value)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(R.Range))
                    {
                        var meow = TargetSelector.GetSelectedTarget();
                        if (target != null)
                        {
                            if (meow != null)
                            {
                                R.Cast(meow);
                            }
                            switch (RootMenu["combo"]["mode"].As<MenuList>().Value)
                            {
                                case 0:

                                    foreach (var ally in GameObjects.EnemyHeroes.Where(
                                            x => x.Distance(Player) <= R.Range && x != null)
                                        .OrderByDescending(x => x.TotalAttackDamage))
                                    {
                                        if (ally != null && !ally.IsDead &&  !RootMenu["blacklist"][ally.ChampionName.ToLower()].Enabled)
                                        {
                                            R.Cast(target);
                                        }
                                    }
                                    break;
                                case 1:

                                    foreach (var ally in GameObjects.EnemyHeroes.Where(
                                            x => x.Distance(Player) <= R.Range && x != null)
                                        .OrderByDescending(x => x.TotalAbilityDamage))
                                    {
                                        if (ally != null && !ally.IsDead && !RootMenu["blacklist"][ally.ChampionName.ToLower()].Enabled)
                                        {
                                            R.Cast(ally);
                                        }
                                    }
                                    break;
                                case 2:

                                    foreach (var ally in GameObjects.EnemyHeroes.Where(
                                            x => x.Distance(Player) <= R.Range && x != null)
                                        .OrderByDescending(x => x.MaxHealth))
                                    {
                                        if (ally != null && !ally.IsDead && !RootMenu["blacklist"][ally.ChampionName.ToLower()].Enabled)
                                        {
                                            R.Cast(ally);
                                        }
                                    }
                                    break;
                                case 3:

                                    foreach (var ally in GameObjects.EnemyHeroes.Where(
                                            x => x.Distance(Player) <= R.Range && x != null)
                                        .OrderByDescending(x => x.Armor))
                                    {
                                        if (ally != null && !ally.IsDead && !RootMenu["blacklist"][ally.ChampionName.ToLower()].Enabled)
                                        {
                                            R.Cast(ally);
                                        }
                                    }
                                    break;


                            }
                        }
                    }
                }
            }
        }

        protected override void SemiR()
        {
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
                    if (RootMenu["farming"]["lane"]["lastq"].Enabled)
                    {


                        if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q) + Player.GetAutoAttackDamage(minion) - 5)
                        {

                                if (Q.Cast())
                                {
                                    Orbwalker.Implementation.ForceTarget(minion);
                                }

                            
                        }
                    }
                    if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                    {


                        if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["qaa"].Enabled)
                        {
                            if (Q.Cast())
                            {
                                Orbwalker.Implementation.ForceTarget(minion);
                            }
                        }

                    }
 
                }
            }

            if (RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value <= Player.ManaPercent())
            {
                foreach (var jungleTarget in Support_AIO.Bases.GameObjects.Jungle
                    .Where(m => m.IsValidTarget(Q.Range)).ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }
                    bool useQ = RootMenu["farming"]["jungle"]["useq"].Enabled;

                    bool useW = RootMenu["farming"]["jungle"]["usew"].Enabled;


                    if (useQ)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range) && !RootMenu["farming"]["jungle"]["qaa"].Enabled)
                        {
                            if (Q.Cast())
                            {
                                Orbwalker.Implementation.ForceTarget(jungleTarget);
                            }
                        }
                    }
                    if (useW)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            W.Cast();
                        }
                    }
                }
            }
        }
    

        protected override void LastHit()
        {
            if (RootMenu["farming"]["lane"]["lastq"].Enabled)
            {

                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                {
              
                    if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q) + Player.GetAutoAttackDamage(minion) - 5)
                    {

                        if (Q.Cast())
                        {
                            Orbwalker.Implementation.ForceTarget(minion);
                        }

                    }
                }
            }
        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, RootMenu["combo"]["wmin"].As<MenuSlider>().Value, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }
            
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            bool useE = RootMenu["harass"]["usee"].Enabled;
            bool useW = RootMenu["harass"]["usew"].Enabled;
            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useQAA = RootMenu["harass"]["qaa"].Enabled;
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range-80);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(E.Range) && target.Distance(Player) > 300)
                    {
                       
                        if (target != null)
                        {
                            E.Cast(Vector3Extensions.To2D(target.ServerPosition).Extend(Vector3Extensions.To2D(Player.ServerPosition), -150).To3D());
                        }
                    }
                }
            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(W.Range))
                    {

                        if (target != null)
                        {
                            W.Cast(Vector3Extensions.To2D(Player.ServerPosition).Extend(Vector3Extensions.To2D(target.ServerPosition), 300).To3D());

                        }
                    }
                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {


                    if (target.IsValidTarget(Q.Range) && !useQAA)
                    {

                        if (target != null)
                        {
                            Q.Cast();
                        }
                    }
                }
            }
        }

        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
          
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuSlider("wmin", "W Max Range", 500, 100, 750));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("health", "^- My Health Percent Lower Than", 50, 1, 100));
                ComboMenu.Add(new MenuList("mode", "R Priority", new[] { "Most AD", "Most AP", "Max Health", "Most Armor" }, 3));
                var BlackList = new Menu("blacklist", "R Blacklist");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Block: " + target.ChampionName, false));
                    }
                }
                ComboMenu.Add(BlackList);
            }
            RootMenu.Add(ComboMenu);

            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));
                HarassMenu.Add(new MenuBool("usew", "Use W in Harass"));
                HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));
            }
            RootMenu.Add(HarassMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("lastq", "Use Q to Last Hit"));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qaa", "^- Only AA Reset"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("qaa", "^- Only AA Reset"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            Gapcloser.Attach(RootMenu, "E Anti-Gap");

            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

                if (target != null && Args.EndPosition.Distance(Player) < E.Range)
                {
                    E.Cast(Args.EndPosition);
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
                if (RootMenu["combo"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }
                if (!Q.Ready && !Player.HasBuff("TrundleTrollSmash"))
                {
                    Console.WriteLine("test");
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
                if (RootMenu["harass"]["qaa"].Enabled)
                {
                    if (Q.Ready)
                    {
                        Q.Cast();

                    }
                }
                if (!Q.Ready && !Player.HasBuff("TrundleTrollSmash"))
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
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                    {
                        foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range))
                            .ToList())
                        {
                            if (jungleTarget.IsValidTarget() && !jungleTarget.UnitSkinName.Contains("Plant"))
                            {
                                if (Q.Ready)
                                {
                                    Q.Cast();

                                }

                                if (!Q.Ready && !Player.HasBuff("TrundleTrollSmash"))
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

                if (RootMenu["farming"]["lane"]["qaa"].Enabled)
                {
                    if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                    {
                        foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (Q.Ready)
                            {
                                Q.Cast();

                            }

                            if (!Q.Ready && !Player.HasBuff("TrundleTrollSmash"))
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
        }


        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 300);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 900);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1000);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 700);


        }
    }
}

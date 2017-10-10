using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Tryndamere : Champion
    {

        protected override void Combo()
        {
            
            if (RootMenu["combo"]["usew"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(W.Range))
                    {

                        if (target != null)
                        {
                            if (RootMenu["combo"]["wface"].Enabled)
                            {
                                if (target.IsFacing(Player))
                                {
                                    W.Cast();
                                }
                            }
                            if (!RootMenu["combo"]["wface"].Enabled)
                            {
                                W.Cast();
                            }
                        }
                    }

                }
            }
            if (RootMenu["combo"]["usee"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range - 30) &&
                        target.Distance(Player) > RootMenu["combo"]["mine"].As<MenuSlider>().Value)
                    {

                        if (target != null)
                        {
                            if (RootMenu["combo"]["turret"].Enabled)

                            {
                                var meowpred = E.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                                }
                                if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80));
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120));
                                }
                                if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150));
                                }
                            }
                            if (!RootMenu["combo"]["turret"].Enabled && !target.IsUnderEnemyTurret())

                            {
                                var meowpred = E.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                                }
                                if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80));
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120));
                                }
                                if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150));
                                }
                            }
                        }
                    }

                }
            }




        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

            if (useE)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(160, false, false,
                            minion.ServerPosition)) >= RootMenu["farming"]["lane"]["hitE"].As<MenuSlider>().Value)
                    {

                        if (minion.IsValidTarget(E.Range) && minion != null)
                        {
                            E.Cast(minion);
                        }
                    }
                }
            }

            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

          
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;

                if (useEs && E.Ready && jungleTarget.IsValidTarget(E.Range))
                {
                    E.Cast(jungleTarget);
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
                    Render.Text("E Under-Turret: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.GreenYellow);

                }
                if (!RootMenu["combo"]["turret"].Enabled)
                {
                    Render.Text("E Under-Turret: OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.Red);

                }
            }


            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.Crimson);
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


                foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range))
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

        protected override void Harass()
        {
            if (RootMenu["harass"]["usew"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(W.Range))
                    {

                        if (target != null)
                        {
                            if (RootMenu["harass"]["wface"].Enabled)
                            {
                                if (target.IsFacing(Player))
                                {
                                    W.Cast();
                                }
                            }
                            if (!RootMenu["harass"]["wface"].Enabled)
                            {
                                W.Cast();
                            }
                        }
                    }

                }
            }
            if (RootMenu["harass"]["usee"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(E.Range - 30) &&
                        target.Distance(Player) > RootMenu["harass"]["mine"].As<MenuSlider>().Value)
                    {

                        if (target != null)
                        {
                            if (RootMenu["combo"]["turret"].Enabled)

                            {
                                var meowpred = E.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                                }
                                if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80));
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120));
                                }
                                if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150));
                                }
                            }
                            if (!RootMenu["combo"]["turret"].Enabled && !target.IsUnderEnemyTurret())

                            {
                                var meowpred = E.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                                }
                                if (target.Distance(Player) <= 400 && target.Distance(Player) >= 300)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -80));
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 400)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -120));
                                }
                                if (target.Distance(Player) <= E.Range && target.Distance(Player) >= 500)
                                {
                                    E.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -150));
                                }
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
                var qset = new Menu("qset", "Q Settings");
                qset.Add(new MenuBool("useq", "Use Q"));
                qset.Add(new MenuSlider("minf", "Min. Fury", 60, 0, 100));
                qset.Add(new MenuSlider("minhp", "Min. HP", 50, 1, 100));
                ComboMenu.Add(qset);
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo", true));
                ComboMenu.Add(new MenuBool("wface", "^- Only if Facing", false));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuSlider("mine", "Min. E Range", 300, 0, 500));
                ComboMenu.Add(new MenuKeyBind("turret", "E Under Turret Toggle", KeyCode.T, KeybindType.Toggle));
                var rset = new Menu("rset", "R Settings");
                rset.Add(new MenuBool("user", "Use R"));
                rset.Add(new MenuList("rusage", "R Usage", new[] {"If Incoming Damage Kills", "At X Health"}, 0));
                rset.Add(new MenuSlider("hpr", "If X Health <= (Health Mode)", 20, 1, 100));
                ComboMenu.Add(rset);
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo", true));
                HarassMenu.Add(new MenuBool("wface", "^- Only if Facing", false));
                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
                HarassMenu.Add(new MenuSlider("mine", "Min. E Range", 300, 0, 500));

            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
                LaneClear.Add(new MenuSlider("hitE", "Min. minion for E", 3, 1, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);

            DrawMenu = new Menu("drawings", "Drawings");
            {

                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw E Under Turret Toggle"));
            }
            var zlib = new Menu("zlib", "ZLib");

            Potato_AIO.ZLib.Attach(RootMenu);
            Gapcloser.Attach(RootMenu, "W Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < W.Range && W.Ready)
            {
                W.Cast();
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 0);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 850);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 660);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            E.SetSkillshot(0f, 93f, 600, false, SkillshotType.Line);

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["qset"]["useq"].Enabled)
            {
                if (Player.HealthPercent() <= RootMenu["combo"]["qset"]["minhp"].As<MenuSlider>().Value)
                {
                    if (Player.Mana >= RootMenu["combo"]["qset"]["minf"].As<MenuSlider>().Value)
                    {
                        Q.Cast();
                    }
                }
            }
            if (RootMenu["combo"]["rset"]["rusage"].As<MenuList>().Value == 1 && RootMenu["combo"]["rset"]["user"].Enabled)
            {


                if (Player.HealthPercent() <= RootMenu["combo"]["rset"]["hpr"].As<MenuSlider>().Value && !Player.IsRecalling())
                {
                    R.Cast();
                }
            }
        }
    



    protected override void LastHit()
        {

        }
    }
}

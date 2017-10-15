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
    public class FarmHelper
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        public class LaneclearResult
        {
            public LaneclearResult(int hit, Vector3 cp)
            {
                this.numberOfMinionsHit = hit;
                this.CastPosition = cp;
            }

            public int numberOfMinionsHit = 0;
            public Vector3 CastPosition;
        }

        public static LaneclearResult GetLineClearLocation(float range, float width)
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().Where(x => x.IsValidSpellTarget(range));

            var positions = minions.Select(x => x.ServerPosition).ToList();

            var locations = new List<Vector3>();

            locations.AddRange(positions);

            var max = positions.Count();

            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (positions[j] != positions[i])
                    {
                        locations.Add((positions[j] + positions[i]) / 2);
                    }
                }
            }

            HashSet<LaneclearResult> results = new HashSet<LaneclearResult>();

            foreach (var p in locations)
            {
                var rect = new Bases.Rectangle(Player.Position, p, width);

                var count = 0;

                foreach (var m in minions)
                {
                    if (rect.Contains(m.Position))
                    {
                        count++;
                    }
                }

                results.Add(new LaneclearResult(count, p));
            }

            var maxhit = results.MaxBy(x => x.numberOfMinionsHit);

            return maxhit;
        }

    }
    class Olaf : Champion
    {

        protected override void Combo()
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

                            var meowpred = Q.GetPrediction(target);
                            if (target.Distance(Player) <= 300)
                            {
                                Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                            }
                            if (target.Distance(Player) <= 500 && target.Distance(Player) >= 300)
                            {
                                Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -90));
                            }
                            if (target.Distance(Player) <= 700 && target.Distance(Player) >= 500)
                            {
                                Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -160));
                            }
                            if (target.Distance(Player) <= Q.Range && target.Distance(Player) >= 700)
                            {
                                Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -210));
                            }
                        }

                    }

                }
            }
            if (RootMenu["combo"]["usew"].Enabled)
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
            if (RootMenu["combo"]["usee"].Enabled && !RootMenu["combo"]["eaa"].Enabled)
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
            if (RootMenu["combo"]["user"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range - 100);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range-100))
                    {

                        if (target != null)
                        {
                            if (Player.HealthPercent() <= RootMenu["combo"]["hpr"].As<MenuSlider>().Value)
                            {
                                if (RootMenu["combo"]["cc"].Enabled)
                                {
                                    if (Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Stun) ||
                                        Player.HasBuffOfType(BuffType.Fear) || Player.HasBuffOfType(BuffType.Snare) ||
                                        Player.HasBuffOfType(BuffType.Taunt) ||
                                        Player.HasBuffOfType(BuffType.Suppression))
                                    {
                                        R.Cast();
                                    }
                                }
                                if (!RootMenu["combo"]["cc"].Enabled)
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

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                bool useQ = RootMenu["farming"]["lane"]["useq"].Enabled;

                bool useE = RootMenu["farming"]["lane"]["usee"].Enabled;

                bool useW = RootMenu["farming"]["lane"]["usew"].Enabled;

                if (useQ)
                {
                    if (Q.Ready)
                    {

                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {

                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {


                                var result = FarmHelper.GetLineClearLocation(Q.Range, 105);

                                if (result != null)
                                {
                                    if (result.numberOfMinionsHit >=
                                        RootMenu["farming"]["lane"]["qhit"].As<MenuSlider>().Value)
                                    {
                                        Q.Cast(result.CastPosition);
                                    }


                                }

                            }
                        }


                    }
                }
                if (useW)
                {
                    if (W.Ready)
                    {

                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                        {

                            if (minion.IsValidTarget(W.Range) && minion != null)
                            {


                                W.Cast();

                            }
                        }


                    }
                }
                if (useE)
                    {
                        if (E.Ready)
                        {
                            if (RootMenu["farming"]["lane"]["laste"].Enabled)
                            {

                                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                                {
                                    if (minion.IsValidTarget(E.Range) && minion != null)
                                    {

                                        if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.E))
                                        {
                                            E.Cast(minion);
                                        }
                                    }
                                }
                            }
                            if (!RootMenu["farming"]["lane"]["laste"].Enabled)
                            {
                                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                                {
                                    if (minion.IsValidTarget(Q.Range) && minion != null)
                                    {


                                        E.Cast(minion);

                                    }
                                }
                            }

                        }
                    }
                

            }
            foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(Q.Range))
                .ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {
                    bool useQs = RootMenu["farming"]["jungle"]["useq"].Enabled;

                   
                        if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }
                        if (RootMenu["farming"]["jungle"]["usew"].Enabled && W.Ready &&
                            jungleTarget.IsValidTarget(W.Range))
                        {
                            W.Cast();
                        }
                        if (RootMenu["farming"]["jungle"]["usee"].Enabled &&  E.Ready && jungleTarget.IsValidTarget(E.Range))
                        {
                            E.Cast(jungleTarget);
                        }

                    }

                }
            


            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useq"].Enabled;
                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {

                    
                        if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }
                        if (RootMenu["farming"]["jungle"]["usew"].Enabled && W.Ready &&
                            jungleTarget.IsValidTarget(W.Range))
                        {
                            W.Cast();
                        }
                        if (RootMenu["farming"]["jungle"]["usee"].Enabled &&  E.Ready && jungleTarget.IsValidTarget(E.Range))
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

            if (RootMenu["drawings"]["drawaxe"].Enabled)
            {
                var daggers = ObjectManager.Get<GameObject>()
                    .Where(d => d.IsValid && !d.IsDead && d.Distance(Player) <= 1000 &&
                                d.Name == "Olaf_Base_Q_Axe_Ally.troy");
                foreach (var dagger in daggers)
                {

                    if (dagger != null)
                    {
                        Render.Circle(dagger.ServerPosition, 150, 10, Color.DarkOrange);
                        Render.Circle(dagger.ServerPosition, 146, 10, Color.PapayaWhip);

                    }
                }
            }


            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(2000))
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
                            var drawStartXPos = (float)(barPos.X + (unit.Health > Player.GetSpellDamage(unit, SpellSlot.Q) + Player.GetSpellDamage(unit, SpellSlot.E)
                                                            ? width * ((unit.Health - (Player.GetSpellDamage(unit, SpellSlot.Q) + Player.GetSpellDamage(unit, SpellSlot.E))) / unit.MaxHealth * 100 / 100)
                                                            : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) + Player.GetSpellDamage(unit, SpellSlot.E) ? Color.GreenYellow : Color.Orange);

                        });
            }

        }



        protected override void Killsteal()
        {


            if (RootMenu["killsteal"]["useQ"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(Q, DamageType.Physical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    var meowpred = Q.GetPrediction(bestTarget);
                    if (bestTarget.Distance(Player) <= 300)
                    {
                        Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                    }
                    if (bestTarget.Distance(Player) <= 500 && bestTarget.Distance(Player) >= 300)
                    {
                        Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -90));
                    }
                    if (bestTarget.Distance(Player) <= 700 && bestTarget.Distance(Player) >= 500)
                    {
                        Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -160));
                    }
                    if (bestTarget.Distance(Player) <= Q.Range && bestTarget.Distance(Player) >= 700)
                    {
                        Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -210));
                    }
                }
            }
            if (RootMenu["killsteal"]["useE"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Physical, false);
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

            var heroTarget = e.Target as Obj_AI_Hero;
            if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Combo))
            {
                Obj_AI_Hero hero = e.Target as Obj_AI_Hero;
                if (hero == null || !hero.IsValid || !hero.IsEnemy)
                {
                    return;
                }
                if (RootMenu["combo"]["eaa"].Enabled)
                {
                    if (E.Ready)
                    {
                        E.Cast(hero);

                    }
                }

                if (RootMenu["combo"]["items"].Enabled && !E.Ready)
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
                if (RootMenu["harass"]["eaa"].Enabled)
                {
                    if (E.Ready && !W.Ready)
                    {
                        E.Cast(hero);

                    }
                }

                if (RootMenu["combo"]["items"].Enabled && !E.Ready)
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

                if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                {
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
                }
                



                if (RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value < Player.ManaPercent())
                {
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
        }


        protected override void Harass()
        {
            float manapercent = RootMenu["harass"]["mana"].As<MenuSlider>().Value;


            if (manapercent < Player.ManaPercent())
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

                                var meowpred = Q.GetPrediction(target);
                                if (target.Distance(Player) <= 300)
                                {
                                    Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -50));
                                }
                                if (target.Distance(Player) <= 500 && target.Distance(Player) >= 300)
                                {
                                    Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -90));
                                }
                                if (target.Distance(Player) <= 700 && target.Distance(Player) >= 500)
                                {
                                    Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -160));
                                }
                                if (target.Distance(Player) <= Q.Range && target.Distance(Player) >= 700)
                                {
                                    Q.Cast(meowpred.CastPosition.Extend(Player.ServerPosition, -210));
                                }
                            }

                        }

                    }
                }
                if (RootMenu["harass"]["usew"].Enabled)
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
                if (RootMenu["harass"]["usee"].Enabled && !RootMenu["harass"]["eaa"].Enabled)
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
            }
        }




        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {

                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo", true));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo", true));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("eaa", "^- Only for AA Reset", true));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hpr", "If lower than X Health", 20, 1, 100));
                ComboMenu.Add(new MenuBool("cc", "^- Only if CC'd"));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass", true));
                HarassMenu.Add(new MenuBool("usew", "Use W in Combo", true));

                HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
                HarassMenu.Add(new MenuBool("eaa", "^- Only for AA Reset", true));

            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));

                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("qhit", "^- if Hits X Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("usew", "Use W to Farm"));
                LaneClear.Add(new MenuBool("usee", "Use E to Farm"));
                LaneClear.Add(new MenuBool("laste", "^- Only for Last Hit"));

            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50, 0, 100));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("usew", "Use W to Farm"));

                JungleClear.Add(new MenuBool("usee", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("useE", "Use E to Killsteal"));

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {

                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawaxe", "Draw Axe Position"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
            }
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 1000);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 300);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 325);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 80, 1600, false, SkillshotType.Line);

        }

        protected override void SemiR()
        {
       
        }
    



    protected override void LastHit()
        {
            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {
                if (RootMenu["farming"]["lane"]["laste"].Enabled)
                {
                    if (E.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                        {
                            if (minion.IsValidTarget(E.Range) && minion != null)
                            {

                                if (minion.Health <= Player.GetSpellDamage(minion, SpellSlot.E))
                                {
                                    E.Cast(minion);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

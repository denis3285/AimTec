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
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Aatrox : Champion
    {

        protected override void Combo()
        {
            if (RootMenu["combo"]["items"].Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(600);
                if (target != null)
                {
                    if (Player.HasItem(ItemId.BladeoftheRuinedKing) || Player.HasItem(ItemId.BilgewaterCutlass))
                    {
                        var items = new[] {ItemId.BladeoftheRuinedKing, ItemId.BilgewaterCutlass};
                        var slot = Player.Inventory.Slots.First(s => items.Contains(s.ItemId));
                        if (slot != null)
                        {
                            var spellslot = slot.SpellSlot;
                            if (spellslot != SpellSlot.Unknown &&
                                Player.SpellBook.GetSpell(spellslot).State == SpellState.Ready)
                            {
                                Player.SpellBook.CastSpell(spellslot, target);
                            }
                        }
                    }

                }
            }
            if (Q.Ready && RootMenu["combo"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && target.Distance(Player) >= RootMenu["combo"]["minQ"].As<MenuSlider>().Value)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["combo"]["useW"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (Player.HasBuff("AatroxWPower") && !target.IsDead && Player.HealthPercent() < RootMenu["combo"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                        if (!Player.HasBuff("AatroxWPower") && !target.IsDead && Player.HealthPercent() > RootMenu["combo"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                    }
                }


            }
            if (E.Ready && RootMenu["combo"]["useE"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast(target);
                        }
                    }
                }


            }
            if (R.Ready && RootMenu["combo"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);
                if (target.IsValidTarget(R.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (Player.CountEnemyHeroesInRange(R.Range) >=
                                RootMenu["combo"]["minr"].As<MenuSlider>().Value)
                            {
                                R.Cast();
                            }
                        }
                    }
                }

            }


        }


        protected override void Farming()
        {

            bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;


            if (RootMenu["farming"]["lane"]["useQ"].Enabled)
            {
                if (Q.Ready)
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(250, false, false,
                                minion.ServerPosition)) >= RootMenu["farming"]["lane"]["hitQ"].As<MenuSlider>().Value)
                        {
                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {

                                Q.Cast(minion);


                            }
                        }
                    }
                }
            }


            if (RootMenu["farming"]["lane"]["useW"].Enabled)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(W.Range))
                {

                    if (minion.IsValidTarget(W.Range) && minion != null)
                    {
                        if (Player.HasBuff("AatroxWPower") && Player.HealthPercent() <
                            RootMenu["farming"]["lane"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                        if (!Player.HasBuff("AatroxWPower") && Player.HealthPercent() >
                            RootMenu["farming"]["lane"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                    }


                }
            }
            if (useE)
            {
                foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(E.Range))
                {


                    if (minion.IsValidTarget(E.Range) && minion != null)
                    {
                        E.Cast(minion);
                    }

                }
            }
        

        foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(E.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    return;
                }

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;

                if (useQs && Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(jungleTarget);
                }
                if (RootMenu["farming"]["jungle"]["useW"].Enabled)
                {
                    if (Player.HasBuff("AatroxWPower") && Player.HealthPercent() <
                        RootMenu["farming"]["jungle"]["wlogic"].As<MenuSlider>().Value)
                    {
                        W.Cast();
                    }
                    if (!Player.HasBuff("AatroxWPower") && Player.HealthPercent() >
                        RootMenu["farming"]["jungle"]["wlogic"].As<MenuSlider>().Value)
                    {
                        W.Cast();
                    }
                }
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
          
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.CornflowerBlue);
            }
        }



        protected override void Killsteal()
        {

   


        }

        protected override void Harass()
        {

            if (Q.Ready && RootMenu["harass"]["useQ"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead && target.Distance(Player) >= RootMenu["harass"]["minQ"].As<MenuSlider>().Value)
                        {
                            Q.Cast(target);
                        }
                    }
                }


            }
            if (W.Ready && RootMenu["harass"]["useW"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range))
                {
                    if (target != null)
                    {
                        if (Player.HasBuff("AatroxWPower") && !target.IsDead && Player.HealthPercent() < RootMenu["harass"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                        if (!Player.HasBuff("AatroxWPower") && !target.IsDead && Player.HealthPercent() > RootMenu["harass"]["wlogic"].As<MenuSlider>().Value)
                        {
                            W.Cast();
                        }
                    }
                }


            }
            if (E.Ready && RootMenu["harass"]["useE"].Enabled)
            {
                var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range))
                {
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            E.Cast(target);
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
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo", true));
                ComboMenu.Add(new MenuSlider("minQ", "^- Min. Q Range", 300, 0, 400));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuSlider("wlogic", "^- Switch to Heal if HP lower than", 50, 0, 100));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("minr", "^- If X Enemies in Range", 2, 0, 5));
                ComboMenu.Add(new MenuBool("items", "Use Items"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuBool("useQ", "Use Q in Harass", true));
                HarassMenu.Add(new MenuSlider("minQ", "^- Min. Q Range", 300, 0, 400));
                HarassMenu.Add(new MenuBool("useW", "Use W in Harass"));
                HarassMenu.Add(new MenuSlider("wlogic", "^- Switch to Heal if HP lower than", 50, 0, 100));
                HarassMenu.Add(new MenuBool("useE", "Use E in Harass"));

            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuSlider("hitQ", "^- if Hits X Minions", 3, 1, 6));
                LaneClear.Add(new MenuBool("useW", "Use W in Farm"));
                LaneClear.Add(new MenuSlider("wlogic", "^- Switch to Heal if HP lower than", 50, 0, 100));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W in Farm"));
                JungleClear.Add(new MenuSlider("wlogic", "^- Switch to Heal if HP lower than", 50, 0, 100));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);              
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            Gapcloser.Attach(RootMenu, "E Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < E.Range && E.Ready)
            {
                E.Cast(Args.EndPosition);
            }

        }
        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 650);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 300);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 1000);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 550);
            E.SetSkillshot(0.25f, 35f, 1250f, false, SkillshotType.Line);
            Q.SetSkillshot(0.6f, 200, 2000f, false, SkillshotType.Circle, false, HitChance.None);

        }

        protected override void SemiR()
        {
          
            
        
        }

        protected override void LastHit()
        {

        }
    }
}

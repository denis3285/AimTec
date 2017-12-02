using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
using Aimtec.SDK.Prediction.Collision;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Potato_AIO;
using Potato_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Potato_AIO.Champions
{
    class Evelynn : Champion
    {
        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useQ"].Enabled;
            bool useE = RootMenu["combo"]["useE"].Enabled;
            bool useW = RootMenu["combo"]["useW"].Enabled;
            if (!RootMenu["combo"]["wait"].Enabled)
            {
                if (useW)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                {

                                    W.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (RootMenu["combo"]["useR"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                    if (R.Ready && target.IsValidTarget(R.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (GetR(target) >= target.Health)
                                {
                                    var meow = R.GetPrediction(target);
                                    Render.Circle(meow.CastPosition, 100, 100, Color.YellowGreen);
                                    if (target.Distance(Player) > R.Range - 210)
                                    {
                                        var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 2.3f);
                                        if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                                .As<MenuSlider>().Value) <
                                            RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                    }
                                    if (target.Distance(Player) < R.Range - 210)
                                    {
                                        var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 1.8f);
                                        if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                                .As<MenuSlider>().Value) <
                                            RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                    }

                                }
                            }


                        }
                    }
                }
                if (useQ)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (Q.Ready && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {


                                if (!Player.HasBuff("EvelynnQ2"))
                                {
                                    Q.Cast(target);
                                }
                                if (Player.HasBuff("EvelynnQ2"))
                                {
                                    if (target.Distance(Player) < 620)
                                    {
                                        Q.Cast();
                                    }
                                }
                            }
                        }
                    }
                }
                if (RootMenu["combo"]["items"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(650);
                    if (target != null)
                    {
                        if (!target.IsDead)
                        {
                            if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                            {
                                if (Player.HasItem(ItemId.HextechProtobelt01))
                                {
                                    var items = new[] {ItemId.HextechProtobelt01};
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

                    }
                }
                if (useE)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (E.Ready && target.IsValidTarget(E.Range))
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
            if (RootMenu["combo"]["wait"].Enabled)
            {
                if (useW)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                    if (W.Ready && target.IsValidTarget(W.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                {

                                    W.Cast(target);
                                }
                            }
                        }
                    }
                }
                if (RootMenu["combo"]["useR"].Enabled)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                    if (R.Ready && target.IsValidTarget(R.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (GetR(target) >= target.Health)
                                {
                                    var meow = R.GetPrediction(target);
                                    Render.Circle(meow.CastPosition, 100, 100, Color.YellowGreen);
                                    if (target.Distance(Player) > R.Range - 210)
                                    {
                                        var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 2.3f);
                                        if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                                .As<MenuSlider>().Value) <
                                            RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                    }
                                    if (target.Distance(Player) < R.Range - 210)
                                    {
                                        var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 1.8f);
                                        if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                                .As<MenuSlider>().Value) <
                                            RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                                        {
                                            R.Cast(target);
                                        }
                                    }

                                }
                            }


                        }
                    }
                }
                if (useQ && meowwwwwwwwwwwww < Game.TickCount)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (Q.Ready && target.IsValidTarget(Q.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead && target.HasBuff("EvelynnW"))
                            {
                                var daggers = ObjectManager.Get<GameObject>()
                                    .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                                d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                foreach (var meow in daggers)
                                {



                                    if (!Player.HasBuff("EvelynnQ2"))
                                    {
                                        Q.Cast(target);
                                    }
                                    if (Player.HasBuff("EvelynnQ2"))
                                    {
                                        if (target.Distance(Player) < 620)
                                        {
                                            Q.Cast();
                                        }
                                    }
                                }
                            }
                            if (!target.IsDead && !target.HasBuff("EvelynnW"))
                            {
                                if (!Player.HasBuff("EvelynnQ2"))
                                {
                                    Q.Cast(target);
                                }
                                if (Player.HasBuff("EvelynnQ2"))
                                {
                                    if (target.Distance(Player) < 620)
                                    {
                                        Q.Cast();
                                    }
                                }
                            }
                        }
                    }
                }
                if (RootMenu["combo"]["items"].Enabled && meowwwwwwwwwwwww < Game.TickCount)
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(650);
                    if (target != null)
                    {
                        if (!target.IsDead && target.HasBuff("EvelynnW"))
                        {
                            var daggers = ObjectManager.Get<GameObject>()
                                .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                            d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                            foreach (var meow in daggers)
                            {


                                if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                                {
                                    if (Player.HasItem(ItemId.HextechProtobelt01))
                                    {
                                        var items = new[] {ItemId.HextechProtobelt01};
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
                        }
                        if (!target.IsDead && !target.HasBuff("EvelynnW"))
                        {
                            if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                            {
                                if (Player.HasItem(ItemId.HextechProtobelt01))
                                {
                                    var items = new[] {ItemId.HextechProtobelt01};
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
                    }

                }
                if (useE && meowwwwwwwwwwwww < Game.TickCount
                )
                {
                    var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                    if (E.Ready && target.IsValidTarget(E.Range))
                    {
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                var daggers = ObjectManager.Get<GameObject>()
                                    .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                                d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                foreach (var meow in daggers)
                                {


                                    E.Cast(target);
                                }
                            }
                            if (!target.IsDead && !target.HasBuff("EvelynnW"))
                            {
                                E.Cast(target);
                            }

                        }
                    }


                }
            }
        }

        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "EvelynnW")
                {

                    meowwwwwwwwwwwww = 600 + Game.TickCount;
                }
            }
        }

        protected override void Farming()
        {

            float manapercent = RootMenu["farming"]["lane"]["mana"].As<MenuSlider>().Value;
            if (manapercent < Player.ManaPercent())
            {

                bool useQ = RootMenu["farming"]["lane"]["useQ"].Enabled;
                bool useE = RootMenu["farming"]["lane"]["useE"].Enabled;

                if (useQ)
                {
                    if (Q.Ready)
                    {
                        foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                        {
                            if (minion.IsValidTarget(Q.Range) && minion != null)
                            {


                                if (!Player.HasBuff("EvelynnQ2"))
                                {
                                    Q.Cast(minion.ServerPosition);
                                }
                                if (Player.HasBuff("EvelynnQ2"))
                                {
                                    if (minion.Distance(Player) < 620)
                                    {
                                        Q.Cast();
                                    }
                                }

                            }
                        }

                    }
                }

                if (useE
                )
                {
                    foreach (var minion in Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
                    {
                        if (minion.IsValidTarget(Q.Range) && minion != null)
                        {


                            if (E.Ready && minion.IsValidTarget(E.Range))
                            {
                                if (minion != null)
                                {
                                    if (!minion.IsDead)
                                    {

                                        E.Cast(minion);
                                    }

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

                bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;

                float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                if (manapercents < Player.ManaPercent())
                {

                    if (useWs)
                    {


                        if (W.Ready && jungleTarget.IsValidTarget(W.Range + 50))
                        {
                            if (jungleTarget != null)
                            {
                                if (!jungleTarget.IsDead)
                                {
                                    W.Cast(jungleTarget);
                                }
                            }
                        }
                    }

                }
            }
            foreach (var jungleTarget in Bases.GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!jungleTarget.IsValidTarget())
                {
                    return;
                }
                if (!jungleTarget.UnitSkinName.Contains("Plant"))
                {
                    bool useQs = RootMenu["farming"]["jungle"]["useQ"].Enabled;
                    bool useWs = RootMenu["farming"]["jungle"]["useW"].Enabled;
                    bool useEs = RootMenu["farming"]["jungle"]["useE"].Enabled;

                    float manapercents = RootMenu["farming"]["jungle"]["mana"].As<MenuSlider>().Value;

                    if (manapercents < Player.ManaPercent())
                    {
                        if (RootMenu["farming"]["jungle"]["wait"].Enabled)
                        {
                            if (useWs)
                            {


                                if (W.Ready && jungleTarget.IsValidTarget(W.Range + 50))
                                {
                                    if (jungleTarget != null)
                                    {
                                        if (!jungleTarget.IsDead)
                                        {
                                            W.Cast(jungleTarget);
                                        }
                                    }
                                }
                            }

                            if (useQs)
                            {

                                if (meowwwwwwwwwwwww < Game.TickCount)
                                {

                                    if (Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                                    {
                                        if (jungleTarget != null)
                                        {
                                            var zz = Bases.GameObjects.Jungle.Where(x =>
                                                x != null && x.HasBuff("EvelynnW"));
                                            var daggers = ObjectManager.Get<GameObject>()
                                                .Where(d => d.IsValid && d.Distance(jungleTarget) <= 350 &&
                                                            d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                            if (!jungleTarget.IsDead && jungleTarget.HasBuff("EvelynnW"))
                                            {

                                                foreach (var meow in daggers)
                                                {



                                                    if (!Player.HasBuff("EvelynnQ2"))
                                                    {

                                                        Q.Cast(jungleTarget.ServerPosition);
                                                    }
                                                    if (Player.HasBuff("EvelynnQ2"))
                                                    {
                                                        if (jungleTarget.Distance(Player) < 620)
                                                        {
                                                            Q.Cast();
                                                        }
                                                    }
                                                }
                                            }

                                            if (zz.Count() == 0)
                                            {
                                                if (!jungleTarget.IsDead && !jungleTarget.HasBuff("EvelynnW"))
                                                {

                                                    if (!Player.HasBuff("EvelynnQ2"))
                                                    {
                                                        Q.Cast(jungleTarget.ServerPosition);
                                                    }
                                                    if (Player.HasBuff("EvelynnQ2"))
                                                    {
                                                        if (jungleTarget.Distance(Player) < 620)
                                                        {
                                                            Q.Cast();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (useEs && meowwwwwwwwwwwww < Game.TickCount
                            )
                            {
                                var zz = Bases.GameObjects.Jungle.Where(x =>
                                    x != null && x.HasBuff("EvelynnW"));
                                var daggers = ObjectManager.Get<GameObject>()
                                    .Where(d => d.IsValid && d.Distance(jungleTarget) <= 350 &&
                                                d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                if (E.Ready && jungleTarget.IsValidTarget(E.Range))
                                {
                                    if (jungleTarget != null)
                                    {
                                        if (!jungleTarget.IsDead)
                                        {

                                            foreach (var meow in daggers)
                                            {


                                                E.Cast(jungleTarget);
                                            }
                                        }
                                        if (zz.Count() == 0)
                                        {
                                            if (!jungleTarget.IsDead && !jungleTarget.HasBuff("EvelynnW"))
                                            {
                                                E.Cast(jungleTarget);
                                            }
                                        }
                                    }
                                }


                            }


                        }
                        if (!RootMenu["farming"]["jungle"]["wait"].Enabled)
                        {
                            if (useWs)
                            {


                                if (W.Ready && jungleTarget.IsValidTarget(W.Range))
                                {
                                    if (jungleTarget != null)
                                    {
                                        if (!jungleTarget.IsDead)
                                        {
                                            W.Cast(jungleTarget);
                                        }
                                    }
                                }
                            }

                            if (useQs)
                            {

                                if (Q.Ready && jungleTarget.IsValidTarget(Q.Range))
                                {
                                    if (jungleTarget != null)
                                    {
                                        if (!jungleTarget.IsDead)
                                        {


                                            if (!Player.HasBuff("EvelynnQ2"))
                                            {
                                                Q.Cast(jungleTarget.ServerPosition);
                                            }
                                            if (Player.HasBuff("EvelynnQ2"))
                                            {
                                                if (jungleTarget.Distance(Player) < 620)
                                                {
                                                    Q.Cast();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (useEs
                            )
                            {

                                if (E.Ready && jungleTarget.IsValidTarget(E.Range))
                                {
                                    if (jungleTarget != null)
                                    {
                                        if (!jungleTarget.IsDead)
                                        {

                                            E.Cast(jungleTarget);
                                        }

                                    }
                                }


                            }


                        }
                    }
                }
            }
        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
        private int hmmm;
        private MenuSlider meowmeowtime;
        private int meowmeowtimes;
        private int meowwwwwwwwwwwww;

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
            if (R.Ready)
            {
                if (RootMenu["drawings"]["drawbackward"].Enabled)
                {
                    var zz = TargetSelector.Implementation.GetTarget(R.Range);

                    if (zz != null)
                    {

                        var meow = R.GetPrediction(zz);

                        if (zz.Distance(Player) > R.Range - 210)
                        {
                            var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 2.3f);
                            if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value) <
                                RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                            {
                                Render.Circle(something, RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value, 40, Color.GreenYellow);
                            }
                            else
                                Render.Circle(something, RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value, 40, Color.Red);
                        }
                        if (zz.Distance(Player) < R.Range - 210)
                        {
                            var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 1.8f);
                            if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value) <
                                RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                            {
                                Render.Circle(something, RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value, 40, Color.GreenYellow);
                            }
                            else
                                Render.Circle(something, RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value, 40, Color.Red);
                        }
                    }
                }
            }
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int) maybeworks.X;
            var yaOffset = (int) maybeworks.Y;
            if (RootMenu["drawings"]["drawtoggle"].Enabled)
            {
                if (RootMenu["combo"]["wait"].Enabled)
                {
                    Render.Text("Wait for Charm: ON", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.GreenYellow);

                }
                if (!RootMenu["combo"]["wait"].Enabled)
                {
                    Render.Text("Wait for Charm: OFF", new Vector2(xaOffset - 50, yaOffset + 10), RenderTextFlags.None,
                        Color.Red);

                }
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
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health >
                                                     GetR(unit)
                                             ? width * ((unit.Health -
                                                         (GetR(unit))) / unit.MaxHealth *
                                                        100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true,
                                unit.Health < GetR(unit)
                                    ? Color.GreenYellow
                                    : Color.Wheat);

                        });
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 50, Color.LightGreen);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 50, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 50, Color.LightGreen);
            }

        }

        static double GetR(Obj_AI_Base target)
        {

            double meow = 0;
            double uhh = 0;
            double mmmmm = 0;
            double meowmeow = 0;
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 1)
            {
                meow = 150;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 2)
            {
                meow = 275;
            }
            if (Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                meow = 400;
            }

            if (target.HealthPercent() >= 30)
            {
                mmmmm = Player.TotalAbilityDamage * 0.75;
                uhh = (mmmmm + meow);
                meowmeow = Player.CalculateDamage(target, DamageType.Magical, uhh);
            }
            if (target.HealthPercent() <= 30)
            {
                mmmmm = Player.TotalAbilityDamage * 0.75;
                uhh = (mmmmm + meow) * 2;
                meowmeow = Player.CalculateDamage(target, DamageType.Magical, uhh);
            }
            return meowmeow;




        }



        protected override void Killsteal()
        {

            if (RootMenu["killsteal"]["useQ"].Enabled)
            {
                var bestTarget = Bases.Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
            if (RootMenu["killsteal"]["useE"].Enabled)
            {
                if (!Player.HasBuff("EvelynnE"))
                {
                    var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.E) - 10 >= bestTarget.Health &&
                        bestTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(bestTarget);
                    }
                }
                if (Player.HasBuff("EvelynnE"))
                {
                    var bestTarget = Bases.Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                    if (bestTarget != null &&
                        Player.GetSpellDamage(bestTarget, SpellSlot.E, DamageStage.Empowered) - 30 >=
                        bestTarget.Health &&
                        bestTarget.IsValidTarget(E.Range))
                    {
                        E.Cast(bestTarget);
                    }
                }
            }
            if (RootMenu["killsteal"]["useR"].Enabled)
            {
                var target = Bases.Extensions.GetBestKillableHero(R, DamageType.Magical, false);
                if (target != null && target.IsValidTarget(R.Range))
                {
                    if (GetR(target) >= target.Health)
                    {
                        var meow = R.GetPrediction(target);
                        Render.Circle(meow.CastPosition, 100, 100, Color.YellowGreen);
                        if (target.Distance(Player) > R.Range - 210)
                        {
                            var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 2.3f);
                            if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value) <
                                RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                            {
                                R.Cast(target);
                            }
                        }
                        if (target.Distance(Player) < R.Range - 210)
                        {
                            var something = meow.CastPosition.Extend(Player.ServerPosition, R.Range * 1.8f);
                            if (something.CountEnemyHeroesInRange(RootMenu["combo"]["backwardR"]
                                    .As<MenuSlider>().Value) <
                                RootMenu["combo"]["enemiesR"].As<MenuSlider>().Value)
                            {
                                R.Cast(target);
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
                bool useQ = RootMenu["harass"]["useQ"].Enabled;
                bool useE = RootMenu["harass"]["useE"].Enabled;
                bool useW = RootMenu["harass"]["useW"].Enabled;
                if (!RootMenu["combo"]["wait"].Enabled)
                {
                    if (useW)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                        if (W.Ready && target.IsValidTarget(W.Range))
                        {
                            if (target != null)
                            {
                                if (!target.IsDead)
                                {
                                    if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                    {

                                        W.Cast(target);
                                    }
                                }
                            }
                        }
                    }

                    if (useQ)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (Q.Ready && target.IsValidTarget(Q.Range))
                        {
                            if (target != null)
                            {
                                if (!target.IsDead)
                                {


                                    if (!Player.HasBuff("EvelynnQ2"))
                                    {
                                        Q.Cast(target);
                                    }
                                    if (Player.HasBuff("EvelynnQ2"))
                                    {
                                        if (target.Distance(Player) < 620)
                                        {
                                            Q.Cast();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (RootMenu["combo"]["items"].Enabled)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(650);
                        if (target != null)
                        {
                            if (!target.IsDead)
                            {
                                if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                                {
                                    if (Player.HasItem(ItemId.HextechProtobelt01))
                                    {
                                        var items = new[] {ItemId.HextechProtobelt01};
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

                        }
                    }
                    if (useE)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (E.Ready && target.IsValidTarget(E.Range))
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
                if (RootMenu["combo"]["wait"].Enabled)
                {
                    if (useW)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(W.Range);

                        if (W.Ready && target.IsValidTarget(W.Range))
                        {
                            if (target != null)
                            {
                                if (!target.IsDead)
                                {
                                    if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                                    {

                                        W.Cast(target);
                                    }
                                }
                            }
                        }
                    }

                    if (useQ && meowwwwwwwwwwwww < Game.TickCount)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                        if (Q.Ready && target.IsValidTarget(Q.Range))
                        {
                            if (target != null)
                            {
                                if (!target.IsDead && target.HasBuff("EvelynnW"))
                                {
                                    var daggers = ObjectManager.Get<GameObject>()
                                        .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                                    d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                    foreach (var meow in daggers)
                                    {



                                        if (!Player.HasBuff("EvelynnQ2"))
                                        {
                                            Q.Cast(target);
                                        }
                                        if (Player.HasBuff("EvelynnQ2"))
                                        {
                                            if (target.Distance(Player) < 620)
                                            {
                                                Q.Cast();
                                            }
                                        }
                                    }
                                }
                                if (!target.IsDead && !target.HasBuff("EvelynnW"))
                                {
                                    if (!Player.HasBuff("EvelynnQ2"))
                                    {
                                        Q.Cast(target);
                                    }
                                    if (Player.HasBuff("EvelynnQ2"))
                                    {
                                        if (target.Distance(Player) < 620)
                                        {
                                            Q.Cast();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (RootMenu["combo"]["items"].Enabled && meowwwwwwwwwwwww < Game.TickCount)
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(650);
                        if (target != null)
                        {
                            if (!target.IsDead && target.HasBuff("EvelynnW"))
                            {
                                var daggers = ObjectManager.Get<GameObject>()
                                    .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                                d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                foreach (var meow in daggers)
                                {


                                    if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                                    {
                                        if (Player.HasItem(ItemId.HextechProtobelt01))
                                        {
                                            var items = new[] {ItemId.HextechProtobelt01};
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
                            }
                            if (!target.IsDead && !target.HasBuff("EvelynnW"))
                            {
                                if (target.Distance(Player) < 650 && target.Distance(Player) > 250)
                                {
                                    if (Player.HasItem(ItemId.HextechProtobelt01))
                                    {
                                        var items = new[] {ItemId.HextechProtobelt01};
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
                        }

                    }
                    if (useE && meowwwwwwwwwwwww < Game.TickCount
                    )
                    {
                        var target = Bases.Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                        if (E.Ready && target.IsValidTarget(E.Range))
                        {
                            if (target != null)
                            {
                                if (!target.IsDead)
                                {
                                    var daggers = ObjectManager.Get<GameObject>()
                                        .Where(d => d.IsValid && d.Distance(target) <= 350 &&
                                                    d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                                    foreach (var meow in daggers)
                                    {


                                        E.Cast(target);
                                    }
                                }
                                if (!target.IsDead && !target.HasBuff("EvelynnW"))
                                {
                                    E.Cast(target);
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
                ComboMenu.Add(new MenuKeyBind("wait", " - Wait for W Charm Toggle", KeyCode.G, KeybindType.Toggle));
                ComboMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("useW", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("useE", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("useR", "Use R if Killable"));
                ComboMenu.Add(new MenuSlider("enemiesR", "^- Don't R if X Enemies Behind", 2, 1, 5));
                ComboMenu.Add(new MenuSlider("backwardR", "^- Backward R Position Check Range", 400, 1, 600));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            var BlackList = new Menu("whitelist", "W Whitelist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Use W on: " + target.ChampionName,
                        true));
                }
            }
            RootMenu.Add(BlackList);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 50));
                HarassMenu.Add(new MenuBool("useQ", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("useW", "Use W in Combo"));
                HarassMenu.Add(new MenuBool("useE", "Use E in Combo"));
            }
            RootMenu.Add(HarassMenu);
            var FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("wait", "Wait for Charm"));
                JungleClear.Add(new MenuBool("useQ", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("useW", "Use W to Farm"));
                JungleClear.Add(new MenuBool("useE", "Use E to Farm"));
            }
            RootMenu.Add(FarmMenu);
            FarmMenu.Add(LaneClear);
            FarmMenu.Add(JungleClear);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useQ", "Use Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("useE", "Use E to Killsteal"));
                KillstealMenu.Add(new MenuBool("useR", "Use R to Killsteal"));
                KillstealMenu.Add(new MenuSlider("enemiesR", "^- Don't R if X Enemies Behind", 2, 1, 5));
                KillstealMenu.Add(new MenuSlider("backwardR", "^- Backward R Position Check Range", 400, 1, 600));

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw R Damage"));
                DrawMenu.Add(new MenuBool("drawbackward", "Draw Backward Enemy Check Range"));
                DrawMenu.Add(new MenuBool("drawtoggle", "Draw Charm Toggle"));

            }
            RootMenu.Add(DrawMenu);

            RootMenu.Attach();
        }



        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 800);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 1200);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 300);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 500);
            Q.SetSkillshot(0.25f, 60f, 2400f, true, SkillshotType.Line);
            R.SetSkillshot(0.25f, 180 * (float) Math.PI / 180, 2500f, false, SkillshotType.Cone, false, HitChance.None);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);


        }

        protected override void SemiR()
        {
            if (Player.GetSpell(SpellSlot.W).Level == 1)
            {
                W.Range = 1200;
            }
            if (Player.GetSpell(SpellSlot.W).Level == 2)
            {
                W.Range = 1300;
            }
            if (Player.GetSpell(SpellSlot.W).Level == 3)
            {
                W.Range = 1400;
            }
            if (Player.GetSpell(SpellSlot.W).Level == 4)
            {
                W.Range = 1500;
            }
            if (Player.GetSpell(SpellSlot.W).Level == 5)
            {
                W.Range = 1600;
            }

        }

        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (RootMenu["combo"]["wait"].Enabled)
            {
                var enemy = TargetSelector.Implementation.GetTarget(400);
                if (enemy != null)
                {
                    var target = enemy as Obj_AI_Base;
                    if (target != null)
                    {
                        if (target.HasBuff("EvelynnW"))
                        {
                            var daggers = ObjectManager.Get<GameObject>()
                                .Where(d => d.IsValid &&
                                            d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                            if (daggers.Count() == 0)
                            {

                                e.Cancel = true;

                            }
                        }
                    }
                }
            }
            if (RootMenu["farming"]["jungle"]["wait"].Enabled)
            {
                foreach (var jungleTarget in Bases.GameObjects.JungleLarge.Where(m => m.IsValidTarget(Q.Range))
                    .ToList())
                {
                    if (!jungleTarget.IsValidTarget() || jungleTarget.UnitSkinName.Contains("Plant"))
                    {
                        return;
                    }

                    if (jungleTarget.HasBuff("EvelynnW"))
                    {
                        var daggers = ObjectManager.Get<GameObject>()
                            .Where(d => d.IsValid &&
                                        d.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                        if (daggers.Count() == 0)
                        {

                            e.Cancel = true;

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
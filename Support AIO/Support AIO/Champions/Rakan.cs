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
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.ThirdParty;
using Support_AIO;
using Support_AIO.Bases;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Rakan : Champion
    {
        private int delayyyyyyyyy;
        private int meowdelay;
        private int meowmeowmeow;
        private bool hmm;

        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;




            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range+W.Range);

            if (!target.IsValidTarget())
            {

                return;
            }
            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
                }
            }
            if (useW)
            {
                foreach (var en in ObjectManager.Get<Obj_AI_Base>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (Player.SpellBook.GetSpell(SpellSlot.E).Cost +
                                        Player.SpellBook.GetSpell(SpellSlot.W).Cost < Player.Mana)
                                    {
                                        if (en.Distance(Player) < E.Range)
                                        {
                                            if (!Player.HasBuff("rakanerecast") && meowdelay <= Game.TickCount)
                                            {
                                                if (en.Distance(target) < Player.Distance(target))
                                                {
                                                    E.CastOnUnit(en);
                                                }
                                            }

                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {

                                                if (W.Cast(target))
                                                {
                                                    meowdelay = Game.TickCount + 1500;
                                                }
                                            }
                                        }
                                    }

                                }
                                if (en.Distance(Player) > E.Range)
                                {
                                    if (Player.SpellBook.GetSpell(SpellSlot.E).Cost +
                                        Player.SpellBook.GetSpell(SpellSlot.W).Cost < Player.Mana)
                                    {
                                        if (target.IsValidTarget(W.Range))
                                        {

                                            if (W.Cast(target))
                                            {
                                                meowdelay = Game.TickCount + 1500;
                                            }
                                        }
                                        if (en.Distance(Player) < E.Range)
                                        {
                                            if (!Player.HasBuff("rakanerecast") && meowdelay < Game.TickCount)
                                            {
                                                if (en.Distance(target) < Player.Distance(target))
                                                {
                                                    E.CastOnUnit(en);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (target.Distance(en) > 580)
                            {
                                if (target.IsValidTarget(W.Range))
                                {

                                    if (W.Cast(target))
                                    {
                                        meowdelay = Game.TickCount + 1500;
                                    }
                                }
                            }
                        }
                        else if (en.Distance(target) > W.Range + E.Range)
                        {
                            if (target.IsValidTarget(W.Range))
                            {

                                if (W.Cast(target))
                                {
                                    meowdelay = Game.TickCount + 1500;
                                }
                            }
                        }
                    }
                }
                if (!E.Ready)
                {
                    if (target.IsValidTarget(W.Range))
                    {

                        if (W.Cast(target))
                        {
                            meowdelay = Game.TickCount + 1500;
                        }
                    }
                }
            }
            if (RootMenu["combo"]["user"].Enabled)
            {
                if (target != null && Player.CountEnemyHeroesInRange(1000) >=
                    RootMenu["combo"]["hitr"].As<MenuSlider>().Value)
                {
                    if (target.HealthPercent() <= RootMenu["combo"]["hp"].As<MenuSlider>().Value)
                    {
                        R.Cast();
                    }
                }
            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["anim"].Enabled)
            {
                if (meowmeowmeow < Game.TickCount && hmm)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                    if (target.IsValidTarget() && target != null && target.IsValidTarget(Q.Range))
                    {

                        DelayAction.Queue(300, () =>
                        {

                            if (Q.Cast(target))
                            {
                                hmm = false;
                            }

                        });

                    }
                }
            }
            if (RootMenu["combo"]["aa"].Enabled)
            {
                if (Player.HasBuff("RakanR"))
                {
                    Orbwalker.Implementation.AttackingEnabled = false;
                }
                else Orbwalker.Implementation.AttackingEnabled = true;
            }
            if (!Player.HasBuff("RakanR"))
            {
                Orbwalker.Implementation.AttackingEnabled = true;
            }
      
            if (RootMenu["combo"]["wflash"].Enabled)
            {

                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range + 410);
                if (W.Ready)
                {
                    if (Flash.Ready && Flash != null)
                    {
                        var ummm = W.GetPrediction(target);
                        Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                        if (target.IsValidTarget())
                        {
                            if (target.Distance(Player) > W.Range)
                            {
                                if (Flash.Cast(target.ServerPosition))
                                {

                                    if (!RootMenu["combo"]["wf"].Enabled)
                                    {
                                        W.Cast(ummm.CastPosition);
                                    }
                                    if (RootMenu["combo"]["wf"].Enabled)
                                    {
                                        if (W.Cast(ummm.CastPosition))
                                        {
                                            R.Cast();
                                        }
                                    }
                                }



                            }
                        }
                    }
                }
            }

            if (RootMenu["combo"]["engage"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range + W.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                foreach (var en in ObjectManager.Get<Obj_AI_Base>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (Player.SpellBook.GetSpell(SpellSlot.E).Cost +
                                        Player.SpellBook.GetSpell(SpellSlot.W).Cost < Player.Mana)
                                    {
                                        if (en != null && delayyyyyyyyy <= Game.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {


                                                W.Cast(target);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var bestAllies = GameObjects.AllyHeroes
                .Where(t =>
                    t.Distance(ObjectManager.GetLocalPlayer()) < Support_AIO.Bases.Champion.E.Range)
                .OrderBy(o => o.Health);
            foreach (var ally in bestAllies)
            {
                if (E.Ready &&
                    ally.IsValidTarget(E.Range) &&
                    ally.CountEnemyHeroesInRange(800f) > 0 &&
                    HealthPrediction.Implementation.GetPrediction(ally, 250 + Game.Ping) <=
                    ally.MaxHealth / 4)
                {
                    E.CastOnUnit(ally);
                }
            }
            if (RootMenu["flee"]["key"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                if (RootMenu["flee"]["user"].Enabled)
                {
                    R.Cast();
                }
                if (RootMenu["flee"]["usee"].Enabled)
                {
                    foreach (var en in ObjectManager.Get<Obj_AI_Base>())
                    {
                        if (!en.IsDead)
                        {
                            if (en.Distance(Game.CursorPos) < 200 && en.IsAlly && !en.IsMe &&
                                en.Distance(Player) <= E.Range)
                            {
                                E.CastOnUnit(en);
                            }

                        }
                    }
                }
            }


        }


        protected override void Farming()
        {
            throw new NotImplementedException();
        }

        protected override void LastHit()
        {
            throw new NotImplementedException();
        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"].Enabled)
            {
                Render.Circle(Player.Position, W.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"].Enabled)
            {
                Render.Circle(Player.Position, E.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawflee"].Enabled && RootMenu["flee"]["key"].Enabled)
            {

                Render.Circle(Game.CursorPos, 200, 100, Color.Ivory);

            }
            if (RootMenu["drawings"]["drawrange"].Enabled)
            {
                Render.Circle(Player.Position, E.Range + W.Range, 40, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["wflash"].Enabled)
            {
                Render.Circle(Player.Position, W.Range + 410, 40, Color.HotPink);
            }

        }

        protected override void Killsteal()
        {

        }


        internal override void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            if (RootMenu["combo"]["anim"].Enabled)
            {
                if (e.Slot == SpellSlot.W && Q.Ready)
                {
                    hmm = true;
                    meowmeowmeow = Game.TickCount + 50;
                }
            }
            if (e.Slot == SpellSlot.E)
            {
                delayyyyyyyyy = Game.TickCount + 300;
            }
        }

        protected override void Harass()
        {

            bool useQ = RootMenu["harass"]["useq"].Enabled;
            bool useE = RootMenu["harass"]["logic"].Enabled;
            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range+W.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (useE)
            {
                foreach (var en in ObjectManager.Get<Obj_AI_Base>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (Player.SpellBook.GetSpell(SpellSlot.E).Cost +
                                        Player.SpellBook.GetSpell(SpellSlot.W).Cost < Player.Mana)
                                    {
                                        if (en != null && delayyyyyyyyy <= Game.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {


                                                W.Cast(target);
                                            }
                                        }
                                        if (en.HasBuff("RakanEShield") && delayyyyyyyyy <= Game.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                    }
                                }

                                if (en.Distance(Player) > E.Range)
                                {

                                    if (Player.SpellBook.GetSpell(SpellSlot.E).Cost +
                                        Player.SpellBook.GetSpell(SpellSlot.W).Cost < Player.Mana)
                                    {

                                        if (target.IsValidTarget(W.Range))
                                        {


                                            W.Cast(target);
                                        }
                                        if (en.Distance(Player) < E.Range && delayyyyyyyyy <= Game.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                            if (en.HasBuff("RakanEShield"))
                                            {
                                                E.CastOnUnit(en);
                                            }
                                        }
                                    }
                                }

                            }
                        }

                    }
                }
            }
            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
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
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));

                ComboMenu.Add(new MenuBool("aa", "^- Block Auto Attacks while in R"));
                ComboMenu.Add(new MenuSlider("hitr", "If X Near Enemies", 2, 1, 5));
                ComboMenu.Add(new MenuSlider("hp", "If Enemy X Health", 50, 1, 100));
                ComboMenu.Add(new MenuKeyBind("engage", "Engage E - W Combo", KeyCode.T, KeybindType.Press));
                ComboMenu.Add(new MenuKeyBind("wflash", "W - Flash", KeyCode.Z, KeybindType.Press));
                ComboMenu.Add(new MenuBool("wf", "^- Use R Meanwhile in W"));
                ComboMenu.Add(new MenuBool("anim", "Cancel W Animation with Q"));

            }
            RootMenu.Add(ComboMenu);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 40, 1, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                HarassMenu.Add(new MenuBool("logic", "Use E - W - E Harass Logic"));

            }
            RootMenu.Add(HarassMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawflee", "Draw Flee Radius"));
                DrawMenu.Add(new MenuBool("drawrange", "Draw Engage Range"));
                DrawMenu.Add(new MenuBool("wflash", "Draw W-Flash"));
            }
            FarmMenu = new Menu("flee", "Flee");
            {
                FarmMenu.Add(new MenuKeyBind("key", "Flee Key", KeyCode.G, KeybindType.Press));
                FarmMenu.Add(new MenuBool("user", "Flee with R", false));
                FarmMenu.Add(new MenuBool("usee", "Flee with E"));

            }
            
            RootMenu.Add(HarassMenu);
            RootMenu.Add(DrawMenu);
            RootMenu.Add(FarmMenu);
            EvadeMenu = new Menu("wset", "Shielding");
            {
                EvadeMenu.Add(new MenuList("modes", "Shielding Mode", new[] { "Spells Detector", "ZLib" }, 1));
                var First = new Menu("first", "Spells Detector");
                SpellBlocking.EvadeManager.Attach(First);
                SpellBlocking.EvadeOthers.Attach(First);
                SpellBlocking.EvadeTargetManager.Attach(First);
               
                EvadeMenu.Add(First);
                var zlib = new Menu("zlib", "ZLib");

                Support_AIO.ZLib.Attach(EvadeMenu);

            }

            RootMenu.Add(EvadeMenu);
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 900);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 650);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 550);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 60, 1800, true, SkillshotType.Line, false, HitChance.None);
            W.SetSkillshot(0.25f, 50, 1800, false, SkillshotType.Circle, false, HitChance.None);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);

        }
    }
}

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
using Support_AIO.Handlers;
using Support_AIO.SpellBlocking;
using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Janna : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;




            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

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
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.CastOnUnit(target);
                }
            }
        }

        protected override void SemiR()
        {


            if (RootMenu["insec"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range + 410);
                if (R.Ready)
                {
                    if (Flash.Ready && Flash != null && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(380))
                        {

                            foreach (var ally in GameObjects.AllyHeroes)
                            {
                                if (ally != null && UnitExtensions.Distance(ally, Player) < 1000 && !ally.IsMe)
                                {
                                    if (Flash.Cast(target.ServerPosition.Extend(ally.ServerPosition, -100)))
                                    {
                                        R.Cast();

                                    }
                                }


                            }
                            if (GameObjects.AllyHeroes.Where(x => UnitExtensions.Distance(x, Player) < E.Range)
                                    .Count() == 1)
                            {
                                if (Flash.Cast(target.ServerPosition.Extend(Player.ServerPosition, -100)))
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
            throw new NotImplementedException();
        }

        internal override void OnPreAttack(object sender, PreAttackEventArgs e)
        {
            if (RootMenu["combo"]["support"].Enabled)
            {
                if (Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Lasthit) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Laneclear) ||
                    Orbwalker.Implementation.Mode.Equals(OrbwalkingMode.Mixed))
                {

                    if (e.Target.IsMinion && GameObjects.AllyHeroes
                            .Where(x => UnitExtensions.Distance(x, Player) < E.Range).Count() > 1)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    
        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);
            RootMenu.Add(new MenuKeyBind("insec", "Insec Key", KeyCode.T, KeybindType.Press));
            Orbwalker.Implementation.Attach(RootMenu);


            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("support", "Support Mode"));
            }
            RootMenu.Add(ComboMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

            }
            RootMenu.Add(DrawMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");

            EvadeMenu = new Menu("wset", "Shielding");
            {
                EvadeMenu.Add(new MenuList("modes", "Shielding Mode", new[] {"Spells Detector", "ZLib"}, 1));
                var First = new Menu("first", "Spells Detector");
                SpellBlocking.EvadeManager.Attach(First);
                SpellBlocking.EvadeOthers.Attach(First);
                SpellBlocking.EvadeTargetManager.Attach(First);
                var test = new Menu("Misc.E.Spell.Menu", "Boost Ally Damage on Spells");
                foreach (var spell in
                    GameObjects.AllyHeroes.Where(h => !h.IsMe)
                        .SelectMany(
                            hero => DamageBoostDatabase.Spells.Where(s => s.Champion == hero.ChampionName)))
                {
                    test.Add(new MenuBool("Misc.E.Spell." + spell.Spell, spell.Champion + " " + spell.Slot));
                }
                EvadeMenu.Add(test);
                EvadeMenu.Add(First);
                var zlib = new Menu("zlib", "ZLib");

                Support_AIO.ZLib.Attach(EvadeMenu);


            }
            RootMenu.Add(EvadeMenu);

            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
         

                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    Q.Cast(Args.EndPosition);
                }
            
        }



        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!E.Ready)
            {
                return;
            }

            if (sender.IsValid && sender.IsAlly && !sender.IsMe && sender.Distance(Player) < E.Range)
            {
                var spell = args.SpellData.Name;
                var caster = sender as Obj_AI_Hero;

                if (DamageBoostDatabase.Spells.Any(s => s.Spell == spell) && caster.CountEnemyHeroesInRange(1500) > 0)
                {
                    if (RootMenu["wset"]["Misc.E.Spell.Menu"]["Misc.E.Spell." + args.SpellData.Name].Enabled)
                    {

                        E.CastOnUnit(caster);
     

                    }
                }
            }
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 830);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 600);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 800);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 725);
            Q.SetSkillshot(0.25f, 120f, 900f, false, SkillshotType.Line);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);
        }
    }
}

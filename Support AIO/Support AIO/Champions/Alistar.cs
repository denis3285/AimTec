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
using Support_AIO;
using Support_AIO.Bases;

using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

namespace Support_AIO.Champions
{
    class Alistar : Champion
    {
        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"].Enabled;
            bool useW = RootMenu["combo"]["usew"].Enabled;
            bool useR = RootMenu["combo"]["user"].Enabled;
            float enemies = RootMenu["combo"]["hitr"].As<MenuSlider>().Value;
            float hp = RootMenu["combo"]["hp"].As<MenuSlider>().Value;
            var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (W.Ready && Q.Ready && useW && target.IsValidTarget(W.Range) && Player.SpellBook.GetSpell(SpellSlot.Q).Cost + Player.SpellBook.GetSpell(SpellSlot.W).Cost <= Player.Mana)
            {

                if (target != null)
                {
                    W.CastOnUnit(target);
                }
            }
            if (RootMenu["combo"]["wtog"].Enabled)
            {
                if (W.Ready && useW && target.IsValidTarget(W.Range))
                {

                    if (target != null)
                    {
                        W.CastOnUnit(target);
                    }
                }

            }
            if (target.IsValidTarget(Q.Range)  && useQ)
            {

                if (target != null)
                {
                    Q.Cast();
                }
            }

            if (target.IsValidTarget(E.Range))
            {

                if (target != null)
                {
                    E.Cast();
                }
            }

            if (useR)
            {

                if (target != null && enemies <= Player.CountEnemyHeroesInRange(W.Range))
                {
                    if (hp >= Player.HealthPercent())
                    {
                        R.Cast();
                    }
                }
            }

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["wait"].Enabled)
            {
                Console.WriteLine(Player.GetRealBuffCount("AlistarE"));
                if (Player.HasBuff("AlistarE") && Player.GetRealBuffCount("AlistarE") < 5) 
                {
                    Orbwalker.Implementation.AttackingEnabled = false;
                }
                
                else Orbwalker.Implementation.AttackingEnabled = true;
                
            }
            if (RootMenu["combo"]["autor"].Enabled)
            {
                if (Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Stun) ||
                    Player.HasBuffOfType(BuffType.Fear) || Player.HasBuffOfType(BuffType.Snare) ||
                    Player.HasBuffOfType(BuffType.Taunt) ||
                    Player.HasBuffOfType(BuffType.Suppression))
                {
                    R.Cast();
                }
            }
            if (RootMenu["flashe"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(1300);
                if (Q.Ready)
                {
                    if (Flash.Ready && Flash != null && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(1300))
                        {
                            foreach (var en in GameObjects.EnemyMinions)
                            {
                                if (!en.IsDead && en.IsValidTarget(W.Range) && en.Distance(target) < 730)
                                {

                                    if (target.Distance(Player) > Q.Range && target != null)
                                    {
                                        W.CastOnUnit(en);
                                    }
                                    if (Q.Ready)
                                    {
                                        if (Flash.Ready && Flash != null && target.IsValidTarget())
                                        {
                                            if (target.IsValidTarget(Q.Range + 410))
                                            {
                                                if (target.Distance(Player) > Q.Range && target != null)
                                                {
                                                    if (Q.Cast())
                                                    {

                                                        Flash.Cast(target.ServerPosition);

                                                    }
                                                }
                                            }
                                        }
                                    };
                                }
                            }
                        }
                    }
                }
            }
            if (RootMenu["flashq"].Enabled)
            {
                Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range + 380);
                if (Q.Ready)
                {
                    if (Flash.Ready && Flash != null && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(Q.Range + 410))
                        {
                            if (target.Distance(Player) > Q.Range && target != null)
                            {
                                if (Q.Cast())
                                {

                                    Flash.Cast(target.ServerPosition);

                                }
                            }
                        }
                    }
                }


            }
        }

        protected override void Farming()
        {
        }

        protected override void LastHit()
        {
        }

        protected override void Drawings()
        {
            Vector2 maybeworks;
            var heropos = Render.WorldToScreen(Player.Position, out maybeworks);
            var xaOffset = (int)maybeworks.X;
            var yaOffset = (int)maybeworks.Y;
       
                if (RootMenu["combo"]["wtog"].Enabled)
                {
                    Render.Text(xaOffset - 50, yaOffset + 10, Color.GreenYellow, "W Without Q: ON",
                        RenderTextFlags.VerticalCenter);
                }
                if (!RootMenu["combo"]["wtog"].Enabled)
                {
                Render.Text(xaOffset - 50, yaOffset + 10, Color.Red, "W Without Q: OFF",
                 RenderTextFlags.VerticalCenter);
                }
            
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
            if (RootMenu["drawings"]["drawflash"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range + 380, 40, Color.WhiteSmoke);
            }
            if (RootMenu["drawings"]["drawengage"].Enabled)
            {
                Render.Circle(Player.Position, 1300, 40, Color.Turquoise);
            }
        }

        protected override void Killsteal()
        {
            if (Q.Ready &&
                RootMenu["killsteal"]["useq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
              Q.Cast();
                }
            }
            if (W.Ready &&
                RootMenu["killsteal"]["usew"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {
                    W.CastOnUnit(bestTarget);
                }
            }
        }

        protected override void Harass()
        {
        }

        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Support AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);

            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuKeyBind("wtog", "Toggle for W without Q", KeyCode.T, KeybindType.Toggle));
                ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                ComboMenu.Add(new MenuBool("wait", "^- Block Auto Attacks if not Stacked", false));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuBool("autor", "Auto R on CC"));
                ComboMenu.Add(new MenuSlider("hp", "Use R if HP <=", 25, 10, 100));
                ComboMenu.Add(new MenuSlider("hitr", "Min. Enemies", 2, 1, 5));

            }
            RootMenu.Add(ComboMenu);
            KillstealMenu = new Menu("killsteal", "Killsteal");
            {
                KillstealMenu.Add(new MenuBool("useq", "Use Q to Killsteal"));
                KillstealMenu.Add(new MenuBool("usew", "Use W to Killsteal"));

            }
            RootMenu.Add(KillstealMenu);
            DrawMenu = new Menu("drawings", "Drawings");
            {
                DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                DrawMenu.Add(new MenuBool("drawflash", "Draw Q Flash Range"));
                DrawMenu.Add(new MenuBool("drawengage", "Draw Engage Range"));

            }
            RootMenu.Add(DrawMenu);
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");

            RootMenu.Add(new MenuKeyBind("flashq", "Q - Flash", KeyCode.T, KeybindType.Press));
            RootMenu.Add(new MenuKeyBind("flashe", "W - Q - Flash", KeyCode.G, KeybindType.Press));
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
           

                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    Q.Cast();
                }
            
        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 365);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 650);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 350);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 0);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner1).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner1, 425);
            if (Player.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == "SummonerFlash")
                Flash = new Aimtec.SDK.Spell(SpellSlot.Summoner2, 425);
        }
    }
}

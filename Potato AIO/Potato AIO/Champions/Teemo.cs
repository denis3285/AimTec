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
    class Teemo : Champion
    {
        protected override void Combo()
        {
            if (RootMenu["combo"]["items"].Enabled)
            {
                var ItemGunblade = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null)
                    .FirstOrDefault(o => o.SpellData.Name == "HextechGunblade");
                if (ItemGunblade != null)
                {
                    Aimtec.SDK.Spell Gunblade = new Aimtec.SDK.Spell(ItemGunblade.Slot, 700);
                    if (Gunblade.Ready)
                    {
                        if (!GlobalKeys.ComboKey.Active)
                        {
                            return;
                        }

                        var Enemies =
                            GameObjects.EnemyHeroes.Where(
                                t => t.IsValidTarget(Gunblade.Range, true) && !t.IsInvulnerable);
                        foreach (var enemy in Enemies.Where(
                            e => e.IsValidTarget(Gunblade.Range) && e != null))
                        {
                            Gunblade.Cast(enemy);
                        }
                    }
                }
                var ItemCutlass = Player.SpellBook.Spells.Where(o => o != null && o.SpellData != null)
                    .FirstOrDefault(o => o.SpellData.Name == "BilgewaterCutlass");
                if (ItemCutlass != null)
                {
                    Aimtec.SDK.Spell Cutlass = new Aimtec.SDK.Spell(ItemCutlass.Slot, 550);

                    var Enemies =
                        GameObjects.EnemyHeroes.Where(t => t.IsValidTarget(Cutlass.Range, true) && !t.IsInvulnerable);
                    foreach (var enemy in Enemies.Where(
                        e => e.IsValidTarget(Cutlass.Range) && e != null))
                    {

                        Cutlass.Cast(enemy);
                    }
                }
            }
            bool useQ = RootMenu["combo"]["useq"].Enabled;


            bool useW = RootMenu["combo"]["usew"].Enabled;
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget())
                {

                    if (target.IsValidTarget(Q.Range) && !RootMenu["combo"]["qaa"].Enabled)
                    {

                        if (target != null)
                        {
                            if (RootMenu["whitelist"][target.ChampionName.ToLower()].Enabled)
                            {

                                Q.CastOnUnit(target);
                            }
                        }
                    }

                }
            }
            if (useW)
            {
                if (!RootMenu["combo"]["waa"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(1000);

                    if (target.IsValidTarget(1000))
                    {

                        if (target.IsValidTarget(1000))
                        {

                            if (target != null)
                            {


                                W.Cast();

                            }
                        }

                    }
                }
                if (RootMenu["combo"]["waa"].Enabled)
                {
                    var target = Extensions.GetBestEnemyHeroTargetInRange(Player.GetFullAttackRange(Player));

                    if (target.IsValidTarget(Player.GetFullAttackRange(Player)))
                    {

                        if (target.IsValidTarget(Player.GetFullAttackRange(Player)))
                        {

                            if (target != null)
                            {


                                W.Cast();

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

                    if (target.IsValidTarget(R.Range) && RootMenu["combo"]["hitr"].As<MenuSlider>().Value <= Player.GetSpell(SpellSlot.R).Ammo)
                    {
                        
                        if (target != null && rdelay < Game.TickCount)
                        {

                            R.Cast(target);

                        }
                    }

                }
            }

        }

        internal override void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SpellData.Name == "TeemoRCast")
                {
                    rdelay = 3000 + Game.TickCount;
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


                        if (minion.IsValidTarget(Q.Range) && !RootMenu["farming"]["lane"]["qaa"].Enabled)
                        {
                            Q.Cast(minion);
                        }
                        if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                        {
                            if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                            {
                                Q.Cast(minion);
                            }
                        }
                    }
                    if (RootMenu["farming"]["lane"]["user"].Enabled && rdelay  < Game.TickCount)
                    {
                        if (minion.IsValidTarget(R.Range) && minion != null)
                        {
                            if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(200, false, false,
                                    minion.ServerPosition)) >=
                                RootMenu["farming"]["lane"]["hitr"].As<MenuSlider>().Value)
                            {
                                R.Cast(minion);
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

                    bool useR = RootMenu["farming"]["jungle"]["user"].Enabled;


                    if (useQ)
                    {
                        if (jungleTarget != null && jungleTarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(jungleTarget);
                        }
                    }
                    if (rdelay < Game.TickCount && useR)
                    {
                        
                        if (jungleTarget != null && jungleTarget.IsValidTarget(R.Range) &&
                            Player.GetSpell(SpellSlot.R).Ammo >=
                            RootMenu["farming"]["jungle"]["hitsr"].As<MenuSlider>().Value)
                        {
                            R.Cast(jungleTarget);
                        }
                    }
                }
            }

        }

        public static readonly List<string> SpecialChampions = new List<string> {"Annie", "Jhin"};
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
            if (RootMenu["drawings"]["drawmushroom"].Enabled)
            {

                // Uhhh, Sorry if I did it in wrong way. :c
                Render.Circle(new Vector3(3700.708f, -11.22648f, 9294.094f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(2314f, 53.165f, 9722f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(3090f, -68.03732f, 10810f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(4722f, -71.2406f, 10010f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(5208f, -71.2406f, 9114f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(4724f, 52.53909f, 7590f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(4564f, 51.83786f, 6060f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(2760f, 52.96445f, 5178f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(4440f, 56.8484f, 11840f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(2420f, 52.8381f, 13482f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(1630f, 52.8381f, 13008f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(1172f, 52.8381f, 12302f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(5666f, 52.8381f, 12722f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(8004f, 56.4768f, 11782f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9194f, 53.35013f, 11368f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(8280f, 50.06194f, 10254f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(6728f, 53.82967f, 11450f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(6242f, 54.09851f, 10270f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(6484f, -71.2406f, 8380f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(8380f, -71.2406f, 6502f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9099.75f, 52.95337f, 7376.637f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(7376f, 52.8726f, 8802f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(7602f, 52.56985f, 5928f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9372f, -71.2406f, 5674f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(10148f, -71.2406f, 4801.525f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9772f, 9.031885f, 6458f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9938f, 51.62378f, 7900f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(11465f, 51.72557f, 7157.772f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(12481f, 51.7294f, 5232.559f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(11266f, -7.897567f, 5542f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(11290f, 64.39886f, 8694f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(12676f, 51.6851f, 7310.818f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(12022f, 9154f, 51.25105f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(6544f, 48.257f, 4732f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(5576f, 51.42581f, 3512f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(6888f, 51.94016f, 3082f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(8070f, 51.5508f, 3472f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(8594f, 51.73177f, 4668f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(10388f, 49.81641f, 3046f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(9160f, 59.97022f, 2122f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(12518f, 53.66707f, 1504f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(13404f, 51.3669f, 2482f), 100, 10, Color.Wheat);
                Render.Circle(new Vector3(11854f, -68.06037f, 3922f), 100, 10, Color.Wheat);

            }
            if (RootMenu["drawings"]["drawq"].Enabled)
            {
                Render.Circle(Player.Position, Q.Range, 40, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawr"].Enabled)
            {
                Render.Circle(Player.Position, R.Range, 40, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawdamage"].Enabled)
            {

                ObjectManager.Get<Obj_AI_Base>()
                    .Where(h => h is Obj_AI_Hero && h.IsValidTarget() && h.IsValidTarget(Q.Range * 2))
                    .ToList()
                    .ForEach(
                        unit =>
                        {
                         
                           
                            var heroUnit = unit as Obj_AI_Hero;
                            int width = 103;

                            int xOffset = SxOffset(heroUnit);
                            int yOffset = SyOffset(heroUnit);
                            var barPos = unit.FloatingHealthBarPosition;
                            barPos.X += xOffset;
                            barPos.Y += yOffset;
                            var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                            var drawStartXPos =
                                (float) (barPos.X + (unit.Health >
                                                     Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                     Player.GetSpellDamage(unit, SpellSlot.E) + Player.GetSpellDamage(unit, SpellSlot.E, DamageStage.DamagePerSecond)
                                             ? width * ((unit.Health -
                                                         (Player.GetSpellDamage(unit, SpellSlot.Q) +
                                                          Player.GetSpellDamage(unit, SpellSlot.E) + Player.GetSpellDamage(unit, SpellSlot.E, DamageStage.DamagePerSecond)

                                                          )) /
                                                        unit.MaxHealth * 100 / 100)
                                             : 0));

                            Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, 8, true,
                                unit.Health < Player.GetSpellDamage(unit, SpellSlot.Q) +
                                Player.GetSpellDamage(unit, SpellSlot.E) + Player.GetSpellDamage(unit, SpellSlot.E, DamageStage.DamagePerSecond)
                                    ? Color.GreenYellow
                                    : Color.Orange);

                        });
            }
        }




        protected override void Killsteal()
        {

            if (Q.Ready &&
                RootMenu["ks"]["ksq"].Enabled)
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >=
                    bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
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
                if (RootMenu["combo"]["qaa"].Enabled)
                {
                    if (RootMenu["whitelist"][hero.ChampionName.ToLower()].Enabled)
                    {
                        if (Q.Ready)
                        {
                            Q.Cast(hero);

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
                        Q.Cast(hero);

                    }
                }

            }

        }

        protected override void Harass()
        {

            if (Player.ManaPercent() >= RootMenu["harass"]["mana"].As<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"].Enabled;
                if (useQ && !RootMenu["harass"]["qaa"].Enabled)
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

            }
        }



        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"Potato AIO - {Program.Player.ChampionName}", true);

            Orbwalker.Implementation.Attach(RootMenu);
            ComboMenu = new Menu("combo", "Combo");
            {
                ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                ComboMenu.Add(new MenuBool("qaa", " ^- Only for AA Reset"));
                ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                ComboMenu.Add(new MenuBool("waa", " ^- Only if in AA Range"));
                ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                ComboMenu.Add(new MenuSlider("hitr", " ^- Min. R Charges", 2, 1, 3));
                ComboMenu.Add(new MenuBool("items", "Use Items"));
            }
            RootMenu.Add(ComboMenu);
            var BlackList = new Menu("whitelist", "Q Whitelist");
            {
                foreach (var target in GameObjects.EnemyHeroes)
                {
                    BlackList.Add(new MenuBool(target.ChampionName.ToLower(), "Use Q on: " + target.ChampionName,
                        true));
                }
            }
            RootMenu.Add(BlackList);
            HarassMenu = new Menu("harass", "Harass");
            {
                HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));
                HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                HarassMenu.Add(new MenuBool("qaa", "^- Only for AA Reset"));

            }
            RootMenu.Add(HarassMenu);
            FarmMenu = new Menu("farming", "Farming");
            var LaneClear = new Menu("lane", "Lane Clear");
            {
                LaneClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                LaneClear.Add(new MenuBool("useq", "Use Q to Farm"));
                LaneClear.Add(new MenuBool("qaa", "^- Only for Last Hit"));
                LaneClear.Add(new MenuBool("user", "Use R to Farm"));
                LaneClear.Add(new MenuSlider("hitr", "^- if Hits X", 3, 0, 6));
            }
            var JungleClear = new Menu("jungle", "Jungle Clear");
            {
                JungleClear.Add(new MenuSlider("mana", "Mana Manager", 50));
                JungleClear.Add(new MenuBool("useq", "Use Q to Farm"));
                JungleClear.Add(new MenuBool("user", "Use R to Farm"));
                JungleClear.Add(new MenuSlider("hitsr", " ^- Min. R Charges", 2, 1, 3));
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
     
                DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                DrawMenu.Add(new MenuBool("drawdamage", "Draw Damage"));
                DrawMenu.Add(new MenuBool("drawmushroom", "Draw Mushroom Positions"));
            }
            Gapcloser.Attach(RootMenu, "Q Anti-Gap");
            RootMenu.Add(DrawMenu);
            RootMenu.Attach();
        }

        internal override void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {


            if (target != null && Args.EndPosition.Distance(Player) < Q.Range && Q.Ready)
            {
                Q.Cast(target);
            }

        }

        protected override void SetSpells()
        {
            Q = new Aimtec.SDK.Spell(SpellSlot.Q, 680);
            W = new Aimtec.SDK.Spell(SpellSlot.W, 0);
            E = new Aimtec.SDK.Spell(SpellSlot.E, 0);
            R = new Aimtec.SDK.Spell(SpellSlot.R, 400);
            R.SetSkillshot(1.4f, 50, 1400, false, SkillshotType.Circle);
        }

        protected override void SemiR()
        {

            if (Player.GetSpell(SpellSlot.R).Level == 1)
            {
                R.Range = 400;
            }
            if (Player.GetSpell(SpellSlot.R).Level == 2)
            {
                R.Range = 650;
            }
            if (Player.GetSpell(SpellSlot.R).Level == 3)
            {
                R.Range = 900;
            }
        }

        protected override void LastHit()
        {
            foreach (var minion in Potato_AIO.Bases.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }

                if (RootMenu["farming"]["lane"]["useq"].Enabled && minion != null)
                { 
                    if (minion.IsValidTarget(Q.Range) && RootMenu["farming"]["lane"]["qaa"].Enabled)
                    {
                        if (minion.Health < Player.GetSpellDamage(minion, SpellSlot.Q))
                        {
                            Q.Cast(minion);
                        }
                    }
                }
            }
        }
    }
}

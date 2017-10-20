using Aimtec.SDK.Damage;

namespace Support_AIO.MorganaSpellBlocking
{
   

    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Aimtec.SDK.Extensions;



    using System;
    using System.Linq;

    internal class EvadeOthers
    {
        public static Menu Menu;
        private static int RivenQTime;
        private static float RivenQRange;
        private static Vector2 RivenDashPos;
        private static IOrderedEnumerable<Obj_AI_Hero> bestAllies;

        internal static void Attach(Menu evadeMenu)
        {

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            Dash.HeroDashed += OnDash;
        }

        private static void OnUpdate()
        {

            bestAllies = GameObjects.AllyHeroes
                .Where(t =>
                    t.Distance(ObjectManager.GetLocalPlayer()) < Bases.Champion.E.Range)
                .OrderBy(x => x.Health);

            foreach (var ally in bestAllies)
            {
                if (ally != null)
                {
                    if (ObjectManager.GetLocalPlayer().IsDead)
                    {
                        return;
                    }
                    if (ally != null)
                    {
                        if (ObjectManager.GetLocalPlayer().HasBuff("karthusfallenonetarget"))
                        {
                            if ((ObjectManager.GetLocalPlayer().GetBuff("karthusfallenonetarget").EndTime -
                                 Game.ClockTime) * 1000 <= 300)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                    Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }

                        if (ObjectManager.GetLocalPlayer().HasBuff("vladimirhemoplaguedebuff"))
                        {
                            if ((ObjectManager.GetLocalPlayer().GetBuff("vladimirhemoplaguedebuff").EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                    Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }
                        if (ObjectManager.GetLocalPlayer().HasBuff("nautilusgrandlinetarget"))
                        {
                            if ((ObjectManager.GetLocalPlayer().GetBuff("nautilusgrandlinetarget").EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                    Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }
                        if (ObjectManager.GetLocalPlayer().HasBuff("nocturneparanoiadash"))
                        {
                            if (GameObjects.EnemyHeroes.FirstOrDefault(
                                    x =>
                                        !x.IsDead && x.ChampionName.ToLower() == "nocturne" &&
                                        x.Distance(ObjectManager.GetLocalPlayer()) < 500) != null)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                    Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }

                        if (ObjectManager.GetLocalPlayer().HasBuff("soulshackles"))
                        {
                            if ((ObjectManager.GetLocalPlayer().GetBuff("soulshackles").EndTime - Game.ClockTime) *
                                1000 <=
                                300)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                   Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }

                        if (ObjectManager.GetLocalPlayer().HasBuff("zedrdeathmark"))
                        {
                            if ((ObjectManager.GetLocalPlayer().GetBuff("zedrdeathmark").EndTime - Game.ClockTime) *
                                1000 <=
                                300)
                            {
                                if (EvadeTargetManager.Menu["whitelist"][
                                        ally.ChampionName.ToLower()]
                                    .As<MenuBool>().Enabled)
                                {
                                    Bases.Champion.E.CastOnUnit(ally);
                                }
                            }
                        }
                    }


                    foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsValidTarget()))
                    {
                        switch (target.ChampionName)
                        {
                            case "Jax":
                                
                                    if (target.HasBuff("jaxcounterstrike"))
                                    {

                                        var buff = target.GetBuff("JaxCounterStrike");

                                        if ((buff.EndTime - Game.ClockTime) * 1000 <= 650 &&
                                            ally.Distance(target) <= 350f)
                                        {

                                            if (EvadeTargetManager.Menu["whitelist"][
                                                    ally.ChampionName.ToLower()]
                                                .As<MenuBool>().Enabled)
                                            {
                                               Bases.Champion.E.CastOnUnit(ally);
                                            }
                                        }

                                    }
                                


                                break;
                            case "Riven":
                              
                                    if (Utils.GameTimeTickCount - RivenQTime <= 100 && RivenDashPos.IsValid() &&
                                        ally.Distance(target) <= RivenQRange)
                                    {
                                        if (EvadeTargetManager.Menu["whitelist"][
                                                ally.ChampionName.ToLower()]
                                            .As<MenuBool>().Enabled)
                                        {
                                            Bases.Champion.E.CastOnUnit(ally);
                                        }

                                    
                                }

                                break;
                        }
                    }
                }
            }
        }



        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs Args)
        {

            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }


            var ally = Args.Target as Obj_AI_Hero;
            var target = sender as Obj_AI_Hero;


            if (target == null || target.Team == ObjectManager.GetLocalPlayer().Team || !target.IsValid ||
                Args.Target == null || string.IsNullOrEmpty(Args.SpellData.Name) || !target.IsHero ||
                Args.SpellData.Name.Contains("BasicAttack"))
            {

                return;
            }

            var spellData =
                EvadeTargetManager.Spells.FirstOrDefault(
                    i =>
                        i.SpellNames.Contains(Args.SpellData.Name.ToLower()));
            if (spellData != null)
            {
                return;
            }
            if (Args.SpellData.Name == "CaitlynAceintheHole")
            {
                return;
            }

            if (Args.Target.Distance(Bases.Champion.Player) < Bases.Champion.E.Range)
            {
                if (ally != null && ally.IsHero && ally.IsAlly)
                {
                    if (EvadeTargetManager.Menu["whitelist"][
                            ally.ChampionName.ToLower()]
                        .As<MenuBool>().Enabled)
                    {
                        Bases.Champion.E.CastOnUnit(Args.Target);
                    }
                }

            }
        }



        private static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs Args)
        {
            var riven = sender as Obj_AI_Hero;

            if (riven == null || riven.Team == ObjectManager.GetLocalPlayer().Team || riven.ChampionName != "Riven" || !riven.IsValid)
            {
                return;
            }


            if (Menu["Block" + riven.ChampionName.ToLower()]["BlockSpell" + SpellSlot.Q.ToString()] != null &&
                Menu["Block" + riven.ChampionName.ToLower()]["BlockSpell" + SpellSlot.Q.ToString()].Enabled)
            {
                if (Args.Animation.ToLower() == "spell1c")
                {
                    RivenQTime = Utils.GameTimeTickCount;
                    RivenQRange = riven.HasBuff("RivenFengShuiEngine") ? 225f : 150f;
                }
            }
        }

        private static void OnDash(object obj, Dash.DashArgs Args)
        {
            var riven = Args.Unit as Obj_AI_Hero;

            if (riven == null || riven.Team == ObjectManager.GetLocalPlayer().Team || riven.ChampionName != "Riven" || !riven.IsValid)
            {
                return;
            }

            if (Menu["Block" + riven.ChampionName.ToLower()]["BlockSpell" + SpellSlot.Q.ToString()] != null &&
               Menu["Block" + riven.ChampionName.ToLower()]["BlockSpell" + SpellSlot.Q.ToString()].Enabled)
            {
                RivenDashPos = Args.EndPos;
            }
        }
    }
}
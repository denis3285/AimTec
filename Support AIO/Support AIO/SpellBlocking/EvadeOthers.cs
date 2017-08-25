using Aimtec.SDK.Damage;

namespace Support_AIO.SpellBlocking
{
    #region

    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Aimtec.SDK.Extensions;



    using System;
    using System.Linq;

    #endregion
    using Support_AIO;
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
            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Karma")
            {
                bestAllies = GameObjects.AllyHeroes
                    .Where(t =>
                        t.Distance(ObjectManager.GetLocalPlayer()) < Bases.Champion.E.Range)
                    .OrderBy(x => x.Health);
            }
            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" || ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

            {
                bestAllies = GameObjects.AllyHeroes
                    .Where(t =>
                        t.Distance(ObjectManager.GetLocalPlayer()) < Bases.Champion.W.Range)
                    .OrderBy(x => x.Health);
            }
            if (bestAllies != null)
            {
                foreach (var ally in bestAllies)
                {

                    if (ObjectManager.GetLocalPlayer().IsDead)
                    {
                        return;
                    }
                    if (ally != null)
                    {

                        if (ally.HasBuff("karthusfallenonetarget"))
                        {
                            if ((ally.GetBuff("karthusfallenonetarget")
                                     .EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {
                                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(ally);
                                    }
                                }
                                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(ally);
                                    }
                                }
                            }
                        }
                        if (ally.HasBuff("nautilusgrandlinetarget"))
                        {
                            if ((ally.GetBuff("nautilusgrandlinetarget")
                                     .EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {

                                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(ally);
                                    }
                                }
                                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(ally);
                                    }
                                }
                            }
                        }
                        if (ally.HasBuff("nocturneparanoiadash"))
                        {
                            if ((ally.GetBuff("nocturneparanoiadash")
                                     .EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {


                                if (GameObjects.EnemyHeroes.FirstOrDefault(
                                        x =>
                                            !x.IsDead && x.ChampionName.ToLower() == "nocturne" &&
                                            x.Distance(ally) < 500) != null)
                                {
                                    if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                        ObjectManager.GetLocalPlayer().ChampionName == "Rakan" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                        ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                                        ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                    {
                                        if (EvadeTargetManager.Menu["whitelist"][
                                                ally.ChampionName.ToLower()]
                                            .As<MenuBool>().Enabled)
                                        {
                                            Bases.Champion.E.CastOnUnit(ally);
                                        }
                                    }
                                    if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                        ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                        ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                    {
                                        if (EvadeTargetManager.Menu["whitelist"][
                                                ally.ChampionName.ToLower()]
                                            .As<MenuBool>().Enabled)
                                        {
                                            Bases.Champion.W.CastOnUnit(ally);
                                        }
                                    }
                                }
                            }
                        }
                        if (ally.HasBuff("soulshackles"))
                        {
                            if ((ally.GetBuff("soulshackles")
                                     .EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {

                                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(ally);
                                    }
                                }
                                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(ally);
                                    }
                                }
                            }
                        }
                        if (ally.HasBuff("zedrdeathmark"))
                        {
                            if ((ally.GetBuff("zedrdeathmark")
                                     .EndTime -
                                 Game.ClockTime) *
                                1000 <= 300)
                            {

                                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(ally);
                                    }
                                }
                                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(ally);
                                    }
                                }
                            }
                        }

                        foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsValidTarget()))
                        {
                            switch (target.ChampionName)
                            {
                                case "Jax":
                                    if (Menu["Blockjax"]["BlockSpellE"] != null &&
                                        Menu["Blockjax"]["BlockSpellE"].Enabled)
                                    {
                                        if (target.HasBuff("jaxcounterstrike"))
                                        {

                                            var buff = target.GetBuff("JaxCounterStrike");

                                            if ((buff.EndTime - Game.ClockTime) * 1000 <= 650 &&
                                                ally.Distance(target) <= 350f)
                                            {

                                                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                                {

                                                    if (EvadeTargetManager.Menu["whitelist"][
                                                            ally.ChampionName.ToLower()]
                                                        .As<MenuBool>().Enabled)
                                                    {
                                                        Bases.Champion.E.CastOnUnit(ally);
                                                    }
                                                }
                                                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                                {
                                                    if (EvadeTargetManager.Menu["whitelist"][
                                                            ally.ChampionName.ToLower()]
                                                        .As<MenuBool>().Enabled)
                                                    {
                                                        Bases.Champion.W.CastOnUnit(ally);
                                                    }
                                                }
                                            }

                                        }
                                    }


                                    break;
                                case "Riven":
                                    if (Menu["Blockriven"]["BlockSpellQ"] != null &&
                                        Menu["Blockriven"]["BlockSpellQ"].Enabled)
                                    {
                                        if (Utils.GameTimeTickCount - RivenQTime <= 100 && RivenDashPos.IsValid() &&
                                            ally.Distance(target) <= RivenQRange)
                                        {
                                            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                                ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                                            {
                                                if (EvadeTargetManager.Menu["whitelist"][
                                                        ally.ChampionName.ToLower()]
                                                    .As<MenuBool>().Enabled)
                                                {

                                                    Bases.Champion.E.CastOnUnit(ally);
                                                }
                                            }
                                            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                                ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                                            {
                                                if (EvadeTargetManager.Menu["whitelist"][
                                                        ally.ChampionName.ToLower()]
                                                    .As<MenuBool>().Enabled)
                                                {
                                                    Bases.Champion.W.CastOnUnit(ally);
                                                }
                                            }

                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs Args)
        {
            if (Bases.Champion.RootMenu["wset"]["modes"].As<MenuList>().Value == 0)
            {
                if (ObjectManager.GetLocalPlayer().IsDead)
                {
                    return;
                }
                var ally = Args.Target as Obj_AI_Hero;
                if (ally != null)
                {

                    if (EvadeTargetManager.AttackMenu["Brian.EvadeTargetMenu.Turret"].Enabled)
                    {
                        if (ally.IsHero && ally.IsAlly)
                        {
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                            {
                                if (sender.IsTurret && Args.Target.IsAlly &&
                                    Args.Target.Distance(ObjectManager.GetLocalPlayer()) <
                                    Bases.Champion.E.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(Args.Target);
                                    }
                                }

                            }
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" || ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                            {
                                if (sender.IsTurret && Args.Target.IsAlly &&
                                    Args.Target.Distance(ObjectManager.GetLocalPlayer()) <
                                    Bases.Champion.W.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(Args.Target);
                                    }
                                }

                            }
                        }
                    }
                }

                var target = sender as Obj_AI_Hero;

                if (ally != null)
                {
                    if (ally.IsHero && ally.IsAlly)
                    {
                        if (Args.SpellData.Name.Contains("BasicAttack") && Args.Sender.IsHero &&
                            EvadeTargetManager.AttackMenu["Brian.EvadeTargetMenu.BAttack"]
                                .Enabled &&
                            ally.HealthPercent() <=
                            EvadeTargetManager.AttackMenu["Brian.EvadeTargetMenu.BAttackHpU"]
                                .Value)
                        {


                            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                            {
                                if (Args.Target.IsAlly &&
                                    Args.Target.Distance(ObjectManager.GetLocalPlayer()) <
                                    Bases.Champion.E.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                       Bases.Champion.E.CastOnUnit(Args.Target);
                                    }
                                }

                            }
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                            {
                                if (Args.Target.IsAlly &&
                                    Args.Target.Distance(ObjectManager.GetLocalPlayer()) <
                                   Bases.Champion.W.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(Args.Target);
                                    }
                                }

                            }
                        }
                    }
                }
                if (ally != null)
                {
                    if (Args.Sender.IsMinion)
                    {

                        if (ally.IsHero && ally.IsAlly)
                        {
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                            {
                                if (Args.Target.IsAlly && Args.Target.Distance(Bases.Champion.Player) <
                                   Bases.Champion.E.Range)
                                {

                                    if (
                                        EvadeTargetManager
                                            .AttackMenu["Brian.EvadeTargetMenu.Minion"]
                                            .Enabled && ally.HealthPercent() <=
                                        EvadeTargetManager
                                            .AttackMenu["Brian.EvadeTargetMenu.HP"]
                                            .Value)
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
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                            {
                                if (Args.Target.IsAlly && Args.Target.Distance(Bases.Champion.Player) <
                                    Bases.Champion.W.Range)
                                {

                                    if (
                                        EvadeTargetManager
                                            .AttackMenu["Brian.EvadeTargetMenu.Minion"]
                                            .Enabled && ally.HealthPercent() <=
                                        EvadeTargetManager
                                            .AttackMenu["Brian.EvadeTargetMenu.HP"]
                                            .Value)
                                    {
                                        if (EvadeTargetManager.Menu["whitelist"][
                                                ally.ChampionName.ToLower()]
                                            .As<MenuBool>().Enabled)
                                        {
                                            Bases.Champion.W.CastOnUnit(ally);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (Args.SpellData.Name.Contains("crit") && Args.Sender.IsHero &&
                      EvadeTargetManager.AttackMenu["Brian.EvadeTargetMenu.CAttack"].Enabled
                        && ally.HealthPercent() <= EvadeTargetManager
                            .AttackMenu["Brian.EvadeTargetMenu.CAttackHpU"].Value)
                    {
                        if (ally.IsHero)
                        {
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                            {
                                if (Args.Target.IsAlly && Args.Target.Distance(Bases.Champion.Player) <
                                    Bases.Champion.E.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.E.CastOnUnit(ally);
                                    }
                                }


                            }
                            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                            {
                                if (Args.Target.IsAlly && Args.Target.Distance(Bases.Champion.Player) <
                                   Bases.Champion.W.Range)
                                {
                                    if (EvadeTargetManager.Menu["whitelist"][
                                            ally.ChampionName.ToLower()]
                                        .As<MenuBool>().Enabled)
                                    {
                                        Bases.Champion.W.CastOnUnit(ally);
                                    }
                                }

                            }

                        }
                    }
                }

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
                if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                    ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                    ObjectManager.GetLocalPlayer().ChampionName == "Lulu" || ObjectManager.GetLocalPlayer().ChampionName == "Ivern" ||
                    ObjectManager.GetLocalPlayer().ChampionName == "Karma")

                {
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
                if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                    ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                    ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                {
                    if (Args.Target.Distance(Bases.Champion.Player) < Bases.Champion.W.Range)
                    {
                        if (ally != null && ally.IsHero && ally.IsAlly)
                        {
                            if (EvadeTargetManager.Menu["whitelist"][
                                    ally.ChampionName.ToLower()]
                                .As<MenuBool>().Enabled)
                            {
                                Bases.Champion.W.CastOnUnit(Args.Target);
                            }
                        }
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
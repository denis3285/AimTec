using Aimtec.SDK.Damage;

namespace Potato_AIO.WDodge
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

    using Potato_AIO;

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

        }


        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs Args)
        {
            if (!EvadeTargetManager.Menu["Brian.EvadeTargetMenu.EvadeTargetW"].Enabled)
            {

                return;
            }

            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }
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
            if (Args.Target.IsMe)
            {
                if (target.Distance(ObjectManager.GetLocalPlayer()) < 525)
                {
                    Potato_AIO.Bases.Champion.W.Cast(target);
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
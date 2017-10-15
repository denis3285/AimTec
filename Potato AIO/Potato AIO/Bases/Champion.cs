using System;
using System.Linq;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util.Cache;
using Potato_AIO.Champions;
using Potato_AIO;
using Potato_AIO.Base;
using Potato_AIO.Handlers;

namespace Potato_AIO.Bases
{
    using Aimtec;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Orbwalking;

    using Spell = Aimtec.SDK.Spell;

    internal abstract class Champion
    {
        private IOrderedEnumerable<Obj_AI_Hero> bestAllies;

        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        internal static Menu ComboMenu { get; set; } = default(Menu);

        internal static Menu FarmMenu { get; set; } = default(Menu);
        internal static Menu EvadeMenu { get; set; }

        internal static Menu KillstealMenu { get; set; } = default(Menu);

        internal static Spell E { get; set; } = default(Spell);

        internal static Menu HarassMenu { get; set; } = default(Menu);

        internal static Menu DrawMenu { get; set; } = default(Menu);
        internal static Menu WhiteList { get; set; } = default(Menu);

        internal static Spell Q { get; set; } = default(Spell);

        internal static Spell R { get; set; } = default(Spell);
        internal static Spell W2 { get; set; } = default(Spell);
        internal static Spell Q2 { get; set; } = default(Spell);
        internal static Spell E2 { get; set; } = default(Spell);
        internal static Menu RootMenu { get; set; } = default(Menu);

        internal static Spell W { get; set; } = default(Spell);
        internal static Spell Flash { get; set; } = default(Spell);

        internal void Initiate()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        internal virtual void OnGameUpdate()
        {
            if (Program.Player.IsDead || MenuGUI.IsChatOpen()) return;
            Killsteal();
            SemiR();
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.None: break;
                case OrbwalkingMode.Combo:
                    this.Combo();
                    break;
                case OrbwalkingMode.Mixed:
                    this.Harass();
                    break;
                case OrbwalkingMode.Laneclear:
                    this.Farming();
                    break;
                case OrbwalkingMode.Lasthit:
                    this.LastHit();
                    break;
                case OrbwalkingMode.Freeze: break;
                case OrbwalkingMode.Custom: break;
            }
           

        }


        internal virtual void SetEvents()
        {
            Game.OnUpdate += this.OnGameUpdate;
            Render.OnPresent += Drawing;
            Orbwalker.Implementation.PreAttack += OnPreAttack;
            Orbwalker.Implementation.PostAttack += PostAttack;
            SpellBook.OnCastSpell += OnCastSpell;
            ZLib.OnPredictDamage += ZLib_OnPredictDamage;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            BuffManager.OnRemoveBuff += uh;
            BuffManager.OnAddBuff += ah;

        }

      
        internal virtual void ah(Obj_AI_Base sender, Buff buff)
        {
        }

        internal virtual void uh(Obj_AI_Base sender, Buff buff)
        {
        }

        private static void ZLib_OnPredictDamage(Base.Unit hero, PredictDamageEventArgs args)
        {


            if (Player.ChampionName == "Tryndamere")
            {
                if (hero.Instance.IsEnemy)
                {
                    return;
                }
                if (hero.IncomeDamage < 0)
                {

                    Helpers.ResetIncomeDamage(hero);
                }


                if (!hero.Instance.IsValidTarget(float.MaxValue, true))
                {
                    Helpers.ResetIncomeDamage(hero);
                }

                if (hero.Instance.HasBuffOfType(BuffType.Invulnerability))
                {
                    args.NoProcess = true;
                }


                var objShop = ObjectManager.Get<GameObject>()
                    .FirstOrDefault(x => x.Type == GameObjectType.obj_Shop && x.Team == hero.Instance.Team);

                if (objShop != null
                    && objShop.Distance(hero.Instance.ServerPosition) <= 1250)
                {
                    args.NoProcess = true;
                }

                if (args.HpInstance.PredictedDmg * 2 >= hero.Instance.Health && R.Ready)
                {

                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled && hero.Instance.IsMe)
                    {
                        if (RootMenu["combo"]["rset"]["user"].Enabled)
                        {
                            R.Cast();
                        }
                        
                    }
                }
            }
        }

        internal virtual void PostAttack(object sender, PostAttackEventArgs e)
        {
            
        }

        internal virtual void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {

        }


        internal virtual void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {

        }


        internal virtual void OnPreAttack(object sender, PreAttackEventArgs e)
        {

        }

        internal virtual void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {

        }

        public void Drawing()
        {
            Drawings();
        }


        protected abstract void Combo();

        protected abstract void SemiR();
        protected abstract void Farming();
        protected abstract void LastHit();

        protected abstract void Drawings();
        protected abstract void Killsteal();
        protected abstract void Harass();

        protected abstract void SetMenu();

        protected abstract void SetSpells();
    }
}
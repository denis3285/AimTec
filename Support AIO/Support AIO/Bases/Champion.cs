using System;
using System.Linq;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util.Cache;
using Support_AIO.SpellBlocking;
using Support_AIO.Champions;
using Support_AIO;

namespace Support_AIO.Bases
{
    using Aimtec;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Orbwalking;
    using Base;
    using Handlers;
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
                case OrbwalkingMode.Lasthit: break;
                case OrbwalkingMode.Freeze: break;
                case OrbwalkingMode.Custom: break;
            }
            if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Karma")
            {
                bestAllies = GameObjects.AllyHeroes
                    .Where(t =>
                        t.Distance(ObjectManager.GetLocalPlayer()) < Champion.E.Range)
                    .OrderBy(x => x.Health);
            }
            if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                ObjectManager.GetLocalPlayer().ChampionName == "Taric")

            {
                bestAllies = GameObjects.AllyHeroes
                    .Where(t =>
                        t.Distance(ObjectManager.GetLocalPlayer()) < Champion.W.Range)
                    .OrderBy(x => x.Health);
            }
            if (bestAllies != null)
            {
                foreach (var t in bestAllies)
                {
                    if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Karma")
                    {

                        if (t != null && EvadeTargetManager.Menu["Brian.EvadeTargetMenu.CC"]
                                .Enabled &&
                            EvadeTargetManager.Menu["whitelist"][t.ChampionName.ToLower()]
                                .As<MenuBool>().Enabled)
                        {

                            if (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                                t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                                t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                                t.HasBuffOfType(BuffType.Suppression))
                            {

                                E.CastOnUnit(t);
                            }
                        }
                    }
                    if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Taric" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "TahmKench")

                    {
                        if (t != null && EvadeTargetManager.Menu["Brian.EvadeTargetMenu.CC"]
                                .Enabled &&
                            EvadeTargetManager.Menu["whitelist"][t.ChampionName.ToLower()]
                                .As<MenuBool>().Enabled)
                        {
                            if (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                                t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                                t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                                t.HasBuffOfType(BuffType.Suppression))
                            {
                                W.CastOnUnit(t);
                            }
                        }
                    }
                }
                foreach (var t in bestAllies)
                {
                    if (ObjectManager.GetLocalPlayer().ChampionName == "Janna" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Rakan" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Lulu" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Karma")
                    {
                        if (t != null &&
                            EvadeTargetManager.Menu["whitelist"][t.ChampionName.ToLower()]
                                .As<MenuBool>().Enabled)
                        {
                            if (t.HasBuffOfType(BuffType.Poison))
                            {

                                E.CastOnUnit(t);
                            }
                        }
                    }
                    if (ObjectManager.GetLocalPlayer().ChampionName == "Lux" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Sona" ||
                        ObjectManager.GetLocalPlayer().ChampionName == "Taric")

                    {
                        if (t != null &&
                            EvadeTargetManager.Menu["whitelist"][t.ChampionName.ToLower()]
                                .As<MenuBool>().Enabled)
                        {
                            if (t.HasBuffOfType(BuffType.Poison))
                            {

                                W.CastOnUnit(t);
                            }
                        }
                    }
                }
            }

        }


        internal virtual void SetEvents()
        {
            Game.OnUpdate += this.OnGameUpdate;
            Render.OnPresent += Drawing;
            Orbwalker.Implementation.PreAttack += OnPreAttack;
            SpellBook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            ZLib.OnPredictDamage += ZLib_OnPredictDamage;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        internal virtual void OnGapcloser(Obj_AI_Hero target, GapcloserArgs Args)
        {
           
        }

        private static void ZLib_OnPredictDamage(Base.Unit hero, PredictDamageEventArgs args)
        {
            if (Player.ChampionName == "Lulu")
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
                    if (RootMenu["combo"]["autor"].Enabled)
                    {

                        R.CastOnUnit(hero.Instance);

                    }
                }
            }

            if (Player.ChampionName == "Zilean")
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
              
                if (args.HpInstance.PredictedDmg*2 >= hero.Instance.Health && R.Ready)
                {
                  
                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled)
                    {

                        R.CastOnUnit(hero.Instance);
                    }
                }
            }
            if (Player.ChampionName == "Janna" || Player.ChampionName == "Lulu" || Player.ChampionName == "Rakan" || Player.ChampionName == "Karma")
            {

                if (Bases.Champion.RootMenu["wset"]["modes"].As<MenuList>().Value == 1)
                {
                    if (hero.Instance.IsEnemy)
                    {
                        return;
                    }
                    if (hero.Attacker.IsAlly)
                    {
                        return;
                    }
                    if (hero.MinionDamage > 0)
                    {
                        if (hero.AbilityDamage == 0 && hero.BuffDamage == 0 && hero.ItemDamage == 0 &&
                            hero.TowerDamage == 0 && hero.TroyDamage == 0)
                        {
                            return;
                        }
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

                    if (hero.Instance.HasBuffOfType(BuffType.PhysicalImmunity)
                        && args.HpInstance.EventType == EventType.AutoAttack)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuffOfType(BuffType.SpellImmunity)
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("sivire")
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("bansheesviel")
                        && args.HpInstance.EventType == EventType.Spell)
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
                    
                  
                      
                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled)
                    {

                        E.CastOnUnit(hero.Instance);
                    }

                }
            }
            if (Player.ChampionName == "Sona")
            {

                if (Bases.Champion.RootMenu["wset"]["modes"].As<MenuList>().Value == 1)
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

                    if (hero.Instance.HasBuffOfType(BuffType.PhysicalImmunity)
                        && args.HpInstance.EventType == EventType.AutoAttack)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuffOfType(BuffType.SpellImmunity)
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("sivire")
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("bansheesviel")
                        && args.HpInstance.EventType == EventType.Spell)
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



                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled && hero.Instance.Distance(Player) < 400)
                    {

                        W2.CastOnUnit(hero.Instance);
                    }

                }
            }
            if (Player.ChampionName == "TahmKench")
            {

                if (Bases.Champion.RootMenu["wset"]["modes"].As<MenuList>().Value == 1)
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

                    if (hero.Instance.HasBuffOfType(BuffType.PhysicalImmunity)
                        && args.HpInstance.EventType == EventType.AutoAttack)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuffOfType(BuffType.SpellImmunity)
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("sivire")
                        && args.HpInstance.EventType == EventType.Spell)
                    {
                        args.NoProcess = true;
                    }

                    if (hero.Instance.HasBuff("bansheesviel")
                        && args.HpInstance.EventType == EventType.Spell)
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


                    if (args.HpInstance.EventType == EventType.AutoAttack)
                    {
                        return;
                    }
                    if (args.HpInstance.EventType == EventType.MinionAttack)
                    {
                        return;
                    }
                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled && hero.Instance.Distance(Player) < 400)
                    {
          
                        W.CastOnUnit(hero.Instance);
                    }
                    if (ZLib.Menu["whitelist"][hero.Instance.ChampionName.ToLower()].Enabled && hero.Instance.Distance(Player) < 400)
                    {

                       E.Cast();
                    }

                }
            }
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

     
        protected abstract void Drawings();
        protected abstract void Killsteal();
        protected abstract void Harass();

        protected abstract void SetMenu();

        protected abstract void SetSpells();
    }
}
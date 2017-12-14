using Aimtec.SDK.Util;

namespace Fiora_By_Kornis.SpellBlocking
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

    internal class EvadeOthers
    {
        public static Menu Menu;
        private static int RivenQTime;
        private static float RivenQRange;
        private static Vector2 RivenDashPos;

        internal static void Attach(Menu evadeMenu)
        {
            Menu = new Menu("EvadeOthers", "Evade Others")
            {
                new MenuSeperator("MadeByNightMoon", "Made by NightMoon"),
                new MenuSeperator("meow"),
                new MenuBool("EnabledWDodge", "Enabled W Block Spell"),
                new MenuSlider("EnabledWHP", "When Player HealthPercent <= x%", 100)
            };

            foreach (
                var hero in
                GameObjects.EnemyHeroes.Where(
                    i => BlockSpellDataBase.Spells.Any(a => a.ChampionName == i.ChampionName)))
            {
                var heroMenu = new Menu("Block" + hero.ChampionName.ToLower(), hero.ChampionName);
                Menu.Add(heroMenu);
            }

            foreach (
                var spell in
                BlockSpellDataBase.Spells.Where(
                    x =>
                        ObjectManager.Get<Obj_AI_Hero>().Any(
                            a => a.IsEnemy &&
                                 string.Equals(a.ChampionName, x.ChampionName,
                                     StringComparison.CurrentCultureIgnoreCase))))
            {
                var heroMenu = Menu["Block" + spell.ChampionName.ToLower()].As<Menu>();
                heroMenu.Add(new MenuBool("BlockSpell" + spell.SpellSlot, spell.ChampionName + " " + spell.SpellSlot));
            }
            evadeMenu.Add(Menu);

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            Dash.HeroDashed += OnDash;
        }

        private static void OnUpdate()
        {
            if (ObjectManager.GetLocalPlayer().IsDead || !ObjectManager.GetLocalPlayer().SpellBook
                    .CanUseSpell(SpellSlot.W) ||
                !Menu["EnabledWDodge"].Enabled)
            {
                return;
            }
            if (ObjectManager.GetLocalPlayer().HasBuff("karthusfallenonetarget"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("karthusfallenonetarget").EndTime - Game.ClockTime) * 1000 <= 300)
                {
                    CastW();
                }
            }

            if (ObjectManager.GetLocalPlayer().HasBuff("kaynrhost"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("kaynrhost").EndTime - Game.ClockTime) * 1000 <= 100)
                {
                    CastW();
                }
            }
            if (ObjectManager.GetLocalPlayer().HasBuff("vladimirhemoplaguedebuff"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("vladimirhemoplaguedebuff").EndTime - Game.ClockTime) *
                    1000 <= 300)
                {
                    CastW();
                }
            }

            if (ObjectManager.GetLocalPlayer().HasBuff("nautilusgrandlinetarget"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("nautilusgrandlinetarget").EndTime - Game.ClockTime) *
                    1000 <= 300)
                {
                    CastW();
                }
            }
            if (ObjectManager.GetLocalPlayer().HasBuff("nocturneparanoiadash"))
            {
                if (GameObjects.EnemyHeroes.FirstOrDefault(
                        x =>
                            !x.IsDead && x.ChampionName.ToLower() == "nocturne" &&
                            x.Distance(ObjectManager.GetLocalPlayer()) < 500) != null)
                {
                    CastW();
                }
            }

            if (ObjectManager.GetLocalPlayer().HasBuff("soulshackles"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("soulshackles").EndTime - Game.ClockTime) * 1000 <= 300)
                {
                    CastW();
                }
            }

            if (ObjectManager.GetLocalPlayer().HasBuff("zedrdeathmark"))
            {
                if ((ObjectManager.GetLocalPlayer().GetBuff("zedrdeathmark").EndTime - Game.ClockTime) * 1000 <= 300)
                {
                    CastW();
                }
            }



            foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsValidTarget()))
            {

                switch (target.ChampionName)
                {
                    case "Rammus":
                        if (Menu["Blockrammus"]["BlockSpellQ"] != null && Menu["Blockrammus"]["BlockSpellQ"].Enabled)
                        {
                            if (target.HasBuff("PowerBall"))
                            {
                         
                                if (target.Distance(ObjectManager.GetLocalPlayer()) <= 300)
                                {
                                    
                                    CastW();
                                }
                                
                            }
                        }
                        break;
                    case "Jax":
                        if (Menu["Blockjax"]["BlockSpellE"] != null && Menu["Blockjax"]["BlockSpellE"].Enabled)
                        {
                            if (target.HasBuff("jaxcounterstrike"))
                            {
                                
                                var buff = target.GetBuff("JaxCounterStrike");
                             
                                if ((buff.EndTime - Game.ClockTime) * 1000 <= 650 &&
                                    ObjectManager.GetLocalPlayer().Distance(target) <= 350f)
                                {
                                
                                    CastW();
                                }
                            }
                        }
                        break;
                    case "Riven":
                      
                        if (Menu["Blockriven"]["BlockSpellQ"] != null && Menu["Blockriven"]["BlockSpellQ"].Enabled)
                        {
                            
                            if (Utils.GameTimeTickCount - RivenQTime <= 100 && RivenDashPos.IsValid() &&
                                ObjectManager.GetLocalPlayer().Distance(target) <= RivenQRange)
                            {
                               
                                CastW();
                            }
                        }
                        break;
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs Args)
        {
            if (ObjectManager.GetLocalPlayer().IsDead || !ObjectManager.GetLocalPlayer().SpellBook.CanUseSpell(SpellSlot.W) ||
                !Menu["EnabledWDodge"].Enabled)
            {
                return;
            }

            var target = sender as Obj_AI_Hero;

            if (target != null && target.Team != ObjectManager.GetLocalPlayer().Team &&
                !string.IsNullOrEmpty(Args.SpellData.Name) &&
                Args.SpellData.Name != "RenektonCleave" && Args.SpellData.Name != "AlistarE" &&
                Args.SpellData.Name != "ChogathEAttack" &&
                Args.SpellData.Name != "FerociousHowl" && Args.SpellData.Name != "NasusE" &&
                Args.SpellData.Name != "GarenQAttack" &&
                !Args.SpellData.Name.Contains("BasicAttack"))
            {
                if (target.ChampionName == "Kayn")
                {
                    var spell =
                        BlockSpellDataBase.Spells.Where(
                            x =>
                                string.Equals(x.ChampionName, target.ChampionName,
                                    StringComparison.CurrentCultureIgnoreCase) &&
                                Menu["Block" + target.ChampionName.ToLower()]["BlockSpell" + x.SpellSlot.ToString()] !=
                                null &&
                                Menu["Block" + target.ChampionName.ToLower()]["BlockSpell" + x.SpellSlot.ToString()]
                                    .Enabled).ToArray();

                    if (spell.Any())
                    {
                        foreach (var x in spell)
                        {
                            switch (x.ChampionName)
                            {
                                case "Kayn":
                                    if (x.SpellSlot == SpellSlot.R)
                                    {
                                        if (ObjectManager.GetLocalPlayer().HasBuff("kaynrhost"))
                                        {

                                            DelayAction.Queue(300, () =>
                                            {
                                                CastW("Kayn", x.SpellSlot);
                                            });
                                        }

                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            if (target == null || target.Team == ObjectManager.GetLocalPlayer().Team || !target.IsValid ||
                Args.Target == null || string.IsNullOrEmpty(Args.SpellData.Name) ||
                Args.SpellData.Name == "RenektonCleave" || Args.SpellData.Name == "AlistarE" ||
                Args.SpellData.Name == "ChogathEAttack" || Args.SpellData.Name == "Rupture" ||
                Args.SpellData.Name == "FerociousHowl" || Args.SpellData.Name == "NasusE" || Args.SpellData.Name == "GarenQAttack" ||
                Args.SpellData.Name.Contains("BasicAttack"))
            {
                return;
            }
        

            var spells =
                BlockSpellDataBase.Spells.Where(
                    x =>
                        string.Equals(x.ChampionName, target.ChampionName, StringComparison.CurrentCultureIgnoreCase) &&
                         Menu["Block" + target.ChampionName.ToLower()]["BlockSpell" + x.SpellSlot.ToString()] != null &&
                        Menu["Block" + target.ChampionName.ToLower()]["BlockSpell" + x.SpellSlot.ToString()].Enabled).ToArray();

            if (spells.Any())
            {
                foreach (var x in spells)
                {
                    switch (x.ChampionName)
                    {


                        case "Alistar":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (target.Distance(ObjectManager.GetLocalPlayer()) <= 350)
                                {
                                    CastW("Alistar", x.SpellSlot);
                                }
                            }

                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Alistar", x.SpellSlot);
                                }
                            }
                            break;

                        case "Blitzcrank":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "PowerFistAttack")
                                {
                                    CastW("Blitzcrank", x.SpellSlot);
                                }
                            }
                            break;
                        case "Leona":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "LeonaShieldOfDaybreakAttack")
                                {
                                    CastW("Leona", x.SpellSlot);
                                }
                            }

                            break;
                        case "Poppy":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Poppy", x.SpellSlot);
                                }
                            }
                            break;
                        case "Chogath":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Chogath", x.SpellSlot);
                                }
                            }
                            break;
                        case "Vladimir":
                            if (x.SpellSlot == SpellSlot.Q && Args.SpellData.Name != "VladimirHemoplague")
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Vladimir", x.SpellSlot);
                                }
                            }
                            break;
                        case "Darius":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Darius", x.SpellSlot);
                                }
                            }
 
                            break;
                        case "Elise":
                            if (x.SpellSlot == SpellSlot.Q && Args.SpellData.Name == "EliseHumanQ")
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Elise", x.SpellSlot);
                                }
                            }
                            break;
                        case "FiddleSticks":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("FiddleSticks", x.SpellSlot);
                                }
                            }
                            break;
                        case "Rammus":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Rammus", x.SpellSlot);
                                }
                            }
                            break;


                        case "Gangplank":
                            if (x.SpellSlot == SpellSlot.Q && Args.SpellData.Name == "Parley")
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Gangplank", x.SpellSlot);
                                }
                            }
                            break;
                        case "Garen":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Garen", x.SpellSlot);
                                }
                            }
                            break;
                        case "Skarner":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Skarner", x.SpellSlot);
                                }
                            }
                            break;
                        case "Hecarim":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "HecarimRampAttack")
                                {
                                    CastW("Hecarim", x.SpellSlot);
                                }
                            }
                            break;
                        case "Irelia":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Irelia", x.SpellSlot);
                                }
                            }
                            break;

                        case "Jarvan":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Jarvan", x.SpellSlot);
                                }
                            }
                            break;
                        case "Kalista":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (ObjectManager.GetLocalPlayer().HasBuff("kalistaexpungemarker") &&
                                    ObjectManager.GetLocalPlayer().Distance(target) <= 950f)
                                {
                                    CastW("Kalista", x.SpellSlot);
                                }
                            }
                            break;
                        case "Kayle":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Kayle", x.SpellSlot);
                                }
                            }
                            break;
                        case "LeeSin":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("LeeSin", x.SpellSlot);
                                }
                            }
                            break;
                        case "Brand":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Brand", x.SpellSlot);
                                }
                            }
                                      
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Brand", x.SpellSlot);
                                }
                            }
                            break;
                        case "Lissandra":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Lissandra", x.SpellSlot);
                                }
                            }
                            break;
                        case "Malzahar":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Mordekaiser", x.SpellSlot);
                                }
                            }
                            break;
                        case "Mordekaiser":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "MordekaiserQAttack2")
                                {
                                    CastW("Mordekaiser", x.SpellSlot);
                                }
                            }

                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Mordekaiser", x.SpellSlot);
                                }
                            }
                            break;
                        case "Nasus":
                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Nasus", x.SpellSlot);
                                }
                            }
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "NasusQAtttack")
                                {
                                    CastW("Nasus", x.SpellSlot);
                                }
                            }
                            break;

                        case "Olaf":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Olaf", x.SpellSlot);
                                }
                            }
                            break;
                        case "Pantheon":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "PantheonQ")
                                {
                                    
                                    CastW("Pantheon", x.SpellSlot);
                                }
                            }

                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {                                 
                                    CastW("Pantheon", x.SpellSlot);
                                }
                            }
                            break;
                        case "Renekton":
                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "RenektonExecute")
                                {
                                    CastW("Renekton", x.SpellSlot);
                                }
                            }
                            break;
                     
                        case "Rengar":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (ObjectManager.GetLocalPlayer().Distance(target) <= 300 && Args.Target.IsMe)
                                {
                                    CastW("Rengar", x.SpellSlot);
                                }
                            }
                            break;
                        case "Ryze":
                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Ryze", x.SpellSlot);
                                }
                            }
                            break;
                        case "Singed":
                            if (x.SpellSlot == SpellSlot.E)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Singed", x.SpellSlot);
                                }
                            }
                            break;
                        case "Syndra":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "SyndraR")
                                {
                                    CastW("Syndra", x.SpellSlot);
                                }
                            }
                            break;
                        case "TahmKench":
                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("TahmKench", x.SpellSlot);
                                }
                            }
                            break;
                        case "Tristana":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "TristanaR")
                                {
                                    CastW("Tristana", x.SpellSlot);
                                }
                            }
                            break;
                        case "Trundle":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Trundle", x.SpellSlot);
                                }
                            }
                            break;
                        case "TwistedFate":
                            if (Args.SpellData.Name.Contains("attack") && Args.Target.IsMe &&
                                target.Buffs.Any(
                                    buff =>
                                        buff.Name == "BlueCardAttack" || buff.Name == "GoldCardAttack" ||
                                        buff.Name == "RedCardAttack"))
                            {
                                CastW("TwistedFate", x.SpellSlot);
                            }
                            break;
                        case "Veigar":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "VeigarPrimordialBurst")
                                {
                                    CastW("Veigar", x.SpellSlot);
                                }
                            }
                            break;
                        case "Vi":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Vi", x.SpellSlot);
                                }
                            }
                            break;
                        case "Volibear":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Volibear", x.SpellSlot);
                                }
                            }

                            if (x.SpellSlot == SpellSlot.W)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Volibear", x.SpellSlot);
                                }
                            }
                            break;
                        case "Warwick":
                            if (x.SpellSlot == SpellSlot.R)
                            {
                                if (Args.Target.IsMe)
                                {
                                    CastW("Warwick", x.SpellSlot);
                                }
                            }
                            break;
                        case "XinZhao":
                            if (x.SpellSlot == SpellSlot.Q)
                            {
                                if (Args.Target.IsMe && Args.SpellData.Name == "XenZhaoThrust3")
                                {
                                    CastW("XinZhao", x.SpellSlot);
                                }
                            }
                            break;
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
                    RivenQRange = riven.HasBuff("RivenFengShuiEngine") ? 250f : 200f;
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

        private static void CastW()
        {
            if (ObjectManager.GetLocalPlayer().IsDead || !ObjectManager.GetLocalPlayer().SpellBook.CanUseSpell(SpellSlot.W))
            {
                return;
            }

            var target =
                GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.Distance(ObjectManager.GetLocalPlayer()) <= 750f)
                    .OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer()))
                    .FirstOrDefault();

            ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.W, target?.Position ?? Game.CursorPos);
        }

        private static void CastW(string name, SpellSlot spellslot)
        {
            if (ObjectManager.GetLocalPlayer().IsDead || !ObjectManager.GetLocalPlayer().SpellBook.CanUseSpell(SpellSlot.W))
            {
                return;
            }

            if (Menu["Block" + name.ToLower()]["BlockSpell" + spellslot.ToString()] != null &&
                Menu["Block" + name.ToLower()]["BlockSpell" + spellslot.ToString()].Enabled)
            {
                var target =
                   GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.Distance(ObjectManager.GetLocalPlayer()) <= 750f)
                        .OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer()))
                        .FirstOrDefault();

                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.W, target?.Position ?? Game.CursorPos);
            }
        }
    }
}
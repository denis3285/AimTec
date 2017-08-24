using System.Drawing;
using Support_AIO.Bases;

namespace Support_AIO.BraumWE
{
    #region

    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal class EvadeTargetManager
    {

        public static Menu Menu, AttackMenu, SpellMenu, WhiteList;
        public static readonly List<SpellData> Spells = new List<SpellData>();
        private static readonly List<Targets> DetectedTargets = new List<Targets>();
        private static IOrderedEnumerable<Obj_AI_Hero> bestAllies;

        public static void Attach(Menu mainMenu)
        {
            Menu = new Menu("EvadeTargetMenu", "Misc.")
            {
                new MenuSeperator("Brian.EvadeTargetMenu.Credit", "Made by Brian"),
                new MenuBool("Brian.EvadeTargetMenu.EvadeTargetW", "Use Shielding"),

            };
            WhiteList = new Menu("whitelist", "Shielding Whitelist");
            {
                foreach (var target in GameObjects.AllyHeroes)
                {
                    WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Enable: " + target.ChampionName));
                }
            }
            Menu.Add(WhiteList);

            InitSpells();
  
            mainMenu.Add(Menu);


          
           
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
        }

        private static void InitSpells()
        {
            Spells.Add(
                new SpellData
                { ChampionName = "Ahri", SpellNames = new[] { "ahrifoxfiremissiletwo" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData
                { ChampionName = "Ahri", SpellNames = new[] { "ahritumblemissile" }, Slot = SpellSlot.R });
            Spells.Add(
                new SpellData { ChampionName = "Akali", SpellNames = new[] { "akalimota" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData { ChampionName = "Anivia", SpellNames = new[] { "frostbite" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Annie", SpellNames = new[] { "disintegrate" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Caitlyn",
                    SpellNames = new[] { "caitlynaceintheholemissile" },
                    Slot = SpellSlot.R
                });
            Spells.Add(
                new SpellData
                { ChampionName = "Cassiopeia", SpellNames = new[] { "cassiopeiae" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Elise", SpellNames = new[] { "elisehumanq" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "" +
                                   "Ezreal" +
                                   "",
                    SpellNames = new[] { "ezrealarcaneshiftmissile" },
                    Slot = SpellSlot.E
                });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "FiddleSticks",
                    SpellNames = new[] { "fiddlesticksdarkwind", "fiddlesticksdarkwindmissile" },
                    Slot = SpellSlot.E
                });
            Spells.Add(
                new SpellData { ChampionName = "Gangplank", SpellNames = new[] { "parley" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData { ChampionName = "Janna", SpellNames = new[] { "sowthewind" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData { ChampionName = "Kassadin", SpellNames = new[] { "nulllance" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Katarina",
                    SpellNames = new[] { "katarinaq", "katarinaqmis" },
                    Slot = SpellSlot.Q
                });
            Spells.Add(
                new SpellData
                { ChampionName = "Kayle", SpellNames = new[] { "judicatorreckoning" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Leblanc",
                    SpellNames = new[] { "leblancchaosorb", "leblancchaosorbm" },
                    Slot = SpellSlot.Q
                });
            Spells.Add(new SpellData { ChampionName = "Lulu", SpellNames = new[] { "luluw" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData
                { ChampionName = "Malphite", SpellNames = new[] { "seismicshard" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "MissFortune",
                    SpellNames = new[] { "missfortunericochetshot", "missFortunershotextra" },
                    Slot = SpellSlot.Q
                });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Nami",
                    SpellNames = new[] { "namiwenemy", "namiwmissileenemy" },
                    Slot = SpellSlot.W
                });
            Spells.Add(
                new SpellData { ChampionName = "Nunu", SpellNames = new[] { "iceblast" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Pantheon", SpellNames = new[] { "pantheonq" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Ryze",
                    SpellNames = new[] { "spellflux", "spellfluxmissile" },
                    Slot = SpellSlot.E
                });
            Spells.Add(
                new SpellData { ChampionName = "Shaco", SpellNames = new[] { "twoshivpoison" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Shen", SpellNames = new[] { "shenvorpalstar" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData { ChampionName = "Sona", SpellNames = new[] { "sonaqmissile" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData { ChampionName = "Swain", SpellNames = new[] { "swaintorment" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Syndra", SpellNames = new[] { "syndrar" }, Slot = SpellSlot.R });
            Spells.Add(
                new SpellData { ChampionName = "Taric", SpellNames = new[] { "dazzle" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData { ChampionName = "Teemo", SpellNames = new[] { "blindingdart" }, Slot = SpellSlot.Q });
            Spells.Add(
                new SpellData
                { ChampionName = "Tristana", SpellNames = new[] { "detonatingshot" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData
                { ChampionName = "TwistedFate", SpellNames = new[] { "bluecardattack" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData
                { ChampionName = "TwistedFate", SpellNames = new[] { "goldcardattack" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData
                { ChampionName = "TwistedFate", SpellNames = new[] { "redcardattack" }, Slot = SpellSlot.W });
            Spells.Add(
                new SpellData
                {
                    ChampionName = "Urgot",
                    SpellNames = new[] { "urgotheatseekinghomemissile" },
                    Slot = SpellSlot.Q
                });
            Spells.Add(
                new SpellData { ChampionName = "Vayne", SpellNames = new[] { "vaynecondemn" }, Slot = SpellSlot.E });
            Spells.Add(
                new SpellData
                { ChampionName = "Veigar", SpellNames = new[] { "veigarprimordialburst" }, Slot = SpellSlot.R });
            Spells.Add(
                new SpellData
                { ChampionName = "Viktor", SpellNames = new[] { "viktorpowertransfer" }, Slot = SpellSlot.Q });
        }


        private static void OnCreate(GameObject sender)
        {

            var missile = sender as MissileClient;
            if (missile == null)
            {
                return;
            }
            if (!missile.SpellCaster.IsValid ||
                missile.SpellCaster.Team == ObjectManager.GetLocalPlayer().Team)
            {
                return;
            }

            bestAllies = GameObjects.AllyHeroes
                .Where(t =>
                    t.Distance(ObjectManager.GetLocalPlayer()) < Bases.Champion.W.Range)
                .OrderBy(x => x.Health);



            var hero = missile.SpellCaster as Obj_AI_Hero;
            if (hero == null)
            {
                return;
            }
            var aaa = missile.Target as Obj_AI_Hero;
            var spellData =
                Spells.FirstOrDefault(
                    i =>
                        i.SpellNames.Contains(missile.SpellData.Name.ToLower()));

            if (spellData == null)
            {
                return;
            }

            if (aaa == null)
            {
                return;
            }

            foreach (var ally in bestAllies)
            {
                if (aaa == ally)
                {

                    if (EvadeTargetManager.Menu["whitelist"][
                                ally.ChampionName.ToLower()]
                            .As<MenuBool>().Enabled &&
                        aaa.Distance(ObjectManager.GetLocalPlayer()) < Bases.Champion.W.Range)
                    {

                        if (ObjectManager.GetLocalPlayer().HealthPercent() >=
                            Support_AIO.Bases.Champion.RootMenu["wset"]["health"].As<MenuSlider>().Value)
                        {

                            Bases.Champion.W.CastOnUnit(aaa);
                            if (ObjectManager.GetLocalPlayer().HealthPercent() >=
                                Support_AIO.Bases.Champion.RootMenu["wset"]["health"].As<MenuSlider>().Value)
                            {


                                if (
                                    !ObjectManager.GetLocalPlayer().IsDashing() &&
                                    
                                    !ObjectManager.GetLocalPlayer().HasBuff("braumeshieldbuff"))
                                {

                                    Support_AIO.Bases.Champion.E.Cast(hero.ServerPosition);


                                }
                                if (!ObjectManager.GetLocalPlayer().IsDashing() &&
                                    ObjectManager.GetLocalPlayer().Distance(ally) < 200 &&
                                    !ObjectManager.GetLocalPlayer().HasBuff("braumeshieldbuff"))
                                {
                                    Support_AIO.Bases.Champion.E.Cast(hero.ServerPosition);

                                }

                            }
                        }
                    }

                }


            }
        }



        private static void OnDestroy(GameObject sender)
        {
            if (!sender.IsValid)
            {
                return;
            }
            var missile = sender as MissileClient;
            if (missile == null)
            {
                return;
            }
 

            

            if (missile.SpellCaster.IsValid && missile.SpellCaster.Team != ObjectManager.GetLocalPlayer().Team)
            {
                DetectedTargets.RemoveAll(i => i.Obj.NetworkId == missile.NetworkId);
            }
        }

        public class SpellData
        {
            public string ChampionName;
            public SpellSlot Slot;
            public string[] SpellNames = { };

            public string MissileName => SpellNames.First();
        }

        private class Targets
        {
            public MissileClient Obj;
            public Vector3 Start;
        }
    }
}

using System.Drawing;

namespace Fiora_By_Kornis.SpellBlocking
{
    #region

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fiora_By_Kornis;
    #endregion

    internal class EvadeTargetManager
    {
        public static Menu Menu, AttackMenu, SpellMenu;
        public static readonly List<SpellData> Spells = new List<SpellData>();
        private static readonly List<Targets> DetectedTargets = new List<Targets>();

        public static void Attach(Menu mainMenu)
        {
            Menu = new Menu("EvadeTargetMenu", "Evade Targets")
            {
                new MenuSeperator("Brian.EvadeTargetMenu.Credit", "Made by Brian"),
                new MenuSeperator("Brian.EvadeTargetMenu.Seperator"),
                new MenuBool("Brian.EvadeTargetMenu.EvadeTargetW", "Use W"),
                new MenuBool("Brian.EvadeTargetMenu.EvadeTargetETower", "Don't Use Under Tower", false)
            };

            InitSpells();

            SpellMenu =new Menu("Brian.EvadeTargetMenu.DodgeSpellMenu", "Dodge Spell");
            {
                foreach (var spell in Spells.Where(i => Fiora_By_Kornis.GameObjects.EnemyHeroes.Any(a => a.ChampionName == i.ChampionName)))
                {
                    if (spell.MissileName == "bluecardattack")
                    {
                        
                        SpellMenu.Add(new MenuBool("Brian.EvadeTargetMenu." + spell.MissileName,
                            spell.ChampionName + "( Blue Card )"));
                    }
                    if (spell.MissileName == "redcardattack")
                    {
                        SpellMenu.Add(new MenuBool("Brian.EvadeTargetMenu." + spell.MissileName,
                            spell.ChampionName + "( Red Card )"));
                    }
                    if (spell.MissileName == "goldcardattack")
                    {
                        SpellMenu.Add(new MenuBool("Brian.EvadeTargetMenu." + spell.MissileName,
                            spell.ChampionName + "( Gold Card )"));
                    }
                    if (spell.MissileName != "bluecardattack" || spell.MissileName != "goldcardattack" || spell.MissileName != "redcardattack")
                    {
                        SpellMenu.Add(new MenuBool("Brian.EvadeTargetMenu." + spell.MissileName,
                            spell.ChampionName + "(" + spell.Slot + ")"));
                    }

                }
            }
            Menu.Add(SpellMenu);

            mainMenu.Add(Menu);


            Game.OnUpdate += OnUpdate;
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
                    SpellNames = new[] { "leblancchaosorb", "leblancchaosorbm", "leblancq" },
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
                new SpellData { ChampionName = "Vayne", SpellNames = new[] { "vaynecondemnmissile" }, Slot = SpellSlot.E });
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

            if (missile.SpellCaster == null || !missile.SpellCaster.IsValid ||
                missile.SpellCaster.Team == ObjectManager.GetLocalPlayer().Team)
            {
                return;
            }
            var hero = missile.SpellCaster as Obj_AI_Hero;
            if (hero == null)
            {
                return;
            }

            var spellData =
                Spells.FirstOrDefault(
                    i =>
                        i.SpellNames.Contains(missile.SpellData.Name.ToLower())
                        && SpellMenu["Brian.EvadeTargetMenu." + i.MissileName].Enabled);


            if (spellData == null || !missile.Target.IsMe || missile.SpellData.Name.Contains("BasicAttack"))
            {
                return;
            }
            DetectedTargets.Add(new Targets { Start = hero.ServerPosition, Obj = missile });
        }

        private static void OnDestroy(GameObject sender)
        {
            var missile = sender as MissileClient;
            if (missile == null)
            {
                return;
            }
            if (!sender.IsValid)
            {
                return;
            }
            var hero = missile.SpellCaster as Obj_AI_Hero;
            if (hero == null)
            {
                return;
            }
            if (missile.SpellCaster.IsValid && missile.SpellCaster.Team != ObjectManager.GetLocalPlayer().Team)
            {
                DetectedTargets.RemoveAll(i => i.Obj.NetworkId == missile.NetworkId);
            }
        }

        private static void OnUpdate()
        {
            if (ObjectManager.GetLocalPlayer().IsDead)
            {
                return;
            }

            if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellImmunity) ||
                ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellShield))
            {
                return;
            }

            foreach (var target in DetectedTargets.Where(i => ObjectManager.GetLocalPlayer().Distance(i.Obj.Position) < 700))
            {
             
                if (Fiora_By_Kornis.Fiora.W.Ready && Menu["Brian.EvadeTargetMenu.EvadeTargetW"].Enabled 
                    && Fiora_By_Kornis.Fiora.W.Cast(ObjectManager.GetLocalPlayer().ServerPosition.Extend(target.Start, 100)))
                {
                    return;
                }
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

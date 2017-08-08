namespace Support_AIO.Handlers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
    using Base;
    using Data;

    #endregion

    public class HPInstance
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets the decay.
        /// </summary>
        /// <value>
        ///     The decay.
        /// </value>
        public int Decay { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets or sets the predicted damage.
        /// </summary>
        /// <value>
        ///     The predicted damage.
        /// </value>
        public float PredictedDmg { get; internal set; }

        /// <summary>
        ///     Gets or sets the target hero.
        /// </summary>
        /// <value>
        ///     The target hero.
        /// </value>
        public Obj_AI_Hero TargetHero { get; internal set; }

        /// <summary>
        ///     Gets or sets the attacker.
        /// </summary>
        /// <value>
        ///     The attacker.
        /// </value>
        public Obj_AI_Base Attacker { get; internal set; }

        /// <summary>
        ///     Gets or sets the game data.
        /// </summary>
        /// <value>
        ///     The game data.
        /// </value>
        public Gamedata GameData { get; internal set; }

        /// <summary>
        ///     Gets or sets the type of the event.
        /// </summary>
        /// <value>
        ///     The type of the event.
        /// </value>
        public EventType EventType { get; internal set; } = EventType.None;

        #endregion
    }

    public class PredictDamageEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the damage process
        /// </summary>
        /// <value>
        ///     The hp instance.
        /// </value>
        public bool NoProcess { get; set; }

        /// <summary>
        ///     Gets or sets the damage instance
        /// </summary>
        /// <value>
        ///     The hp instance.
        /// </value>
        public HPInstance HpInstance { get; internal set; }

        #endregion
    }

    internal class Projections
    {
        #region Static Fields and Constants

        internal static int Id;
        internal static Obj_AI_Hero Player;

        #endregion

        #region Private Methods

        private static void MissileClient_OnSpellMissileCreate(GameObject sender)
        {
            #region FoW / Missile

            if (sender.Type != GameObjectType.MissileClient)
            {
                return;
            }

            var missile = (MissileClient) sender;

            if (missile.SpellCaster is Obj_AI_Hero
                && missile.SpellCaster?.Team != Player.Team)
            {
                var startPos = missile.StartPosition.To2D();
                var endPos = missile.EndPosition.To2D();

                var data = Gamedata.GetByMissileName(missile.SpellData.Name.ToLower());

                if (data == null)
                {
                    return;
                }

                // set line width
                if (data.Radius == 0f)
                {
                    data.Radius = missile.SpellData.LineWidth;
                }

                var direction = (endPos - startPos).Normalized();

                if (startPos.Distance(endPos) > data.CastRange)
                {
                    endPos = startPos + direction * data.CastRange;
                }

                if (startPos.Distance(endPos) < data.CastRange
                    && data.FixedRange)
                {
                    endPos = startPos + direction * data.CastRange;
                }

                foreach (var hero in ZLib.Allies())
                {
                    var distance = (1000 * (startPos.Distance(hero.Player.ServerPosition) / data.MissileSpeed));
                    var endtime = -100 + Game.Ping / 2 + distance;

                    // setup projection
                    var proj = hero.Player.ServerPosition.To2D().ProjectOn(startPos, endPos);
                    var projdist = hero.Player.ServerPosition.To2D().Distance(proj.SegmentPoint);

                    // get the evade time
                    var evadetime = (int) (1000 *
                        (data.Radius - projdist + hero.Player.BoundingRadius) / hero.Player.MoveSpeed);

                    // check if hero on segment
                    if (proj.IsOnSegment
                        && projdist <= data.Radius + hero.Player.BoundingRadius + 35)
                    {
                        if (data.CastRange > 10000)
                        {
                            // ignore if can evade
                            if (hero.Player.NetworkId == Player.NetworkId)
                            {
                                if (evadetime < endtime)
                                {
                                    // check next player
                                    continue;
                                }
                            }
                        }

                        if (SpellEnabled(data))
                        {
                            EmulateDamage(missile.SpellCaster, hero, data, EventType.Spell, "missile.OnCreate", 0f,
                                (int) endtime);
                        }
                    }
                }
            }

            #endregion
        }

        private static void Obj_AI_Base_OnUnitSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            var aiHero = sender as Obj_AI_Hero;
            if (aiHero != null
                && ZLib.Menu["debugmenu"]["dumpdata"].As<MenuBool>().Enabled)
            {
                var clientdata = new Gamedata
                {
                    SpellName = args.SpellData.Name.ToLower(),
                    ChampionName = aiHero.UnitSkinName.ToLower(),
                    Slot = args.SpellSlot,
                    Radius = args.SpellData.LineWidth > 0
                        ? args.SpellData.LineWidth
                        : (args.SpellData.CastRadiusSecondary > 0
                            ? args.SpellData.CastRadiusSecondary
                            : args.SpellData.CastRadius),
                    CastRange = args.SpellData.CastRange,
                    Delay = 250f,
                    MissileSpeed = (int) args.SpellData.MissileSpeed
                };

                Helpers.ExportSpellData(clientdata, args.SpellData.TargettingType.ToString().ToLower());
            }

            if (Helpers.IsEpicMinion(sender)
                || Helpers.IsCrab(sender))
            {
                return;
            }

            #region Hero

            if (sender.IsEnemy
                && sender is Obj_AI_Hero)
            {
                var attacker = (Obj_AI_Hero) sender;

                if (attacker.IsValid
                    && attacker is Obj_AI_Hero)
                {
                    foreach (var hero in ZLib.Allies())
                    {
                        #region auto attack

                        if (args.SpellData.Name.ToLower().Contains("attack")
                            && args.Target != null)
                        {
                            if (args.Target.NetworkId == hero.Player.NetworkId)
                            {
                                float dmg = 0;

                                dmg += (int) Math.Max(attacker.GetAutoAttackDamage(hero.Player), 0);

                                if (attacker.HasBuff("sheen"))
                                    dmg += (int) Math.Max(attacker.GetAutoAttackDamage(hero.Player) +
                                        attacker.GetCustomDamage("sheen", hero.Player), 0);

                                if (attacker.HasBuff("lichbane"))
                                    dmg += (int) Math.Max(attacker.GetAutoAttackDamage(hero.Player) +
                                        attacker.GetCustomDamage("lichbane", hero.Player), 0);

                                if (attacker.HasBuff("itemstatikshankcharge")
                                    && attacker.GetBuff("itemstatikshankcharge").Count == 100)
                                    dmg += new[] { 62, 120, 200, 200 }[Math.Min(18, attacker.Level) / 6];

                                if (args.SpellData.Name.ToLower().Contains("crit"))
                                {
                                    dmg += (int) Math.Max(attacker.GetAutoAttackDamage(hero.Player), 0);
                                }

                                EmulateDamage(attacker, hero, new Gamedata(), EventType.AutoAttack,
                                    "enemy.AutoAttack", dmg);
                            }
                        }

                        #endregion

                        var data = ZLib.CachedSpells.Find(x => x.SpellName.ToLower() == args.SpellData.Name.ToLower());
                        if (data == null)
                        {
                            continue;
                        }

                        #region self/selfaoe

                        if (args.SpellData.TargettingType == (ulong) ProcessSpellType.Self
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.SelfAoE
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.SelfAndUnit)
                        {
                            if (data.Radius == 0f)
                                data.Radius = args.SpellData.CastRadiusSecondary != 0
                                    ? args.SpellData.CastRadiusSecondary
                                    : args.SpellData.CastRadius;

                            GameObject fromobj = null;

                            if (data.FromObject != null)
                            {
                                fromobj =
                                    ObjectManager.Get<GameObject>()
                                        .FirstOrDefault(
                                            x =>
                                                data.FromObject != null && // todo: actually get team
                                                data.FromObject.Any(y => x.Name.Contains(y)));
                            }

                            var correctpos = fromobj?.Position ?? attacker.ServerPosition;

                            if (hero.Player.Distance(correctpos) <= data.CastRange + 125)
                            {
                                if (data.SpellName == "kalistaexpungewrapper"
                                    && !hero.Player.HasBuff("kalistaexpungemarker"))
                                {
                                    continue;
                                }

                                if (SpellEnabled(data))
                                {
                                    EmulateDamage(attacker, hero, data, EventType.Spell, "enemy.SelfAoE");
                                }
                            }
                        }

                        #endregion

                        #region skillshot

                        if (args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationCone
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationUnknown
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationLine2
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationLine
                            || args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationCircle)
                        {
                            GameObject fromobj = null;

                            if (data.FromObject != null)
                            {
                                fromobj =
                                    ObjectManager.Get<GameObject>()
                                        .FirstOrDefault(
                                            x =>
                                                data.FromObject != null && // todo: actually get team
                                                data.FromObject.Any(y => x.Name.Contains(y)));
                            }

                            var isline = args.SpellData.TargettingType == (ulong) ProcessSpellType.LocationCone
                                ||
                                args.SpellData.LineWidth > 0;

                            if (!(args.SpellData.LineWidth > 0)
                                && data.Radius == 0f)
                            {
                                data.Radius = args.SpellData.CastRadiusSecondary != 0
                                    ? args.SpellData.CastRadiusSecondary
                                    : args.SpellData.CastRadius;
                            }

                            var startpos = fromobj?.Position ?? attacker.ServerPosition;

                            if (hero.Player.Distance(startpos) > data.CastRange + 35)
                            {
                                continue;
                            }

                            if ((data.SpellName == "azirq" || data.SpellName == "azire")
                                && fromobj == null)
                            {
                                continue;
                            }

                            var distance = (int) (1000 * (startpos.Distance(hero.Player.ServerPosition)
                                / data.MissileSpeed));
                            var endtime = data.Delay + distance - Game.Ping / 2f;

                            var iscone = args.SpellData.TargettingType
                                == (ulong) ProcessSpellType.LocationCone;
                            var direction = (args.End.To2D() - startpos.To2D()).Normalized();
                            var endpos = startpos.To2D() + direction * startpos.To2D().Distance(args.End.To2D());

                            if (startpos.To2D().Distance(endpos) > data.CastRange)
                            {
                                endpos = startpos.To2D() + direction * data.CastRange;
                            }

                            if (startpos.To2D().Distance(endpos) < data.CastRange
                                && data.FixedRange)
                            {
                                endpos = startpos.To2D() + direction * data.CastRange;
                            }

                            var proj = hero.Player.ServerPosition.To2D().ProjectOn(startpos.To2D(), endpos);
                            var projdist = hero.Player.ServerPosition.To2D().Distance(proj.SegmentPoint);

                            var evadetime = 0;

                            if (isline)
                                evadetime =
                                    (int) (1000 * (data.Radius - projdist + hero.Player.BoundingRadius)
                                        / hero.Player.MoveSpeed);

                            if (!isline || iscone)
                                evadetime =
                                    (int) (1000 * (data.Radius - hero.Player.Distance(startpos) + hero.Player.BoundingRadius)
                                        / hero.Player.MoveSpeed);

                            if (proj.IsOnSegment && projdist <= data.Radius + hero.Player.BoundingRadius + 35 && isline
                                || (iscone || !isline) && hero.Player.Distance(endpos)
                                <= data.Radius + hero.Player.BoundingRadius + 35)
                            {
                                if (data.CastRange > 10000
                                    && hero.Player.NetworkId == Player.NetworkId)
                                {
                                    if (evadetime < endtime)
                                    {
                                        continue;
                                    }
                                }

                                if (SpellEnabled(data))
                                {
                                    EmulateDamage(attacker, hero, data, EventType.Spell, "enemy.Skillshot", 0f,
                                        (int) endtime);
                                }
                            }
                        }

                        #endregion

                        #region unit type

                        if (args.SpellData.TargettingType == (ulong) ProcessSpellType.Targeted)
                        {
                            if (args.Target == null
                                || args.Target.Type != GameObjectType.obj_AI_Hero)
                            {
                                continue;
                            }

                            // check if is targeteting the hero on our table
                            if (hero.Player.NetworkId != args.Target.NetworkId)
                            {
                                continue;
                            }

                            // target spell dectection
                            if (hero.Player.Distance(attacker.ServerPosition) > data.CastRange + 100)
                            {
                                continue;
                            }

                            var distance =
                                (int) (1000 * (attacker.Distance(hero.Player.ServerPosition) / data.MissileSpeed));

                            var endtime = data.Delay + distance - Game.Ping / 2f;

                            if (SpellEnabled(data))
                            {
                                EmulateDamage(attacker, hero, data, EventType.Spell, "enemy.TargetSpell", 0f,
                                    (int) endtime);
                            }
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region Turret

            if (sender.IsEnemy
                && sender.Type == GameObjectType.obj_AI_Turret
                && args.Target is Obj_AI_Hero)
            {
                var turret = sender as Obj_AI_Turret;

                if (turret != null
                    && turret.IsValid)
                {
                    foreach (var hero in ZLib.Allies())
                    {
                        if (args.Target.NetworkId == hero.Player.NetworkId)
                        {
                            if (turret.Distance(hero.Player.ServerPosition) <= 900
                                && Player.Distance(hero.Player.ServerPosition) <= 1000)
                            {
                                EmulateDamage(turret, hero, new Gamedata(), EventType.TurretAttack, "enemy.Turret");
                            }
                        }
                    }
                }
            }

            #endregion

            #region Minion

            if (sender.IsEnemy
                && sender.Type == GameObjectType.obj_AI_Minion
                && args.Target.Type == Player.Type)
            {
                var minion = sender as Obj_AI_Minion;

                if (minion != null
                    && minion.IsValidTarget())
                {
                    foreach (var hero in ZLib.Allies())
                    {
                        if (hero.Player.NetworkId == args.Target.NetworkId)
                        {
                            if (hero.Player.Distance(minion.ServerPosition) <= 750
                                && Player.Distance(hero.Player.ServerPosition) <= 1000)
                            {
                                EmulateDamage(minion, hero, new Gamedata(), EventType.MinionAttack, "enemy.Minion");
                            }
                        }
                    }
                }
            }

            #endregion

            #region Gangplank Barrel

            if (sender.IsEnemy
                && sender.Type == GameObjectType.obj_AI_Hero)
            {
                var attacker = sender as Obj_AI_Hero;

                if (attacker.ChampionName == "Gangplank"
                    && attacker.IsValid)
                {
                    foreach (var hero in ZLib.Allies())
                    {
                        var gplist = new List<Obj_AI_Minion>();

                        gplist.AddRange(ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                x =>
                                    x.UnitSkinName.ToLower() == "gangplankbarrel" &&
                                    x.Position.Distance(x.Position) <= 375 && x.IsFloatingHealthBarActive)
                            .OrderBy(y => y.Position.Distance(hero.Player.ServerPosition)));

                        foreach (var obj in gplist)
                        {
                            if (hero.Player.Distance(obj.Position) <= 375
                                && args.Target.Name == "Barrel")
                            {
                                var dmg = (float) Math.Abs(attacker.GetAutoAttackDamage(hero.Player) * 1.2 + 150);

                                if (args.SpellData.Name.ToLower().Contains("crit"))
                                {
                                    dmg = dmg * 2;
                                }

                                EmulateDamage(attacker, hero, new Gamedata(), EventType.Spell,
                                    "enemy.GankplankBarrel", dmg);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Items

            if (sender.IsEnemy
                && sender.Type == GameObjectType.obj_AI_Hero)
            {
                var attacker = sender as Obj_AI_Hero;

                if (attacker != null
                    && attacker.IsValid)
                {
                    if (args.SpellData.TargettingType == (ulong) ProcessSpellType.Targeted)
                    {
                        foreach (var hero in ZLib.Allies())
                        {
                            if (args.Target.NetworkId != hero.Player.NetworkId)
                            {
                                continue;
                            }

                            // todo: item damage

                            if (args.SpellData.Name.ToLower() == "bilgewatercutlass")
                            {
                                var dmg = (float) 0;
                                EmulateDamage(attacker, hero, new Gamedata(), EventType.Item, "enemy.ItemCast", dmg);
                            }

                            if (args.SpellData.Name.ToLower() == "itemswordoffeastandfamine")
                            {
                                var dmg = (float) 0;
                                EmulateDamage(attacker, hero, new Gamedata(), EventType.Item, "enemy.ItemCast", dmg);
                            }

                            if (args.SpellData.Name.ToLower() == "hextechgunblade")
                            {
                                var dmg = (float) 0;
                                EmulateDamage(attacker, hero, new Gamedata(), EventType.Item, "enemy.ItemCast", dmg);
                            }
                        }
                    }

                    // todo:
                    if (args.SpellData.TargettingType == (ulong) ProcessSpellType.Self)
                    {
                        foreach (var hero in ZLib.Allies())
                        {
                            if (args.SpellData.Name.ToLower() == "itemtiamatcleave")
                            {
                                if (attacker.Distance(hero.Player.ServerPosition) <= 375)
                                {
                                    var dmg = (float) 0;
                                    EmulateDamage(attacker, hero, new Gamedata(), EventType.Item, "enemy.ItemCast",
                                        dmg);
                                }
                            }
                        }
                    }

                    if (args.SpellData.TargettingType.ToString().Contains("Location")) { }
                }
            }

            #endregion

            #region LucianQ

            if (sender.IsEnemy
                && args.SpellData.Name.ToLower() == "lucianq")
            {
                var data = ZLib.CachedSpells.Find(x => x.SpellName.ToLower() == "lucianq");

                if (data != null)
                {
                    foreach (var hero in ZLib.Allies())
                    {
                        var delay = ((350 - Game.Ping) / 1000f);

                        var herodir = (hero.Player.ServerPosition - hero.Player.Position).Normalized();
                        var expectedpos = args.Target.Position + herodir * hero.Player.MoveSpeed * (delay);

                        if (args.Start.Distance(expectedpos) < 1100)
                            expectedpos = args.Target.Position +
                                (args.Target.Position - sender.ServerPosition).Normalized() * 800;

                        var proj = hero.Player.ServerPosition.To2D().ProjectOn(args.Start.To2D(), expectedpos.To2D());
                        var projdist = hero.Player.ServerPosition.To2D().Distance(proj.SegmentPoint);

                        if (SpellEnabled(data))
                        {
                            if (100 + hero.Player.BoundingRadius > projdist)
                            {
                                EmulateDamage(sender, hero, data, EventType.Spell, "enemy.LucianQ");
                            }
                        }
                    }
                }
            }

            #endregion
        }

        private static void Obj_AI_Base_OnStealth(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            #region Stealth

            var attacker = sender as Obj_AI_Hero;

            if (attacker == null
                || attacker.IsAlly
                || !attacker.IsValid)
            {
                return;
            }

            foreach (var hero in ZLib.Heroes.Where(h => h.Player.Distance(attacker) <= 1000))
            {
                foreach (var entry in ZLib.CachedSpells.Where(s => s.EventTypes.Contains(EventType.Stealth)))
                {
                    if (entry.SpellName.ToLower() == args.SpellData.Name.ToLower())
                    {
                        EmulateDamage(sender, hero, new Gamedata { SpellName = "Stealth" }, EventType.Stealth,
                            "process.OnStealth");
                        break;
                    }
                }
            }

            #endregion
        }

        private static bool SpellEnabled(Gamedata data)
        {
            return ZLib.Menu["evadem"][data.ChampionName.ToLower() + "menu"][data.SpellName]
                [data.SpellName + "predict"].As<MenuBool>().Enabled;
        }

        private static bool SpellEnabled(Gamedata data, EventType type)
        {
            if (type != EventType.CrowdControl
                || type != EventType.Danger
                || type != EventType.Ultimate
                || type != EventType.ForceExhaust)
            {
                return false;
            }

            return ZLib.Menu["evadem"][data.ChampionName.ToLower() + "menu"][data.SpellName]
                [data.SpellName + type.ToString().ToLower()].As<MenuBool>().Enabled;
        }

        #endregion

        #region Public Methods

        internal static void Init()
        {
            Player = ObjectManager.GetLocalPlayer();
            GameObject.OnCreate += MissileClient_OnSpellMissileCreate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnUnitSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnStealth;
        }

        internal static void EmulateDamage(Obj_AI_Base sender, Champion hero, Gamedata data, EventType dmgType, string notes = null,
            float dmgEntry = 0f, int expiry = 500)
        {
            var hpred = new HPInstance();
            hpred.EventType = dmgType;
            hpred.TargetHero = hero.Player;
            hpred.GameData = data;
            hpred.Name = string.Empty;

            if (!string.IsNullOrEmpty(data?.SpellName))
            {
                hpred.Name = data.SpellName;
            }

            if (sender is Obj_AI_Hero)
            {
                hpred.Attacker = sender;
            }

            if (dmgEntry == 0f
                && sender != null)
            {
                switch (dmgType)
                {
                    case EventType.AutoAttack:
                        hpred.PredictedDmg = (float) Math.Max(sender.GetAutoAttackDamage(hero.Player), 0);
                        break;

                    case EventType.MinionAttack:
                    case EventType.TurretAttack:
                        hpred.PredictedDmg =
                            (float)
                            Math.Max(sender.CalculateDamage(hero.Player, DamageType.Physical,
                                sender.BaseAttackDamage + sender.FlatPhysicalDamageMod), 0);
                        break;

                    default:
                        var aiHero = sender as Obj_AI_Hero;

                        if (!string.IsNullOrEmpty(data?.SpellName))
                        {
                            hpred.PredictedDmg = (float) Math.Max(aiHero.GetSpellDamage(hero.Player, data.Slot), 0);
                        }

                        break;
                }
            }

            else
            {
                var idmg = dmgEntry;
                hpred.PredictedDmg = (float) Math.Round(idmg);
            }

            if (hpred.PredictedDmg > 0)
            {
                var idmg = hpred.PredictedDmg * ZLib.Menu["weightdmg"].As<MenuSlider>().Value / 100;
                hpred.PredictedDmg = (float) Math.Round(idmg);
            }

            else
            {
                var idmg = (hero.Player.Health / hero.Player.MaxHealth) * 5;
                hpred.PredictedDmg = (float) Math.Round(idmg);
            }

            if (dmgType != EventType.Buff
                && dmgType != EventType.Troy)
            {
                // check duplicates (missiles and process spell)
                if (ZLib.DamageCollection.Select(entry => entry.Value).Any(o => data != null && o.Name == data.SpellName))
                {
                    return;
                }
            }

            var dmg = AddDamage(hpred, hero, notes);
            var extendedEndtime = ZLib.Menu["lagtolerance"].As<MenuSlider>().Value * 10;
            DelayAction.Queue(expiry + extendedEndtime, () => RemoveDamage(dmg, notes));
        }

        internal static int AddDamage(HPInstance hpi, Champion hero, string notes)
        {
            Id++;
            var id = Id;

            var damageEventArgs = new PredictDamageEventArgs { HpInstance = hpi };
            ZLib.TriggerOnPredictDamage(hero, damageEventArgs);

            if (damageEventArgs.NoProcess)
            {
                ZLib.DamageCollection.Add(id, null);
                return id;
            }

            var aiHero = ZLib.Allies().FirstOrDefault(x => x.Player.NetworkId == hero.Player.NetworkId);

            if (aiHero != null
                && !ZLib.DamageCollection.ContainsKey(id))
            {
                aiHero.Attacker = hpi.Attacker;

                if (aiHero.Player.IsValid)
                {
                    var checkmenu = false;

                    switch (hpi.EventType)
                    {
                        case EventType.Spell:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Spell);
                            checkmenu = true;
                            break;

                        case EventType.Buff:
                            aiHero.BuffDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Buff);

                            if (notes == "aura.Evade")
                            {
                                aiHero.Events.Add(EventType.Ultimate);
                            }

                            break;

                        case EventType.Troy:
                            aiHero.TroyDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Troy);

                            if (notes != "debug.Test")
                            {
                                aiHero.Events.AddRange(hpi.GameData.EventTypes);
                            }

                            break;

                        case EventType.Item:
                            aiHero.ItemDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Spell);
                            break;

                        case EventType.TurretAttack:
                            aiHero.TowerDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.TurretAttack);
                            break;

                        case EventType.AutoAttack:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.AutoAttack);
                            break;

                        case EventType.Stealth:
                            aiHero.Events.Add(EventType.Stealth);
                            break;

                        case EventType.Initiator:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Initiator);
                            break;
                    }

                    if (notes == "debug.Test")
                    {
                        if (hpi.EventType == EventType.Danger)
                        {
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Danger);
                        }

                        if (hpi.EventType == EventType.CrowdControl)
                        {
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.CrowdControl);
                        }

                        if (hpi.EventType == EventType.Ultimate)
                        {
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Ultimate);
                        }

                        if (hpi.EventType == EventType.ForceExhaust)
                        {
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.ForceExhaust);
                        }
                    }

                    if (checkmenu && !string.IsNullOrEmpty(hpi.Name)) // QWER Only
                    {
                        if (notes != "debug.Test")
                        {
                            // add spell flags
                            hero.Events.AddRange(Helpers.MenuTypes.Where(x => SpellEnabled(hpi.GameData, x)));
                        }
                    }

                    if (hpi.EventType == EventType.Stealth)
                    {
                        hpi.PredictedDmg = 0;
                    }

                    if (ZLib.Menu["debugmenu"]["acdebug"].As<MenuBool>().Enabled)
                    {
                        Console.WriteLine(
                            hpi.TargetHero.ChampionName + " << [added]: " + hpi.Name + " - "
                            + hpi.PredictedDmg + " / " + hpi.EventType + " / " + notes);
                    }

                    hpi.Id = id;
                    ZLib.DamageCollection.Add(id, hpi);
                }
            }

            return id;
        }

        internal static void RemoveDamage(int id, string notes)
        {
            var entry = ZLib.DamageCollection.FirstOrDefault(x => x.Key == id);
            if (entry.Value == null)
            {
                ZLib.DamageCollection.Remove(id);
                return;
            }

            if (ZLib.DamageCollection.ContainsKey(entry.Key))
            {
                var hpi = entry.Value;

                var aiHero = ZLib.Allies().FirstOrDefault(x => x.Player.NetworkId == hpi.TargetHero.NetworkId);
                if (aiHero != null)
                {
                    var checkmenu = false;

                    switch (hpi.EventType)
                    {
                        case EventType.Spell:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Spell);
                            checkmenu = true;
                            break;

                        case EventType.Buff:
                            aiHero.BuffDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Buff);

                            if (notes == "aura.Evade")
                            {
                                aiHero.Events.Remove(EventType.Ultimate);
                            }

                            break;

                        case EventType.Troy:
                            aiHero.TroyDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Troy);
                            aiHero.Events.RemoveAll(x => hpi.GameData.EventTypes.Contains(x));
                            break;

                        case EventType.Item:
                            aiHero.ItemDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Item);
                            break;

                        case EventType.TurretAttack:
                            aiHero.TowerDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.TurretAttack);
                            break;

                        case EventType.AutoAttack:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.AutoAttack);
                            break;

                        case EventType.Stealth:
                            aiHero.Events.Remove(EventType.Stealth);
                            break;

                        case EventType.Initiator:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Initiator);
                            break;
                    }

                    if (notes == "debug.Test")
                    {
                        if (hpi.EventType == EventType.Danger)
                        {
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Danger);
                        }

                        if (hpi.EventType == EventType.CrowdControl)
                        {
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.CrowdControl);
                        }

                        if (hpi.EventType == EventType.Ultimate)
                        {
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Ultimate);
                        }

                        if (hpi.EventType == EventType.ForceExhaust)
                        {
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.ForceExhaust);
                        }
                    }

                    if (checkmenu && !string.IsNullOrEmpty(hpi.Name)) // QWER Only
                    {
                        if (notes != "debug.Test")
                        {
                            // remove spell flags
                            aiHero.Events.RemoveAll(
                                x =>
                                    !x.Equals(EventType.Spell) && SpellEnabled(hpi.GameData, x));
                        }
                    }

                    if (ZLib.Menu["debugmenu"]["acdebug"].As<MenuBool>().Enabled)
                    {
                        Console.WriteLine(hpi.TargetHero.ChampionName + " >> [removed]: " + hpi.Name + " - " +
                            hpi.PredictedDmg + " / " + hpi.EventType + " / " + notes);
                    }

                    aiHero.Attacker = null;
                    ZLib.DamageCollection.Remove(id);
                }

                else
                {
                    var deadHero = ZLib.Heroes.FirstOrDefault(x => x.Player.NetworkId == hpi.TargetHero.NetworkId);
                    if (deadHero != null)
                    {
                        Helpers.ResetIncomeDamage(deadHero);
                    }
                }
            }
        }

        #endregion
    }
}
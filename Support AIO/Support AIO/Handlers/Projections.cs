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

    /// <summary>
    ///     Class HPInstance.
    /// </summary>
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
        public Obj_AI_Base Target { get; internal set; }

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

    /// <summary>
    ///     Class PredictDamageEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
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

    /// <summary>
    ///     Class Projections.
    /// </summary>
    internal class Projections
    {
        #region Static Fields and Constants

        internal static int Id;
        internal static Obj_AI_Hero Player;

        #endregion

        #region Internal Methods and Operators

        internal static void MissileClient_OnSpellMissileCreate(GameObject sender)
        {
            try
            {
                #region FoW / Missile

                if (sender.Type != GameObjectType.MissileClient)
                {
                    return;
                }

                var missile = (MissileClient)sender;
                if (missile.SpellCaster is Obj_AI_Hero && missile.SpellCaster?.Team != Player.Team)
                {
                    var startPos = missile.StartPosition.To2D();
                    var endPos = missile.EndPosition.To2D();

                    var data = Gamedata.GetByMissileName(missile.SpellData.Name.ToLower());
                    if (data == null)
                    {
                        return;
                    }

                    if (data.Radius < 1f)
                    {
                        data.Radius = missile.SpellData.LineWidth;
                    }

                    var direction = (endPos - startPos).Normalized();
                    if (startPos.Distance(endPos) > data.CastRange)
                    {
                        endPos = startPos + direction * data.CastRange;
                    }

                    if (startPos.Distance(endPos) < data.CastRange && data.FixedRange)
                    {
                        endPos = startPos + direction * data.CastRange;
                    }

                    foreach (var hero in ZLib.GetUnits())
                    {
                        var distance = (1000 * (startPos.Distance(hero.Instance.ServerPosition) / data.MissileSpeed));
                        var endtime = -100 + Game.Ping / 2 + distance;

                        // setup projection
                        var proj = hero.Instance.ServerPosition.To2D().ProjectOn(startPos, endPos);
                        var projdist = hero.Instance.ServerPosition.To2D().Distance(proj.SegmentPoint);

                        // get the evade time
                        var evadetime = (int)(1000 *
                                              (data.Radius - projdist + hero.Instance.BoundingRadius)
                                              / hero.Instance.MoveSpeed);

                        // check if hero on segment
                        if (proj.IsOnSegment && projdist <= data.Radius + hero.Instance.BoundingRadius + 35)
                        {
                            if (data.CastRange > 10000)
                            {
                                // ignore if can evade
                                if (hero.Instance.NetworkId == Player.NetworkId)
                                {
                                    if (evadetime < endtime)
                                    {
                                        // check next player
                                        continue;
                                    }
                                }
                            }

                            EmulateDamage(missile.SpellCaster, hero, data, EventType.Spell, "missile.oncreate", 0f, (int)endtime);
                        }
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.Missile");
                    Console.WriteLine(e);
                }
            }
        }

        internal static void Obj_AI_Base_OnUnitSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            var aiHero = sender as Obj_AI_Hero;
            if (aiHero != null && ZLib.Menu["dumpdata"].As<MenuBool>().Enabled)
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
                    MissileSpeed = (int)args.SpellData.MissileSpeed
                };

                Helpers.ExportSpellData(clientdata, args.SpellData.TargettingType.ToString().ToLower());
            }

            if (Helpers.IsEpicMinion(sender) || Helpers.IsCrab(sender))
            {
                return;
            }

            try
            {
                #region Hero

                var attacker = sender as Obj_AI_Hero;
                if (attacker != null)
                {
                    foreach (var hero in ZLib.GetUnits())
                    {
                        #region auto attack

                        if (args.SpellData.Name.ToLower().Contains("attack") && args.Target != null)
                        {
                            if (args.Target.NetworkId == hero.Instance.NetworkId)
                            {
                                double dmg = 0;
                                dmg += (int)Math.Max(attacker.GetAutoAttackDamage(hero.Instance), 0);

                                if (attacker.HasBuffOfType(BuffType.CombatEnchancer))
                                {
                                    dmg += attacker.AmplifyAuto(hero.Instance);
                                }

                                if (args.SpellData.Name.ToLower().Contains("crit"))
                                {
                                    dmg = dmg * 2;
                                }

                                EmulateDamage(attacker, hero, new Gamedata { SpellName = args.SpellData.Name }, EventType.AutoAttack,
                                    "enemy.autoattack", (float)dmg);
                            }
                        }

                        #endregion

                        var data = ZLib.CachedSpells.Find(x => x.SpellName.ToLower() == args.SpellData.Name.ToLower());
                        if (data == null)
                        {
                            continue;
                        }

                        #region self/selfaoe

                        if (data.CastType == CastType.Proximity)
                        {
                            if (data.Radius < 1f)
                            {
                                data.Radius = args.SpellData.CastRadiusSecondary > 0
                                    ? args.SpellData.CastRadiusSecondary
                                    : args.SpellData.CastRadius;
                            }

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
                            if (hero.Instance.Distance(correctpos) <= data.CastRange + 125)
                            {
                                if (!data.SpellName.Equals("kalistaexpungewrapper") || hero.Instance.HasBuff("kalistaexpungemarker"))
                                {
                                    EmulateDamage(attacker, hero, data, EventType.Spell, "spell.proximity");
                                }
                            }
                        }

                        #endregion

                        #region skillshot line

                        if (data.CastType.ToString().Contains("Linear"))
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

                            if (args.SpellData.LineWidth > 0 && data.Radius < 1f)
                            {
                                data.Radius = args.SpellData.LineWidth;
                            }

                            var startpos = fromobj?.Position ?? attacker.ServerPosition;
                            if (hero.Instance.Distance(startpos) > data.CastRange + 35)
                            {
                                continue;
                            }

                            if ((data.SpellName == "azirq" || data.SpellName == "azire") && fromobj == null)
                            {
                                continue;
                            }

                            var distance = (int)
                            (1000 * (startpos.Distance(hero.Instance.ServerPosition)
                                     / data.MissileSpeed));

                            var endtime = data.Delay + distance - Game.Ping / 2f;
                            var direction = (args.End.To2D() - startpos.To2D()).Normalized();
                            var endpos = startpos.To2D() + direction * startpos.To2D().Distance(args.End.To2D());

                            if (startpos.To2D().Distance(endpos) > data.CastRange)
                            {
                                endpos = startpos.To2D() + direction * data.CastRange;
                            }

                            if (startpos.To2D().Distance(endpos) < data.CastRange && data.FixedRange)
                            {
                                endpos = startpos.To2D() + direction * data.CastRange;
                            }

                            if (data.IsPerpindicular)
                            {
                                startpos = (endpos - direction.Perpendicular() * data.CastRange).To3D();
                                endpos = endpos + direction.Perpendicular() * data.CastRange;
                            }

                            var proj = hero.Instance.ServerPosition.To2D().ProjectOn(startpos.To2D(), endpos);
                            var projdist = hero.Instance.ServerPosition.To2D().Distance(proj.SegmentPoint);
                            var evadetime = (int)(1000 * (data.Radius - projdist + hero.Instance.BoundingRadius)
                                                  / hero.Instance.MoveSpeed);

                            if (proj.IsOnSegment && projdist <= data.Radius + hero.Instance.BoundingRadius + 35)
                            {
                                if (data.CastRange > 9000 || data.Global)
                                {
                                    if (hero.Instance.NetworkId == Player.NetworkId)
                                    {
                                        if (evadetime < endtime)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                if (data.CastType == CastType.LinearAoE || data.CastType == CastType.MissileLinearAoE)
                                {
                                    if (hero.Instance.Distance(endpos) <= hero.Instance.BoundingRadius + 235)
                                    {
                                        EmulateDamage(attacker, hero, data, EventType.Spell, "spell.linear.explosion");
                                    }
                                }

                                EmulateDamage(attacker, hero, data, EventType.Spell, "spell.linear", 0f, (int)endtime);
                            }
                        }

                        #endregion

                        #region skillshot circle/cone

                        if (data.CastType == CastType.Circlular || data.CastType == CastType.Sector)
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

                            if (data.Radius < 1f)
                            {
                                data.Radius = args.SpellData.CastRadiusSecondary > 0
                                    ? args.SpellData.CastRadiusSecondary
                                    : args.SpellData.CastRadius;
                            }

                            var startpos = fromobj?.Position ?? attacker.ServerPosition;
                            if (hero.Instance.Distance(startpos) > data.CastRange + 35)
                            {
                                continue;
                            }

                            if ((data.SpellName == "azirq" || data.SpellName == "azire") && fromobj == null)
                            {
                                continue;
                            }

                            var distance = (int)(1000 *
                                                 (startpos.Distance(hero.Instance.ServerPosition)
                                                  / data.MissileSpeed));

                            var endtime = data.Delay + distance - Game.Ping / 2f;
                            var direction = (args.End.To2D() - startpos.To2D()).Normalized();
                            var endpos = startpos.To2D() + direction * startpos.To2D().Distance(args.End.To2D());

                            if (startpos.To2D().Distance(endpos) > data.CastRange)
                            {
                                endpos = startpos.To2D() + direction * data.CastRange;
                            }

                            var evadetime = (int)(1000 *
                                                  (data.Radius - hero.Instance.Distance(startpos) + hero.Instance.BoundingRadius)
                                                  / hero.Instance.MoveSpeed);

                            if (hero.Instance.Distance(endpos) <= data.Radius + hero.Instance.BoundingRadius + 35)
                            {
                                if (evadetime > endtime)
                                {
                                    EmulateDamage(attacker, hero, data, EventType.Spell, "spell.circular", 0f, (int)endtime);
                                }
                            }
                        }

                        #endregion

                        #region unit type

                        if (data.CastType == CastType.Targeted)
                        {
                            if (args.Target == null || args.Target.Type != GameObjectType.obj_AI_Hero)
                            {
                                continue;
                            }

                            if (hero.Instance.NetworkId == args.Target.NetworkId)
                            {
                                if (hero.Instance.Distance(attacker.ServerPosition) <= data.CastRange + 100)
                                {
                                    var distance =
                                        (int)(1000 * (attacker.Distance(hero.Instance.ServerPosition)
                                                      / data.MissileSpeed));

                                    var endtime = data.Delay + distance - Game.Ping / 2f;
                                    EmulateDamage(attacker, hero, data, EventType.Spell, "spell.targeted", 0f, (int)endtime);
                                }
                            }
                        }

                        #endregion
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.Hero");
                    Console.WriteLine(e);
                }
            }

            try
            {
                #region Turret

                if (sender.Type == GameObjectType.obj_AI_Turret && args.Target is Obj_AI_Hero)
                {
                    var turret = sender as Obj_AI_Turret;
                    if (turret != null && turret.IsValid)
                    {
                        foreach (var hero in ZLib.GetUnits())
                        {
                            if (args.Target.NetworkId == hero.Instance.NetworkId)
                            {
                                if (turret.Distance(hero.Instance.ServerPosition) <= 900
                                    && Player.Distance(hero.Instance.ServerPosition) <= 1000)
                                {
                                    EmulateDamage(turret, hero, new Gamedata { SpellName = args.SpellData.Name },
                                        EventType.TurretAttack, "enemy.turret");
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.Turret");
                    Console.WriteLine(e);
                }
            }

            try
            {
                #region Minion

                if (sender.Type == GameObjectType.obj_AI_Minion && args.Target.Type == Player.Type)
                {
                    var minion = sender as Obj_AI_Minion;
                    if (minion != null && minion.IsValidTarget())
                    {
                        foreach (var hero in ZLib.GetUnits())
                        {
                            if (hero.Instance.NetworkId == args.Target.NetworkId)
                            {
                                if (hero.Instance.Distance(minion.ServerPosition) <= 750
                                    && Player.Distance(hero.Instance.ServerPosition) <= 1000)
                                {
                                    EmulateDamage(minion, hero, new Gamedata { SpellName = args.SpellData.Name },
                                        EventType.MinionAttack, "enemy.minion");
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.Minion");
                    Console.WriteLine(e);
                }
            }

            try
            {
                #region Gangplank Barrel

                if (sender.Type == GameObjectType.obj_AI_Hero)
                {
                    var attacker = sender as Obj_AI_Hero;
                    if (attacker.ChampionName.Equals("Gangplank") && attacker.IsValid)
                    {
                        var data = ZLib.CachedSpells.Find(x => x.SpellName.ToLower() == "gangplanke");
                        if (data != null)
                        {
                            foreach (var hero in ZLib.GetUnits())
                            {
                                var gplist = new List<Obj_AI_Minion>();

                                gplist.AddRange(ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(
                                        x =>
                                            x.UnitSkinName.ToLower() == "gangplankbarrel" &&
                                            x.Position.Distance(x.Position) <= 375 && x.IsFloatingHealthBarActive)
                                    .OrderBy(y => y.Position.Distance(hero.Instance.ServerPosition)));

                                foreach (var obj in gplist)
                                {
                                    if (hero.Instance.Distance(obj.Position) <= 375 && args.Target.Name == "Barrel")
                                    {
                                        var dmg = (float)Math.Abs(attacker.GetAutoAttackDamage(hero.Instance) * 1.2 + 150);

                                        if (args.SpellData.Name.ToLower().Contains("crit"))
                                        {
                                            dmg = dmg * 2;
                                        }

                                        EmulateDamage(attacker, hero, data, EventType.Spell, "enemy.gankplankbarrel", dmg);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.GangplankBarrel");
                    Console.WriteLine(e);
                }
            }

            try
            {
                #region Items

                if (sender.Type == GameObjectType.obj_AI_Hero)
                {
                    var attacker = sender as Obj_AI_Hero;
                    if (attacker != null && attacker.IsValid)
                    {
                        if (args.SpellData.TargettingType == (ulong)ProcessSpellType.Targeted)
                        {
                            foreach (var hero in ZLib.GetUnits())
                            {
                                if (args.Target.NetworkId != hero.Instance.NetworkId)
                                {
                                    continue;
                                }

                                // todo: item damage

                                if (args.SpellData.Name.ToLower() == "bilgewatercutlass")
                                {
                                    var dmg = (float)0;
                                    EmulateDamage(attacker, hero, new Gamedata { SpellName = args.SpellData.Name }, EventType.Item,
                                        "enemy.itemcast", dmg);
                                }

                                if (args.SpellData.Name.ToLower() == "itemswordoffeastandfamine")
                                {
                                    var dmg = (float)0;
                                    EmulateDamage(attacker, hero, new Gamedata { SpellName = args.SpellData.Name }, EventType.Item,
                                        "enemy.itemcast", dmg);
                                }

                                if (args.SpellData.Name.ToLower() == "hextechgunblade")
                                {
                                    var dmg = (float)0;
                                    EmulateDamage(attacker, hero, new Gamedata { SpellName = args.SpellData.Name }, EventType.Item,
                                        "enemy.itemcast", dmg);
                                }
                            }
                        }

                        // todo:
                        if (args.SpellData.TargettingType == (ulong)ProcessSpellType.Self)
                        {
                            foreach (var hero in ZLib.GetUnits())
                            {
                                if (args.SpellData.Name.ToLower() == "itemtiamatcleave")
                                {
                                    if (attacker.Distance(hero.Instance.ServerPosition) <= 375)
                                    {
                                        var dmg = (float)0;
                                        EmulateDamage(attacker, hero, new Gamedata { SpellName = args.SpellData.Name }, EventType.Item,
                                            "enemy.itemcast", dmg);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.Items");
                    Console.WriteLine(e);
                }
            }

            try
            {
                #region LucianQ

                if (args.SpellData.Name.ToLower() == "lucianq")
                {
                    var data = ZLib.CachedSpells.Find(x => x.SpellName.ToLower() == "lucianq");
                    if (data != null)
                    {
                        foreach (var hero in ZLib.GetUnits())
                        {
                            var delay = ((350 - Game.Ping) / 1000f);

                            var herodir = (hero.Instance.ServerPosition - hero.Instance.Position).Normalized();
                            var expectedpos = args.Target.Position + herodir * hero.Instance.MoveSpeed * (delay);

                            if (args.Start.Distance(expectedpos) < 1100)
                                expectedpos = args.Target.Position +
                                              (args.Target.Position - sender.ServerPosition).Normalized() * 800;

                            var proj = hero.Instance.ServerPosition.To2D().ProjectOn(args.Start.To2D(), expectedpos.To2D());
                            var projdist = hero.Instance.ServerPosition.To2D().Distance(proj.SegmentPoint);

                            if (100 + hero.Instance.BoundingRadius > projdist)
                            {
                                EmulateDamage(sender, hero, data, EventType.Spell, "enemy.lucianq");
                            }
                        }
                    }
                }

                #endregion
            }

            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Projections.LucianQ");
                    Console.WriteLine(e);
                }
            }
        }

        internal static void Obj_AI_Base_OnStealth(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            #region Stealth

            var attacker = sender as Obj_AI_Hero;
            if (attacker == null || attacker.IsAlly || !attacker.IsValid)
            {
                return;
            }

            foreach (var hero in ZLib.GetUnits().Where(h => h.Instance.Distance(attacker) <= 1000))
            {
                foreach (var entry in ZLib.CachedSpells.Where(s => s.EventTypes.Contains(EventType.Stealth)))
                {
                    if (entry.SpellName.ToLower() == args.SpellData.Name.ToLower())
                    {
                        EmulateDamage(sender, hero, new Gamedata { SpellName = "Stealth" }, EventType.Stealth, "process.onstealth");
                        break;
                    }
                }
            }

            #endregion
        }

        internal static void EmulateDamage(Obj_AI_Base sender, Unit hero, Gamedata data, EventType dmgType, string notes = null,
            float dmgEntry = 0f, int expiry = 500)
        {
            var hpred = new HPInstance();
            hpred.EventType = dmgType;
            hpred.Target = hero.Instance;
            hpred.GameData = data;
            hpred.Name = string.Empty;

            if (sender != null)
            {
                hpred.Attacker = sender;
            }

            if (!string.IsNullOrEmpty(data?.SpellName))
            {
                hpred.Name = data.SpellName;
            }

            if (dmgEntry < 1f && sender != null)
            {
                switch (dmgType)
                {
                    case EventType.AutoAttack:
                        hpred.PredictedDmg = (float)Math.Max(sender.GetAutoAttackDamage(hero.Instance), 0);
                        break;

                    case EventType.MinionAttack:
                    case EventType.TurretAttack:
                        hpred.PredictedDmg =
                            (float)
                            Math.Max(sender.CalculateDamage(hero.Instance, DamageType.Physical,
                                sender.BaseAttackDamage + sender.FlatPhysicalDamageMod), 0);
                        break;

                    default:
                        var aiHero = sender as Obj_AI_Hero;
                        if (aiHero != null)
                        {
                            if (!string.IsNullOrEmpty(data?.SpellName))
                            {
                                hpred.PredictedDmg = (float)Math.Max(aiHero.GetSpellDamage(hero.Instance, data.Slot), 0);
                            }
                        }

                        break;
                }
            }

            else
            {
                var idmg = dmgEntry;
                hpred.PredictedDmg = (float)Math.Round(idmg);
            }

            if (hpred.PredictedDmg > 0)
            {
                var idmg = hpred.PredictedDmg * ZLib.Menu["weightdmg"].As<MenuSlider>().Value / 100;
                hpred.PredictedDmg = (float)Math.Round(idmg);
            }

            else
            {
                var idmg = (hero.Instance.Health / hero.Instance.MaxHealth) * 5;
                hpred.PredictedDmg = (float)Math.Round(idmg);
            }

            if (dmgType != EventType.Buff && dmgType != EventType.Troy)
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

        internal static int AddDamage(HPInstance hpi, Unit hero, string notes)
        {
            Id++;
            var id = Id;

            var aiHero = ZLib.GetUnits().FirstOrDefault(x => x.Instance.NetworkId == hero.Instance.NetworkId);
            if (aiHero != null && !ZLib.DamageCollection.ContainsKey(id))
            {
                aiHero.Attacker = hpi.Attacker;

                if (aiHero.Instance.IsValid)
                {
                    switch (hpi.EventType)
                    {
                        case EventType.Spell:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Spell);
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

                        case EventType.MinionAttack:
                            aiHero.MinionDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.MinionAttack);
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
                        case EventType.Danger:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Danger);
                            break;
                        case EventType.CrowdControl:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.CrowdControl);
                            break;
                        case EventType.Ultimate:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.Ultimate);
                            break;
                        case EventType.ForceExhaust:
                            aiHero.AbilityDamage += hpi.PredictedDmg;
                            aiHero.Events.Add(EventType.ForceExhaust);
                            break;
                    }

                    if (hpi.EventType == EventType.Stealth)
                    {
                        hpi.PredictedDmg = 0;
                    }

                    var damageEventArgs = new PredictDamageEventArgs { HpInstance = hpi };
                    ZLib.TriggerOnPredictDamage(hero, damageEventArgs);

                    if (damageEventArgs.NoProcess)
                    {
                        Helpers.ResetIncomeDamage(aiHero);
                        ZLib.DamageCollection.Add(id, null);
                        return id;
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

                var aiHero = ZLib.GetUnits().FirstOrDefault(x => x.Instance.NetworkId == hpi.Target.NetworkId);
                if (aiHero != null)
                {
                    switch (hpi.EventType)
                    {
                        case EventType.Spell:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Spell);
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

                        case EventType.MinionAttack:
                            aiHero.MinionDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.MinionAttack);
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
                        case EventType.Danger:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Danger);
                            break;
                        case EventType.CrowdControl:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.CrowdControl);
                            break;
                        case EventType.Ultimate:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.Ultimate);
                            break;
                        case EventType.ForceExhaust:
                            aiHero.AbilityDamage -= hpi.PredictedDmg;
                            aiHero.Events.Remove(EventType.ForceExhaust);
                            break;
                    }

                    aiHero.Attacker = null;
                    ZLib.DamageCollection.Remove(id);
                }

                else
                {
                    // if this block is reached the hero is dead
                    var deadHero = ZLib.GetUnits().FirstOrDefault(x => x.Instance.NetworkId == hpi.Target.NetworkId);
                    if (deadHero != null)
                    {
                        Helpers.ResetIncomeDamage(deadHero);
                    }
                }
            }
        }

        internal static void Initizialize()
        {
            Player = ObjectManager.GetLocalPlayer();
            GameObject.OnCreate += MissileClient_OnSpellMissileCreate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnUnitSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnStealth;
        }

        #endregion
    }
}
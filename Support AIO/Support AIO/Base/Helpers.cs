#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Base/Helpers.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Support_AIO.Base
{
    #region

    using System;
    using System.IO;
    using System.Linq;
    using Aimtec;
    using Data;

    #endregion

    #region Public Enums

    /// <summary>
    ///     Enum EventType
    /// </summary>
    public enum EventType
    {
        /// <summary>
        ///     The Non-important event type
        /// </summary>
        None,

        /// <summary>
        ///     The auto attack event type
        /// </summary>
        AutoAttack,

        /// <summary>
        ///     The minion attack event type
        /// </summary>
        MinionAttack,

        /// <summary>
        ///     The turret attack event type
        /// </summary>
        TurretAttack,

        /// <summary>
        ///     The spell event type
        /// </summary>
        Spell,

        /// <summary>
        ///     The dangerevent type
        /// </summary>
        Danger,

        /// <summary>
        ///     The ultimate spell event type
        /// </summary>
        Ultimate,

        /// <summary>
        ///     The crowd control event type
        /// </summary>
        CrowdControl,

        /// <summary>
        ///     The stealth event type
        /// </summary>
        Stealth,

        /// <summary>
        ///     The force exhaust event type
        /// </summary>
        ForceExhaust,

        /// <summary>
        ///     The initiator event type
        /// </summary>
        Initiator,

        /// <summary>
        ///     The troy event type
        /// </summary>
        Troy,

        /// <summary>
        ///     The item event type
        /// </summary>
        Item,

        /// <summary>
        ///     The buff event type
        /// </summary>
        Buff
    }

    /// <summary>
    ///     Enum CollisionObjectType
    /// </summary>
    public enum CollisionObjectType
    {
        /// <summary>
        ///     The ally heroes
        /// </summary>
        AllyHeroes,

        /// <summary>
        ///     The enemy heroes
        /// </summary>
        EnemyHeroes,

        /// <summary>
        ///     The ally minions
        /// </summary>
        AllyMinions,

        /// <summary>
        ///     The enemy minions
        /// </summary>
        EnemyMinions,

        /// <summary>
        ///     The terrain
        /// </summary>
        Terrain
    }

    public enum CastType
    {
        Linear,
        MissileLinear,
        LinearAoE,
        MissileLinearAoE,
        Proximity,
        Targeted,
        Circlular,
        Sector,
        None
    }

    /// <summary>
    ///     Enum ProcessSpellType
    /// </summary>
    public enum ProcessSpellType
    {
        /// <summary>
        ///     The self process type
        /// </summary>
        Self = 0,

        /// <summary>
        ///     The targeted process type
        /// </summary>
        Targeted = 1,

        /// <summary>
        ///     The location circle process type
        /// </summary>
        LocationCircle = 2,

        /// <summary>
        ///     The location cone process type
        /// </summary>
        LocationCone = 3,

        /// <summary>
        ///     The self and unit process type
        /// </summary>
        SelfAndUnit = 4,

        /// <summary>
        ///     The self aoe/location something idk process type ?
        /// </summary>
        UnitAndAoE = 5,

        /// <summary>
        ///     The location line process type
        /// </summary>
        LocationLineMissile = 6,

        /// <summary>
        ///     The location line2 process type
        /// </summary>
        LocationLine = 7,

        /// <summary>
        ///     The location line3 process type
        /// </summary>
        LocationUnknown = 11
    }

    #endregion

    internal class Helpers
    {
        #region Internal Methods and Operators

        /// <summary>
        ///     Returns if the matched hero is valid and in the current game.
        /// </summary>
        /// <param name="heroname">The heroname.</param>
        /// <returns>
        ///     <c>true</c> if hero is in game, <c>false</c> otherwise.
        /// </returns>
        internal static bool IsHeroInGame(string heroname)
        {
            return ZLib.GetUnits().ToList().Exists(x => x.Instance is Obj_AI_Hero
                                                        && ((Obj_AI_Hero)x.Instance).ChampionName.ToLower() == heroname.ToLower());
        }

        /// <summary>
        ///     Will try to reset income damage if target is not valid.
        /// </summary>
        /// <param name="hero">The hero to reset damage. </param>
        internal static void ResetIncomeDamage(Unit hero)
        {
            hero.Attacker = null;
            hero.BuffDamage = 0;
            hero.TroyDamage = 0;
            hero.AbilityDamage = 0;
            hero.MinionDamage = 0;
            hero.TowerDamage = 0;
            hero.Events.Clear();
        }

        /// <summary>
        ///     Creates the log path for error reporting.
        /// </summary>
        internal static void CreateLogPath()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ZZephyr");

            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Exports the spell data to AppData\Local\AimtecLoader\Data\System\Logs.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        internal static void ExportSpellData(Gamedata data, string type = null)
        {
            var path =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ZZephyr",
                    $"zzephyr_{data.ChampionName.ToLower()}.txt");

            var file = new StreamWriter(path, true);
            if (data.SpellName.Contains("attack"))
            {
                return;
            }

            file.WriteLine(@"#region Spelldata dumper © 2015 Kurisu Solutions");
            file.WriteLine(@"// Dumps spell data from the client into a text file.");
            file.WriteLine(@"// {0}", DateTime.Now.ToString("F"));
            file.WriteLine(@"#endregion");
            file.WriteLine(@"");
            file.WriteLine(@"ZLib.Spells.Add(new Gamedata");
            file.WriteLine(@"{");
            file.WriteLine(@"    // TargetingType = ""{0}"",", type);
            file.WriteLine(@"    SpellName = ""{0}"",", data.SpellName.ToLower());
            file.WriteLine(@"    ChampionName = ""{0}"",", data.ChampionName.ToLower());
            file.WriteLine(@"    Slot = SpellSlot.{0},", data.Slot);
            file.WriteLine(@"    CastRange = {0},", data.CastRange);
            file.WriteLine(@"    Radius = {0},", data.Radius);
            file.WriteLine(@"    Delay = {0}f,", data.Delay);
            file.WriteLine(@"    EventTypes = new[] {{ }},");
            file.WriteLine(@"    FixedRange = true,");
            file.WriteLine(@"    MissileName = """",");
            file.WriteLine(@"    EventTypes = new EventType[] {{ }},");
            file.WriteLine(@"    MissileSpeed = {0},", data.MissileSpeed);
            file.WriteLine(@"});");
            file.WriteLine(@"");
            file.Close();
        }

        /// <summary>
        ///     Returns if the minion is an "Epic" minion (baron, dragon, etc)
        /// </summary>
        /// <param name="minion">The minion.</param>
        /// <returns>
        ///     <c>true</c> if the epic minion is the specified minion; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsEpicMinion(Obj_AI_Base minion)
        {
            var name = minion.Name;
            return minion is Obj_AI_Minion &&
                   (name.StartsWith("SRU_Baron") || name.StartsWith("SRU_Dragon") ||
                    name.StartsWith("SRU_RiftHerald") || name.StartsWith("TT_Spiderboss"));
        }

        /// <summary>
        ///     Determines whether the specified unit is a crab.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>
        ///     <c>true</c> if the specified unit is a crab; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsCrab(Obj_AI_Base unit)
        {
            return unit.Name.StartsWith("Sru_Crab");
        }

        #endregion
    }
}
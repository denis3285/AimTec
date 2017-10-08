#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Document:	Data/Gamedata.cs
// Date:		28/07/2016
// Author:		Robin Kurisu

#endregion

namespace Potato_AIO.Data
{
    #region

    using System;
    using System.Linq;
    using Aimtec;
    using Base;

    #endregion

    /// <summary>
    ///     Class Gamedata.
    /// </summary>
    public class Gamedata
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the spell.
        /// </summary>
        /// <value>
        ///     The name of the spell.
        /// </value>
        public string SpellName { get; internal set; } = "";

        /// <summary>
        ///     Gets or sets the name of the champion.
        /// </summary>
        /// <value>
        ///     The name of the champion.
        /// </value>
        public string ChampionName { get; internal set; } = "";

        /// <summary>
        ///     Gets or sets the slot.
        /// </summary>
        /// <value>
        ///     The slot.
        /// </value>
        public SpellSlot Slot { get; internal set; } = SpellSlot.Q;

        /// <summary>
        ///     Gets or sets the cast range.
        /// </summary>
        /// <value>
        ///     The cast range.
        /// </value>
        public float CastRange { get; internal set; } = 0f;

        /// <summary>
        ///     Gets or sets the radius.
        /// </summary>
        /// <value>
        ///     The radius.
        /// </value>
        public float Radius { get; internal set; } = 0f;

        /// <summary>
        ///     Gets or sets a value indicating whether spell is global.
        /// </summary>
        /// <value>
        ///     <c>true</c> if spell is global; otherwise, <c>false</c>.
        /// </value>
        public bool Global { get; internal set; } = false;

        /// <summary>
        ///     Gets or sets the delay.
        /// </summary>
        /// <value>
        ///     The delay.
        /// </value>
        public float Delay { get; internal set; } = 250f;

        /// <summary>
        ///     Gets or sets a value indicating whether is fixed range.
        /// </summary>
        /// <value>
        ///     <c>true</c> if is fixed range; otherwise, <c>false</c>.
        /// </value>
        public bool FixedRange { get; internal set; } = false;

        /// <summary>
        ///     Gets or sets the name of the missile.
        /// </summary>
        /// <value>
        ///     The name of the missile.
        /// </value>
        public string MissileName { get; internal set; } = "";

        /// <summary>
        ///     Gets or sets the extra missile names.
        /// </summary>
        /// <value>
        ///     The extra missile names.
        /// </value>
        public string[] ExtraMissileNames { get; internal set; } = { "" };

        /// <summary>
        ///     Gets or sets the missile speed.
        /// </summary>
        /// <value>
        ///     The missile speed.
        /// </value>
        public int MissileSpeed { get; internal set; } = 4800;

        /// <summary>
        ///     Gets or sets from object.
        /// </summary>
        /// <value>
        ///     From object.
        /// </value>
        public string[] FromObject { get; internal set; } = { "" };

        /// <summary>
        ///     Gets or sets the event types.
        /// </summary>
        /// <value>
        ///     The event types.
        /// </value>
        public EventType[] EventTypes { get; internal set; } = {};

        /// <summary>
        ///     Gets the type of the cast.
        /// </summary>
        /// <value>
        ///     The type of the cast.
        /// </value>
        public CastType CastType { get; internal set; } = CastType.None;

        /// <summary>
        ///     Gets the collisional values
        /// </summary>
        /// <value>
        ///     What it collides with.
        /// </value>
        public CollisionObjectType[] CollidesWith { get; internal set; } = { };

        /// <summary>
        ///     Gets a value indicating whether this instance is perpindicular.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is perpindicular; otherwise, <c>false</c>.
        /// </value>
        public bool IsPerpindicular { get; internal set; } = false;

        /// <summary>
        ///     Gets a value indicating whether the spell is a basic attack amplifier.
        /// </summary>
        /// <value><c>true</c> if it is a basic attack amplifier; otherwise, <c>false</c>.</value>
        public bool BasicAttackAmplifier { get; internal set; } = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes the game data spell list.
        /// </summary>
        static Gamedata()
        {
            Console.WriteLine("ZLib: Gamedata Loaded!");

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aatroxq",
                ChampionName = "aatrox",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 875f,
                Radius = 200f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aatroxw",
                ChampionName = "aatrox",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                BasicAttackAmplifier = true,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aatroxw2",
                ChampionName = "aatrox",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                BasicAttackAmplifier = true,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aatroxe",
                ChampionName = "aatrox",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1025f,
                Radius = 150f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "aatroxeconemissile",
                MissileSpeed = 1250
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aatroxr",
                ChampionName = "aatrox",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ahriorbofdeception",
                ChampionName = "ahri",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 900f,
                Radius = 80f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "ahriorbmissile",
                ExtraMissileNames = new[] { "ahriorbreturn" },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ahrifoxfire",
                ChampionName = "ahri",
                Slot = SpellSlot.W,
                FixedRange = true,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Radius = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ahriseduce",
                ChampionName = "ahri",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 975f,
                Radius = 60f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "ahriseducemissile",
                MissileSpeed = 1550
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ahritumble",
                ChampionName = "ahri",
                Slot = SpellSlot.R,
                CastType = CastType.LinearAoE,
                CastRange = 450f,
                Radius = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "akalimota",
                ChampionName = "akali",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                Radius = 171.9f,
                CastRange = 600f,
                Delay = 650f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "akalismokebomb",
                ChampionName = "akali",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 1000f, // Range: 700 + additional for stealth detection
                Delay = 500f,
                EventTypes = new[] { EventType.Stealth },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "akalishadowswipe",
                ChampionName = "akali",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                Radius = 325f,
                CastRange = 325f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "akalishadowdance",
                ChampionName = "akali",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                Radius = 300f,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pulverize",
                ChampionName = "alistar",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 330f,
                Radius = 365f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "headbutt",
                ChampionName = "alistar",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 660f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "alistare",
                ChampionName = "alistar",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Radius = 300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "feroucioushowl",
                ChampionName = "alistar",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bandagetoss",
                ChampionName = "amumu",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1100f,
                Radius = 80f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "sadmummybandagetoss",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "auraofdespair",
                ChampionName = "amumu",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tantrum",
                ChampionName = "amumu",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 150f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "curseofthesadmummy",
                ChampionName = "amumu",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 560f,
                Radius = 560f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "flashfrost",
                ChampionName = "anivia",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1150f, // 1075 + Shatter Radius
                Radius = 110f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "flashfrostspell",
                MissileSpeed = 850
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "crystalize",
                ChampionName = "anivia",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                IsPerpindicular = true,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "frostbite",
                ChampionName = "anivia",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "glacialstorm",
                ChampionName = "anivia",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "disintegrate",
                ChampionName = "annie",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Radius = 710f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "incinerate",
                ChampionName = "annie",
                Slot = SpellSlot.W,
                CastType = CastType.Sector,
                CastRange = 625f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "moltenshield",
                ChampionName = "annie",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "infernalguardian",
                ChampionName = "annie",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 900f, // 600 + Cast Radius
                Delay = 0f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "frostshot",
                ChampionName = "ashe",
                Slot = SpellSlot.Q,
                BasicAttackAmplifier = true,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "frostarrow",
                ChampionName = "ashe",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "volley",
                ChampionName = "ashe",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1200f,
                Radius = 250f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "volleyattack",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ashespiritofthehawk",
                ChampionName = "ashe",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "enchantedcrystalarrow",
                ChampionName = "ashe",
                Slot = SpellSlot.R,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 20000f,
                Global = true,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "enchantedcrystalarrow",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aurelionsolq",
                ChampionName = "aurelionsol",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 1500f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "aurelionsolqmissile",
                MissileSpeed = 850
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aurelionsolw",
                ChampionName = "aurelionsol",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.None },
                MissileName = "aurelionsolwmis",
                MissileSpeed = 450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aurelionsole",
                ChampionName = "aurelionsol",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.None },
                MissileName = "aurelionsole",
                MissileSpeed = 900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aurelionsolr",
                ChampionName = "aurelionsol",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 1420f,
                Delay = 300f,
                EventTypes = new[]
                {
                    EventType.CrowdControl, EventType.Ultimate, EventType.Danger,
                    EventType.Initiator
                },
                MissileName = "aurelionsolrbeammissile",
                MissileSpeed = 4600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "azirq",
                ChampionName = "azir",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 875f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "azirqmissile",
                FromObject = new[] { "AzirSoldier" },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "azirqwrapper",
                ChampionName = "azir",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 875f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "azirqmissile",
                FromObject = new[] { "AzirSoldier" },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "azirr",
                ChampionName = "azir",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 475f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bardq",
                ChampionName = "bard",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "bardqmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bardw",
                ChampionName = "bard",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "barde",
                ChampionName = "bard",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 350f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bardr",
                ChampionName = "bard",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 3400f,
                Delay = 450f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileName = "bardr",
                MissileSpeed = 2100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rocketgrab",
                ChampionName = "blitzcrank",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1050f,
                Radius = 70f,
                Delay = 250f,
                MissileName = "rocketgrabmissile",
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "overdrive",
                ChampionName = "blitzcrank",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Radius = 100f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "powerfist",
                ChampionName = "blitzcrank",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "staticfield",
                ChampionName = "blitzcrank",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                FixedRange = true,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "brandq",
                ChampionName = "brand",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1150f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "brandqmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "brandw",
                ChampionName = "brand",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 240f,
                Delay = 550f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "",
                MissileSpeed = 20
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "brande",
                ChampionName = "brand",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "brandr",
                ChampionName = "brand",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 750f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "braumq",
                ChampionName = "braum",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1100f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "braumqmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "braumqmissle",
                ChampionName = "braum",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1100f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "braumw",
                ChampionName = "braum",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "braume",
                ChampionName = "braum",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "braumrwrapper",
                ChampionName = "braum",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1250f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "braumrmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "camilleq",
                ChampionName = "camille",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 325f,
                Radius = 325f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "camillew",
                ChampionName = "camille",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 325f,
                Radius = 325f,
                Delay = 500f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2696
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "camillee",
                ChampionName = "camille",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1100f,
                Radius = 35f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "camilleemissile",
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "camilleedash2",
                ChampionName = "camille",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 800f,
                Radius = 165f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "camiller",
                ChampionName = "camille",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 475f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 11200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "caitlynpiltoverpeacemaker",
                ChampionName = "caitlyn",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                Radius = 60f,
                CastRange = 1300f,
                Delay = 450f,
                EventTypes = new EventType[] { },
                MissileName = "caitlynpiltoverpeacemaker",
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "caitlynyordletrap",
                ChampionName = "caitlyn",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                Radius = 75f,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "caitlynentrapment",
                ChampionName = "caitlyn",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                Radius = 70f,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "caitlynentrapmentmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "caitlynaceinthehole",
                ChampionName = "caitlyn",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 2000f,
                Radius = 100f,
                Delay = 900f,
                FixedRange = false,
                MissileName = "",
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cassiopeiaq",
                ChampionName = "cassiopeia",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "cassiopeianoxiousblast",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cassiopeiw",
                ChampionName = "cassiopeia",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                IsPerpindicular = true,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 2500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cassiopeiae",
                ChampionName = "cassiopeia",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cassiopeiar",
                ChampionName = "cassiopeia",
                Slot = SpellSlot.R,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 875f,
                Delay = 350f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "cassiopeiar",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cassiopeiarstun",
                ChampionName = "cassiopeia",
                Slot = SpellSlot.R,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 875f,
                Delay = 350f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "cassiopeiarstun",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rupture",
                ChampionName = "chogath",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 950f,
                Radius = 250f,
                Delay = 900f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "feralscream",
                ChampionName = "chogath",
                Slot = SpellSlot.W,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 300f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vorpalspikes",
                ChampionName = "chogath",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 347
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "feast",
                ChampionName = "chogath",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "phosphorusbomb",
                ChampionName = "corki",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 875f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "phosphorusbombmissile",
                MissileSpeed = 1125
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "carpetbomb",
                ChampionName = "corki",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ggun",
                ChampionName = "corki",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                CastRange = 750f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "missilebarrage",
                ChampionName = "corki",
                Slot = SpellSlot.R,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinearAoE,
                FixedRange = true,
                CastRange = 1225f,
                Delay = 150f,
                EventTypes = new EventType[] { },
                MissileName = "missilebarragemissile",
                ExtraMissileNames = new[] { "missilebarragemissile2" },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dariuscleave",
                ChampionName = "darius",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                FixedRange = true,
                Radius = 425f,
                CastRange = 425f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dariusnoxiantacticsonh",
                ChampionName = "darius",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 205f,
                Delay = 150f,
                EventTypes = new[] { EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dariusaxegrabcone",
                ChampionName = "darius",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 555f,
                Delay = 150f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger, EventType.Initiator },
                MissileName = "dariusaxegrabcone",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dariusexecute",
                ChampionName = "darius",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                Radius = 475f,
                CastRange = 475f,
                Delay = 450f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dianaarc",
                ChampionName = "diana",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 830f,
                Radius = 195f,
                Delay = 300f,
                EventTypes = new EventType[] { },
                MissileName = "dianaarc",
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dianaorbs",
                ChampionName = "diana",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 200f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dianavortex",
                ChampionName = "diana",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Radius = 450f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dianateleport",
                ChampionName = "diana",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 825f,
                Radius = 250f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dravenspinning",
                ChampionName = "draven",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dravenfury",
                ChampionName = "draven",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dravendoubleshot",
                ChampionName = "draven",
                Slot = SpellSlot.E,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "dravendoubleshotmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "dravenrcast",
                ChampionName = "draven",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 20000f,
                Global = true,
                Delay = 500f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "dravenr",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "infectedcleavermissilecast",
                ChampionName = "drmundo",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "infectedcleavermissile",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "burningagony",
                ChampionName = "drmundo",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "masochism",
                ChampionName = "drmundo",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sadism",
                ChampionName = "drmundo",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ekkoq",
                ChampionName = "ekko",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1075f,
                Radius = 60f,
                Delay = 66f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "ekkoqmis",
                ExtraMissileNames = new[] { "ekkoqreturn" },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ekkoeattack",
                ChampionName = "ekko",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 0f,
                EventTypes = new[] { EventType.Danger },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ekkor",
                ChampionName = "ekko",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 425f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                FromObject = new[] { "Ekko_Base_R_TrailEnd" },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisehumanq",
                ChampionName = "elise",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 550f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisespiderqcast",
                ChampionName = "elise",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 475f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisehumanw",
                ChampionName = "elise",
                Slot = SpellSlot.W,
                CastType = CastType.LinearAoE,
                CastRange = 0f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 5000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisespiderw",
                ChampionName = "elise",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisehumane",
                ChampionName = "elise",
                Slot = SpellSlot.E,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1075f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileName = "elisehumane",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisespidereinitial",
                ChampionName = "elise",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisespideredescent",
                ChampionName = "elise",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "eliser",
                ChampionName = "elise",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "elisespiderr",
                ChampionName = "elise",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "evelynnq",
                ChampionName = "evelynn",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                FixedRange = true,
                CastRange = 500f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "evelynnw",
                ChampionName = "evelynn",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "evelynne",
                ChampionName = "evelynn",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 225f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "evelynnr",
                ChampionName = "evelynn",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 900f, // 650f + Radius
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "evelynnr",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ezrealmysticshot",
                ChampionName = "ezreal",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1150f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "ezrealmysticshotmissile",
                ExtraMissileNames = new[] { "ezrealmysticshotpulsemissile" },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ezrealessenceflux",
                ChampionName = "ezreal",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "ezrealessencefluxmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ezrealessencemissle",
                ChampionName = "ezreal",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ezrealarcaneshift",
                ChampionName = "ezreal",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 750f, // 475f + Bolt Range
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ezrealtrueshotbarrage",
                ChampionName = "ezreal",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 20000f,
                Global = true,
                Delay = 1000f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "ezrealtrueshotbarrage",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "terrify",
                ChampionName = "fiddlesticks",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 575f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "drain",
                ChampionName = "fiddlesticks",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 575f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fiddlesticksdarkwind",
                ChampionName = "fiddlesticks",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 750f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "crowstorm",
                ChampionName = "fiddlesticks",
                Slot = SpellSlot.R,
                CastType = CastType.LinearAoE,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.ForceExhaust, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fioraq",
                ChampionName = "fiora",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fioraw",
                ChampionName = "fiora",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 750f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fiorae",
                ChampionName = "fiora",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fiorar",
                ChampionName = "fiora",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 500f,
                Delay = 150f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizzq",
                ChampionName = "fizz",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                FixedRange = true,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizzw",
                ChampionName = "fizz",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 175f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizze",
                ChampionName = "fizz",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 450f,
                Delay = 700f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizzebuffer",
                ChampionName = "fizz",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 330f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizzejumptwo",
                ChampionName = "fizz",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 270f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fizzr",
                ChampionName = "fizz",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1275f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "fizzmarinerdoommissile",
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galioq",
                ChampionName = "galio",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1200f,
                Radius = 60,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "galioqmissile",
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galioqsuper",
                ChampionName = "galio",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 200f,
                Radius = 200,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galiow",
                ChampionName = "galio",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 275f,
                Radius = 275f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galiow2",
                ChampionName = "galio",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 275f,
                Radius = 275f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galioe",
                ChampionName = "galio",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "galior",
                ChampionName = "galio",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gangplankqwrapper",
                ChampionName = "gangplank",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gangplankqproceed",
                ChampionName = "gangplank",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gangplankw",
                ChampionName = "gangplank",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gangplanke",
                ChampionName = "gangplank",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gangplankr",
                ChampionName = "gangplank",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 20000f,
                Global = true,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "garenq",
                ChampionName = "garen",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 300f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "garenqattack",
                ChampionName = "garen",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 350f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gnarq",
                ChampionName = "gnar",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1185f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 2400,
                MissileName = "gnarqmissile",
                ExtraMissileNames = new[] { "gnarqmissilereturn" }
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gnarbigq",
                ChampionName = "gnar",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1150f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 2000,
                MissileName = "gnarbigqmissile"
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gnarw",
                ChampionName = "gnar",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gnarbigw",
                ChampionName = "gnar",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 600f,
                Delay = 600f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gnarult",
                ChampionName = "gnar",
                CastRange = 600f,
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },

                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "garenw",
                ChampionName = "garen",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "garene",
                ChampionName = "garen",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 660f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "garenr",
                ChampionName = "garen",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 400f,
                Radius = 100f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 4800
            });

            // todo: improve gragas
            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragasq",
                ChampionName = "gragas",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 1000, // 850f + Radius
                Delay = 500f,
                EventTypes = new EventType[] { },
                MissileName = "gragasqmissile",
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragasqtoggle",
                ChampionName = "gragas",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 1000, // 850f + Radius
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileName = "gragasq"
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragasqtoggle",
                ChampionName = "gragas",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 1100f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragasw",
                ChampionName = "gragas",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragase",
                ChampionName = "gragas",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileName = "gragase",
                MissileSpeed = 550
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gragasr",
                ChampionName = "gragas",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 1300f,
                Radius = 120f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileName = "gragasrboom",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gravesq",
                ChampionName = "graves",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1025,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "gravesclustershotattack",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gravesw",
                ChampionName = "graves",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 1100f, // 950 + Radius
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gravese",
                ChampionName = "graves",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 278f,
                Delay = 300f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "gravesr",
                ChampionName = "graves",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinearAoE,
                CastRange = 1000f,
                FixedRange = true,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "graveschargeshotshot",
                MissileSpeed = 2100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hecarimrapidslash",
                ChampionName = "hecarim",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hecarimw",
                ChampionName = "hecarim",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hecarimramp",
                ChampionName = "hecarim",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hecarimult",
                ChampionName = "hecarim",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1525f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl
                    },

                MissileName = "hecarimultmissile",
                ExtraMissileNames =
                    new[]
                    {
                        "hecarimultmissileskn4r1", "hecarimultmissileskn4r2", "hecarimultmissileskn4l1",
                        "hecarimultmissileskn4l2", "hecarimultmissileskn4rc"
                    },
                MissileSpeed = 1100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingerturretenergyblast",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 435f,
                EventTypes = new EventType[] { },
                FromObject = new[] { "heimerdinger_turret_idle" },
                MissileSpeed = 1650
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingerturretbigenergyblast",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 350f,
                EventTypes = new EventType[] { },
                FromObject = new[] { "heimerdinger_base_r" },
                MissileSpeed = 1650
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingerw",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1100,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingere",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 1025f, // 925 + Radius
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "heimerdingerespell",
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingerr",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 230f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "heimerdingereult",
                ChampionName = "heimerdinger",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                FixedRange = true,
                CastRange = 1450f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ireliagatotsu",
                ChampionName = "irelia",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 150f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ireliahitenstyle",
                ChampionName = "irelia",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 230f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ireliaequilibriumstrike",
                ChampionName = "irelia",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 450f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ireliatranscendentblades",
                ChampionName = "irelia",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                FixedRange = true,
                CastRange = 1200f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileName = "ireliatranscendentbladesspell",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "illaoiq",
                ChampionName = "illaoi",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "illaoiemis",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "illaoiw",
                ChampionName = "illaoi",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 365f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "illaoie",
                ChampionName = "illaoi",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "illaoiemis",
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "illaoir",
                ChampionName = "illaoi",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Delay = 500f,
                EventTypes = new[] { EventType.Ultimate, EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ivernq",
                ChampionName = "ivern",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                Radius = 65f,
                CastRange = 1100f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "ivernq",
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ivernq",
                ChampionName = "ivern",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "ivernw",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "iverne",
                ChampionName = "ivern",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "iverne",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "howlinggalespell",
                ChampionName = "janna",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1550f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "howlinggalespell",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sowthewind",
                ChampionName = "janna",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "eyeofthestorm",
                ChampionName = "janna",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reapthewhirlwind",
                ChampionName = "janna",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 725f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jarvanivdragonstrike",
                ChampionName = "jarvaniv",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jarvanivgoldenaegis",
                ChampionName = "jarvaniv",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jarvanivdemacianstandard",
                ChampionName = "jarvaniv",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 830f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "jarvanivdemacianstandard",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jarvanivcataclysm",
                ChampionName = "jarvaniv",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 825f,
                Delay = 0f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaxleapstrike",
                ChampionName = "jax",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaxempowertwo",
                ChampionName = "jax",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaxrelentlessasssault",
                ChampionName = "jax",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycetotheskies",
                ChampionName = "jayce",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jayceshockblast",
                ChampionName = "jayce",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1570f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileName = "jayceshockblastmis",
                MissileSpeed = 2350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycestaticfield",
                ChampionName = "jayce",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 285f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycehypercharge",
                ChampionName = "jayce",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycethunderingblow",
                ChampionName = "jayce",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 325f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jayceaccelerationgate",
                ChampionName = "jayce",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                IsPerpindicular = true,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycestancehtg",
                ChampionName = "jayce",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jaycestancegth",
                ChampionName = "jayce",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jhinq",
                ChampionName = "jhin",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 575f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jhinw",
                ChampionName = "jhin",
                Slot = SpellSlot.W,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastType = CastType.Linear,
                CastRange = 2250f,
                Delay = 750f,
                FixedRange = true,
                EventTypes = new EventType[] { },
                MissileName = "jhinwmissile",
                MissileSpeed = 5000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jhine",
                ChampionName = "jhin",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 2250f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jhinrshot",
                ChampionName = "jhin",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 3500f,
                Delay = 250f,
                FixedRange = true,
                MissileName = "jhinrshotmis",
                EventTypes = new[] { EventType.Initiator },
                ExtraMissileNames = new[] { "jhinrmmissile", "jhinrshotmis4" },
                MissileSpeed = 5000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jinxq",
                ChampionName = "jinx",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jinxw",
                ChampionName = "jinx",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                Radius = 60f,
                FixedRange = true,
                CastRange = 1500f,
                Delay = 450f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "jinxwmissile",
                MissileSpeed = 3300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jinxe",
                ChampionName = "jinx",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Radius = 315f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jinxr",
                ChampionName = "jinx",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 25000f,
                Radius = 140f,
                Delay = 450f,
                MissileName = "jinxr",
                ExtraMissileNames = new[] { "jinxrwrapper" },
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "karmaq",
                ChampionName = "karma",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinearAoE,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "karmaqmissile",
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "karmaspiritbind",
                ChampionName = "karma",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "karmasolkimshield",
                ChampionName = "karma",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "karmamantra",
                ChampionName = "karma",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "laywaste",
                ChampionName = "karthus",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 875f,
                Delay = 900f,
                EventTypes = new EventType[] { },
                ExtraMissileNames = new[]
                {
                    "karthuslaywastea3", "karthuslaywastea1", "karthuslaywastedeada1", "karthuslaywastedeada2",
                    "karthuslaywastedeada3"
                },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "wallofpain",
                ChampionName = "karthus",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "defile",
                ChampionName = "karthus",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fallenone",
                ChampionName = "karthus",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 22000f,
                Global = true,
                Delay = 2800f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nulllance",
                ChampionName = "kassadin",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "netherblade",
                ChampionName = "kassadin",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "forcepulse",
                ChampionName = "kassadin",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "riftwalk",
                ChampionName = "kassadin",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 675f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "riftwalk",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "katarinaq",
                ChampionName = "katarina",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 675f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "katarinaw",
                ChampionName = "katarina",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "katarinae",
                ChampionName = "katarina",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "katarinar",
                ChampionName = "katarina",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[] { EventType.ForceExhaust, EventType.Initiator },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "judicatorreckoning",
                ChampionName = "kayle",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "judicatordevineblessing",
                ChampionName = "kayle",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 900f,
                Delay = 220f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "judicatorrighteousfury",
                ChampionName = "kayle",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "judicatorintervention",
                ChampionName = "kayle",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            #region Kayn

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kaynq",
                ChampionName = "kayn",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                FixedRange = true,
                CastRange = 345f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kaynw",
                ChampionName = "kayn",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 700f,
                Delay = 500f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kaynr",
                ChampionName = "kayn",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kaynrjumpout",
                ChampionName = "kayn",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            #endregion

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kennenshurikenhurlmissile1",
                ChampionName = "kennen",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1175f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "kennenshurikenhurlmissile1",
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kennenbringthelight",
                ChampionName = "kennen",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kennenlightningrush",
                ChampionName = "kennen",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kennenshurikenstorm",
                ChampionName = "kennen",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 550f,
                Delay = 500f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixq",
                ChampionName = "khazix",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 325f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixqlong",
                ChampionName = "khazix",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 375f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixw",
                ChampionName = "khazix",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "khazixwmissile",
                MissileSpeed = 81700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixwlong",
                ChampionName = "khazix",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixe",
                ChampionName = "khazix",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "khazixe",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixelong",
                ChampionName = "khazix",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixr",
                ChampionName = "khazix",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 0f,
                EventTypes = new[] { EventType.Stealth, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "khazixrlong",
                ChampionName = "khazix",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 0f,
                EventTypes = new[] { EventType.Stealth },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kindredq",
                ChampionName = "kindred",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kindrede",
                ChampionName = "kindred",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 510f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kledq",
                ChampionName = "kled",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 800f,
                Radius = 45f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "kledqmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kledriderq",
                ChampionName = "kled",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 700f,
                Radius = 40f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "kledriderqmissile",
                MissileSpeed = 3000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kledw",
                ChampionName = "kled",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "klede",
                ChampionName = "kled",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 800f,
                Radius = 124f,
                Delay = 0f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileName = "kledemissile",
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kledr",
                ChampionName = "kled",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kogmawq",
                ChampionName = "kogmaw",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "kogmawqmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kogmawbioarcanebarrage",
                ChampionName = "kogmaw",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kogmawvoidooze",
                ChampionName = "kogmaw",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1150f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "kogmawvoidoozemissile",
                MissileSpeed = 1250
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kogmawlivingartillery",
                ChampionName = "kogmaw",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 2200f,
                Delay = 1200f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancq",
                ChampionName = "leblanc",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancw",
                ChampionName = "leblanc",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileName = "leblancw",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblacwreturn",
                ChampionName = "leblanc",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblance",
                ChampionName = "leblanc",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "leblancemissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancrq",
                ChampionName = "leblanc",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancrw",
                ChampionName = "leblanc",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate, EventType.Initiator },
                MissileName = "leblancrw",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancrwreturn",
                ChampionName = "leblanc",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leblancre",
                ChampionName = "leblanc",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "leblancremissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkqone",
                ChampionName = "leesin",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "blindmonkqone",
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkqtwo",
                ChampionName = "leesin",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkwone",
                ChampionName = "leesin",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkwtwo",
                ChampionName = "leesin",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 700f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkeone",
                ChampionName = "leesin",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 425f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonketwo",
                ChampionName = "leesin",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindmonkrkick",
                ChampionName = "leesin",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 375f,
                Delay = 500f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leonashieldofdaybreak",
                ChampionName = "leona",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 215f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leonasolarbarrier",
                ChampionName = "leona",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 250f,
                Delay = 3000f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leonazenithblade",
                ChampionName = "leona",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 900f,
                Delay = 0f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "leonazenithblademissile",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "leonasolarflare",
                ChampionName = "leona",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 1200f,
                Delay = 1200f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "leonasolarflare",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lissandraq",
                ChampionName = "lissandra",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 725f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "lissandraqmissile",
                MissileSpeed = 2250
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lissandraw",
                ChampionName = "lissandra",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lissandrae",
                ChampionName = "lissandra",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "lissandraemissile",
                MissileSpeed = 850
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lissandrar",
                ChampionName = "lissandra",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[]
                {
                    EventType.CrowdControl, EventType.Initiator,
                    EventType.Danger, EventType.Ultimate
                },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lucianq",
                ChampionName = "lucian",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                FixedRange = true,
                CastRange = 1150f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "lucianq",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lucianw",
                ChampionName = "lucian",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinearAoE,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "lucianwmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luciane",
                ChampionName = "lucian",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lucianr",
                ChampionName = "lucian",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1400f,
                Radius = 110,
                Delay = 500f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "lucianrmissileoffhand",
                ExtraMissileNames = new[] { "lucianrmissile" },
                MissileSpeed = 2800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luluq",
                ChampionName = "lulu",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 925f,
                Radius = 60,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "luluqmissile",
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luluw",
                ChampionName = "lulu",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 640f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lulue",
                ChampionName = "lulu",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 640f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "lulur",
                ChampionName = "lulu",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luxlightbinding",
                ChampionName = "lux",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1300f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "luxlightbindingmis",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luxprismaticwave",
                ChampionName = "lux",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luxlightstrikekugel",
                ChampionName = "lux",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 1100f,
                Radius = 330f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "luxlightstrikekugel",
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luxlightstriketoggle",
                ChampionName = "lux",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "luxmalicecannon",
                ChampionName = "lux",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 3500f,
                Radius = 299.3f,
                Delay = 1000f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "luxmalicecannonmis",
                MissileSpeed = 3000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kalistamysticshot",
                ChampionName = "kalista",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastType = CastType.MissileLinear,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "kalistamysticshotmis",
                ExtraMissileNames = new[] { "kalistamysticshotmistrue" },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kalistaw",
                ChampionName = "kalista",
                Slot = SpellSlot.W,
                CastType = CastType.Linear,
                CastRange = 5000f,
                Delay = 800f,
                EventTypes = new EventType[] { },
                MissileSpeed = 200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "kalistaexpungewrapper",
                ChampionName = "kalista",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "seismicshard",
                ChampionName = "malphite",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "obduracy",
                ChampionName = "malphite",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "landslide",
                ChampionName = "malphite",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 400f,
                Delay = 500f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ufslash",
                ChampionName = "malphite",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "ufslash",
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "malzaharq",
                ChampionName = "malzahar",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                IsPerpindicular = true,
                CastRange = 900f,
                Delay = 600f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "alzaharcallofthevoid",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "malzaharw",
                ChampionName = "malzahar",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "malzahare",
                ChampionName = "malzahar",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "malzaharr",
                ChampionName = "malzahar",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "maokaiq",
                ChampionName = "maokai",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 650f,
                Radius = 110f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "maokaiqmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "maokaiw",
                ChampionName = "maokai",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 525f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "maokaie",
                ChampionName = "maokai",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 1100f,
                Radius = 120f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "maokaiemissile",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "maokair",
                ChampionName = "maokai",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinearAoE,
                Radius = 120f,
                CastRange = 3000f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "maokairmis",
                MissileSpeed = 400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "alphastrike",
                ChampionName = "masteryi",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 600f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "meditate",
                ChampionName = "masteryi",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "wujustyle",
                ChampionName = "masteryi",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 230f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "highlander",
                ChampionName = "masteryi",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 370f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "missfortunericochetshot",
                ChampionName = "missfortune",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "missfortuneviciousstrikes",
                ChampionName = "missfortune",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "missfortunescattershot",
                ChampionName = "missfortune",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "missfortunebullettime",
                ChampionName = "missfortune",
                Slot = SpellSlot.R,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 1400f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingdoubleattack",
                ChampionName = "monkeyking",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 20
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingdecoy",
                ChampionName = "monkeyking",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.Stealth },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingdecoyswipe",
                ChampionName = "monkeyking",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                FromObject = new[] { "Base_W_Copy" },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingnimbus",
                ChampionName = "monkeyking",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingspintowin",
                ChampionName = "monkeyking",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "monkeykingspintowinleave",
                ChampionName = "monkeyking",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "mordekaisermaceofspades",
                ChampionName = "mordekaiser",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "mordekaisercreepindeathcast",
                ChampionName = "mordekaiser",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 750f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "mordekaisersyphoneofdestruction",
                ChampionName = "mordekaiser",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "mordekaiserchildrenofthegrave",
                ChampionName = "mordekaiser",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "darkbindingmissile",
                ChampionName = "morgana",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1175f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "darkbindingmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tormentedsoil",
                ChampionName = "morgana",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blackshield",
                ChampionName = "morgana",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "soulshackles",
                ChampionName = "morgana",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "namiq",
                ChampionName = "nami",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 875f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileName = "namiqmissile",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "namiw",
                ChampionName = "nami",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 725f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "namie",
                ChampionName = "nami",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "namir",
                ChampionName = "nami",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 2550f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileName = "namirmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nasusq",
                ChampionName = "nasus",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 450f,
                Delay = 500f,
                EventTypes = new[] { EventType.Danger },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nasusw",
                ChampionName = "nasus",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nasuse",
                ChampionName = "nasus",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nasusr",
                ChampionName = "nasus",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nautilusanchordrag",
                ChampionName = "nautilus",
                Slot = SpellSlot.Q,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1080f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger, EventType.Initiator },
                MissileName = "nautilusanchordragmissile",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nautiluspiercinggaze",
                ChampionName = "nautilus",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nautilussplashzone",
                ChampionName = "nautilus",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nautilusgrandline",
                ChampionName = "nautilus",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 1250f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "javelintoss",
                ChampionName = "nidalee",
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                Slot = SpellSlot.Q,
                FixedRange = true,
                CastRange = 1500f,
                Radius = 299.3f,
                Delay = 125f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "javelintoss",
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "takedown",
                ChampionName = "nidalee",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 500f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bushwhack",
                ChampionName = "nidalee",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pounce",
                ChampionName = "nidalee",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 375f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "primalsurge",
                ChampionName = "nidalee",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "swipe",
                ChampionName = "nidalee",
                FixedRange = true,
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "aspectofthecougar",
                ChampionName = "nidalee",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nocturneduskbringer",
                ChampionName = "nocturne",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1125f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nocturneshroudofdarkness",
                ChampionName = "nocturne",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nocturneunspeakablehorror",
                ChampionName = "nocturne",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "nocturneparanoia",
                ChampionName = "nocturne",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 20000f,
                Global = true,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "consume",
                ChampionName = "nunu",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bloodboil",
                ChampionName = "nunu",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "iceblast",
                ChampionName = "nunu",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "absolutezero",
                ChampionName = "nunu",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "olafaxethrowcast",
                ChampionName = "olaf",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "olafaxethrow",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "olaffrenziedstrikes",
                ChampionName = "olaf",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "olafrecklessstrike",
                ChampionName = "olaf",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 325f,
                Delay = 500f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "olafragnarok",
                ChampionName = "olaf",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "orianaizunacommand",
                ChampionName = "orianna",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "orianaizuna",
                FromObject = new[] { "yomu_ring" },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "orianadissonancecommand",
                ChampionName = "orianna",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "orianadissonancecommand",
                FromObject = new[] { "yomu_ring" },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "orianaredactcommand",
                ChampionName = "orianna",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 1095f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "orianaredact",
                FromObject = new[] { "yomu_ring" },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "orianadetonatecommand",
                ChampionName = "orianna",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 425f,
                Delay = 500f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "orianadetonatecommand",
                FromObject = new[] { "yomu_ring" },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pantheonq",
                ChampionName = "pantheon",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pantheonw",
                ChampionName = "pantheon",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pantheone",
                ChampionName = "pantheon",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pantheonrjump",
                ChampionName = "pantheon",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 1000f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 3000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pantheonrfall",
                ChampionName = "pantheon",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 1000f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 3000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poppyq",
                ChampionName = "poppy",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 450f,
                Delay = 500f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poppyw",
                ChampionName = "poppy",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poppye",
                ChampionName = "poppy",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 525f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poppyrspell",
                ChampionName = "poppy",
                FixedRange = true,
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 1150f,
                Delay = 300f,
                EventTypes = new EventType[] { },
                MissileName = "poppyrspellmissile",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poppyrspellinstant",
                ChampionName = "poppy",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                FixedRange = true,
                CastRange = 450f,
                Delay = 300f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "quinnq",
                ChampionName = "quinn",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinearAoE,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "quinnqmissile",
                ExtraMissileNames = new[] { "quinnq" },
                MissileSpeed = 1550
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "quinnw",
                ChampionName = "quinn",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "quinne",
                ChampionName = "quinn",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 775
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "quinnr",
                ChampionName = "quinn",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "quinnrfinale",
                ChampionName = "quinn",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 700f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakanq",
                ChampionName = "rakan",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinearAoE,
                CastRange = 900f,
                Radius = 65f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "rakanqmis",
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakanw",
                ChampionName = "rakan",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 650f,
                Radius = 285f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1425
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakanwcast",
                ChampionName = "rakan",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 285f,
                Radius = 285f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakane",
                ChampionName = "rakan",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 210f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 3430
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakanecast",
                ChampionName = "rakan",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 210f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 3430
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rakanr",
                ChampionName = "rakan",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 210f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "powerball",
                ChampionName = "rammus",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 775
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "defensiveballcurl",
                ChampionName = "rammus",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "puncturingtaunt",
                ChampionName = "rammus",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 325f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tremors2",
                ChampionName = "rammus",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "renektoncleave",
                ChampionName = "renekton",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 225f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "renektonpreexecute",
                ChampionName = "renekton",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                BasicAttackAmplifier = true,
                CastRange = 275f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "renektonsliceanddice",
                ChampionName = "renekton",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 450f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "renektonreignofthetyrant",
                ChampionName = "renekton",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rengarq2",
                ChampionName = "rengar",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CastRange = 275f,
                Radius = 150f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rengarq2emp",
                ChampionName = "rengar",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CastRange = 275f,
                Radius = 150f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rengarw",
                ChampionName = "rengar",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 500f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rengare",
                ChampionName = "rengar",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "rengaremis",
                ExtraMissileNames = new[] { "rengareempmis" },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rengarr",
                ChampionName = "rengar",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaiq",
                ChampionName = "reksai",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                FixedRange = true,
                CastRange = 275f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaiqburrowed",
                ChampionName = "reksai",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastRange = 1650f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "reksaiqburrowedmis",
                MissileSpeed = 1950
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaiw",
                ChampionName = "reksai",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 350f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaiwburrowed",
                ChampionName = "reksai",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 200f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaie",
                ChampionName = "reksai",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 250f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksaieburrowed",
                ChampionName = "reksai",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 350f,
                Delay = 900f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "reksair",
                ChampionName = "reksai",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 850,
                Delay = 1000f,
                EventTypes = new[] { EventType.Initiator, EventType.Ultimate },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "riventricleave",
                ChampionName = "riven",
                Slot = SpellSlot.Q,
                CastType = CastType.LinearAoE,
                FixedRange = true,
                CastRange = 270f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rivenmartyr",
                ChampionName = "riven",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 260f,
                Delay = 100f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rivenfeint",
                ChampionName = "riven",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rivenfengshuiengine",
                ChampionName = "riven",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rivenizunablade",
                ChampionName = "riven",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1075f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "rivenlightsabermissile",
                ExtraMissileNames = new[] { "rivenlightsabermissileside" },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rumbleflamethrower",
                ChampionName = "rumble",
                Slot = SpellSlot.Q,
                CastType = CastType.Sector,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rumbleshield",
                ChampionName = "rumble",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rumbegrenade",
                ChampionName = "rumble",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "rumblecarpetbomb",
                ChampionName = "rumble",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 1700f,
                Delay = 400f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "rumblecarpetbombmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ryzeq",
                ChampionName = "ryze",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "ryzeqmissile",
                ExtraMissileNames = new[] { "ryzeq" },
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ryzew",
                ChampionName = "ryze",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ryzee",
                ChampionName = "ryze",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ryzer",
                ChampionName = "ryze",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sejuaniarcticassault",
                ChampionName = "sejuani",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 650f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileName = "",
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sejuaninorthernwinds",
                ChampionName = "sejuani",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 1000f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sejuaniwintersclaw",
                ChampionName = "sejuani",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 550f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sejuaniglacialprisoncast",
                ChampionName = "sejuani",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 1200f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "sejuaniglacialprison",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "deceive",
                ChampionName = "shaco",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.Stealth },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "jackinthebox",
                ChampionName = "shaco",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twoshivpoison",
                ChampionName = "shaco",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hallucinatefull",
                ChampionName = "shaco",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 1125f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 395
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shenq",
                ChampionName = "shen",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 1650f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                FromObject = new[] { "ShenArrowVfxHostMinion" },
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shenw",
                ChampionName = "shen",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shene",
                ChampionName = "shen",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 675f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileName = "shene",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shenr",
                ChampionName = "shen",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanadoubleattack",
                ChampionName = "shyvana",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                BasicAttackAmplifier = true,
                CastRange = 275f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanadoubleattackdragon",
                ChampionName = "shyvana",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 325f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanaimmolationauraqw",
                ChampionName = "shyvana",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 275f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanaimmolateddragon",
                ChampionName = "shyvana",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 250f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanafireball",
                ChampionName = "shyvana",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "shyvanafireballmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanafireballdragon2",
                ChampionName = "shyvana",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "shyvanatransformcast",
                ChampionName = "shyvana",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 100f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.CrowdControl,
                        EventType.Ultimate, EventType.Initiator
                    },
                MissileName = "shyvanatransformcast",
                MissileSpeed = 1100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "poisentrail",
                ChampionName = "singed",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 250f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "megaadhesive",
                ChampionName = "singed",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 1175f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "fling",
                ChampionName = "singed",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 125f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "insanitypotion",
                ChampionName = "singed",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sionq",
                ChampionName = "sion",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sionwdetonate",
                ChampionName = "sion",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sione",
                ChampionName = "sion",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 725f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "sionemissile",
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sionr",
                ChampionName = "sion",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "",
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sivirq",
                ChampionName = "sivir",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1165f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "sivirqmissile",
                ExtraMissileNames = new[] { "sivirqmissilereturn" },
                MissileSpeed = 1350
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sivirw",
                ChampionName = "sivir",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sivire",
                ChampionName = "sivir",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sivirr",
                ChampionName = "sivir",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "skarnervirulentslash",
                ChampionName = "skarner",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "skarnerexoskeleton",
                ChampionName = "skarner",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "skarnerfracture",
                ChampionName = "skarner",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1100f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "skarnerfracturemissile",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "skarnerimpale",
                ChampionName = "skarner",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 350f,
                Delay = 350f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sonaq",
                ChampionName = "sona",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sonaw",
                ChampionName = "sona",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sonae",
                ChampionName = "sona",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sonar",
                ChampionName = "sona",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "sonar",
                MissileSpeed = 2400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sorakaq",
                ChampionName = "soraka",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 970f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "",
                MissileSpeed = 1100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sorakaw",
                ChampionName = "soraka",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sorakae",
                ChampionName = "soraka",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 925f,
                Delay = 1750f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "sorakar",
                ChampionName = "soraka",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "swaindecrepify",
                ChampionName = "swain",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "swainshadowgrasp",
                ChampionName = "swain",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 1040f,
                Delay = 1100f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "swainshadowgrasp",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "swaintorment",
                ChampionName = "swain",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 625f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "swainmetamorphism",
                ChampionName = "swain",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1950
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "syndraq",
                ChampionName = "syndra",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "syndraq",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "syndrawcast",
                ChampionName = "syndra",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "syndrawcast",
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "syndrae",
                ChampionName = "syndra",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                FixedRange = true,
                CastRange = 950f,
                Delay = 300f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "syndrae",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "syndrar",
                ChampionName = "syndra",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 675f,
                Delay = 450f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 1250
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tahmkenchq",
                ChampionName = "tahmkench",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 2800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taliyahq",
                ChampionName = "taliyah",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                CastRange = 1000f,
                Radius = 80f,
                Delay = 250f,
                FixedRange = true,
                MissileName = "taliyahqmis",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taliyahwvc",
                ChampionName = "taliyah",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Radius = 150f,
                Delay = 900f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taliyahe",
                ChampionName = "taliyah",
                Slot = SpellSlot.E,
                CastType = CastType.Sector,
                CastRange = 500f,
                Radius = 165f,
                Delay = 250f,
                FixedRange = true,
                EventTypes = new EventType[] { },
                MissileSpeed = 1650
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "talonq",
                ChampionName = "talon",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 275f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "talonw",
                ChampionName = "talon",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "talonwmissileone",
                ExtraMissileNames = new[] { "talonwmissiletwo" },
                MissileSpeed = 2300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "talone",
                ChampionName = "talon",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "talonr",
                ChampionName = "talon",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 750f,
                Delay = 260f,
                MissileName = "talonrmisone",
                EventTypes = new[] { EventType.Stealth, EventType.Initiator },
                ExtraMissileNames = new[] { "talonrmistwo" },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taricq",
                ChampionName = "taric",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taricw",
                ChampionName = "taric",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tarice",
                ChampionName = "taric",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 625f,
                Delay = 1000f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "taricr",
                ChampionName = "taric",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "blindingdart",
                ChampionName = "teemo",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 580f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "movequick",
                ChampionName = "teemo",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 943
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "toxicshot",
                ChampionName = "teemo",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bantamtrap",
                ChampionName = "teemo",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "threshq",
                ChampionName = "thresh",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1175f,
                Delay = 500f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileName = "threshqmissile",
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "threshw",
                ChampionName = "thresh",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "threshe",
                ChampionName = "thresh",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "threshemissile1",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "threshrpenta",
                ChampionName = "thresh",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 420f,
                Delay = 300f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1550
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tristanaq",
                ChampionName = "tristana",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tristanaw",
                ChampionName = "tristana",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                Radius = 270f,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tristanae",
                ChampionName = "tristana",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                Radius = 210f,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 2400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "tristanar",
                ChampionName = "tristana",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                Radius = 200f,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "trundleq",
                ChampionName = "trundle",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 800f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "trundletrollsmash",
                ChampionName = "trundle",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                Radius = 210f,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "trundledesecrate",
                ChampionName = "trundle",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "trundlecircle",
                ChampionName = "trundle",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 1000f,
                Radius = 340f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "trundlepain",
                ChampionName = "trundle",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 650f,
                Radius = 300f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bloodlust",
                ChampionName = "tryndamere",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "mockingshout",
                ChampionName = "tryndamere",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "slashcast",
                ChampionName = "tryndamere",
                Slot = SpellSlot.E,
                CastType = CastType.Linear,
                CastRange = 660f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "slashcast",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "undyingrage",
                ChampionName = "tryndamere",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchhideinshadows",
                ChampionName = "twitch",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 450f,
                EventTypes = new[] { EventType.Stealth },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchvenomcask",
                ChampionName = "twitch",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "twitchvenomcaskmissile",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchvenomcaskmissle",
                ChampionName = "twitch",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchexpungewrapper",
                ChampionName = "twitch",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchexpunge",
                ChampionName = "twitch",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "twitchfullautomatic",
                ChampionName = "twitch",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "wildcards",
                ChampionName = "twistedfate",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CastRange = 1450f,
                FixedRange = true,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "sealfatemissile",
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "pickacard",
                ChampionName = "twistedfate",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "goldcardpreattack",
                ChampionName = "twistedfate",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "redcardpreattack",
                ChampionName = "twistedfate",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bluecardpreattack",
                ChampionName = "twistedfate",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "cardmasterstack",
                ChampionName = "twistedfate",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "destiny",
                ChampionName = "twistedfate",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "udyrtigerstance",
                ChampionName = "udyr",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "udyrturtlestance",
                ChampionName = "udyr",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "udyrbearstanceattack",
                ChampionName = "udyr",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 250f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "udyrphoenixstance",
                ChampionName = "udyr",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotheatseekinglineqqmissile",
                ChampionName = "urgot",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotheatseekingmissile",
                ChampionName = "urgot",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotterrorcapacitoractive2",
                ChampionName = "urgot",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotplasmagrenade",
                ChampionName = "urgot",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "urgotplasmagrenadeboom",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotplasmagrenadeboom",
                ChampionName = "urgot",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "urgotswap2",
                ChampionName = "urgot",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "varusq",
                ChampionName = "varus",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1250f,
                Delay = 0f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "varusqmissile",
                MissileSpeed = 1900
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "varusw",
                ChampionName = "varus",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "varuse",
                ChampionName = "varus",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 925f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "varuse",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "varusr",
                ChampionName = "varus",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinearAoE,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1300f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileName = "varusrmissile",
                MissileSpeed = 1950
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vaynetumble",
                ChampionName = "vayne",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vaynesilverbolts",
                ChampionName = "vayne",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vaynecondemnmissile",
                ChampionName = "vayne",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 550f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vayneinquisition",
                ChampionName = "vayne",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.Stealth, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "veigarbalefulstrike",
                ChampionName = "veigar",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 950f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "veigarbalefulstrikemis",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "veigardarkmatter",
                ChampionName = "veigar",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Delay = 1200f,
                EventTypes = new EventType[] { },
                MissileName = "",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "veigareventhorizon",
                ChampionName = "veigar",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 650f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "veigarprimordialburst",
                ChampionName = "veigar",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "velkozq",
                ChampionName = "velkoz",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1250f,
                Delay = 100f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "velkozqmissile",
                MissileSpeed = 1300
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "velkozqsplitactivate",
                ChampionName = "velkoz",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                IsPerpindicular = true,
                CastRange = 1050f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileName = "velkozqmissilesplit",
                MissileSpeed = 2100
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "velkozw",
                ChampionName = "velkoz",
                Slot = SpellSlot.W,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1050f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileName = "velkozwmissile",
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "velkoze",
                ChampionName = "velkoz",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 950f,
                Delay = 0f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "velkozemissile",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "velkozr",
                ChampionName = "velkoz",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                CastRange = 1575f,
                Delay = 0f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viqmissile",
                ChampionName = "vi",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viw",
                ChampionName = "vi",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vie",
                ChampionName = "vi",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vir",
                ChampionName = "vi",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Initiator },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viktorpowertransfer",
                ChampionName = "viktor",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1050
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viktorgravitonfield",
                ChampionName = "viktor",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 815f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viktordeathray",
                ChampionName = "viktor",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileName = "viktordeathraymis",
                ExtraMissileNames = new[] { "viktoreaugmissile" },
                MissileSpeed = 1210
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "viktorchaosstorm",
                ChampionName = "viktor",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 710f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.CrowdControl, EventType.Ultimate,
                        EventType.Danger, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vladimirq",
                ChampionName = "vladimir",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vladimirw",
                ChampionName = "vladimir",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vladimire",
                ChampionName = "vladimir",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 610f,
                Delay = 800f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "vladimirr",
                ChampionName = "vladimir",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 875f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "volibearq",
                ChampionName = "volibear",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new[] { EventType.Initiator, EventType.CrowdControl },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "volibearw",
                ChampionName = "volibear",
                Slot = SpellSlot.W,
                CastType = CastType.Targeted,
                CastRange = 400f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 1450
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "volibeare",
                ChampionName = "volibear",
                CastType = CastType.Proximity,
                Slot = SpellSlot.E,
                CastRange = 425f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 825
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "volibearr",
                ChampionName = "volibear",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 825
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hungeringstrike",
                ChampionName = "warwick",
                Slot = SpellSlot.Q,
                CastType = CastType.Targeted,
                CastRange = 400f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "hunterscall",
                ChampionName = "warwick",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "bloodscent",
                ChampionName = "warwick",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 0f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "infiniteduress",
                ChampionName = "warwick",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                CastRange = 700f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "infiniteduresschannel",
                ChampionName = "warwick",
                Slot = SpellSlot.R,
                CastType = CastType.Linear,
                CastRange = 700f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xayahq",
                ChampionName = "xayah",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1100f,
                Delay = 500f,
                EventTypes = new EventType[] { },
                MissileName = "xayahqmissile1",
                MissileSpeed = 2060
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xayahw",
                ChampionName = "xayah",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xayahe",
                ChampionName = "xayah",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CastRange = 2000f,
                Radius = 80,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "xayahemissilesfx",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xayahr",
                ChampionName = "xayah",
                Slot = SpellSlot.R,
                CastType = CastType.MissileLinear,
                CastRange = 1100f,
                Radius = 25,
                Delay = 500f,
                EventTypes = new[] { EventType.Initiator },
                MissileName = "xayahrmissile",
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xeratharcanopulsechargeup",
                ChampionName = "xerath",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 750f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xeratharcanebarrage2",
                ChampionName = "xerath",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 1100f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "xeratharcanebarrage2",
                MissileSpeed = 20
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xerathmagespear",
                ChampionName = "xerath",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 1050f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger },
                MissileName = "xerathmagespearmissile",
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xerathlocusofpower2",
                ChampionName = "xerath",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 5600f,
                Delay = 750f,
                EventTypes = new EventType[] { },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaocombotarget",
                ChampionName = "xinzhao",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 375,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaothrust",
                ChampionName = "xinzhao",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 625f,
                Radius = 225f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaothrust2",
                ChampionName = "xinzhao",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 625f,
                Radius = 225f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaothrust3",
                ChampionName = "xinzhao",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 625f,
                Radius = 225f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaobattlecry",
                ChampionName = "xinzhao",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Radius = 210f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaosweep",
                ChampionName = "xinzhao",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 600f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger, EventType.Initiator },
                MissileSpeed = 2400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "xenzhaoparry",
                ChampionName = "xinzhao",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 500f,
                Radius = 210f,
                Delay = 250f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl
                    },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuoqw",
                ChampionName = "yasuo",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 475f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuoq2w",
                ChampionName = "yasuo",
                Slot = SpellSlot.Q,
                CastType = CastType.Linear,
                FixedRange = true,
                CastRange = 475f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuoq3",
                ChampionName = "yasuo",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1000f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "yasuoq3mis",
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuowmovingwall",
                ChampionName = "yasuo",
                CastType = CastType.Linear,
                Slot = SpellSlot.W,
                IsPerpindicular = true,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuodashwrapper",
                ChampionName = "yasuo",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 475f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 20
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yasuorknockupcombow",
                ChampionName = "yasuo",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 1200f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yorickq",
                ChampionName = "yorick",
                Slot = SpellSlot.Q,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                BasicAttackAmplifier = true,
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yorickw",
                ChampionName = "yorick",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yoricke",
                ChampionName = "yorick",
                Slot = SpellSlot.E,
                CastType = CastType.LinearAoE,
                CastRange = 700f,
                Radius = 125f,
                Delay = 250f,
                MissileName = "yorickemissile",
                EventTypes = new EventType[] { },
                MissileSpeed = 750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "yorickr",
                ChampionName = "yorick",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zacq",
                ChampionName = "zac",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                FixedRange = true,
                CastRange = 800f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "zaqmissile",
                MissileSpeed = 2600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zacw",
                ChampionName = "zac",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 350f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zace",
                ChampionName = "zac",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                MissileSpeed = 1500
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zacr",
                ChampionName = "zac",
                Slot = SpellSlot.R,
                CastType = CastType.Proximity,
                CastRange = 600f,
                Radius = 300f,
                Delay = 250f,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl, EventType.Ultimate },
                MissileSpeed = 1800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zedq",
                ChampionName = "zed",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "zedqmissile",
                FromObject = new[] { "Zed_Base_W_tar.troy", "Zed_Base_W_cloneswap_buf.troy" },
                ExtraMissileNames = new[] { "zedqmissiletwo", "zedqmissilethree" },
                MissileSpeed = 1700
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zedw",
                ChampionName = "zed",
                Slot = SpellSlot.W,
                CastType = CastType.LinearAoE,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 1600
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zede",
                ChampionName = "zed",
                Slot = SpellSlot.E,
                CastType = CastType.Proximity,
                CastRange = 300f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zedr",
                ChampionName = "zed",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 850f,
                Delay = 450f,
                EventTypes = new[] { EventType.Danger, EventType.Initiator },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggsq",
                ChampionName = "ziggs",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CollidesWith = new[] { CollisionObjectType.EnemyHeroes },
                CastRange = 850f,
                Radius = 100f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "ziggsqspell",
                ExtraMissileNames = new[] { "ziggsqspell2", "ziggsqspell3" },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggsw",
                ChampionName = "ziggs",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "ziggsw",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggswtoggle",
                ChampionName = "ziggs",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggse",
                ChampionName = "ziggs",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "ziggse",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggse2",
                ChampionName = "ziggs",
                Slot = SpellSlot.E,
                CastType = CastType.Circlular,
                CastRange = 850f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "ziggsr",
                ChampionName = "ziggs",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 2250f,
                Delay = 1800f,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                MissileName = "ziggsr",
                MissileSpeed = 1750
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zileanq",
                ChampionName = "zilean",
                Slot = SpellSlot.Q,
                CastType = CastType.Circlular,
                CastRange = 900f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "zileanqmissile",
                MissileSpeed = 2000
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zileanw",
                ChampionName = "zilean",
                Slot = SpellSlot.W,
                CastType = CastType.Proximity,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zileane",
                ChampionName = "zilean",
                Slot = SpellSlot.E,
                CastType = CastType.Targeted,
                CastRange = 700f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zileanr",
                ChampionName = "zilean",
                Slot = SpellSlot.R,
                CastType = CastType.Targeted,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 4800
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zyraq",
                ChampionName = "zyra",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                CastRange = 800f,
                Radius = 430f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "zyraqmissile",
                MissileSpeed = 1400
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zyraqplantmissile",
                ChampionName = "zyra",
                Slot = SpellSlot.Q,
                CastType = CastType.MissileLinear,
                IsPerpindicular = true,
                CastRange = 675f,
                Radius = 710f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileName = "zyraqplantmissile",
                MissileSpeed = 1200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zyraw",
                ChampionName = "zyra",
                Slot = SpellSlot.W,
                CastType = CastType.Circlular,
                CastRange = 0f,
                Delay = 250f,
                EventTypes = new EventType[] { },
                MissileSpeed = 2200
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zyrae",
                ChampionName = "zyra",
                Slot = SpellSlot.E,
                CastType = CastType.MissileLinear,
                FixedRange = true,
                CastRange = 1150f,
                Radius = 70f,
                Delay = 250f,
                EventTypes = new[] { EventType.CrowdControl },
                MissileName = "zyraemissile",
                MissileSpeed = 1150
            });

            ZLib.Spells.Add(new Gamedata
            {
                SpellName = "zyrar",
                ChampionName = "zyra",
                Slot = SpellSlot.R,
                CastType = CastType.Circlular,
                CastRange = 700f,
                Radius = 500f,
                Delay = 500f,
                EventTypes =
                    new[]
                    {
                        EventType.Danger, EventType.Ultimate,
                        EventType.CrowdControl, EventType.Initiator
                    },
                MissileSpeed = 4800
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Checks if the hero name matches the string
        /// </summary>
        /// <param name="championname">The championname.</param>
        /// <returns></returns>
        public bool HeroNameMatch(string championname)
        {
            return ChampionName.ToLower() == championname.ToLower();
        }

        /// <summary>
        ///     Gets the spell data by the name of missile.
        /// </summary>
        /// <param name="missilename">The missilename.</param>
        /// <returns></returns>
        internal static Gamedata GetByMissileName(string missilename)
        {
            foreach (var sdata in ZLib.Spells)
            {
                if (sdata.MissileName != null && sdata.MissileName.ToLower() == missilename
                    || sdata.ExtraMissileNames != null && sdata.ExtraMissileNames.Contains(missilename))
                {
                    return sdata;
                }
            }

            return null;
        }

        #endregion
    }
}
﻿#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Document:	Data/Auradata.cs
// Date:		28/07/2016
// Author:		Robin Kurisu

#endregion

namespace Support_AIO.Data
{
    #region

    using System;
    using Aimtec;

    #endregion

    /// <summary>
    ///     Class Auradata.
    /// </summary>
    public class Auradata
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets or sets the name of the menu.
        /// </summary>
        /// <value>
        ///     The name of the menu.
        /// </value>
        public string MenuName { get; internal set; }

        /// <summary>
        ///     Gets or sets the champion.
        /// </summary>
        /// <value>
        ///     The champion.
        /// </value>
        public string Champion { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether should evade.
        /// </summary>
        /// <value>
        ///     <c>true</c> if should evade; otherwise, <c>false</c>.
        /// </value>
        public bool Evade { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the aura is reverse.
        /// </summary>
        /// <value>
        ///     <c>true</c> if aura reverse; otherwise, <c>false</c>.
        /// </value>
        public bool Reverse { get; internal set; }

        /// <summary>
        ///     Gets or sets the radius.
        /// </summary>
        /// <value>
        ///     The radius.
        /// </value>
        public float Radius { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is damage over time.
        /// </summary>
        /// <value>
        ///     <c>true</c> is damage over time; otherwise, <c>false</c>.
        /// </value>
        public bool DoT { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether should cleanse.
        /// </summary>
        /// <value>
        ///     <c>true</c> if shoul cleanse; otherwise, <c>false</c>.
        /// </value>
        public bool Cleanse { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether should ignore aura.
        /// </summary>
        /// <value>
        ///     <c>true</c> if should ignore aura; otherwise, <c>false</c>.
        /// </value>
        public bool QssIgnore { get; internal set; }

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        /// <value>
        ///     The interval.
        /// </value>
        public double Interval { get; internal set; }

        /// <summary>
        ///     Gets or sets the tick limiter.
        /// </summary>
        /// <value>
        ///     The tick limiter.
        /// </value>
        public int TickLimiter { get; internal set; }

        /// <summary>
        ///     Gets or sets the evade timer.
        /// </summary>
        /// <value>
        ///     The evade timer.
        /// </value>
        public int EvadeTimer { get; internal set; }

        /// <summary>
        ///     Gets or sets the cleanse timer.
        /// </summary>
        /// <value>
        ///     The cleanse timer.
        /// </value>
        public int CleanseTimer { get; internal set; }

        /// <summary>
        ///     Gets or sets the income damage.
        /// </summary>
        /// <value>
        ///     The income damage.
        /// </value>
        public int IncomeDamage { get; internal set; }

        /// <summary>
        ///     Gets or sets the slot.
        /// </summary>
        /// <value>
        ///     The slot.
        /// </value>
        public SpellSlot Slot { get; internal set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes the aura list.
        /// </summary>
        static Auradata()
        {
            Console.WriteLine("ZLib: Auradata Loaded!");
            ZLib.AuraList.Add(new Auradata
            {
                Name = "suppression",
                MenuName = "Suppresion",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "summonerdot",
                MenuName = "Summoner Ignite",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "summonerexhaust",
                MenuName = "Summoner Exhaust",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "masteryburndebuff",
                MenuName = "Deathfire Touch",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "itemdusknightfall",
                MenuName = "Duskblade (Nightfall)",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 1650,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Vi",
                Name = "virknockup",
                MenuName = "Vi R Knockup",
                Evade = true,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Amumu",
                Name = "curseofthesadmummy",
                MenuName = "Amumu Curse of the Sad Mummy",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "itemsmitechallenge",
                MenuName = "Challenging Smite",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.7
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Gangplank",
                Name = "gangplankpassiveattackdot",
                MenuName = "Gangplank Passive Burn",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Teemo",
                Name = "bantamtraptarget",
                MenuName = "Teemo Shroom",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Teemo",
                Name = "toxicshotparticle",
                MenuName = "Teemo Toxic Shot",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Ahri",
                Name = "ahriseduce",
                MenuName = "Ahri Charm",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Elise",
                Name = "elisehumane",
                MenuName = "Elise Cocoon",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 25,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Heimerdinger",
                Name = "heimerdingerespell",
                MenuName = "Heimerdinger Grenade",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 25,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Talon",
                Name = "talonbleeddebuf",
                MenuName = "Talon Bleed",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Malzahar",
                Name = "alzaharnethergrasp",
                MenuName = "Malzahar Nether Grasp",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Malzahar",
                Name = "alzaharmaleficvisions",
                MenuName = "Malzahar Aids",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "FiddleSticks",
                Name = "drainchannel",
                MenuName = "Fiddle Drain",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Galio",
                Name = "galioidolofdurand",
                MenuName = "Galio Idol of Durand",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Nasus",
                Name = "nasusw",
                MenuName = "Nasus Wither",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Gnar",
                Name = "gnarstun",
                MenuName = "Gnar Ultimate",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Gragas",
                Name = "gragasestun",
                MenuName = "Gragas Body Slam",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Bard",
                Name = "bardqshackledebuff",
                MenuName = "Bard Cosmic Binding",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 25,
                Slot = SpellSlot.Q,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Akali",
                Name = "akalimota",
                MenuName = "Akali Mark of the Assassin",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 1.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Hecarim",
                Name = "hecarimdefilelifeleech",
                MenuName = "Hecarim Defile Leech",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Swain",
                Name = "swaintormentdot",
                MenuName = "Swain Torment",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Brand",
                Name = "brandablaze",
                MenuName = "Brand Burn Passive",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Fizz",
                Name = "fizzseastonetrident",
                MenuName = "Fizz Burn Passive",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Tristana",
                Name = "tristanaechargesound",
                MenuName = "Tristana Explosive Charge",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Darius",
                Name = "dariushemo",
                MenuName = "Darius Hemorrhage",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Nidalee",
                Name = "bushwackdamage",
                MenuName = "Nidalee Bushwhack",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Nidalee",
                Name = "nidaleepassivehunted",
                MenuName = "Nidalee Passive Mark",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Shyvana",
                Name = "shyvanaimmolationaura",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "MissFortune",
                Name = "missfortunescattershotslow",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "MissFortune",
                Name = "missfortunepassivestack",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Shyvana",
                Name = "shyvanaimmolatedragon",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Zilean",
                Name = "zileanqenemybomb",
                MenuName = "Zilean Bomb",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 1.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Wukong",
                Name = "monkeykingspintowin",
                Evade = true,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Reverse = true,
                Radius = 200f,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Katarina",
                Name = "katarinaqmark",
                MenuName = "Katarina Bouncing Blades",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 1.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Kindred",
                Name = "kindredecharge",
                MenuName = "Kindred Mounting Dread",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 2.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Cassiopeia",
                Name = "cassiopeiaqdebuff",
                MenuName = "Cassio Noxious Blast",
                Evade = false,
                Cleanse = false,
                DoT = true,
                EvadeTimer = 0,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 0.4
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Cassiopeia",
                Name = "cassiopeiarstun",
                MenuName = "Cassio Petrifying Gaze",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Ekko",
                Name = "ekkowstun",
                MenuName = "Ekko Parellel Convergence",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Lissandra",
                Name = "lissandrarenemy2",
                MenuName = "Lissandra Frozen Tomb",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Sejuani",
                Name = "sejuaniglacialprison",
                MenuName = "Sejuani Glacial Prison",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Fiora",
                Name = "fiorarmark",
                MenuName = "Fiora Grand Challenge",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 100,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Twitch",
                Name = "twitchdeadlyvenon",
                MenuName = "Twitch Deadly Venom",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 0.6
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Swain",
                Name = "swainqtraptarget",
                MenuName = "Swain Decrepify",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Swain",
                Name = "swaintorment",
                MenuName = "Swain Torment",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Urgot",
                Name = "urgotcorrosivedebuff",
                MenuName = "Urgot Corrosive Charge",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Mordekaiser",
                Name = "mordekaiserchildrenofthegrave",
                MenuName = "Mordekaiser Children of the Grave",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Unknown,
                Interval = 1.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Garen",
                Name = "garene",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.E,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Hecarim",
                Name = "hecarimw",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.W,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Braum",
                Name = "braummark",
                MenuName = "Braum Passive",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 200,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Zed",
                Name = "zedultexecute",
                MenuName = "Zed Mark",
                Evade = true,
                DoT = false,
                EvadeTimer = 2600,
                Cleanse = true,
                CleanseTimer = 500,
                Slot = SpellSlot.R,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Karthus",
                Name = "fallenonetarget",
                Evade = true,
                DoT = false,
                EvadeTimer = 2600,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Karthus",
                Name = "karthusfallenonetarget",
                Evade = true,
                DoT = false,
                EvadeTimer = 2600,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Fizz",
                Name = "fizzmarinerdoombomb",
                MenuName = "Fizz Shark Bait",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 1.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Morgana",
                Name = "soulshackles",
                MenuName = "Morgana Soul Shackles",
                Evade = true,
                DoT = false,
                EvadeTimer = 3200,
                Cleanse = true,
                CleanseTimer = 1100,
                Slot = SpellSlot.R,
                Interval = 3.9
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Varus",
                Name = "varusrsecondary",
                MenuName = "Varus Chains of Corruption",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Caitlyn",
                Name = "caitlynaceinthehole",
                MenuName = "Caitlyn Ace in the Hole",
                Evade = false,
                DoT = true,
                EvadeTimer = 900,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Vladimir",
                Name = "vladimirhemoplague",
                MenuName = "Vladimir Hemoplague",
                Evade = true,
                DoT = false,
                EvadeTimer = 4500,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R,
                Interval = 4.4
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Diana",
                Name = "dianamoonlight",
                MenuName = "Diana Moonlight",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.Q
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Urgot",
                Name = "urgotswap2",
                MenuName = "Urgot Swap",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Skarner",
                Name = "skarnerimpale",
                MenuName = "Skarner Impale",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 500,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Maokai",
                Name = "maokaiunstablegrowthroot",
                MenuName = "Maokai Root",
                Evade = false,
                DoT = false,
                EvadeTimer = 0,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.W
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "LeeSin",
                Name = "blindmonkqonechaos",
                MenuName = "Lee Sonic Wave",
                Evade = false,
                DoT = true,
                EvadeTimer = 0,
                Cleanse = false,
                CleanseTimer = 0,
                Slot = SpellSlot.Q,
                Interval = 3.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Leblanc",
                Name = "leblancsoulshackle",
                MenuName = "Leblanc Shackle",
                Evade = false,
                DoT = false,
                EvadeTimer = 2000,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.E
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Leblanc",
                Name = "leblancsoulshacklem",
                MenuName = "Leblanc Shackle (R)",
                Evade = true,
                DoT = false,
                EvadeTimer = 2000,
                Cleanse = true,
                CleanseTimer = 0,
                Slot = SpellSlot.E
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Rammus",
                Name = "puncturingtauntarmordebuff",
                MenuName = "Rammus Puncturing Taunt",
                Evade = false,
                DoT = false,
                Cleanse = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Vi",
                Name = "vir",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Yasuo",
                Name = "yasuorknockupcombo",
                Evade = false,
                DoT = true,
                Cleanse = true,
                CleanseTimer = 100,
                EvadeTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Yasuo",
                Name = "yasuorknockupcombotar",
                Evade = false,
                DoT = true,
                Cleanse = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Zyra",
                Name = "zyrabramblezoneknockup",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Velkoz",
                Name = "velkozresearchstack",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown,
                Interval = 0.3
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "frozenheartaura",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Darius",
                Name = "dariusaxebrabcone",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "frozenheartauracosmetic",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "itemsunfirecapeaura",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown,
                Reverse = true,
                Radius = 200f,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Fizz",
                Name = "fizzmoveback",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "blessingofthelizardelderslow",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "dragonburning",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Blitzcrank",
                Name = "rocketgrab2",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Ashe",
                Name = "frostarrow",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Alistar",
                Name = "pulverize",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Q
            });

            ZLib.AuraList.Add(new Auradata
            {
                Name = "chilled",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Azir",
                Name = "azirqslow",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Q
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Rammus",
                Name = "powerballslow",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Q
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Rammus",
                Name = "powerballstun",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Q
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "MonkeyKing",
                Name = "monkeykingspinknockup",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Alistar",
                Name = "headbutttarget",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.W
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Hecarim",
                Name = "hecarimrampstuncheck",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Hecarim",
                Name = "hecarimrampattackknockback",
                Evade = false,
                DoT = false,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                QssIgnore = true,
                Slot = SpellSlot.Unknown
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "DrMundo",
                Name = "burningagony",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 175f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Amumu",
                Name = "auraofdespair",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 175f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Ahri",
                Name = "ahrifoxfire",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 550f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Rakan",
                Name = "rakanr",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 200f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Camille",
                Name = "camillewconeslashcharge",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 250f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Camille",
                Name = "camilleq",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.Q,
                Reverse = true,
                Radius = 250f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Camille",
                Name = "camilleqprimingcomplete",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.Q,
                Reverse = true,
                Radius = 250f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Galio",
                Name = "galiow",
                Evade = false,
                DoT = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 250f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Galio",
                Name = "galiowbuff",
                Evade = false,
                DoT = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 250f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Galio",
                Name = "galioemove",
                Evade = false,
                DoT = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 100f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Galio",
                Name = "galiorallybuff",
                Evade = false,
                DoT = true,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 325f,
                Interval = 1.0
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Diana",
                Name = "dianaorbs",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 200f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Alistar",
                Name = "alistare",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 300f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Garen",
                Name = "garene",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 300f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Jax",
                Name = "jaxevasion",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 200f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Jayce",
                Name = "jaycestaticfield",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 285f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Karthus",
                Name = "karthusdefile",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 425f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Leona",
                Name = "leonasolarbarrier",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 425f,
                Interval = 1.1
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Pantheon",
                Name = "panthesound",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 300f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Shyvana",
                Name = "shyvanascorchedearth",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 175f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Shyvana",
                Name = "shyvanascorchedearthdragon",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.W,
                Reverse = true,
                Radius = 300f,
                Interval = 0.5
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Shyvana",
                Name = "swainmetamorphism",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 600f,
                Interval = 0.2
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Lissandra",
                Name = "lissandrarself",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 600f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Kennen",
                Name = "kennenlightningrush",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 165f,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Kennen",
                Name = "kennenshurikenstorm",
                Evade = true,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 275,
                Interval = 0.8
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Kled",
                Name = "kledrdash",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 200,
                Interval = 0.3
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Zac",
                Name = "zacemove",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.E,
                Reverse = true,
                Radius = 300,
                Interval = 0.3
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Zac",
                Name = "zacrmove",
                Evade = true,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 300,
                Interval = 0.3
            });

            ZLib.AuraList.Add(new Auradata
            {
                Champion = "Nunu",
                Name = "absolutezero",
                Evade = false,
                DoT = true,
                Cleanse = false,
                CleanseTimer = 0,
                EvadeTimer = 0,
                Slot = SpellSlot.R,
                Reverse = true,
                Radius = 600f,
                Interval = 1.2
            });
        }

        #endregion
    }
}
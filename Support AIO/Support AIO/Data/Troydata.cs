#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Document:	Data/Troydata.cs
// Date:		28/07/2016
// Author:		Robin Kurisu

#endregion

namespace Support_AIO.Data
{
    #region

    using System.Collections.Generic;
    using Aimtec;
    using Base;

    #endregion

    public class Troydata
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
        ///     Gets or sets the name of the champion.
        /// </summary>
        /// <value>
        ///     The name of the champion.
        /// </value>
        public string ChampionName { get; internal set; }

        /// <summary>
        ///     Gets or sets the slot.
        /// </summary>
        /// <value>
        ///     The slot.
        /// </value>
        public SpellSlot Slot { get; internal set; }

        /// <summary>
        ///     Gets or sets the radius.
        /// </summary>
        /// <value>
        ///     The radius.
        /// </value>
        public float Radius { get; internal set; }

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        /// <value>
        ///     The interval.
        /// </value>
        public double Interval { get; internal set; }

        /// <summary>
        ///     Gets or sets the predicted damage.
        /// </summary>
        /// <value>
        ///     The predicted damage.
        /// </value>
        public bool PredictDmg { get; internal set; }

        /// <summary>
        ///     Gets or sets the event types.
        /// </summary>
        /// <value>
        ///     The event types.
        /// </value>
        public EventType[] EventTypes { get; internal set; }

        /// <summary>
        ///     Gets or sets the delay from start.
        /// </summary>
        /// <value>
        ///     The delay from start.
        /// </value>
        public int DelayFromStart { get; internal set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes the troy list.
        /// </summary>
        static Troydata()
        {
            ZLib.TroyList.Add(new Troydata
            {
                Name = "R_buf",
                ChampionName = "Renekton",
                Radius = 266f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "R_buf",
                ChampionName = "Renekton",
                Radius = 266f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "SpiritFire",
                ChampionName = "Nasus",
                Radius = 400f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "R_Avatar",
                ChampionName = "Nasus",
                Radius = 266f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "AnnieTibbers",
                ChampionName = "Annie",
                Radius = 266f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "E_TrampleAOE",
                ChampionName = "Alistar",
                Radius = 266f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "MonkeyKing_Base_R",
                ChampionName = "MonkeyKing",
                Radius = 165 + 100 + 0 + 1,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate, EventType.Initiator },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Ryze_Base_E",
                ChampionName = "Ryze",
                Radius = 200f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = .75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Hecarim_Defile",
                ChampionName = "Hecarim",
                Radius = 425f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = .75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_AoE",
                ChampionName = "Hecarim",
                Radius = 425f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = .75
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Gangplank_Base_R",
                ChampionName = "Gangplank",
                Radius = 400f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_Shield",
                ChampionName = "Diana",
                Radius = 225f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_Shield",
                ChampionName = "Sion",
                Radius = 225f,
                Slot = SpellSlot.W,
                DelayFromStart = 2800,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_aoe_red",
                ChampionName = "Malzahar",
                Radius = 325f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "E_Defile",
                ChampionName = "Karthus",
                Radius = 425f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_volatile",
                ChampionName = "Elise",
                Radius = 250f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.3
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "DarkWind_tar",
                ChampionName = "FiddleSticks",
                Radius = 250f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.8
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "lr_buf",
                ChampionName = "Kennen",
                Radius = 250f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.8
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "ss_aoe",
                ChampionName = "Kennen",
                Radius = 475f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Ahri_Base_FoxFire",
                ChampionName = "Ahri",
                Radius = 550f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "AurelionSol_Base_P",
                ChampionName = "AurelionSol",
                Radius = 165f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Fizz_Ring_Red",
                ChampionName = "Fizz",
                Radius = 300f,
                Slot = SpellSlot.R,
                DelayFromStart = 800,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "katarina_deathLotus_tar",
                ChampionName = "Katarina",
                Radius = 550f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.ForceExhaust, EventType.Danger },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Nautilus_R_sequence_impact",
                ChampionName = "Nautilus",
                Radius = 250f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.CrowdControl, EventType.Danger, EventType.Ultimate },
                PredictDmg = false,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Acidtrail_buf",
                ChampionName = "Singed",
                Radius = 200f,
                Slot = SpellSlot.Q,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Tremors_cas",
                ChampionName = "Rammus",
                Radius = 450f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Crowstorm_red",
                ChampionName = "FiddleSticks",
                Radius = 425f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.Danger, EventType.Ultimate, EventType.ForceExhaust },
                PredictDmg = true,
                Interval = 0.5
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "yordleTrap_idle",
                ChampionName = "Caitlyn",
                Radius = 265f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = false,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "tar_aoe_red",
                ChampionName = "Lux",
                Radius = 400f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 2.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Viktor_ChaosStorm",
                ChampionName = "Viktor",
                Radius = 425f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.Danger, EventType.CrowdControl },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "Viktor_Catalyst",
                ChampionName = "Viktor",
                Radius = 375f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = false,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_AUG",
                ChampionName = "Viktor",
                Radius = 375f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = false,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "cryo_storm",
                ChampionName = "Anivia",
                Radius = 400f,
                Slot = SpellSlot.R,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "ZiggsE",
                ChampionName = "Ziggs",
                Radius = 325f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "ZiggsWRing",
                ChampionName = "Ziggs",
                Radius = 325f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.CrowdControl },
                PredictDmg = false,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_Miasma_tar",
                ChampionName = "Cassiopeia",
                Radius = 365f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "E_rune",
                ChampionName = "Soraka",
                Radius = 375f,
                Slot = SpellSlot.E,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = 1.0
            });

            ZLib.TroyList.Add(new Troydata
            {
                Name = "W_Tar",
                ChampionName = "Morgana",
                Radius = 275f,
                Slot = SpellSlot.W,
                EventTypes = new[] { EventType.None },
                PredictDmg = true,
                Interval = .75
            });
        }

        #endregion
    }
}
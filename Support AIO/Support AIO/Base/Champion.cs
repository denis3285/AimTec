#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Base/Champion.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Support_AIO.Base
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;

    #endregion

    public class Champion
    {
        #region Public Properties

        /// <summary>
        ///     W
        ///     Gets or sets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        public Obj_AI_Hero Player { get; internal set; }

        /// <summary>
        ///     Gets or sets the attacker.
        /// </summary>
        /// <value>
        ///     The attacker.
        /// </value>
        public Obj_AI_Base Attacker { get; internal set; }

        /// <summary>
        ///     Gets or sets the ability damage.
        /// </summary>
        /// <value>
        ///     The ability damage.
        /// </value>
        public float AbilityDamage { get; internal set; }

        /// <summary>
        ///     Gets or sets the tower damage.
        /// </summary>
        /// <value>
        ///     The tower damage.
        /// </value>
        public float TowerDamage { get; internal set; }

        /// <summary>
        ///     Gets or sets the troy damage.
        /// </summary>
        /// <value>
        ///     The troy damage.
        /// </value>
        public float TroyDamage { get; internal set; }

        /// <summary>
        ///     Gets or sets the buff damage.
        /// </summary>
        /// <value>
        ///     The buff damage.
        /// </value>
        public float BuffDamage { get; internal set; }

        /// <summary>
        ///     Gets or sets the item damage.
        /// </summary>
        /// <value>
        ///     The item damage.
        /// </value>
        public float ItemDamage { get; internal set; }

        /// <summary>
        ///     Gets the event types.
        /// </summary>
        /// <value>
        ///     The event types.
        /// </value>
        public List<EventType> Events { get; internal set; } = new List<EventType>();

        /// <summary>
        ///     Gets the predicted buff count.
        /// </summary>
        /// <value>
        ///     The buff count.
        /// </value>
        public int BuffCount => Events.Count(t => t == EventType.Buff);

        /// <summary>
        ///     Gets the predicted troy count.
        /// </summary>
        /// <value>
        ///     The troy count.
        /// </value>
        public int TroyCount => Events.Count(t => t == EventType.Troy);

        /// <summary>
        ///     Gets the predicted spell count.
        /// </summary>
        /// <value>
        ///     The spell count.
        /// </value>
        public int SpellCount => Events.Count(t => t == EventType.Spell) + TroyCount;

        /// <summary>
        ///     Gets the predicted danger spell count.
        /// </summary>
        /// <value>
        ///     The danger count.
        /// </value>
        public int DangerCount => Events.Count(t => t == EventType.Danger);

        /// <summary>
        ///     Gets the predicted crowd control spell count.
        /// </summary>
        /// <value>
        ///     The crowd control count.
        /// </value>
        public int CrowdControlCount => Events.Count(t => t == EventType.CrowdControl);

        /// <summary>
        ///     Gets the predicted income damage ammount.
        /// </summary>
        /// <value>
        ///     The income damage.
        /// </value>
        public float IncomeDamage => TroyDamage + AbilityDamage + BuffDamage + ItemDamage;

        #endregion

        /// <summary>
        ///     Checks if the hero name matches the string.
        /// </summary>
        /// <param name="championname">The string.</param>
        /// <returns></returns>
        internal bool HeroNameMatch(string championname)
        {
            return Player.ChampionName.ToLower() == championname.ToLower();
        }
    }
}
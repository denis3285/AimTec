#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Base/Unit.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Potato_AIO.Base
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;

    #endregion

    /// <summary>
    ///     Class Unit.
    /// </summary>
    public class Unit
    {
        #region Public Properties

        /// <summary>
        ///     W
        ///     Gets or sets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        public Obj_AI_Hero Instance { get; internal set; }
        public Obj_AI_Base Instances { get; internal set; }
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
        ///     Gets or sets the minion damage.
        /// </summary>
        /// <value>
        ///     The minion damage.
        /// </value>
        public float MinionDamage { get; internal set; }

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

        /// <summary>
        ///     Gets or sets the last debuff timestamp.
        /// </summary>
        /// <value>
        ///     The last debuff timestamp.
        /// </value>
        [System.Obsolete("LastDebuffTimestamp is deprecated, ignore and do not use thanks.")]
        public int LastDebuffTimestamp { get; set; }

        /// <summary>
        ///     Gets or sets the cleanse buff count.
        /// </summary>
        /// <value>
        ///     The cleanse buff count.
        /// </value>
        [System.Obsolete("QssBuffCount is deprecated, ignore and do not use thanks.")]
        public int QssBuffCount { get; set; }

        /// <summary>
        ///     Gets or sets the QSS highest buff time.
        /// </summary>
        /// <value>
        ///     The QSS highest buff time.
        /// </value>
        [System.Obsolete("QssHighestBuffTime is deprecated, ignore and do not use thanks.")]
        public int QssHighestBuffTime { get; set; }

        /// <summary>
        ///     Gets or sets the cleanse buff count.
        /// </summary>
        /// <value>
        ///     The cleanse buff count.
        /// </value>
        [System.Obsolete("CleanseBuffCount is deprecated, ignore and do not use thanks.")]
        public int CleanseBuffCount { get; set; }

        /// <summary>
        ///     Gets or sets the cleanse highest buff time.
        /// </summary>
        /// <value>
        ///     The cleanse highest buff time.
        /// </value>
        [System.Obsolete("CleanseHighestBuffTime is deprecated, ignore and do not use thanks.")]
        public int CleanseHighestBuffTime { get; set; }

        /// <summary>
        ///     Gets or sets the dervish buff count.
        /// </summary>
        /// <value>
        ///     The dervish buff count.
        /// </value>
        [System.Obsolete("DervishBuffCount is deprecated, ignore and do not use thanks.")]
        public int DervishBuffCount { get; set; }

        /// <summary>
        ///     Gets or sets the dervish highest buff time.
        /// </summary>
        /// <value>
        ///     The dervish highest buff time.
        /// </value>
        [System.Obsolete("DervishHighestBuffTime is deprecated, ignore and do not use thanks.")]
        public int DervishHighestBuffTime { get; set; }

        /// <summary>
        ///     Gets or sets the mercurial buff count.
        /// </summary>
        /// <value>
        ///     The mercurial buff count.
        /// </value>
        [System.Obsolete("MercurialBuffCount is deprecated, ignore and do not use thanks.")]
        public int MercurialBuffCount { get; set; }

        /// <summary>
        ///     Gets or sets the mercurial highest buff time.
        /// </summary>
        /// <value>
        ///     The mercurial highest buff time.
        /// </value>
        [System.Obsolete("MercurialHighestBuffTime is deprecated, ignore and do not use thanks.")]
        public int MercurialHighestBuffTime { get; set; }

        /// <summary>
        ///     Gets or sets the mikaels buff count.
        /// </summary>
        /// <value>
        ///     The mikaels buff count.
        /// </value>
        [System.Obsolete("MikaelsBuffCount is deprecated, ignore and do not use thanks.")]
        public int MikaelsBuffCount { get; set; }

        /// <summary>
        ///     Gets or sets the mikaels highest buff time.
        /// </summary>
        /// <value>
        ///     The mikaels highest buff time.
        /// </value>
        [System.Obsolete("MikaelsHighestBuffTime is deprecated, ignore and do not use thanks.")]
        public int MikaelsHighestBuffTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to force qss/cleanse
        /// </summary>
        /// <value>
        ///     <c>true</c> if we should force qss/cleanse otherwise, <c>false</c>.
        /// </value>
        [System.Obsolete("ForceQss is deprecated, ignore and do not use thanks.")]
        public bool ForceQss { get; set; }
        public object Player { get; internal set; }

        #endregion

        #region Internal Methods and Operators

        /// <summary>
        ///     Checks if the hero name matches the string.
        /// </summary>
        /// <param name="championname">The string.</param>
        /// <returns></returns>
        internal bool HeroNameMatch(string championname)
        {
            return Instance is Obj_AI_Hero && ((Obj_AI_Hero)Instance).ChampionName.ToLower() == championname.ToLower();
        }

        #endregion
    }
}
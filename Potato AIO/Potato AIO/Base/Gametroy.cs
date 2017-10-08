#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Document:    Base/Gametroy.cs
// Date:        22/09/2015
// Author:      Robin Kurisu

#endregion

namespace Potato_AIO.Base
{
    #region

    using System.Collections.Generic;
    using Aimtec;

    #endregion

    internal class Gametroy
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <value>
        ///     The damage.
        /// </value>
        public int Damage { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether is included.
        /// </summary>
        /// <value>
        ///     <c>true</c> if included; otherwise, <c>false</c>.
        /// </value>
        public bool Included { get; internal set; }

        /// <summary>
        ///     Gets the owner.
        /// </summary>
        /// <value>
        ///     The owner.
        /// </value>
        public string Owner { get; }

        /// <summary>
        ///     Gets or sets the limiter.
        /// </summary>
        /// <value>
        ///     The limiter.
        /// </value>
        public int Limiter { get; internal set; }

        /// <summary>
        ///     Gets or sets the object.
        /// </summary>
        /// <value>
        ///     The object.
        /// </value>
        public GameObject Obj { get; internal set; }

        /// <summary>
        ///     Gets the slot.
        /// </summary>
        /// <value>
        ///     The slot.
        /// </value>
        public SpellSlot Slot { get; }

        /// <summary>
        ///     Gets or sets the start tickcount.
        /// </summary>
        /// <value>
        ///     The tick.
        /// </value>
        public int Start { get; internal set; }

        #endregion

        #region Static Fields and Constants

        internal static List<Gametroy> Troys = new List<Gametroy>();

        #endregion

        #region Constructors and Destructors

        public Gametroy(string owner, SpellSlot slot, string name, int start, bool inculded, int incdmg = 0, GameObject obj = null)
        {
            Owner = owner;
            Slot = slot;
            Start = start;
            Name = name;
            Obj = obj;
            Included = inculded;
            Damage = incdmg;
        }

        #endregion
    }
}
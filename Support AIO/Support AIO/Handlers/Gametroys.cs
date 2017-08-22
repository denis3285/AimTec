#region Copyright © 2015 Kurisu Solutions

// All rights are reserved. Transmission or reproduction in part or whole,
// any form or by any means, mechanical, electronical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Document:	Handlers/ObjectHandler.cs
// Date:		28/07/2016
// Author:		Robin Kurisu

#endregion

namespace Support_AIO.Handlers
{
    #region

    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Base;
    using Data;

    #endregion

    internal class Gametroys
    {
        #region Internal Methods and Operators

        internal static void OnDestory(GameObject obj)
        {
            if (obj.Type == GameObjectType.MissileClient)
                return;

            foreach (var troy in Gametroy.Troys)
            {
                if (troy.Included && obj.Name.ToLower().Contains(troy.Name.ToLower()))
                {
                    troy.Obj = null;
                    troy.Start = 0;
                    troy.Limiter = 0; // reset limiter
                    troy.Included = false;
                }
            }
        }

        internal static void OnCreate(GameObject obj)
        {
            if (obj.Type == GameObjectType.MissileClient)
                return;

            foreach (var troy in Gametroy.Troys)
            {
                if (obj.Name.ToLower().Contains(troy.Name.ToLower()))
                {
                    troy.Obj = obj;
                    troy.Start = (int)(Game.ClockTime * 1000);

                    if (!troy.Included)
                        troy.Included = Helpers.IsHeroInGame(troy.Owner);
                }
            }
        }

        internal static void OnUpdate()
        {
            try
            {
                foreach (var unit in ZLib.GetUnits())
                {
                    var troy = Gametroy.Troys.Find(x => x.Included);
                    if (troy == null)
                        continue;

                    if (!troy.Obj.IsVisible || !troy.Obj.IsValid)
                    {
                        continue;
                    }

                    foreach (var entry in ZLib.TroyList.Where(x => x.Name.ToLower() == troy.Name.ToLower()))
                    {
                        var owner = ZLib.GetUnits().FirstOrDefault(x => x.HeroNameMatch(entry.ChampionName));
                        if (owner == null || owner.Instance == null)
                        {
                            continue;
                        }

                        if (owner.Instance.NetworkId == unit.Instance.NetworkId)
                        {
                            continue;
                        }

                        Gamedata data = null;

                        if (entry.ChampionName == null && entry.Slot == SpellSlot.Unknown)
                            data = new Gamedata { SpellName = troy.Obj.Name };

                        if (entry.ChampionName != null && entry.Slot != SpellSlot.Unknown)
                        {
                            data = ZLib.CachedSpells.Where(x => x.Slot == entry.Slot)
                                .FirstOrDefault(x => x.HeroNameMatch(entry.ChampionName));

                            if (data != null)
                                data.EventTypes = entry.EventTypes;
                        }

                        if (unit.Instance.Distance(troy.Obj.Position) <= entry.Radius + unit.Instance.BoundingRadius)
                        {
                            // check delay (e.g fizz bait)
                            if ((int)(Game.ClockTime * 1000) - troy.Start >= entry.DelayFromStart)
                            {
                                // limit the damage using an interval
                                if ((int)(Game.ClockTime * 1000) - troy.Limiter >= entry.Interval * 1000)
                                {
                                    Projections.EmulateDamage(owner.Instance, unit, data, EventType.Troy, "troy.onupdate");
                                    troy.Limiter = (int)(Game.ClockTime * 1000);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (ZLib.Menu["logerror"].As<MenuBool>().Enabled)
                {
                    Console.WriteLine("== Error at: ZLib.Gametroys.OnUpdate");
                    Console.WriteLine(e);
                }
            }
        }

        internal static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestory;
        }

        #endregion
    }
}
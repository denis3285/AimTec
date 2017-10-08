using Potato_AIO.Bases;

namespace Potato_AIO
{
    using System;
    using System.Reflection;

    using Aimtec;
    using Aimtec.SDK.Events;

    internal static class Program
    {
        internal static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        private static string Namespace => MethodBase.GetCurrentMethod().DeclaringType?.Namespace;

        private static void Main()
        {
            GameEvents.GameStart += OnGameStart;
        }

        private static void OnGameStart()
        {
            var championType = Type.GetType($"{Namespace}.Champions.{Player.ChampionName}");

            if (championType == null) return;

            ((Champion)Activator.CreateInstance(championType)).Initiate();
        }
    }
}
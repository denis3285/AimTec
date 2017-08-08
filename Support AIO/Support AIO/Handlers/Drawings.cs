namespace Support_AIO.Handlers
{
    #region

    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Menu.Components;

    #endregion

    internal class Drawings
    {
        #region Private Methods and Operators

        private static void Render_OnRender()
        {
            if (ZLib.Menu["debugmenu"]["acdebug"].As<MenuBool>().Enabled)
            {
                foreach (var hero in ZLib.Allies())
                {
                    if (Render.WorldToScreen(hero.Player.Position, out Vector2 mpos))
                    {
                        if (!hero.Player.IsDead)
                        {
                            Render.Text(mpos.X - 40, mpos.Y - 15, Color.Wheat,
                                "Ability Damage: " + hero.AbilityDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 0, Color.Wheat,
                                "Tower Damage: " + hero.TowerDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 15, Color.Wheat,
                                "Buff Damage: " + hero.BuffDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 30, Color.Wheat,
                                "Troy Damage: " + hero.TroyDamage);
  
                        }
                    }
                }
            }
        }

        #endregion

        internal static void Init()
        {
            Render.OnRender += Render_OnRender;
        }
    }
}
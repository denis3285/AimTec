namespace Support_AIO.Handlers
{
    #region

    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Menu.Components;

    #endregion

    internal class Drawings
    {
        #region Internal Methods and Operators

        internal static void Render_OnRender()
        {
            if (ZLib.Menu["acdebug"].As<MenuBool>().Enabled)
            {
                foreach (var hero in ZLib.GetUnits())
                {
                    Vector2 mpos;
                    if (Render.WorldToScreen(hero.Instance.Position, out mpos))
                    {
                        if (!hero.Instance.IsDead)
                        {
                            Render.Text(mpos.X - 40, mpos.Y - 15, Color.Wheat, "Ability Damage: " + hero.AbilityDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 0, Color.Wheat, "Tower Damage: " + hero.TowerDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 15, Color.Wheat, "Buff Damage: " + hero.BuffDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 30, Color.Wheat, "Troy Damage: " + hero.TroyDamage);
                            Render.Text(mpos.X - 40, mpos.Y + 45, Color.Wheat, "Minion Damage: " + hero.MinionDamage);
                        }
                    }
                }
            }
        }

        internal static void Initialize()
        {
            Render.OnRender += Render_OnRender;
        }

        #endregion
    }
}
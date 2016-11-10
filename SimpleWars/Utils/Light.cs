namespace SimpleWars.Utils
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.DisplayManagement;

    /// <summary>
    /// The light.
    /// </summary>
    public static class Light
    {
        /// <summary>
        /// The sun direction.
        /// </summary>
        private static readonly Vector3 SunDirection = 
            Vector3.Normalize(new Vector3(-100, -200, -100));

        /// <summary>
        /// The sun color.
        /// </summary>
        private static readonly Vector3 SunColor = Color.LightYellow.ToVector3();

        /// <summary>
        /// The sunlight.
        /// </summary>
        /// <param name="effect">
        /// The effect.
        /// </param>
        /// <param name="specularColor">
        /// The specular color.
        /// </param>
        public static void Sunlight(BasicEffect effect, Vector3 specularColor)
        {
            effect.EnableDefaultLighting();

            effect.LightingEnabled = true;

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = SunDirection;
            effect.DirectionalLight0.DiffuseColor = SunColor;
            effect.DirectionalLight0.SpecularColor = specularColor;
        }

        /// <summary>
        /// The sunlight.
        /// </summary>
        /// <param name="effect">
        /// The effect.
        /// </param>
        /// <param name="specularColor">
        /// The specular color.
        /// </param>
        public static void Sunlight(SkinnedEffect effect, Vector3 specularColor)
        {
            effect.EnableDefaultLighting();

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = SunDirection;
            effect.DirectionalLight0.DiffuseColor = SunColor;
            effect.DirectionalLight0.SpecularColor = specularColor;
        }
    }
}
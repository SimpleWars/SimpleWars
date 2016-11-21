namespace SimpleWars.Utils
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class PrimitiveShapes
    {
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }
}
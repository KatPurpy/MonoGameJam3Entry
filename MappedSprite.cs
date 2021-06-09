using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public class MappedSprite
    {
        public Texture2D texture;
        public Rectangle rectangle;

        public Vector2 Center => new(rectangle.Width / 2, rectangle.Height / 2);

        public void Draw(Vector2 position, Color? color = null, float rotation = 0, Vector2? origin =null, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0f)
        {
            Game._.spriteBatch.Draw(
                texture,
                position: position,
                sourceRectangle: rectangle,
                color: color ?? Color.White,
                rotation: rotation,
                origin: origin ?? Vector2.Zero,
                scale: scale ?? Vector2.One,
                effects: effects,
                layerDepth: layerDepth);
        }
    }
}

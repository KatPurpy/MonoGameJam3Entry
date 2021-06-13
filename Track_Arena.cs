using DSastR.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    class Track_Arena : Entity
    {
        public override Vector2 VisualPosition { get => Vector2.Zero; set => _ = value; }


        World world;

        public Track_Arena(World w)
        {
            world = w;

        }

        public override void Draw(GameTime time)
        {
            Game._.spriteBatch.Draw(Assets.Sprites.circle, Vector2.Zero, null, new Color(Vector3.One * 0f), 0, new Vector2(128), 16f, SpriteEffects.None, 0);

            /*for (int x = -20; x < 20; x++)
                for (int y = -20; y < 20; y++)
                    Game._.spriteBatch.Draw(Assets.Sprites.circle, new Rectangle(x*40, y*10, 10, 10),null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.005f);
            */

            Game._.spriteBatch.Draw(Assets.Sprites.space, camera.XY/2, null, Color.Gray, 0.25f* (float)time.TotalGameTime.TotalSeconds,

                new Vector2(700 / 2),5, SpriteEffects.None, 0.005f);

            Game._.spriteBatch.Draw(Assets.Sprites.circle, Vector2.Zero, null, new Color(Vector3.One * 0.25f), 0, new Vector2(128), 8f, SpriteEffects.None, 0.01f);
            
        }

        public override void IMGUI(GameTime time)
        {
            //throw new NotImplementedException();
        }

        public override void RestoreState(JsonElement state)
        {
           // throw new NotImplementedException();
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
          //  throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            //throw new NotImplementedException();
        }
    }
}

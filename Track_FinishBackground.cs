using DSastR.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    class Track_FinishBackground : Entity
    {
       

        public override Vector2 VisualPosition { get => rect[0]*5; set => rect[0] = value; }

        List<Vector2> rect = new(2)
        {
            Vector2.Zero,
            Vector2.Zero
        };

        Rectangle rectangle;

        public override void Update(GameTime time){}

        public override void Draw(GameTime time)
        {
            rectangle.X = (int)rect[0].X;
            rectangle.Y = (int)rect[0].Y;
            rectangle.Width = (int)rect[1].X;
            rectangle.Height = (int)rect[1].Y;
            for (int x = rectangle.X; x < rectangle.X + rectangle.Width; x+=5)
            {
                for (int y = rectangle.Y; y < rectangle.Y + rectangle.Height; y+=5)
                {
                    Game._.spriteBatch.Draw(Assets.Sprites.checkerboard, 
                        new Vector2(x,y)*5,
                       null, Color.White,0,Vector2.Zero,2.5f * 5,SpriteEffects.None,0);
                }
            }
                
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            writer.WriteNumber("X",rectangle.X);
            writer.WriteNumber("Y", rectangle.Y);
            writer.WriteNumber("W", rectangle.Width);
            writer.WriteNumber("H", rectangle.Height);
        }

        public override void RestoreState(JsonElement state)
        {
            rectangle = new(state.GetProperty("X").GetInt32(),
                state.GetProperty("Y").GetInt32(),
                state.GetProperty("W").GetInt32(),
                state.GetProperty("H").GetInt32());

            rect[0] = new(rectangle.X, rectangle.Y);
            rect[1] = new(rectangle.Width,rectangle.Height);
        }

        public override void IMGUI(GameTime time)
        {
            rect[0] = ImGuiUtils.VecField("Position",rect[0]);
            rect[1] = ImGuiUtils.VecField("Size", rect[1]);
        }
    }
}

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
    public class Track_FlowerField : Entity
    {
        Rectangle rectangle;
        Vector2[] rect = new []{ Vector2.Zero, Vector2.Zero };

        Random random = new(3);

        public override Vector2 VisualPosition { get => rect[0]; set => rect[0] = value; }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            writer.WriteNumber("X", rectangle.X);
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
            rect[1] = new(rectangle.Width, rectangle.Height);
        }

        public override void IMGUI(GameTime time)
        {
            rect[0] = ImGuiUtils.VecField("Position", rect[0]);
            rect[1] = ImGuiUtils.VecField("Size", rect[1]);
            rectangle.X = (int)rect[0].X;
            rectangle.Y = (int)rect[0].Y;
            rectangle.Width = (int)rect[1].X;
            rectangle.Height = (int)rect[1].Y;
        }

        public override void Update(GameTime time){}

        public override void Draw(GameTime time)
        {
            random = new Random((int)(rect[0].X * rect[0].Y * rect[1].X * rect[1].Y));
            for(int i = 0; i < rect[1].X * rect[1].Y / (Game.PixelsPerMeter * Game.PixelsPerMeter) * 0.01f; i++)
            {
                int x = random.Next((int)rect[0].X,(int)(rect[0].X + rect[1].X)),
                    y = random.Next((int)rect[0].Y, (int)(rect[0].Y + rect[1].Y));
                bool flip = random.Next(0, 2) == 1;
                int flowerType = random.Next((int)DecorationType.flower1, (int)DecorationType.flower8 + 1);

                Assets.Sprites.decorMap[(DecorationType)flowerType].Draw(new(x, y), layerDepth: 0, effects:flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }
        }
    }
}

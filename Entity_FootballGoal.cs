using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace MonoGameJam3Entry
{
    class Entity_FootballGoal : Entity
    {


            public override Vector2 VisualPosition { get => rect[0] * 5; set => rect[0] = value; }

            List<Vector2> rect = new(2)
            {
                Vector2.Zero,
                Vector2.Zero
            };

            Rectangle rectangle;
        World world;
        Body body;
        public bool IsPlayerGoal;

        bool updaterect;

        public Entity_FootballGoal(World world)
        {
            this.world = world;
            
            
        }
        void updateBody()
        {
            if(body != null) world.Remove(body);
            body = world.CreateRectangle(rectangle.Width * 5/ Game.PixelsPerMeter  + 0.1f, rectangle.Height * 5 / Game.PixelsPerMeter + 0.1f, 1,
                new Vector2(rectangle.Width/2 + rectangle.X, rectangle.Height/2+ rectangle.Y) * 5 / Game.PixelsPerMeter);
            body.FixtureList[0].IsSensor = true;
            body.FixtureList[0].OnCollision += delegate (Fixture sender, Fixture other, Contact contact)
            {
                Console.WriteLine(other);
                Console.WriteLine("" + other.Tag);
                if(other.Body.Tag is Entity_FootballBall ball)
                {
                    if (IsPlayerGoal)
                    {
                        GameScene.EnemyGoals++;
                        if(GameScene.EnemyGoals == GameScene.MaxGoals)
                        {
                            GameScene.Lose();
                        }
                    }
                    else
                    {
                        
                        GameScene.PlayerGoals++;
                        if (GameScene.PlayerGoals == GameScene.MaxGoals)
                        {
                            GameScene.Win();
                        }
                    }
                    Console.WriteLine("BALL!!");
                    ball.Reset();
                }
                return true;
            };
            //(body.FixtureList[0] as tainicom.Aether.Physics2D.Collision.Shapes.PolygonShape) rectangle.Width / Game.PixelsPerMeter
        }
        

            public override void Update(GameTime time) {


        }

        public override void Draw(GameTime time)
            {
            rectangle.X = (int)rect[0].X;
            rectangle.Y = (int)rect[0].Y;
            rectangle.Width = (int)rect[1].X;
            rectangle.Height = (int)rect[1].Y;

            if (updaterect) updateBody();

            for (int x = rectangle.X; x < rectangle.X + rectangle.Width; x += 1)
                {
                    for (int y = rectangle.Y; y < rectangle.Y + rectangle.Height; y += 1)
                    {
                        Game._.spriteBatch.Draw(Assets.Sprites.checkerboard,
                            new Vector2(x, y) * 5,
                           null, IsPlayerGoal ? Color.Blue : Color.Red , 0, Vector2.Zero, 5f/2f, SpriteEffects.None, 0);
                    }
                }

            }

            public override void SerializeState(Utf8JsonWriter writer)
            {
            writer.WriteBoolean(nameof(IsPlayerGoal), IsPlayerGoal);
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

                IsPlayerGoal = state.GetProperty(nameof(IsPlayerGoal)).GetBoolean();

            updateBody();
        }

            public override void IMGUI(GameTime time)
            {
            bool pos = false, sca = false;
                rect[0] = ImGuiUtils.VecField("Position", rect[0], ref pos);
                rect[1] = ImGuiUtils.VecField("Size", rect[1], ref sca);
            ImGui.Checkbox("IsPlayerGoal",ref IsPlayerGoal);
            updaterect = pos || sca;    
        }
        public override void Destroy()
        {
            try
            {
                world.Remove(body);
            }
            catch { }
        }
    }
    }


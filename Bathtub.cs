using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;
using DSastR.Core;
using ImGuiNET;
using System.Text.Json;

namespace MonoGameJam3Entry
{
    enum Direction
    {
        Up = 0,
        UpRight = 1,
        Right = 2,
        DownRight = 3,
        Down = 4,
        DownLeft = 5,
        Left = 6,
        UpLeft = 7,
    }
    class Bathtub : Entity
    {
        public Texture2D CarTexture;
        public Texture2D CharacterTexture;

        public Vector2 RealPosition { get => physicsBody.Position;
        set
            {
                physicsBody.Position = value;
            }
        }

        public override Vector2 VisualPosition {
            set => RealPosition = value / Game.PixelsPerMeter;
            get => RealPosition * Game.PixelsPerMeter;
        }


        public Vector2 Velocity;
        public float Rotation;
        public float Speed = 10;

        public bool PlayerControlled = true;

        World world;
        public Body physicsBody;

        public Bathtub(World world)
        {
            this.world = world;
            physicsBody = world.CreateEllipse(64/Game.PixelsPerMeter, 42/Game.PixelsPerMeter,8,1,bodyType: BodyType.Dynamic);
            physicsBody.LinearDamping = 1f;
            physicsBody.SetRestitution(0.7f);
            physicsBody.SetFriction(1.5f);
            physicsBody.FixedRotation = true;
        }
        bool Left, Right, Up, Down;
        public override void Update(GameTime time)
        {
            Left = Right = Up = Down = false;

            

            if (PlayerControlled)
            {
                var stat = Keyboard.GetState();
                Left = stat.IsKeyDown(Keys.Left);
                Right = stat.IsKeyDown(Keys.Right);
                Up = stat.IsKeyDown(Keys.Up);
                Down = stat.IsKeyDown(Keys.Down);

                //Console.WriteLine(physicsBody.LinearVelocity);
                 //Console.WriteLine(physicsBody.Position);
                //physicsBody.Rotation = -rads + MathHelper.ToRadians(90);
            }
            Movement(time);
            //Position += Speed * Velocity * (float)time.ElapsedGameTime.TotalSeconds;
        }



        private void Movement(GameTime time)
        {
            if (Left)
            {
                Rotation -= (float)time.ElapsedGameTime.TotalSeconds * 200;
                physicsBody.ApplyLinearImpulse((float)time.ElapsedGameTime.TotalSeconds * 10 * -physicsBody.LinearVelocity * 2);
            }
            if (Right)
            {
                Rotation += (float)time.ElapsedGameTime.TotalSeconds * 200;
                physicsBody.ApplyLinearImpulse((float)time.ElapsedGameTime.TotalSeconds * 10 * -physicsBody.LinearVelocity * 2);
            }
            float rads = MathHelper.ToRadians(Rotation);
            if (Up)
            {
                physicsBody.ApplyLinearImpulse(-(float)time.ElapsedGameTime.TotalSeconds * physicsBody.Mass * 120 * Vector2.TransformNormal(Vector2.UnitY, Matrix.CreateRotationZ(rads)));

            }
            if (Down)
            {
                if (physicsBody.LinearVelocity.Length() > 5)
                {
                    physicsBody.LinearVelocity *= 0.89f;
                }
                else
                {
                    physicsBody.ApplyLinearImpulse(-(float)time.ElapsedGameTime.TotalSeconds * physicsBody.Mass * -200 * Vector2.TransformNormal(Vector2.UnitY, Matrix.CreateRotationZ(rads)));
                }
            }
        }

        public override void Draw(GameTime time)
        {
            
            DrawBathtub(VisualPosition, (Direction)((int)Rotation / 45 % 8), Color.White);
        }


        public override void IMGUI(GameTime time)
        {
            RealPosition = ImGuiUtils.VecField("POSITION",RealPosition);
            bool val = physicsBody.FixtureList[0].IsSensor;
            if(ImGui.Checkbox("Disable collisions", ref val)) physicsBody.FixtureList[0].IsSensor = val;
        }

        public override void Destroy()
        {
            base.Destroy();
            world.Remove(physicsBody);
        }

        void DrawBathtub(Vector2 pos, Direction dir, Color color)
        {
            SpriteEffects se = SpriteEffects.None;
            if((int)dir < 0)
            {
                dir = 8 + dir;
            }
            if ((int)dir > (int)Direction.Down)
            {
                se = SpriteEffects.FlipHorizontally;
                dir = (8 - dir);
            }
            Game._.spriteBatch.Draw(CarTexture,
               pos, new Rectangle(0, (int)dir * 128, 128, 128), color, 0, new Vector2(128 / 2, 128 / 2), Vector2.One, se, 0
               );
            Game._.spriteBatch.Draw(CharacterTexture,
               pos, new Rectangle(0, (int)dir * 128, 128, 128), color, 0, new Vector2(128 / 2, 128 / 2), Vector2.One, se, 0
                );
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            WritePosition(writer);
        }

        public override void RestoreState(ref JsonDocument json)
        {
            throw new NotImplementedException();
            //VisualPosition = new((float)reader.GetDecimal(),(float)reader.GetDecimal());
        }
    }
}

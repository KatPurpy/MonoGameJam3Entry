﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;

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
    class Bathtub
    {
        public Texture2D Texture;
        public Vector2 RealPposition { get => physicsBody.Position;
        set
            {
                physicsBody.Position = value;
            }
        }

        public Vector2 VisualPosition => RealPposition * Game.PixelsPerMeter;


        public Vector2 Velocity;
        public float Rotation;
        public float Speed = 10;

        public bool PlayerControlled = true;

        Body physicsBody;

        public Bathtub(World world)
        {

            physicsBody = world.CreateEllipse(64/Game.PixelsPerMeter, 42/Game.PixelsPerMeter,8,2.25f,bodyType: BodyType.Dynamic);
            
            physicsBody.LinearDamping = 1f;
            physicsBody.SetRestitution(1);
            physicsBody.SetFriction(1.5f);
            physicsBody.FixedRotation = true;
        }
        
        public void Update(GameTime time)
        {
           
            if (PlayerControlled)
            {
                var stat = Keyboard.GetState();
                if (stat.IsKeyDown(Keys.Left))
                {
                    Rotation -= (float)time.ElapsedGameTime.TotalSeconds * 200;
                    physicsBody.ApplyLinearImpulse((float)time.ElapsedGameTime.TotalSeconds * 10 * -physicsBody.LinearVelocity * 2);
                }
                if (stat.IsKeyDown(Keys.Right))
                {
                    Rotation += (float)time.ElapsedGameTime.TotalSeconds * 200;
                    physicsBody.ApplyLinearImpulse((float)time.ElapsedGameTime.TotalSeconds * 10 * -physicsBody.LinearVelocity * 2);
                }
                float rads = MathHelper.ToRadians(Rotation);
                if (stat.IsKeyDown(Keys.Up))
                {
                    physicsBody.ApplyLinearImpulse(-(float)time.ElapsedGameTime.TotalSeconds*physicsBody.Mass*120*Vector2.TransformNormal(Vector2.UnitY, Matrix.CreateRotationZ(rads)));

                    //physicsBody.LinearVelocity -= ;
                }
        
                Console.WriteLine(physicsBody.LinearVelocity);
               // Console.WriteLine(physicsBody.Position);
                //physicsBody.Rotation = -rads + MathHelper.ToRadians(90);
            }
            //Position += Speed * Velocity * (float)time.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime time)
        {
            
            DrawBathtub(VisualPosition, (Direction)((int)Rotation / 45 % 8), Color.White);
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
            Game._.spriteBatch.Draw(Texture,
               pos, new Rectangle(0, (int)dir * 128, 128, 128), color, 0, new Vector2(128 / 2, 128 / 2), Vector2.One, se, 0
               );
        }
    }
}

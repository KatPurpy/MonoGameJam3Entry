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
using System.Diagnostics;

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

        public int RacerID;

        public Track_Waypoints AI_Waypoints;
        int AI_WaypointID;
        public Vector2 AI_TargetPosition;
        float AI_TurnThreshold;

        public float Rotation;
        public float Speed = 10;

        public bool PlayerControlled = true;

        int Laps = 0;
        public TimeSpan time;
       

        World world;
        public Body physicsBody;
        int frame = 0;

        public static bool AI_RacingMode = true;
        
        public Bathtub(World world)
        {
            this.world = world;
            physicsBody = world.CreateEllipse(64/Game.PixelsPerMeter, 42/Game.PixelsPerMeter,8,1,bodyType: BodyType.Dynamic);
            physicsBody.LinearDamping = 1f;
            physicsBody.SetRestitution(1f);
            physicsBody.SetFriction(1.5f);
            physicsBody.FixedRotation = true;
            PlayerControlled = true;
        }
        bool Left, Right, Up, Down;
        Random r = new();

        public override void Start()
        {
            if (AI_Waypoints != null && AI_Waypoints.Positions.Count > 1)
            {
                var pos = AI_Waypoints.Positions[0] * Game.PixelsPerMeter;
                var dir = pos * Game.PixelsPerMeter - VisualPosition;
                dir = new(MathF.Round(dir.X), MathF.Round(dir.Y));
                Rotation = MathHelper.ToDegrees(MathF.Atan2(dir.Y, dir.X)) + 90;
            }
        }

        public override void Update(GameTime time)
        {
            Left = Right = Up = Down = false;

            if(AI_TargetPosition == Vector2.Zero)
            {
                 if(AI_Waypoints != null && AI_Waypoints.Positions.Count > 1) AI_TargetPosition = AI_Waypoints.Positions[0];
            }

            float angle = MathHelper.ToRadians(Rotation);
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

            if (PlayerControlled)
            {

                var stat = Keyboard.GetState();
                Up = stat.IsKeyDown(Keys.Up);
                Down = stat.IsKeyDown(Keys.Down);
                if (frame % 2 == 0)
                {
                    
                    Left = stat.IsKeyDown(Keys.Left);
                    Right = stat.IsKeyDown(Keys.Right);
                    
                }
                GameScene.PlayerID = RacerID;

                if (AI_Waypoints != null && AI_Waypoints.Positions.Count > 1)
                {

                    Vector2 dirToTarget = AI_Waypoints.Positions[AI_WaypointID] * Game.PixelsPerMeter - VisualPosition;
                    AI_TargetPosition = AI_Waypoints.Positions[AI_WaypointID];
                    if (dirToTarget.Length() < Track_Waypoints.triggerradius * Game.PixelsPerMeter)
                    {
                        AI_WaypointID++;
                        AI_WaypointID = AI_WaypointID % (AI_Waypoints.Positions.Count);
                        LapCheck();

                    }
                    GameScene.PlayerLaps = Laps;
                    try
                    {
                        GameScene.LapTimers[Laps] = GameScene.LapTimers[Laps].Add(time.ElapsedGameTime);
                    }
                    catch { }
                    dirToTarget.Normalize();
                    float a = MathF.Acos(Vector2.Dot(dirToTarget, dir));
                    Console.WriteLine(MathHelper.ToDegrees(a));
                }
                //GameScene.WrongWay = a > MathHelper.ToRadians(90);
                //Console.WriteLine(physicsBody.LinearVelocity);
                //Console.WriteLine(physicsBody.Position);
                //physicsBody.Rotation = -rads + MathHelper.ToRadians(90);
            }
            else
            {
                Up = true;
                

                Vector2 dirToTarget = AI_TargetPosition * Game.PixelsPerMeter - VisualPosition;
                
                
                if(AI_Waypoints != null && AI_Waypoints.Positions.Count > 1 && dirToTarget.Length() < (AI_TurnThreshold  + Track_Waypoints.aiTriggerRadius) * Game.PixelsPerMeter)
                {
                    AI_WaypointID++;
                    if (AI_RacingMode)
                    {
                        AI_TargetPosition = AI_Waypoints.Positions[AI_WaypointID % (AI_Waypoints.Positions.Count)];
                        LapCheck();
                        AI_TurnThreshold = (float)r.NextDouble() * 3;
                    }
                        //AI_TargetPosition += 0.5f* Track_Waypoints.aiTriggerRadius * (new Vector2((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f)) * 2;
                }

                dirToTarget.Normalize();
                if (Vector2.Dot(dir, dirToTarget) > 0)
                {
                    Right = true;
                }
                else
                {
                    Left = true;
                }
            }
            Movement(time);
            //Position += Speed * Velocity * (float)time.ElapsedGameTime.TotalSeconds;
        }

        private void LapCheck()
        {
            if (AI_WaypointID % (AI_Waypoints.Positions.Count) == 0)
            {
                Laps++;


                if(Laps == GameScene.GoalLaps)
                {
                    if (RacerID == GameScene.PlayerID) {
                        GameScene.Win();
                    }
                    else
                    {
                        GameScene.Lose();
                    }
                    return;
                }

                if (PlayerControlled)
                {
                    GameScene.LapTimers.Add(new());
                }
            }
        }

        private void Movement(GameTime time)
        {
            this.time = this.time.Add(time.ElapsedGameTime);
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
            AI_Waypoints?.DrawWayPoints();

            RealPosition = ImGuiUtils.VecField("POSITION",RealPosition);
            ImGui.Checkbox("PLAYER",ref PlayerControlled);
            AI_TargetPosition = ImGuiUtils.VecField("TARGET", AI_TargetPosition);

            ImGui.InputInt("RacerID", ref RacerID);

            ImGui.InputInt("Lap", ref Laps);

            ImGui.GetBackgroundDrawList().AddCircleFilled(
                NumericToXNA.ConvertXNAToNumeric(Vector2.Transform(AI_TargetPosition*Game.PixelsPerMeter, camera.View())),
                10,
                0xFFFFFFFF
                );

            bool val = physicsBody.FixtureList[0].IsSensor;
            if(ImGui.Checkbox("Disable collisions", ref val)) physicsBody.FixtureList[0].IsSensor = val;
        }

        public override void Destroy()
        {
            base.Destroy();
            world.Remove(physicsBody);
        }

        public static Color[] colors = new Color[]
        {
            Color.Red,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Orange,
            Color.Cyan,
            Color.Magenta
        };

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
               pos, new Rectangle(0, (int)dir * 128, 128, 128), colors[RacerID], 0, new Vector2(128 / 2, 128 / 2), Vector2.One, se, GetLayerDepth()
               );
            
            Game._.spriteBatch.Draw(CharacterTexture,
               pos, new Rectangle(0, (int)dir * 128, 128, 128), color, 0, new Vector2(128 / 2, 128 / 2), Vector2.One, se, GetLayerDepth()+0.001f
                );
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            WriteVisualPosition(writer);
            writer.WriteNumber(nameof(RacerID),RacerID);
            writer.WriteBoolean(nameof(PlayerControlled), PlayerControlled);
        }

        public override void RestoreState(JsonElement state)
        {
            ReadVisualPosition(state);
            RacerID = state.GetProperty(nameof(RacerID)).GetInt32();
            PlayerControlled = state.GetProperty(nameof(PlayerControlled)).GetBoolean();
        }
    }
}

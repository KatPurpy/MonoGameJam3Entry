using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    class Entity_FootballBall : Entity
    {

        public override Vector2 VisualPosition { get => body.Position * Game.PixelsPerMeter; set => body.Position = value/Game.PixelsPerMeter; }

        World w;
        Body body;
        EntityManager entityManager;
        Vector2 startPos;
        public Entity_FootballBall(World world,EntityManager entman)
        {
            w = world;
            body = w.CreateCircle(6, 1, default, BodyType.Dynamic);
            body.SetFriction(0);
            body.SetRestitution(1.75f);
            body.LinearDamping = 1;
           // body.Mass = 40;
            body.Tag = this;
            entityManager = entman;
        }

        public override void Draw(GameTime time)
        {
            for (float f = 0f; f <= 1; f += 1/5f)
            {
                Point size = new Point((int)(120 * f));
                Game._.spriteBatch.Draw(
                    Assets.Sprites.circle, new Rectangle(
                        new((int)VisualPosition.X - size.X / 2, (int)VisualPosition.Y - size.Y / 2),
                        size), new Color(Vector3.One*(0.1f+f))
                    );
            }
        }

        public override void Destroy()
        {
            try
            {
                w.Remove(body);
            }
            catch { }
        }

        public override void IMGUI(GameTime time)
        {
            VisualPosition = ImGuiUtils.VecField("Position", VisualPosition);
        }

        public override void RestoreState(JsonElement state)
        {
            ReadVisualPosition(state);
            startPos = VisualPosition / Game.PixelsPerMeter;
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            WriteVisualPosition(writer);
            
        }

        Random r;
        bool scheduleREset;
        public void Reset()
        {
            scheduleREset = true;
        }

        public override void Update(GameTime time)
        {
            if (scheduleREset)
            {
                body.LinearVelocity = -body.LinearVelocity;
                body.Position = startPos;
                scheduleREset = false;
            }

            r = new Random((int)body.Position.X*10);
            foreach (var obj in entityManager.SerializableEntities)
            {
                if (obj is Bathtub b)
                {
                    Vector2 offset = Vector2.UnitX * 5 + Vector2.UnitX * ((float)r.NextDouble()) * 2 * 10;
                    if (b.RacerID < 4)
                    {
                        b.AI_TargetPosition = body.Position - offset;
                    }
                    else
                    {
                        b.AI_TargetPosition = body.Position + offset;
                    }
                }
            }
        }
    }
}

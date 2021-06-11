using Dcrew.Camera;
using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public class Track_PalmWall : Entity
    {
        public override Vector2 VisualPosition { get => 
                Positions.Count == 0 ? new (Mouse.GetState().X-120,Mouse.GetState().Y) : 
                
                Positions[Math.Min(selected,Positions.Count-1)] * Game.PixelsPerMeter; set{ } }

        
        public List<Vector2> Positions = new List<Vector2>();
        public List<Track_Palm> palms = new();

        public World world;

        public EntityManager EntityManager;

        public override void Update(GameTime time){}

        

        int selected, dragging;
        public override void Draw(GameTime time){
        
        }
        float palmDistance = 10 * Game.PixelsPerMeter;
        void Rebuild()
        {
            if (Positions.Count <= 1) return;
            foreach (var p in palms) p.Destroy();
            palms.Clear();
            if (Positions.Count > 1)
            for(int i = 0; i < Positions.Count-1; i++)
            {
                float distance = (Positions[i+1] - Positions[i]).Length() * Game.PixelsPerMeter;
                Vector2 dir = (Positions[i+ 1] - Positions[i]);
                dir.Normalize();
                Vector2 pos = Positions[i] * Game.PixelsPerMeter;

                for (float j = 0; j < distance ; j += palmDistance)
                {
                    var palm = new Track_Palm(world)
                    {
                        texture = Assets.Sprites.palm,
                        ShowInInspector = false
                    }; ;

                    palms.Add(palm);

                    EntityManager.AddEntity(palm);

                    Vector2 spawnPos = pos + dir * j;
                    palm.VisualPosition = spawnPos;
                }

               
            }
        }


        public override void Destroy()
        {
            foreach (var p in palms) p.Destroy();
            Dead = true;
        }

        public override void IMGUI(GameTime time)
        {
            ImGuiUtils.PolylineEditor(Positions, camera, ref dragging, ref selected, Rebuild, 
                VisualPosition / Game.PixelsPerMeter
                
                );
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            writer.WriteStartArray(nameof(Positions));
            foreach(var vec in Positions)
            {
                SerializeVector2(writer,vec);
            }
            writer.WriteEndArray();
        }

        public override void RestoreState(JsonElement state)
        {
            Positions.Clear();

            foreach (var val in state.GetProperty(nameof(Positions)).EnumerateArray())
            {
                Positions.Add(ReadVector2(val));
            }
            Rebuild();
        }
        /*reader.Read();
        reader.Read();
        reader.Read(); //start array
        Debug.Assert(reader.TokenType == JsonTokenType.StartArray);
        do
        {
            reader.Read(); //start object or check if the array has ended
            if ((reader.TokenType == JsonTokenType.EndArray)) break;
            Vector2 vec;
            reader.Read(); //X property
            reader.Read(); //X value
            vec.X = (float)reader.GetDecimal();
            reader.Read(); //Y property
            reader.Read(); //Y value
            vec.Y = (float)reader.GetDecimal();
            reader.Read(); //end object
            Positions.Add(vec);
        } while (true);
    }*/
    }
}

